function openReturnModal(id, userName, bookTitle) {
    document.getElementById('returnUserName').innerText = userName;
    document.getElementById('returnBookTitle').innerText = bookTitle;
    document.getElementById('returnForm').action = '/RentalEdit/ReturnRental/' + id;
    new bootstrap.Modal(document.getElementById('returnModal')).show();
}

function openDeleteModal(id, userName, bookTitle) {
    document.getElementById('deleteUserName').innerText = userName;
    document.getElementById('deleteBookTitle').innerText = bookTitle;
    document.getElementById('deleteForm').action = '/RentalEdit/DeleteRental/' + id;
    new bootstrap.Modal(document.getElementById('deleteModal')).show();
}