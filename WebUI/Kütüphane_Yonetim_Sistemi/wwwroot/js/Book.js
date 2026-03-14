function openDeleteModal(id, bookTitle) {
    document.getElementById('deleteBookTitle').innerText = bookTitle;
    document.getElementById('deleteForm').action = '/EditBook/DeleteBook/' + id;
    new bootstrap.Modal(document.getElementById('deleteModal')).show();
}