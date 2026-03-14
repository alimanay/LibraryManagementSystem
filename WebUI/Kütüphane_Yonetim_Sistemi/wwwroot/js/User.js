function openDeleteModal(id, userName) {
    document.getElementById('deleteUserName').innerText = userName;
    document.getElementById('deleteForm').action = '/User/DeleteUser/' + id;
    new bootstrap.Modal(document.getElementById('deleteModal')).show();
}