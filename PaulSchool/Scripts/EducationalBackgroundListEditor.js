var user = [];
var educationList = [];
var currentUser;
var experience;
var isWilling;
var instructorUrl;
$(document).ready(function () {
    $('#btnSubmit').click(function () {
        var experience = $('#Experience').val();
        var isWilling = $('#WillingToTravel').is(":checked");
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
            dataType: "json",
            data: JSON.stringify(applicationFromView),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
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
    var UniversityOrCollege = "EducationalBackGround_" + count + "__UniversityOrCollege";
    var AreaOfStudy = "EducationalBackGround_" + count + "__AreaOfStudy";
    var Degree = "EducationalBackGround_" + count + "__Degree";
    var YearReceived = "EducationalBackGround_" + count + "__YearReceived";
    var clonedRow = $('.newRow').html();
    $("#editorRows .editorRow:last").after(clonedRow);
    $("#editorRows .editorRow:last").find('.university').attr('id', UniversityOrCollege).attr('name', 'EducationalBackGround[' + count + '].UniversityOrCollege');
    $("#editorRows .editorRow:last").find('.area').attr('id', AreaOfStudy).attr('name', 'EducationalBackGround[' + count + '].AreaOfStudy');
    $("#editorRows .editorRow:last").find('.degree').attr('id', Degree).attr('name', 'EducationalBackGround[' + count + '].Degree');
    $("#editorRows .editorRow:last").find('.year').attr('id', YearReceived).attr('name', 'EducationalBackGround[' + count + '].YearReceived');
    return false;
});