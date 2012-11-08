var user = [];
var educationList = [];
var currentUser;
var experience;
var isWilling;
var instructorUrl;

$(document).ready(function () {

    $('#appForm').submit(function () {
        experience = $('#Experience').val();
        isWilling = $('#WillingToTravel').is(":checked");
        $('#editorRows .editorRow').each(function () {
            var education = {
                UniversityOrCollege: $(this).find('.university').val(),
                AreaOfStudy: $(this).find('.area').val(),
                Degree: $(this).find('.degree').val(),
                YearReceived: $(this).find('.year').val()
            }
            educationList.push(education);
        });
        var applicationFromView = {
            EducationalBackgrounds: educationList,
            CurrentUserId: currentUser,
            Experience: experience,
            WillingToTravel: isWilling
        }
        $.ajax({
            type: 'POST',
            url: instructorUrl,
            dataType: 'JSON',
            async: false,
            data: applicationFromView.serialize(),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                return false;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
                alert(xhr.responseText);
                return false;
            }
        });
    });

    $('a.deleteRow').live('click', function () {
        var count = $("#editorRows .editorRow").size();
        $(this).parents("div.editorRow:first").remove();
        return false;
    });
});


$('#addItem').live('click', function () {
    var count = $("#editorRows .editorRow").size();
    var UniversityOrCollege = "EducationalBackGrounds_" + count + "__UniversityOrCollege";
    var AreaOfStudy = "EducationalBackGrounds_" + count + "__AreaOfStudy";
    var Degree = "EducationalBackGrounds_" + count + "__Degree";
    var YearReceived = "EducationalBackGrounds_" + count + "__YearReceived";
    var clonedRow = $('.newRow').html();
    $("#editorRows .editorRow:last").after(clonedRow);
    $("#editorRows .editorRow:last").find('.university').attr('id', UniversityOrCollege).attr('name', 'EducationalBackGrounds[' + count + '].UniversityOrCollege');
    $("#editorRows .editorRow:last").find('.area').attr('id', AreaOfStudy).attr('name', 'EducationalBackGrounds[' + count + '].AreaOfStudy');
    $("#editorRows .editorRow:last").find('.degree').attr('id', Degree).attr('name', 'EducationalBackGrounds[' + count + '].Degree');
    $("#editorRows .editorRow:last").find('.year').attr('id', YearReceived).attr('name', 'EducationalBackGrounds[' + count + '].YearReceived');
    return false;
});