var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/User/Test/GetAll/"
        },
        "columns": [
            { "data": "scheduleModel.title", "width": "15%" },
            { "data": "title", "width": "15%" },
            { "data": "numberOfQuestion", "width": "20%" },
            { "data": "status", "width": "10%" },
            {
                "data": {
                    id :"id", lockoutEnd : "LockoutEnd" },
                "render": function (data) {
                    var today = new Date().getTime()
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        return `
                            <div class="text-center">                             
                                <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white btn-sm" style="cursor:pointer">
                                    <i class="fas fa-lock-open"></i> Unlock
                                </a>                         
                                <a href="/User/Test/Upsert/${data.id}" class="btn btn-success text-white btn-sm" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                <a onclick=Delete("/User/Test/Delete/${data.id}") class="btn btn-danger text-white btn-sm" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                            </div>
                           `;
                    } else {
                        return `
                            <div class="text-center">                             
                                <a onclick=LockUnlock('${data.id}') class="btn btn-success text-white btn-sm" style="cursor:pointer">
                                    <i class="fas fa-lock"></i> Lock
                                </a>
                                <a href="/User/Test/Upsert/${data.id}" class="btn btn-success text-white btn-sm" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                <a onclick=Delete("/User/Test/Delete/${data.id}") class="btn btn-danger text-white btn-sm" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                            </div>
                          
                           `;
                    }
                }, "width": "60%"
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
};
function LockUnlock(id) {
            $.ajax({
                type: "POST",
                url: '/User/Test/LockUnlock',
                data: JSON.stringify(id),
                contentType: "application/json",
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
    