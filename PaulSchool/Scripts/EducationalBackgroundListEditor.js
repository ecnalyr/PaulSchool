$(document).ready(function() {
    $("#addItem").click(function() {
        $.ajax({
            url: this.href,
            cache: false,
            success: function(data) {
                $("#editorRows").append(data);
                return false;
            }
        });
        return false;
    });

    $("a.deleteRow").live("click", function() {
        $(this).parents("div.editorRow:first").remove();
        return false;
    });
});