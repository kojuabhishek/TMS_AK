using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class LogApiController : ControllerBase
{
    private readonly LogRepository _logRepository;

    public LogApiController(LogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    [HttpGet("GetLogs")]
    public async Task<IActionResult> GetLogs(int page = 1, int pageSize = 10)
    {
        var (logs, totalRecords) = await _logRepository.GetLogsAsync(page, pageSize);
        return Ok(new { data = logs, totalRecords });
    }
}
