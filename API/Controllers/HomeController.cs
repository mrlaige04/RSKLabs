using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController, Route("api")]
public class HomeController : Controller
{
    [HttpPost, Route("file")]
    public async Task<IActionResult> File(IFormFile file)
    {
        using var sr = new StreamReader(file.OpenReadStream());
        var content = await sr.ReadToEndAsync();
        var lines = content.Replace("\r\n", "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var solution = new Solution(lines);
        solution.CalculateTechGroups();
        Result result = solution.GetResult();

        return Ok(result);
    }
}
