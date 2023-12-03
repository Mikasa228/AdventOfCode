namespace Common;

public static class PrepScript
{
    public static void Run(string year, string day)
    {
        var path = $"../../../../Year{year}/Day{day}/";

        var fileNames = new List<string>()
        {
            "input1.txt",
            "input2.txt",
            "testInput1.txt",
            "testInput2.txt"
        };

        Directory.CreateDirectory(path);

        foreach (var fileName in fileNames)
        {
            var fullPath = Path.Combine(path, fileName);
            Console.WriteLine($"Creating {fileName}...");
            File.Create(fullPath).Dispose();
        }

    }
}
