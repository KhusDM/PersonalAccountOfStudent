// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $("#ClassId").on("change", function () {
        $list = $("#StudentId");
        $.ajax({
            url: "GetStudents",
            type: "GET",
            data: { id: $("#ClassId").val() },
            traditional: true,
            success: function (result) {
                $list.empty();
                $.each(result, function (i, item) {
                    $list.append('<option value="' + item.id + '"> ' + item.fio + ' </option>');
                });
            },
            error: function () {
                alert("Something went wrong...");
            }
        });
    });
});

$('input[type=file]').bootstrapFileInput();
$('.file-inputs').bootstrapFileInput();