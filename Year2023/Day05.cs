using Common;

namespace Year2023;

internal class Day05 : Day
{
    protected override int TestSolutionOne { get; set; } = 35;
    protected override int TestSolutionTwo { get; set; } = 46;

    private readonly List<List<long>> Seed2Soil = new();
    private readonly List<List<long>> Soil2Fertilizer = new();
    private readonly List<List<long>> Fertilizer2Water = new();
    private readonly List<List<long>> Water2Light = new();
    private readonly List<List<long>> Light2Temperature = new();
    private readonly List<List<long>> Temperature2Humidity = new();
    private readonly List<List<long>> Humidity2Location = new();


    protected override long SolveOne(string input)
    {
        using var reader = new StreamReader(input);
        var seeds = reader.ReadLine()?[7..].Split(' ').Select(value => long.Parse(value)).ToList();
        if (seeds == null) throw new NullReferenceException(nameof(seeds));

        Console.WriteLine("Reading seed-to-soil map...");
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrEmpty(line)) continue;
            if (line == "seed-to-soil map:") continue;
            if (line == "soil-to-fertilizer map:") break;
            Seed2Soil.Add(line.Split(' ').Select(value => long.Parse(value)).ToList());
        }

        Console.WriteLine("Reading soil-to-fertilizer map...");
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrEmpty(line)) continue;
            if (line == "fertilizer-to-water map:") break;
            Soil2Fertilizer.Add(line.Split(' ').Select(value => long.Parse(value)).ToList());

        }
        Console.WriteLine("Reading fertilizer-to-water map...");
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrEmpty(line)) continue;
            if (line == "water-to-light map:") break;
            Fertilizer2Water.Add(line.Split(' ').Select(value => long.Parse(value)).ToList());

        }
        Console.WriteLine("Reading water-to-light map...");
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrEmpty(line)) continue;
            if (line == "light-to-temperature map:") break;
            Water2Light.Add(line.Split(' ').Select(value => long.Parse(value)).ToList());

        }
        Console.WriteLine("Reading light-to-temperature map...");
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrEmpty(line)) continue;
            if (line == "temperature-to-humidity map:") break;
            Light2Temperature.Add(line.Split(' ').Select(value => long.Parse(value)).ToList());

        }
        Console.WriteLine("Reading temperature-to-humidity map...");
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrEmpty(line)) continue;
            if (line == "humidity-to-location map:") break;
            Temperature2Humidity.Add(line.Split(' ').Select(value => long.Parse(value)).ToList());

        }
        Console.WriteLine("Reading humidity-to-location map...");
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrEmpty(line)) continue;
            Humidity2Location.Add(line.Split(' ').Select(value => long.Parse(value)).ToList());

        }

        return seeds.Select(seed => GetLocation(seed)).Min();
    }

    protected override long SolveTwo(string input)
    {
        Seed2Soil.Clear();
        Soil2Fertilizer.Clear();
        Fertilizer2Water.Clear();
        Water2Light.Clear();
        Light2Temperature.Clear();
        Temperature2Humidity.Clear();
        Humidity2Location.Clear();

        long output = long.MaxValue;
        using var reader = new StreamReader(input);
        var seeds = new List<long>();
        var seedLine = (reader.ReadLine()?[7..].Split(' ').Select(value => long.Parse(value)).ToList()) ?? throw new NullReferenceException();

        Console.WriteLine("Reading seed-to-soil map...");
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrEmpty(line)) continue;
            if (line == "seed-to-soil map:") continue;
            if (line == "soil-to-fertilizer map:") break;
            Seed2Soil.Add(line.Split(' ').Select(value => long.Parse(value)).ToList());
        }

        Console.WriteLine("Reading soil-to-fertilizer map...");
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrEmpty(line)) continue;
            if (line == "fertilizer-to-water map:") break;
            Soil2Fertilizer.Add(line.Split(' ').Select(value => long.Parse(value)).ToList());

        }
        Console.WriteLine("Reading fertilizer-to-water map...");
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrEmpty(line)) continue;
            if (line == "water-to-light map:") break;
            Fertilizer2Water.Add(line.Split(' ').Select(value => long.Parse(value)).ToList());

        }
        Console.WriteLine("Reading water-to-light map...");
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrEmpty(line)) continue;
            if (line == "light-to-temperature map:") break;
            Water2Light.Add(line.Split(' ').Select(value => long.Parse(value)).ToList());

        }
        Console.WriteLine("Reading light-to-temperature map...");
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrEmpty(line)) continue;
            if (line == "temperature-to-humidity map:") break;
            Light2Temperature.Add(line.Split(' ').Select(value => long.Parse(value)).ToList());

        }
        Console.WriteLine("Reading temperature-to-humidity map...");
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrEmpty(line)) continue;
            if (line == "humidity-to-location map:") break;
            Temperature2Humidity.Add(line.Split(' ').Select(value => long.Parse(value)).ToList());

        }
        Console.WriteLine("Reading humidity-to-location map...");
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrEmpty(line)) continue;
            Humidity2Location.Add(line.Split(' ').Select(value => long.Parse(value)).ToList());

        }

        long num = 0;
        for (int i = 0; i < seedLine.Count / 2; i++)
        {
            num += seedLine[i * 2 + 1];
        }
        Console.WriteLine($"Estimated number of seeds: {num}");
        var counter = 0;
        for (int i = 0; i < seedLine.Count / 2; i++)
        {
            for (int j = 0; j < seedLine[i * 2 + 1]; j++)
            {
                output = Math.Min(output, GetLocation(seedLine[i * 2] + j));
                if (counter % 1000000 == 0)
                {
                    Console.WriteLine($"{num - counter:N3} seeds left");
                }
                counter++;
            }
        }

        return output;
    }

    private long GetLocation(long seed)
    {
        foreach (var line in Seed2Soil)
        {
            if (seed >= line[1] && seed <= line[1] + line[2] - 1)
            {
                seed += line[0] - line[1];
                break;
            }
        }
        foreach (var line in Soil2Fertilizer)
        {
            if (seed >= line[1] && seed <= line[1] + line[2] - 1)
            {
                seed += line[0] - line[1];
                break;
            }
        }
        foreach (var line in Fertilizer2Water)
        {
            if (seed >= line[1] && seed <= line[1] + line[2] - 1)
            {
                seed += line[0] - line[1];
                break;
            }
        }
        foreach (var line in Water2Light)
        {
            if (seed >= line[1] && seed <= line[1] + line[2] - 1)
            {
                seed += line[0] - line[1];
                break;
            }
        }
        foreach (var line in Light2Temperature)
        {
            if (seed >= line[1] && seed <= line[1] + line[2] - 1)
            {
                seed += line[0] - line[1];
                break;
            }
        }
        foreach (var line in Temperature2Humidity)
        {
            if (seed >= line[1] && seed <= line[1] + line[2] - 1)
            {
                seed += line[0] - line[1];
                break;
            }
        }
        foreach (var line in Humidity2Location)
        {
            if (seed >= line[1] && seed <= line[1] + line[2] - 1)
            {
                seed += line[0] - line[1];
                break;
            }
        }
        return seed;
    }
}
