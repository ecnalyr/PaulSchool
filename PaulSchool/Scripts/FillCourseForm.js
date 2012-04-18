/// <reference path="jquery-1.5.1-vsdoc.js" />
/// <reference path="jquery-ui-1.8.11.js" />

//
$(document).ready(function () {
    $("#SelectedCourse").change(function () {
        // When the dropdown is changed on the Apply To Teach A Course page, 
        // a request for a PartialView that contains a pre-filled form is sent to the controller
        var strSelected = "";
        $("#SelectedCourse option:selected").each(function () {
            strSelected += $(this)[0].value;
        });
        var url = "/Course/PreFillCourse/?SelectedCourse=" + strSelected;

        $.post(url, function (data) {
            // appends the PartialView that was sent by the controller to "divToAppend"
            $("#originalForm").remove();
            $('#divToAppend').html(data);
        });
    });
});