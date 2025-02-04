using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementStystem.Interfaces;

namespace TaskManagementStystem.Controllers
{
    public class ReportController : Controller
    {
        private readonly ITaskRepository _taskRepository;

        public ReportController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IActionResult> ExportReportPdf()
        {
            var tasks = await _taskRepository.GetAllTasksAsync();

            int total = tasks.Count();
            int pending = tasks.Count(t => t.Status == "Pending");
            int inProgress = tasks.Count(t => t.Status == "In Progress");
            int completed = tasks.Count(t => t.Status == "Completed");
            double completedPercentage = total > 0 ? ((double)completed / total) * 100 : 0;

            using (var memoryStream = new MemoryStream())
            {
                var document = new Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                var titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
                var titleParagraph = new Paragraph("Task Status Report", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20
                };
                document.Add(titleParagraph);

                PdfPTable table = new PdfPTable(2)
                {
                    WidthPercentage = 80,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };

                table.AddCell(new PdfPCell(new Phrase("Metric")) { BackgroundColor = BaseColor.LightGray });
                table.AddCell(new PdfPCell(new Phrase("Value")) { BackgroundColor = BaseColor.LightGray });

                table.AddCell("Total Tasks");
                table.AddCell(total.ToString());

                table.AddCell("Pending Tasks");
                table.AddCell(pending.ToString());

                table.AddCell("In Progress Tasks");
                table.AddCell(inProgress.ToString());

                table.AddCell("Completed Tasks");
                table.AddCell(completed.ToString());

                table.AddCell("Percentage Completed");
                table.AddCell(completedPercentage.ToString("N2") + "%");

                document.Add(table);

                document.Close();

                return File(memoryStream.ToArray(), "application/pdf", "TaskReport.pdf");
            }
        }
    }
}
