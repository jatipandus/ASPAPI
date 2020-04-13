$(document).ready(function () {

    // Checkbox click
    $(".hidecol").click(function () {

        var id = this.id;
        var splitid = id.split("_");
        var colno = splitid[1];
        var checked = true;

        // Checking Checkbox state
        if ($(this).is(":checked")) {
            checked = true;
        } else {
            checked = false;
        }
        setTimeout(function () {
            if (checked) {
                $('#Divisi td:nth-child(' + colno + ')').hide();
                $('#Divisi th:nth-child(' + colno + ')').hide();
            } else {
                $('#Divisi td:nth-child(' + colno + ')').show();
                $('#Divisi th:nth-child(' + colno + ')').show();
            }

        }, 1500);

    });
});