(function () {
    'use strict';
    //Add event listener for when view filter is applied
    $('#btnApplyFilter').click(function () {
        var ref = $('#tblDataTable').DataTable();
        ref.ajax.reload();
        $("#filterModal").modal("hide");
    });

    $('form').submit(function () {
        if ($('#formPost').valid()) {
            $('#submitButton').prop('disabled', true);
            $('#submitButton').html("<i class='fa fa-sync fa-spin'></i>&nbsp;Please Wait...");

        }
    });

})();

function GetDetails(element) {
    var link = element.attributes.data;
    var spinner = $('#loader');
    spinner.show();
    console.log(link);
    if (link != null || link.value != "") {
        $.ajax({
            url: link.value,
            success: function (response) {
                spinner.hide();
                $('#myModalContent').html(response)
                $('#myModal').modal('show')

            },
            error: function (error) {
                swal({
                    title: "Oops...",
                    text: error.responseJSON.message,
                    type: "error",
                    animation: true,
                    customClass: 'animated tada',
                });

            }
        });
    }
}
