namespace API.Models;

public record GroupIteration(int[][] Matrix, IEnumerable<int> Group, int MaxItem, IList<Coord> MaxItemIndexes);
public record Coord(int row, int col);
