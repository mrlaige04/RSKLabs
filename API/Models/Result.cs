namespace API.Models;

public class Result
{
    public int[][] SimilarityMatrix { get; set; }
    public string[] UniqueOperations { get; set; }
    public string[] Sets { get; set; }
    public IList<GroupIteration> GroupIterations { get; set; }
    public IEnumerable<MyGroup> Clarified { get; set; }
    public IList<GraphData> GraphDatas { get; set; }
}