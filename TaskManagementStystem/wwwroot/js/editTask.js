$(document).ready(function () {
    let params = new URLSearchParams(window.location.search);
    let taskId = params.get("id");

    $.ajax({
        url: `/api/TaskApi/GetTaskById/${taskId}`,
        type: "GET",
        success: function (task) {
            $("#taskId").val(task.id);
            $("#title").val(task.title);
            $("#description").val(task.description);
            if (task.dueDate) {
                let dateObj = new Date(task.dueDate);
                let formattedDate = dateObj.toISOString().split('T')[0];
                $("#date").val(formattedDate);
            } else {
                $("#date").val("");
            }

            $("#status").val(task.status);
        }
    });

    $("#editTaskForm").submit(function (e) {
        e.preventDefault();

        let updatedTask = {
            id: $("#taskId").val(),
            title: $("#title").val(),
            description: $("#description").val(),
            dueDate: $("#date").val(),
            status: $("#status").val()
        };

        $.ajax({
            url: `/api/TaskApi/UpdateTask/${taskId}`,
            type: "PUT",
            contentType: "application/json",
            data: JSON.stringify(updatedTask),
            success: function () {
                window.location.href = "/Task/Index";
            }
        });
    });
});
