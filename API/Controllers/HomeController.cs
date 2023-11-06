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
        var result = solution.GetResult();

        var sets = solution.GetSets();

        var intGroups = result.GroupIterations.Select(g => g.Group);

        var groups = new List<Group>();

        int i = 0;
        foreach (var gr in intGroups)
        {
            groups.Add(new Group() { Sets = sets.Where(s => gr.Contains(s.Code)), Code = i });
            i++;
        }

        var clarification = new Clarification()
        {
            Sets = sets,
            Groups = groups.OrderByDescending(m => m.Operations.Count()),
        };

        result.ClarifiedGroups = clarification.ClarifyGroups();

        return Ok(result);
    }

    /*[HttpPost, Route("model")]
    public async Task<IActionResult> Model()
    {
        
    }*/
}
