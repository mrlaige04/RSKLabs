namespace API.Models;

public class MyGroup
{
    public IEnumerable<Set> Sets { get; set; }

    public IEnumerable<string> Operations => Sets.SelectMany(set => set.Operations).DistinctBy(s => s.ToLower());
}
