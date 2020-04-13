$(document).ready(function () {
    table = $('#Department').dataTable({
        "ajax": {
            url: "/Department/XLoadDepartment",
            type: "GET",
            dataType: "json",
            dataSrc: "",
        },
        "columnDefs": [
            { "orderable": false, "targets": 3 },
            { "searchable": false, "targets": 3 }
        ],
        "columns": [
            { "data": "Name" },
            {
                "data": "CreateDate", "render": function (data) {
                    return moment(data).format('DD/MM/YYYY, h:mm a');
                }
            },
            {
                "data": "UpdateDate", "render": function (data) {
                    var dateupdate = "Not Updated Yet";
                    var nulldate = null;
                    if (data == nulldate) {
                        return dateupdate;
                    } else {
                        return moment(data).format('DD/MM/YYYY, h:mm a');
                    }
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return " <td><button type='button' class='btn btn-warning' id='BtnEdit' onclick=GetById('" + row.Id + "');>Edit</button> <button type='button' class='btn btn-danger' id='BtnDelete' onclick=Delete('" + row.Id + "');>Delete</button ></td >";
                }
            },
        ]
    });
}); //load
document.getElementById("btnaddDept").addEventListener("click", function () {
    $('#Id').val('');
    $('#Name').val('');
    $('#SaveBtn').show();
    $('#UpdateBtn').hide();
});

function Save() {
    $.fn.dataTable.ext.errMode = 'none';
    var table = $('#Department').DataTable({
        "ajax": {
            url: "/Department/XLoadDepartment"
        }
    });
    var Department = new Object();
    Department.Name = $('#Name').val();
    if ($('#Name').val() == "") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Name Cannot be Empty',
        })
        return false;
    } else {
        $.ajax({
            type: 'POST',
            url: '/Department/Insert',
            data: Department,
        }).then((result) => {
            if (result.StatusCode == 200) {
                Swal.fire({
                    icon: 'success',
                    potition: 'center',
                    title: 'Department Add Successfully',
                    timer: 2500
                }).then(function () {
                    table.ajax.reload();
                    $('#myModal').modal('hide');
                    $('#Id').val('');
                    $('#Name').val('');
                });
            }
            else {
                Swal.fire('Error', 'Failed to Input', 'error');
            }
        })
    }
}


function GetById(Id) {
    $.ajax({
        url: "/Department/GetById/" + Id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            const obj = JSON.parse(result);
            $('#Id').val(obj.Id);
            $('#Name').val(obj.Name);
            $('#myModal').modal('show');
            $('#UpdateBtn').show();
            $('#SaveBtn').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    })
}

function Edit() {
    $.fn.dataTable.ext.errMode = 'none';
    var table = $('#Department').DataTable({
        "ajax": {
            url: "/Department/XLoadDepartment"
        }
    });
    var Department = new Object();
    Department.Id = $('#Id').val();
    Department.Name = $('#Name').val();
    $.ajax({
        type: 'POST',
        url: '/Department/Insert',
        data: Department
    }).then((result) => {
        if (result.StatusCode == 200) {
            Swal.fire({
                icon: 'success',
                potition: 'center',
                title: 'Department Update Successfully',
                timer: 2500
            }).then(function () {
                table.ajax.reload();
                $('#myModal').modal('hide');
                $('#Id').val('');
                $('#Name').val('');
            });
        } else {
            Swal.fire('Error', 'Failed to Edit', 'error');
        }
    })
}

function Delete(Id) {
    $.fn.dataTable.ext.errMode = 'none';
    var table = $('#Department').DataTable({
        "ajax": {
            url: "/Department/XLoadDepartment"
        }
    });
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        showCancelButton: true,
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/Department/Delete/",
                data: { Id: Id }
            }).then((result) => {
                if (result.StatusCode == 200) {
                    Swal.fire({
                        icon: 'success',
                        position: 'center',
                        title: 'Delete Successfully',
                        timer: 2000
                    }).then(function () {
                        table.ajax.reload();
                        $('#myModal').modal('hide');
                        $('#Id').val('');
                        $('#Name').val('');
                    });
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'error',
                        text: 'Failed to Delete',
                    })
                    ClearScreen();
                }
            })
        }
    });
}
