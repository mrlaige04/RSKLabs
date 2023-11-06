namespace API.Models
{
    public class GraphData
    {
        public int GroupNumber { get; set; }
        public List<GraphNode> Nodes { get; set; }
        public List<GraphLink> Links { get; set; }
    }
}
