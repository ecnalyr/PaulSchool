/// <reference path="jquery-1.5.1-vsdoc.js" />
/// <reference path="jquery-ui-1.8.11.js" />


//$(document).ready(function () {
//    $("#SelectedCourse").change(function () {
//        /*var strSelected = jQuery("select#SelectedCourse").val();
//        $("#CourseFormFields").replaceWith('<div id="CourseFormFields">' + strSelected + '</div>');*/
//        $.ajax({  async: false,
//            url: "http://localhost:49462/Course/PreFillCourse/?SelectedCourse=" + $(this).val(),
//            success: function (str) {
//                //Here eval the JSon value and load it's data into form fields.
//                $("#CourseFormFields").replaceWith('<h2>hi</h2>');
//        }
//    });
//});

$(document).ready(function () {
    $("#SelectedCourse").change(function () {
        var strSelected = "";
        $("#SelectedCourse option:selected").each(function () {
            strSelected += $(this)[0].value;
        });
        var url = "/Course/PreFillCourse/?SelectedCourse=" + strSelected;

        $.post(url, function (data) {
            $("#originalForm").remove();
            $('#divToAppend').html(data);
            // What do I do here to have my partial view returned at the end of the currently displayed page?
        });
    });
});