const string inputPath = @"..\..\..\input.txt";

int resultFirst = -1;
int resultSecond = -1;

var treeRows = new List<List<int>>();
var treeColumns = new List<List<int>>();
var visibleTrees = new List<int>();
var visibleString = string.Join(' ', visibleTrees);

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
    treeRows.Add(treeRow);
}

for (int i = 0; i < treeRows.Count; i++)
{
    var treeColumn = new List<int>();
    foreach (var treeRow in treeRows)
    {
        treeColumn.Add(treeRow[i]);
    }
    treeColumns.Add(treeColumn);
}

for (int i = 0; i < treeRows.Count; i++)
{
    for (int j = 0; j < treeColumns.Count; j++)
    {
        var theTree = treeRows[i][j];
        var treeRow = treeRows[i];
        var treeColumn = treeColumns[j];
        var treeRowString = string.Join("", treeRow);
        var Sub = treeRow.GetRange(0, j);
        var subString = string.Join("", Sub);
        if (Sub.All(x => x < theTree))
        {
            visibleTrees.Add(theTree);
            resultSecond = Math.Max(resultSecond, GetScenicScore(theTree, treeRow, treeColumn, j, i));
            visibleString = string.Join(' ', visibleTrees);
            continue;
        }
        if (j + 1 <= treeRows.Count)
        {
            Sub = treeRow.GetRange(j + 1, treeRows.Count - (j + 1));
            subString = string.Join("", Sub);
            if (Sub.All(x => x < theTree))
            {
                visibleTrees.Add(theTree);
                resultSecond = Math.Max(resultSecond, GetScenicScore(theTree, treeRow, treeColumn, j, i));
                visibleString = string.Join(' ', visibleTrees);
                continue;
            }
        }

        var treeColumnString = string.Join("", treeColumn);
        var SubColumn = treeColumn.GetRange(0, i);
        var subColumnString = string.Join("", SubColumn);
        if (SubColumn.All(x => x < theTree))
        {
            visibleTrees.Add(theTree);
            resultSecond = Math.Max(resultSecond, GetScenicScore(theTree, treeRow, treeColumn, j, i));
            visibleString = string.Join(' ', visibleTrees);
            continue;
        }
        if (i + 1 <= treeColumns.Count)
        {
            SubColumn = treeColumn.GetRange(i + 1, treeColumns.Count - (i + 1));
            subColumnString = string.Join("", SubColumn);
            if (SubColumn.All(x => x < theTree))
            {
                visibleTrees.Add(theTree);
                resultSecond = Math.Max(resultSecond, GetScenicScore(theTree, treeRow, treeColumn, j, i));
                visibleString = string.Join(' ', visibleTrees);
            }
        }
    }
}

resultFirst = visibleTrees.Count;
Console.WriteLine($"Part one: {resultFirst}\nPart two: {resultSecond}\n"); ;


static int GetScenicScore(int theTree, List<int> row, List<int> column, int rowPosition, int columnPosition)
{
    int left = 0;
    int right = 0;
    int up = 0;
    int down = 0;

    for (int i = rowPosition; i >= 0; i--)
    {
        if (i == rowPosition) continue;
        left++;
        if (theTree <= row[i])
        {
            break;
        }
    }

    for (int i = rowPosition; i < row.Count; i++)
    {
        if (i == rowPosition) continue;
        right++;
        if (theTree <= row[i])
        {
            break;
        }
    }

    for (int i = columnPosition; i >= 0; i--)
    {
        if (i == columnPosition) continue;
        up++;
        if (theTree <= column[i])
        {
            break;
        }
    }

    for (int i = columnPosition; i < column.Count; i++)
    {
        if (i == columnPosition) continue;
        down++;
        if (theTree <= column[i])
        {
            break;
        }
    }

    return left * right * up * down;
}