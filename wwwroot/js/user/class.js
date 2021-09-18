var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/user/class/getall/",
            "type": "GET",
            "dataType": "json"
        },
        "columns": [
            { "data": "name", "width": "55%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
<                               a href="/User/Student/StudentInClass/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit">View Students</i>
                                </a>
                                <a href="/User/Schedule/ClassSchedule/?classId=${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit">Schedule</i>
                                </a>
                                <a href="/User/Class/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onclick=Delete("/User/Class/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                            </div>
                           `;
                }, "width": "45%"
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