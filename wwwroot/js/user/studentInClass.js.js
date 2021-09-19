var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/User/Student/StudentInClass/",
            "type": "GET",
            "data": jQuery.param({ classId: $('#classId').val()})

        },
        "columns": [
            { "data": "classModel.name", "width": "30%" },
            { "data": "subjectModel.name", "width": "30%" },
            { "data": "dateTime", "width": "30%" },
            { "data": "startTime", "width": "30%" },
            { "data": "endTime", "width": "30%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/User/Schedule/Upsert/${data}" class="btn btn-success text-white btn-sm" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                <a onclick=Delete("/User/Schedule/Delete/${data}") class="btn btn-danger text-white btn-sm" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>

                            </div>
                           `;
                }, "width": "40%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}