$(document).ready(function () {
    loadLogs(1);

    $("#prevPage").click(function () {
        var currentPage = parseInt($("#currentPage").text());
        if (currentPage > 1) {
            loadLogs(currentPage - 1);
        }
    });

    $("#nextPage").click(function () {
        var currentPage = parseInt($("#currentPage").text());
        var totalPages = parseInt($("#totalPages").text());
        if (currentPage < totalPages) {
            loadLogs(currentPage + 1);
        }
    });
});

function loadLogs(page) {
    $.ajax({
        url: `/api/LogApi/GetLogs?page=${page}&pageSize=10`,
        type: "GET",
        dataType: "json",
        success: function (response) {
            var rows = "";
            $.each(response.data, function (index, log) {
                rows += `<tr>
                    <td>${new Date(log.timeStamp).toLocaleString()}</td>
                    <td>${log.message}</td>
                    <td>${log.messageTemplate}</td>
                </tr>`;
            });

            $("#logTable").html(rows);
            $("#currentPage").text(page);
            $("#totalPages").text(Math.ceil(response.totalRecords / 10));
        },
        error: function () {
            alert("Failed to load logs.");
        }
    });
}
