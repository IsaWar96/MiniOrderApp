// Automatically detect the correct API URL based on current page protocol and host
const API_BASE_URL = `${window.location.protocol}//${window.location.host}/api`;

// Notifications
function showNotification(message, type = 'success') {
    const notification = document.getElementById('notification');
    notification.textContent = message;
    notification.className = `notification ${type} show`;
    setTimeout(() => {
        notification.classList.remove('show');
    }, 3000);
}

// Email validation helper
function isValidEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
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
    const email = document.getElementById('customerEmail').value;
    
    // Validate email format if provided
    if (email && !isValidEmail(email)) {
        showNotification('Invalid email format', 'error');
        return;
    }
    
    const customerData = {
        name: document.getElementById('customerName').value,
        email: email,
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

// Initialize the page
loadCustomers();
