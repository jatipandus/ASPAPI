﻿var Departments = [];
/*--------------------------------------------------------------------------------------------------*/
$(document).ready(function () {
    loadDepartment2($('#DepartmentOption'));
    table = $('#Divisi').dataTable({
        "ajax": {
            url: "/Divisi/XLoadDivisi",
            type: "GET",
            dataType: "json",
            dataSrc: "",
        },
        "columnDefs": [
            { "orderable": false, "targets": 4 },
            { "searchable": false, "targets": 4 }
        ],
        "columns": [
            { "data": "DivisiName" },
            { "data": "DepartmentName" },
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
}); //function load data division
/*--------------------------------------------------------------------------------------------------*/
document.getElementById("btnaddDiv").addEventListener("click", function () {
    $('#Id').val('');
    $('#Name').val('');
    $('#DepartmentOption').val('');
    $('#SaveBtn').show();
    $('#UpdateBtn').hide();
}); //fungsi btn add
/*--------------------------------------------------------------------------------------------------*/
function loadDepartment2(element) {
    if (Departments.length == 0) {
        $.ajax({
            type: "GET",
            url: "/Department/XLoadDepartment",
            success: function (data) {
                Departments = data;
                renderDepartment(element);
            }
        })
    }
    else {
        renderDepartment(element);
    }
} //Load data department
function renderDepartment(element) {
    var $option = $(element);
    $option.empty();
    $option.append($('<option/>').val('0').text('Select Department').hide());
    $.each(Departments, function (i, val) {
    $option.append($('<option/>').val(val.Id).text(val.Name));
    })
} //Load data department to option field

function GetById(Id) {
    $.ajax({
        url: "/Divisi/GetById/" + Id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            const obj = JSON.parse(result);
            $('#Id').val(obj.Id);
            $('#Name').val(obj.DivisiName);
            $('#DepartmentOption').val(obj.DepartmentId);
            $('#myModal').modal('show');
            $('#UpdateBtn').show();
            $('#SaveBtn').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responsText);
        }
    })
}

function Save()
{
    $.fn.dataTable.ext.errMode = 'none';
    table = $('#Divisi').DataTable({
        "ajax": {
            url: "/Divisi/XLoadDivisi"
        }
    });
    var Divisi = new Object();
    Divisi.Name = $('#Name').val();
    Divisi.DepartmentId = $('#DepartmentOption').val();
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
            url: '/Divisi/Insert/',
            data: Divisi
        }).then((result) => {
            if (result.StatusCode == 200) {
                Swal.fire({
                    icon: 'success',
                    potition: 'center',
                    title: 'Division Add Successfully',
                    timer: 2500
                }).then(function () {
                    $('#myModal').modal('hide');
                    table.ajax.reload();
                    $('#Id').val('');
                    $('#Name').val('');
                    $('#DepartmentOption').val('');
                });
            }
            else {
                Swal.fire('Error', 'Failed to Input', 'error');
            }
        })
    }
}

function Edit() {
    $.fn.dataTable.ext.errMode = 'none';
    var table = $('#Divisi').DataTable({
        "ajax": {
            url: "/Divisi/XLoadDivisi"
        }
    });
    var Divisi = new Object();
    Divisi.Id = $('#Id').val();
    Divisi.Name = $('#Name').val();
    Divisi.DepartmentId = $('#DepartmentOption').val();
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
            url: '/Divisi/Insert',
            data: Divisi
        }).then((result) => {
            if (result.StatusCode == 200) {
                Swal.fire({
                    icon: 'success',
                    potition: 'center',
                    title: 'Division Update Successfully',
                    timer: 2500
                }).then(function () {
                    $('#myModal').modal('hide');
                    table.ajax.reload();
                    $('#Id').val('');
                    $('#Name').val('');
                    $('#DepartmentOption').val('');
                });
            } else {
                Swal.fire('Error', 'Failed to Edit', 'error');
            }
        })
    }
}


function Delete(Id) {
    $.fn.dataTable.ext.errMode = 'none';
    var table = $('#Divisi').DataTable({
        "ajax": {
            url: "/Divisi/XLoadDivisi"
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
                url: "/Divisi/Delete/",
                data: { Id: Id }
            }).then((result) => {
                if (result.StatusCode == 200) {
                    Swal.fire({
                        icon: 'success',
                        position: 'center',
                        title: 'Delete Successfully',
                        timer: 2000
                    }).then(function () {
                        $('#myModal').modal('hide');
                        table.ajax.reload();
                        $('#Id').val('');
                        $('#Name').val('');
                        $('#DepartmentOption').val('');
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