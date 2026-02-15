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

// ==================== RETURNS ====================

async function loadReturns() {
    try {
        const response = await fetch(`${API_BASE_URL}/returns`);
        if (!response.ok) throw new Error('Failed to load returns');
        
        const returns = await response.json();
        displayReturns(returns);
    } catch (error) {
        showNotification('Error loading returns: ' + error.message, 'error');
    }
}

function displayReturns(returns) {
    const tbody = document.getElementById('returnsTableBody');
    tbody.innerHTML = '';

    if (returns.length === 0) {
        const row = tbody.insertRow();
        row.innerHTML = '<td colspan="5" style="text-align: center;">No returns found</td>';
        return;
    }

    returns.forEach(returnInfo => {
        const row = tbody.insertRow();
        row.innerHTML = `
            <td>${returnInfo.id}</td>
            <td>${returnInfo.orderId}</td>
            <td>${new Date(returnInfo.returnDate).toLocaleDateString()}</td>
            <td>${returnInfo.reason || '-'}</td>
            <td>$${returnInfo.refundedAmount.toFixed(2)}</td>
        `;
    });
}

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
    
    const orderId = parseInt(document.getElementById('returnOrderId').value);
    const reason = document.getElementById('returnReason').value;
    const refundedAmount = parseFloat(document.getElementById('returnAmount').value);
    
    if (orderId <= 0) {
        showNotification('Order ID must be greater than 0', 'error');
        return;
    }
    
    if (reason.length < 2) {
        showNotification('Reason must be at least 2 characters', 'error');
        return;
    }
    
    if (refundedAmount < 0) {
        showNotification('Refunded amount cannot be negative', 'error');
        return;
    }
    
    const returnData = {
        orderId: orderId,
        reason: reason,
        refundedAmount: refundedAmount
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
        loadReturns();
    } catch (error) {
        showNotification('Error creating return: ' + error.message, 'error');
    }
}

// Initialize the page
loadReturns();
