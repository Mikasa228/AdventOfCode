const string inputPath = @"..\..\..\input.txt";

int resultFirst = -1;
int resultSecond = -1;

var treeMatrix = new List<List<int>>();
var visibleTrees = new List<int>();

using var reader = new StreamReader(inputPath);
while (!reader.EndOfStream)
{
    string? fullString = reader.ReadLine();
    if (fullString is null) throw new NullReferenceException();

    var treeRow = new List<int>();
    foreach (var tree in fullString)
    {
        treeRow.Add(int.Parse(tree.ToString()));
    }
    treeMatrix.Add(treeRow);
}

for (int rowIndex = 0; rowIndex < treeMatrix.Count; rowIndex++)
{
    for (int columnIndex = 0; columnIndex < treeMatrix.Count; columnIndex++)
    {
        var theTree = treeMatrix[rowIndex][columnIndex];

        foreach (var subList in treeMatrix.GetDirections(rowIndex, columnIndex))
        {
            if (subList.Any(tree => tree >= theTree)) continue;

            visibleTrees.Add(theTree);
            resultSecond = Math.Max(resultSecond, treeMatrix.GetTotalScenicScore(rowIndex, columnIndex));
            break;
        }
    }
}

resultFirst = visibleTrees.Count;
Console.WriteLine($"Part one: {resultFirst}\nPart two: {resultSecond}\n");


static class Extensions
{
    public static List<List<int>> GetDirections(this List<List<int>> matrix, int rowIndex, int columnIndex)
    {
        var row = matrix[rowIndex];
        var column = matrix.Select(row => row[columnIndex]).ToList();

        return new List<List<int>>()
        {
            row.GetRange(0, columnIndex).Reversed(),
            row.GetRange(columnIndex + 1, matrix.Count - (columnIndex + 1)),
            column.GetRange(0, rowIndex).Reversed(),
            column.GetRange(rowIndex + 1, matrix.Count - (rowIndex + 1))
        };
    }

    public static int GetTotalScenicScore(this List<List<int>> matrix, int rowIndex, int columnIndex)
    {
        var theTree = matrix[rowIndex][columnIndex];

        var scenicScores = matrix.GetDirections(rowIndex, columnIndex)
                                 .Select(direction => direction.GetScenicScore(theTree));

        return scenicScores.Aggregate(1, (seed, value) => seed * value);
    }

    private static int GetScenicScore(this List<int> list, int theTree)
    {
        int scenicScore = 0;

        foreach (var tree in list)
        {
            scenicScore++;
            if (theTree <= tree) break;
        }

        return scenicScore;
    }

    public static List<T> Reversed<T>(this List<T> list)
    {
        list.Reverse();
        return list;
    }
}