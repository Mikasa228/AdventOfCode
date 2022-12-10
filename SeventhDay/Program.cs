namespace SeventhDay;

class Program
{
    static Folder current = new("/");

    static void Main()
    {
        const string inputPath = @"..\..\..\input.txt";

        int resultFirst = -1;
        int resultSecond = -1;

        var root = current;

        using var reader = new StreamReader(inputPath);
        while (!reader.EndOfStream)
        {
            string? fullString = reader.ReadLine();
            if (fullString is null) throw new NullReferenceException();

            if (fullString.StartsWith("$"))
            {
                Interpret(fullString);
            }
            else
            {
                var args = fullString.Split(' ', 2);
                if (args[0] == "dir")
                {
                    current.AddItem(new Folder(args[1], current));
                }
                else
                {
                    current.AddItem(new File(args[1], int.Parse(args[0])));
                }
            }
        }

        //resultFirst = Folder.GetFoldersWithSize(root, 100000).Select(item => item.Size).Sum();
        var folders = Folder.GetFolderTree(root);
        var unuzedSpace = 70000000 - root.Size;
        var requiredSpace = 30000000 - unuzedSpace;

        resultFirst = folders
            .Where(folder => folder.Size <= 100000)
            .Select(item => item.Size).Sum();

        resultSecond = folders
            .Where(folder => folder.Size >= requiredSpace)
            .Select(item => item.Size).Min();

        Console.WriteLine($"Part one: {resultFirst}\nPart two: {resultSecond}");


        static void Interpret(string command)
        {
            command = command[2..];
            if (command == "ls")
            {
                return;
            }
            else if (command.EndsWith(".."))
            {
                current = current.Parent ?? current;
            }
            else
            {
                if (command[3..] == current.Name) return;

                current = current.GetFolder(command[3..]) ?? current;
            }
        }
    }

    abstract class BaseItem
    {
        virtual public int Size { get; set; }
        public string Name { get; set; }

        public BaseItem(string name)
        {
            Name = name;
        }
    }

    class Folder : BaseItem
    {
        private readonly List<BaseItem> _items = new();

        public override int Size { get => _items.Select(folder => folder.Size).Sum(); }
        public Folder? Parent { get; private set; }

        public Folder(string name, Folder? parent = null) : base(name)
        {
            Parent = parent;
        }

        public void AddItem(BaseItem item)
        {
            _items.Add(item);
        }

        public Folder? GetFolder(string name)
        {
            return _items.FirstOrDefault(item => item.Name == name) as Folder;
        }

        public static List<Folder> GetFolderTree(Folder start)
        {
            var outputList = new List<Folder>();

            foreach (var item in start._items)
            {
                if (item is not Folder folder) continue;
                outputList.Add(folder);
                outputList.AddRange(GetFolderTree(folder));
            }

            return outputList;
        }
    }

    class File : BaseItem
    {
        public File(string name, int size) : base(name)
        {
            Size = size;
        }
    }
}
