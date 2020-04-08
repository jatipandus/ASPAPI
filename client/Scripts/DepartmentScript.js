﻿//$(document).ready(function () {
//    $('#Department').dataTable({
//        "ajax": loadDepartment(),
//        "responsive": true,
//    });
//    $('[data-toggle="tooltip"]').tooltip();
//});

$(document).ready(function () {
    $('#Edit').hide();
    loadDepartment();
    $(function () {
        $('[data-toggle="tooltip"]').tooltip();
    });
});
function loadDepartment() {
    $.ajax({
        url: "/Department/XLoadDepartment",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
    }).done(function (data) {
        $('#Department').dataTable({
            "data": data,
            "columns": [
                //{ "data": "Id" },
                { "data": "Name" },
                {
                    "data": "CreateDate", "render": function (data) {
                        return moment(data).format('DD/MM/YYYY');
                    }
                },
                {
                    "data": "UpdateDate", "render": function (data) {
                        var text = "Not Update Yet";
                        var nulldate = "";
                        if (data == nulldate) {
                            return text();
                        } else {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    }
                },
                {
                    data: null, orderable: false, render: function (data, type, row) {
                        return '<button type="button" class="btn btn-warning" id="BtnEdit" data-toggle="tooltip" data-placement="top" title="Edit" onclick="return GetById(' + row.Id + ')"><i class="mdi mdi-pencil"></i></button> &nbsp; <button type="button" class="btn btn-danger" id="BtnDelete" data-toggle="tooltip" data-placement="top" title="Hapus" onclick="return Delete(' + row.Id + ')"><i class="mdi mdi-delete"></i></button>';
                    }
                }
            ]
        })
    });
}


//function loadDepartment() {
//    $.ajax({
//        url: "/Department/XLoadDepartment",
//        type: "GET",
//        contentType: "application/json;charset=utf-8",
//        dataType: "json",
//        success: function (result) {
//            var html = '';
//            $.each(result, function (key, Department) {
//                html += '<tr>';
//                html += '<td>' + Department.Name + '</td>';
//                html += '<td>' + moment(Department.CreateDate).format('DD-MM-YYYY') + '</td>';
//                html += '<td>' + moment(Department.UpdateDate).format('DD-MM-YYYY') + '</td>';
//                html += '<td><button type="button" class="btn btn-warning" id="Update" onclick="return GetById(' + Department.Id + ')">Edit </button>';
//                html += '<button type="button" class="btn btn-danger" id="Delete" onclick="return Delete(' + Department.Id + ')">Delete </button></td>';
//                html += '</tr>';
//            });
//            $('.departmentbody').html(html);
//        },
//        error: function (errormessage) {
//            alert(errormessage.responseText)
//        }
//    });
//}

function Save() {
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
            url: '/Department/Insert/',
            data: Department
        }).then((result) => {
            if (result.StatusCode == 200) {
                Swal.fire({
                    icon: 'success',
                    position: 'center',
                    title: 'Saved Successfully',
                    showConfirmButton: false,
                    timer: 2000
                }).then(function () {
                    location.reload();
                    ClearScreen();
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Failed to Insert',
                })
                ClearScreen();
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
                icon: 'Success',
                position: 'Center',
                title: 'Department Update Successfully',
                timer: 2000
            }).then(function () {
                location.reload();
                ClearScreen();
            });
        }
        else {
            Swal.fire('Error', 'Failed To Input', 'Error');
            ClearScreen();
        }
     })
}

function Delete(Id) {
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
                        location.reload();
                        ClearScreen();
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





