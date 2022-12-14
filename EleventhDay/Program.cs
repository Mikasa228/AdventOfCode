namespace EleventhDay;

class Program
{
    const string inputPath = @"..\..\..\input.txt";

    static long resultFirst = -1;
    static long resultSecond = -1;
    static List<Monkey> monkeys = new();
    static int core = 1;

    static void Main()
    {
        int counter = 0;

        Dictionary<int, List<long>> storage = new();

        using var reader = new StreamReader(inputPath);
        while (!reader.EndOfStream)
        {
            var currentMonkey = new Monkey(counter);
            monkeys.Add(currentMonkey);
            storage[counter] = new();

            string monkeyLine = reader.ReadLine() ?? "";

            string itemsLine = reader.ReadLine()?.Trim() ?? "";
            var items = itemsLine.Split(": ")[1].Split(", ").Select(item => long.Parse(item));
            foreach (var item in items)
            {
                storage[counter].Add(item);
            }

            //string operationLine = reader.ReadLine()?.Trim() ?? "";
            //var operationParts = operationLine.Split(" ");
            //currentMonkey.Action.Add(operationParts[4]);
            //currentMonkey.Action.Add(operationParts[5]);

            //string testLine = reader.ReadLine()?.Trim() ?? "";
            //currentMonkey.TestValue = int.Parse(testLine.Split(" ")[3]);
            //core *= currentMonkey.TestValue;

            //string trueLine = reader.ReadLine()?.Trim() ?? "";
            //currentMonkey.MonkeyOne = int.Parse(trueLine.Split(" ")[5]);

            //string falseLine = reader.ReadLine()?.Trim() ?? "";
            //currentMonkey.MonkeyTwo = int.Parse(falseLine.Split(" ")[5]);

            string operationLine = reader.ReadLine()?.Trim() ?? "";
            var operationParts = operationLine.Split(" ");
            currentMonkey.Action.Add(operationParts[4]);
            currentMonkey.Action.Add(operationParts[5]);

            string testLine = reader.ReadLine()?.Trim() ?? "";
            currentMonkey.TestValue = int.Parse(ReadItemFromLine(reader.ReadLine(), 3));
            core *= currentMonkey.TestValue;

            string trueLine = reader.ReadLine()?.Trim() ?? "";
            currentMonkey.MonkeyOne = int.Parse(trueLine.Split(" ")[5]);

            string falseLine = reader.ReadLine()?.Trim() ?? "";
            currentMonkey.MonkeyTwo = int.Parse(falseLine.Split(" ")[5]);

            string? emptyLine = reader.ReadLine();
            counter++;
        }

        foreach (var pair in storage)
        {
            foreach (var item in pair.Value)
            {
                monkeys[pair.Key].Items.Enqueue(item);
                for (int i = 0; i < 10000; i++)
                {
                    for (int j = 0; j < monkeys.Count; j++)
                    {
                        monkeys[j].PlayTurn();
                    }

                }
                foreach (var monkey in monkeys)
                {
                    monkey.Items = new();
                }
            }
        }


        var results = monkeys.Select(monkey => monkey.InspectionCounter).ToList();
        results.Sort();
        results.Reverse();

        resultFirst = results[0] * results[1];

        Console.WriteLine($"Part one: {resultFirst}\nPart two: {resultSecond}");

        static string ReadItemFromLine(string? line, int index)
        {
            if (line is null) return "";

            line = line.Trim();
            return line.Split(" ")[index];
        }
    }

    class Monkey
    {
        public long InspectionCounter { get; set; } = 0;
        public long Id { get; set; }
        public Queue<long> Items { get; set; } = new();
        public int TestValue { get; set; } = 0;
        public int MonkeyOne { get; set; } = 0;
        public int MonkeyTwo { get; set; } = 0;
        public List<string> Action { get; set; } = new();

        public Monkey(long id)
        {
            Id = id;
        }

        public void PlayTurn()
        {
            while (Items.Count > 0)
            {
                InspectionCounter++;
                var Item = Items.Dequeue();

                Item = PerformOperation(Item);

                Item %= core;

                if (Item % TestValue == 0)
                {
                    monkeys[MonkeyOne].Items.Enqueue(Item);
                }
                else
                {
                    monkeys[MonkeyTwo].Items.Enqueue(Item);
                }
            }
        }

        public long PerformOperation(long item)
        {
            switch (Action[1])
            {
                case "old":
                    item *= item;
                    break;
                default:
                    switch (Action[0])
                    {
                        case "*":
                            item *= long.Parse(Action[1]); break;
                        case "+":
                            item += long.Parse(Action[1]) /** stash*/; break;
                        default: throw new ArgumentException();
                    }
                    break;
            }

            return item;
        }
    }
}
