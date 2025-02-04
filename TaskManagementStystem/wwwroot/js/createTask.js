$(document).ready(function () {
    $("#taskForm").submit(function (e) {
        e.preventDefault();

        let task = {
            title: $("#title").val(),
            description: $("#description").val(),
            status: $("#status").val(),
            dueDate: $("#date").val(),
        };

        $.ajax({
            url: "/api/TaskApi/CreateTask",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(task),
            success: function () {
                window.location.href = "/Task/Index";
            }
        });
    });
});
