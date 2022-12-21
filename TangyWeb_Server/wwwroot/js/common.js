window.showToastr = (type, message) => {
    if (type === "success") {
        toastr.success(message, "Great", { timeOut: 5000});
    } else {
        toastr.error(message, "Not So Great", { timeOut: 5000});
    }
}

function showDeleteConfirmationModal() {
    $("#deleteConfirmationModal").modal("show");
}

function hideDeleteConfirmationModal() {
    $("#deleteConfirmationModal").modal("hide");
}