namespace API.Models
{
    public class Solution
    {
        private readonly string[] Sets;
        private readonly string[] UniqueOperation;
        private int[,] SimilarityMatrix;
        private List<GroupIteration> GroupIterations;

        public Solution(string[] sets)
        {
            Sets = sets;
            int setsLength = Sets.Length;
            SimilarityMatrix = new int[setsLength, setsLength];
            UniqueOperation = ExtractUniqueOperations(sets);
            GroupIterations = new List<GroupIteration>();
        }

        public List<Set> GetSets()
        {
            var sets = new List<Set>();

            for (int i = 0; i < Sets.Length; i++)
            {
                var set = new Set()
                {
                    Code = i + 1,
                    Operations = Sets[i].Split(' ')
                };
                sets.Add(set);
            }

            return sets;
        }

        public void CalculateTechGroups()
        {
            SimilarityMatrix = CalculateSimilarityMatrix();

            while (true)
            {
                var maxItemIndex = GetMaxElementIndexInLowerTriangle(SimilarityMatrix);
                int maxItem = SimilarityMatrix[maxItemIndex.Item1, maxItemIndex.Item2];

                if (maxItem == -1)
                {
                    break;
                }

                var similarItems = GetSimilarMaxItems(maxItemIndex, maxItem);

                foreach (var item in similarItems)
                {
                    FillWithMinusOne(SimilarityMatrix, item.row);
                    FillWithMinusOne(SimilarityMatrix, item.column);
                }

                var group = similarItems.Select(x => x.row + 1).Union(similarItems.Select(x => x.column + 1)).Distinct();
                var coords = similarItems.Select(si => new Coord(si.row + 1, si.column + 1)).ToList();
                GroupIterations.Add(new GroupIteration(ConvertToJaggedArray(SimilarityMatrix), group, maxItem, coords));
            }

            GroupIterations = GroupIterations.DistinctBy(x => x.Group).ToList();
        }

        public Result GetResult()
        {
            var matrix = CalculateSimilarityMatrix();
            var jagged = ConvertToJaggedArray(matrix);
            var clarified = ClarifyGroups();
            List<GraphData> graphsDatas = GetGraphDatas(clarified);

            return new Result()
            {
                Sets = Sets,
                UniqueOperations = UniqueOperation,
                SimilarityMatrix = jagged,
                GroupIterations = GroupIterations,
                Clarified = clarified,
                GraphDatas = graphsDatas
            };
        }

        private List<GraphData> GetGraphDatas(IEnumerable<MyGroup> clarifyGroup)
        {
            if (clarifyGroup == null)
            {
                throw new ArgumentNullException(nameof(clarifyGroup));
            }

            List<GraphData> graphDatas = new List<GraphData>();

            for (int j = 0; j < clarifyGroup.Count(); j++)
            {
                List<GraphLink> graphLinks = new List<GraphLink>();

                foreach (var set in clarifyGroup.ElementAt(j).Sets)
                {
                    for (int i = 0; i < set.Operations.Count() - 1; i++)
                    {
                        if (!graphLinks.Any(x => x.Target == set.Operations.ElementAt(i + 1) && x.Source == set.Operations.ElementAt(i)))
                        {
                            graphLinks.Add(new GraphLink() { Source = set.Operations.ElementAt(i), Target = set.Operations.ElementAt(i + 1) });
                        }
                    }
                }

                List<GraphNode> graphNodes = clarifyGroup.ElementAt(j).Operations.Select(x => new GraphNode() { Id = x, Label = x }).ToList();
                GraphData graphData = new GraphData()
                {
                    Nodes = graphNodes,
                    Links = graphLinks,
                    GroupNumber = j
                };

                graphDatas.Add(graphData);
            }

            return graphDatas;
        }

        private string[] ExtractUniqueOperations(string[] sets)
        {
            return sets
                .SelectMany(line => line.Replace("\r", " ").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                .Select(x => x.ToUpperInvariant())
                .Distinct()
                .ToArray();
        }

        private List<(int row, int column)> GetSimilarMaxItems((int row, int column) maxIndex, int maxItem)
        {
            var similarItems = new List<(int row, int column)> { maxIndex };
            SimilarityMatrix[maxIndex.row, maxIndex.column] = -1;

            for (var i = 0; i < SimilarityMatrix.GetLength(0); i++)
            {
                if (SimilarityMatrix[maxIndex.row, i] == maxItem)
                {
                    similarItems.AddRange(GetSimilarMaxItems((maxIndex.row, i), maxItem));
                }
            }

            for (var j = 0; j < SimilarityMatrix.GetLength(1); j++)
            {
                if (SimilarityMatrix[j, maxIndex.column] == maxItem)
                {
                    similarItems.AddRange(GetSimilarMaxItems((j, maxIndex.column), maxItem));
                }
            }

            return similarItems;
        }

        private IEnumerable<MyGroup> ClarifyGroups()
        {
            var mainSets = GetSets();
            var groups = GroupIterations.Select(gi =>
            {
                var sets = new List<Set>();

                foreach (var set in gi.Group)
                {
                    sets.Add(mainSets[set - 1]);
                }
                return new MyGroup() { Sets = sets };
            }).OrderByDescending(g => g.Operations.Count()).ToList();


            for (var i = 0; i < groups.Count - 1; i++)
            {
                for (var j = i + 1; j < groups.Count; j++)
                {
                    if (IsGroupAbsorbsGroup(groups[i], groups[j]))
                    {
                        var mainGroupSets = groups[i].Sets.ToList();
                        mainGroupSets.AddRange(groups[j].Sets);

                        groups[i].Sets = mainGroupSets;
                        groups.Remove(groups[j]);
                        j--;
                    }
                    else
                    {
                        foreach (var set in groups[j].Sets)
                        {
                            if (IsGroupsAbsorbsSet(groups[i], set))
                            {
                                var mainGroupsSets = groups[i].Sets.ToList();
                                mainGroupsSets.Add(set);
                                groups[i].Sets = mainGroupsSets;

                                var otherGroupSets = groups[j].Sets.ToList();
                                otherGroupSets.Remove(set);
                                groups[j].Sets = otherGroupSets;
                            }
                        }
                    }
                }
            }

            groups.ForEach(g =>
            {
                g.Sets = g.Sets.OrderBy(s => s.Code);
            });

            return groups.OrderByDescending(g => g.Operations.Count());
        }

        private static bool IsGroupAbsorbsGroup(MyGroup main, MyGroup other)
        {

            return other.Operations.All(op => main.Operations.Contains(op));
        }

        private static bool IsGroupsAbsorbsSet(MyGroup group, Set set)
        {
            return set.Operations.All(op => group.Operations.Contains(op));
        }

        private (int, int) GetMaxElementIndexInLowerTriangle(int[,] matrix)
        {
            int maxElement = int.MinValue;
            int maxRowIndex = -1;
            int maxColIndex = -1;
            int n = matrix.GetLength(0);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    if (matrix[i, j] > maxElement)
                    {
                        maxElement = matrix[i, j];
                        maxRowIndex = i;
                        maxColIndex = j;
                    }
                }
            }

            return (maxRowIndex, maxColIndex);
        }

        private void FillWithMinusOne(int[,] matrix, int index)
        {
            int n = matrix.GetLength(0);

            for (int i = 0; i < n; i++)
            {
                matrix[index, i] = -1;
                matrix[i, index] = -1;
            }
        }

        private int[,] CalculateSimilarityMatrix()
        {
            int allOps = UniqueOperation.Length;
            for (var i = 0; i < Sets.Length; i++)
            {
                for (var j = i; j < Sets.Length; j++)
                {
                    var setOne = Sets[i].Split(new[] { ' ' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    var setTwo = Sets[j].Split(new[] { ' ' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    var similarity = allOps - SetsSimilarity(setOne, setTwo);

                    SimilarityMatrix[i, j] = similarity;
                    SimilarityMatrix[j, i] = similarity;

                    SimilarityMatrix[i, i] = 0;
                }
            }

            return SimilarityMatrix;
        }

        private int SetsSimilarity(string[] setOne, string[] setTwo)
        {
            var dissimilarOne = setOne.Except(setTwo, StringComparer.InvariantCultureIgnoreCase);
            var dissimilarTwo = setTwo.Except(setOne, StringComparer.InvariantCultureIgnoreCase);

            return dissimilarOne.Count() + dissimilarTwo.Count();
        }

        private T[][] ConvertToJaggedArray<T>(T[,] multiArray)
        {
            int rows = multiArray.GetLength(0);
            int cols = multiArray.GetLength(1);

            T[][] jaggedArray = new T[rows][];

            for (int i = 0; i < rows; i++)
            {
                jaggedArray[i] = new T[cols];
                for (int j = 0; j < cols; j++)
                {
                    jaggedArray[i][j] = multiArray[i, j];
                }
            }

            return jaggedArray;
        }
    }
}