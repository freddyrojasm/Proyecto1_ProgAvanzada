
$(document).ready(function () {
    var successMessage = '@TempData["SuccessMessage"]';
    if (successMessage !== '') {
        toastr.success(successMessage);
    }
});