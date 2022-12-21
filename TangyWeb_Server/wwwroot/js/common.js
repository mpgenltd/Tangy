window.showToastr = (type, message) => {
    if (type === "success") {
        toastr.success(message, "Great", { timeOut: 5000});
    } else {
        Swal.fire({
            title: 'Error!',
            text: message,
            icon: 'error',
            confirmButtonText: 'Cool'
        });    
    }
}

function showDeleteConfirmationModal() {
    $("#deleteConfirmationModal").modal("show");
}

function hideDeleteConfirmationModal() {
    $("#deleteConfirmationModal").modal("hide");
}