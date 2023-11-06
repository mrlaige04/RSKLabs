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
        }
        public Result GetResult()
        {
            var matrix = CalculateSimilarityMatrix();
            var jagged = ConvertToJaggedArray(matrix);
            return new Result()
            {
                Sets = Sets,
                UniqueOperations = UniqueOperation,
                SimilarityMatrix = jagged,
                GroupIterations = GroupIterations,
            };
        }

        private string[] ExtractUniqueOperations(string[] sets)
        {
            return sets
                .SelectMany(line => line.Replace("\r", " ").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                .Select(x => x.ToLowerInvariant())
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