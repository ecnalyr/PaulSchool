var userId;
var attendanceDay;
var isPresent;
var updateAttendanceUrl;
var absentText;
var presentText;
var courseId;
$(document).ready(function (event) {
    $('.attendance').live('click', function () {
        userId = $(this).parents('tr').attr('id');
        if (userId != '') {
            attendanceDay = $(this).attr('id');
            isPresent = $.trim($(this).find('span').text().toLowerCase());
            courseId = $(this).parents('tbody').attr('id');
            $(this).find('span').addClass('currentClass');
            if (isPresent == absentText) {
                UpdateAttendance(1);
            } else {
                UpdateAttendance(0);
            }
        } else {
            event.preventDefault();
        }
    });
});
function UpdateAttendance(present) {
    url = updateAttendanceUrl;
    $.ajax({
        type: "POST",
        url: url,
        data: "userId=" + userId + "&attendanceDay=" + attendanceDay + "&courseId=" + courseId + "&present=" + present,
        success: function (data) {
            if (isPresent == absentText) {
                $('#' + userId).find('.currentClass').text(presentText).removeAttr('class');
            } else {
                $('#' + userId).find('.currentClass').text(absentText).removeAttr('class');
            }
            return true;
        }
    });
}