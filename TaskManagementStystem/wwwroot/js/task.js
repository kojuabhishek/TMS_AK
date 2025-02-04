$(document).ready(function () {
    let currentPage = 1;
    let pageSize = 5;

    loadTasks();

    $("#searchBox").on("keyup", function () {
        loadTasks(1, $(this).val());
    });

    $("#generateReport").on("click", function () {
        window.location.href = "/Report/ExportReportPdf";
    });

    $("#taskForm").submit(function (e) {
        e.preventDefault();

        let task = {
            title: $("#title").val(),
            description: $("#description").val(),
            status: $("#status").val()
        };

        $.ajax({
            url: "/api/TaskApi",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(task),
            success: function () {
                loadTasks();
            },
            error: function () {
                alert("Failed to add task.");
            }
        });
    });

    function loadTasks(page = 1, search = "") {
        $.ajax({
            url: "/api/TaskApi/GetAllTasks",
            type: "GET",
            data: { search: search, page: page, pageSize: pageSize },
            dataType: "json",
            success: function (response) {
                $("#taskTable").empty();
                response.data.forEach(task => {
                    $("#taskTable").append(`
                        <tr>
                            <td>${task.title}</td>
                            <td>${task.status}</td>
                            <td>${task.dueDate ? new Date(task.dueDate).toISOString().split('T')[0] : 'N/A'}</td>
                            <td>
                                <a href="/Task/EditTask?id=${task.id}" class="btn btn-warning btn-sm">Edit</a>
                                <button class="btn btn-danger btn-sm delete-btn" data-id="${task.id}">Delete</button>
                            </td>
                        </tr>
                    `);
                });

                updatePagination(response.totalRecords, page);
            },
            error: function () {
                alert("Failed to load tasks.");
            }
        });
    }

    function updatePagination(totalRecords, currentPage) {
        const totalPages = Math.ceil(totalRecords / pageSize);
        let paginationHTML = '';

        for (let i = 1; i <= totalPages; i++) {
            paginationHTML += `<button class="btn ${i === currentPage ? 'btn-primary' : 'btn-outline-primary'} page-btn" data-page="${i}">${i}</button> `;
        }

        $("#pagination").html(paginationHTML);
    }

    $(document).on("click", ".page-btn", function () {
        currentPage = $(this).data("page");
        loadTasks(currentPage, $("#searchBox").val());
    });

    $(document).on("click", ".delete-btn", function () {
        let id = $(this).data("id");
        if (confirm("Are you sure you want to delete this task?")) {
            $.ajax({
                url: `/api/TaskApi/DeleteTask/${id}`,
                type: "DELETE",
                success: function () {
                    loadTasks();
                },
                error: function () {
                    alert("Failed to delete task.");
                }
            });
        }
    });
});
