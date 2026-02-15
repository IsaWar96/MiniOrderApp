// Automatically detect the correct API URL based on current page protocol and host
const API_BASE_URL = `${window.location.protocol}//${window.location.host}/api`;

// Navigation
document.querySelectorAll('.nav-btn').forEach(btn => {
    btn.addEventListener('click', () => {
        const view = btn.dataset.view;
        switchView(view);
    });
});

function switchView(viewName) {
    // Update active states
    document.querySelectorAll('.nav-btn').forEach(btn => {
        btn.classList.toggle('active', btn.dataset.view === viewName);
    });
    document.querySelectorAll('.view').forEach(view => {
        view.classList.toggle('active', view.id === `${viewName}-view`);
    });

    // Load data for the selected view
    if (viewName === 'customers') {
        loadCustomers();
    } else if (viewName === 'orders') {
        loadOrders();
    } else if (viewName === 'returns') {
        // Returns are loaded on demand
    }
}

// Notifications
function showNotification(message, type = 'success') {
    const notification = document.getElementById('notification');
    notification.textContent = message;
    notification.className = `notification ${type} show`;
    setTimeout(() => {
        notification.classList.remove('show');
    }, 3000);
}

// ==================== CUSTOMERS ====================

async function loadCustomers() {
    try {
        console.log('Fetching customers from:', `${API_BASE_URL}/customers`);
        const response = await fetch(`${API_BASE_URL}/customers`);
        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(`Failed to load customers: ${response.status} - ${errorText}`);
        }
        
        const customers = await response.json();
        console.log('Customers loaded:', customers);
        displayCustomers(customers);
    } catch (error) {
        console.error('Error loading customers:', error);
        showNotification('Error loading customers: ' + error.message, 'error');
    }
}

function displayCustomers(customers) {
    const tbody = document.getElementById('customersTableBody');
    tbody.innerHTML = '';

    customers.forEach(customer => {
        const row = tbody.insertRow();
        row.innerHTML = `
            <td>${customer.id}</td>
            <td>${customer.name}</td>
            <td>${customer.email || '-'}</td>
            <td>${customer.phone}</td>
            <td>
                <button class="btn btn-info" onclick="editCustomer(${customer.id})">Edit</button>
                <button class="btn btn-danger" onclick="deleteCustomer(${customer.id})">Delete</button>
            </td>
        `;
    });
}

function showCustomerForm() {
    document.getElementById('customer-form').style.display = 'block';
    document.getElementById('customer-form-title').textContent = 'Add Customer';
    document.getElementById('customerForm').reset();
    document.getElementById('customerId').value = '';
}

function cancelCustomerForm() {
    document.getElementById('customer-form').style.display = 'none';
    document.getElementById('customerForm').reset();
}

async function saveCustomer(event) {
    event.preventDefault();
    
    const customerId = document.getElementById('customerId').value;
    const customerData = {
        name: document.getElementById('customerName').value,
        email: document.getElementById('customerEmail').value,
        phone: document.getElementById('customerPhone').value
    };

    try {
        let response;
        if (customerId) {
            // Update existing customer
            response = await fetch(`${API_BASE_URL}/customers/${customerId}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(customerData)
            });
        } else {
            // Create new customer
            response = await fetch(`${API_BASE_URL}/customers`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(customerData)
            });
        }

        if (!response.ok) {
            const error = await response.text();
            throw new Error(error);
        }

        showNotification(customerId ? 'Customer updated successfully' : 'Customer created successfully');
        cancelCustomerForm();
        loadCustomers();
    } catch (error) {
        showNotification('Error saving customer: ' + error.message, 'error');
    }
}

async function editCustomer(id) {
    try {
        const response = await fetch(`${API_BASE_URL}/customers/${id}`);
        if (!response.ok) throw new Error('Failed to load customer');
        
        const customer = await response.json();
        
        document.getElementById('customerId').value = customer.id;
        document.getElementById('customerName').value = customer.name;
        document.getElementById('customerEmail').value = customer.email || '';
        document.getElementById('customerPhone').value = customer.phone;
        document.getElementById('customer-form-title').textContent = 'Edit Customer';
        document.getElementById('customer-form').style.display = 'block';
    } catch (error) {
        showNotification('Error loading customer: ' + error.message, 'error');
    }
}

async function deleteCustomer(id) {
    if (!confirm('Are you sure you want to delete this customer?')) return;

    try {
        const response = await fetch(`${API_BASE_URL}/customers/${id}`, {
            method: 'DELETE'
        });

        if (!response.ok) throw new Error('Failed to delete customer');

        showNotification('Customer deleted successfully');
        loadCustomers();
    } catch (error) {
        showNotification('Error deleting customer: ' + error.message, 'error');
    }
}

// ==================== ORDERS ====================

let orderItemsCount = 0;

async function loadOrders() {
    try {
        const response = await fetch(`${API_BASE_URL}/orders`);
        if (!response.ok) throw new Error('Failed to load orders');
        
        const orders = await response.json();
        displayOrders(orders);
    } catch (error) {
        showNotification('Error loading orders: ' + error.message, 'error');
    }
}

function displayOrders(orders) {
    const tbody = document.getElementById('ordersTableBody');
    tbody.innerHTML = '';

    orders.forEach(order => {
        const row = tbody.insertRow();
        row.innerHTML = `
            <td>${order.id}</td>
            <td>${order.customerId}</td>
            <td>${new Date(order.orderDate).toLocaleDateString()}</td>
            <td>${getStatusText(order.status)}</td>
            <td>$${order.totalAmount.toFixed(2)}</td>
            <td>
                <button class="btn btn-info" onclick="viewOrderDetails(${order.id})">View</button>
                <button class="btn btn-danger" onclick="deleteOrder(${order.id})">Delete</button>
            </td>
        `;
    });
}

function getStatusText(status) {
    const statusMap = {
        0: 'Created',
        1: 'Shipped',
        2: 'Delivered',
        3: 'Returned'
    };
    return statusMap[status] || 'Unknown';
}

async function showOrderForm() {
    document.getElementById('order-form').style.display = 'block';
    document.getElementById('orderForm').reset();
    document.getElementById('order-items-container').innerHTML = '';
    orderItemsCount = 0;
    
    // Load customers for the dropdown
    try {
        const response = await fetch(`${API_BASE_URL}/customers`);
        if (!response.ok) throw new Error('Failed to load customers');
        
        const customers = await response.json();
        const select = document.getElementById('orderCustomerId');
        select.innerHTML = '<option value="">Select Customer</option>';
        customers.forEach(customer => {
            const option = document.createElement('option');
            option.value = customer.id;
            option.textContent = `${customer.name} (ID: ${customer.id})`;
            select.appendChild(option);
        });
    } catch (error) {
        showNotification('Error loading customers: ' + error.message, 'error');
    }
    
    addOrderItem(); // Add first item by default
}

function cancelOrderForm() {
    document.getElementById('order-form').style.display = 'none';
    document.getElementById('orderForm').reset();
}

function addOrderItem() {
    const container = document.getElementById('order-items-container');
    const itemDiv = document.createElement('div');
    itemDiv.className = 'order-item';
    itemDiv.innerHTML = `
        <div class="order-item-row">
            <div class="form-group">
                <label>Product Name *</label>
                <input type="text" class="item-product" required>
            </div>
            <div class="form-group">
                <label>Quantity *</label>
                <input type="number" class="item-quantity" min="1" required>
            </div>
            <div class="form-group">
                <label>Unit Price *</label>
                <input type="number" class="item-price" step="0.01" min="0" required>
            </div>
            <div>
                <button type="button" class="btn btn-danger" onclick="removeOrderItem(this)">Remove</button>
            </div>
        </div>
    `;
    container.appendChild(itemDiv);
    orderItemsCount++;
}

function removeOrderItem(button) {
    if (orderItemsCount > 1) {
        button.closest('.order-item').remove();
        orderItemsCount--;
    } else {
        showNotification('Order must have at least one item', 'error');
    }
}

async function saveOrder(event) {
    event.preventDefault();
    
    const customerId = parseInt(document.getElementById('orderCustomerId').value);
    const items = [];
    
    document.querySelectorAll('.order-item').forEach(itemDiv => {
        items.push({
            productName: itemDiv.querySelector('.item-product').value,
            quantity: parseInt(itemDiv.querySelector('.item-quantity').value),
            unitPrice: parseFloat(itemDiv.querySelector('.item-price').value)
        });
    });

    const orderData = {
        customerId: customerId,
        items: items
    };

    try {
        const response = await fetch(`${API_BASE_URL}/orders`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(orderData)
        });

        if (!response.ok) {
            const error = await response.text();
            throw new Error(error);
        }

        showNotification('Order created successfully');
        cancelOrderForm();
        loadOrders();
    } catch (error) {
        showNotification('Error creating order: ' + error.message, 'error');
    }
}

async function viewOrderDetails(id) {
    try {
        const response = await fetch(`${API_BASE_URL}/orders/${id}`);
        if (!response.ok) throw new Error('Failed to load order');
        
        const order = await response.json();
        
        let itemsHtml = '<ul>';
        order.items.forEach(item => {
            itemsHtml += `<li>${item.productName} - Qty: ${item.quantity} x $${item.unitPrice.toFixed(2)} = $${(item.quantity * item.unitPrice).toFixed(2)}</li>`;
        });
        itemsHtml += '</ul>';
        
        const message = `Order #${order.id}\nCustomer ID: ${order.customerId}\nDate: ${new Date(order.orderDate).toLocaleDateString()}\nStatus: ${getStatusText(order.status)}\nTotal: $${order.totalAmount.toFixed(2)}\n\nItems:\n${itemsHtml}`;
        
        alert(message.replace(/<[^>]*>/g, '\n'));
    } catch (error) {
        showNotification('Error loading order details: ' + error.message, 'error');
    }
}

async function deleteOrder(id) {
    if (!confirm('Are you sure you want to delete this order?')) return;

    try {
        const response = await fetch(`${API_BASE_URL}/orders/${id}`, {
            method: 'DELETE'
        });

        if (!response.ok) throw new Error('Failed to delete order');

        showNotification('Order deleted successfully');
        loadOrders();
    } catch (error) {
        showNotification('Error deleting order: ' + error.message, 'error');
    }
}

// ==================== RETURNS ====================

function showReturnForm() {
    document.getElementById('return-form').style.display = 'block';
    document.getElementById('returnForm').reset();
}

function cancelReturnForm() {
    document.getElementById('return-form').style.display = 'none';
    document.getElementById('returnForm').reset();
}

async function saveReturn(event) {
    event.preventDefault();
    
    const returnData = {
        orderId: parseInt(document.getElementById('returnOrderId').value),
        reason: document.getElementById('returnReason').value,
        refundedAmount: parseFloat(document.getElementById('returnAmount').value)
    };

    try {
        const response = await fetch(`${API_BASE_URL}/returns`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(returnData)
        });

        if (!response.ok) {
            const error = await response.text();
            throw new Error(error);
        }

        showNotification('Return created successfully');
        cancelReturnForm();
    } catch (error) {
        showNotification('Error creating return: ' + error.message, 'error');
    }
}

// Initialize the app
loadCustomers();
