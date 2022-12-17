

using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace SixteenthDay;

class Program
{
    const string inputPath = @"..\..\..\input.txt";

    static int resultFirst = -1;
    static int resultSecond = -1;

    static List<Valve> valves = new();
    static int minutesLeft = 30;
    static int minutesLeftTwo = 26;

    static void Main()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var currentPoint = new Valve("AA");
        valves.Add(currentPoint);
        var pressureReleased = 0;

        using var reader = new StreamReader(inputPath);
        while (!reader.EndOfStream)
        {
            string fullString = reader.ReadLine();
            if (fullString is null) throw new NullReferenceException();

            var match = Regex.Match(fullString
                , "Valve (?'Name'\\w+) has flow rate=(?'Rate'\\d+); tunnels? leads? to valves? (?'Neighbors'.+)");

            var name = match.Groups["Name"].Value;
            var valve = valves.Find(v => v.Name == name);
            if (valve is null)
            {
                valve = new Valve(name);
                valves.Add(valve);
            }

            var rate = int.Parse(match.Groups["Rate"].Value);
            valve.Rate = rate;

            var neighborNames = match.Groups["Neighbors"].Value.Split(", ").ToList();
            foreach (var neighborName in neighborNames)
            {
                var current = valves.Find(v => v.Name == neighborName);
                if (current is null)
                {
                    current = new Valve(neighborName);
                    valves.Add(current);
                }
                valve.Neighbors.Add(current);
            }
        }

        var unreleased = new List<Valve>();
        //do
        //{
        //    unreleased = valves.Where(v => !v.IsReleased).ToList();
        //    var profits = new Dictionary<Valve, int>();
        //    foreach (var unv in unreleased)
        //    {
        //        var deest = currentPoint.GetShortestDistance(unv.Name, new());
        //        var profit = GetPossibleProfit(deest, unv);
        //        profits[unv] = profit;
        //    }
        //    var best = unreleased.MaxBy(v => GetPossibleProfit(currentPoint.GetShortestDistance(v.Name, new()), v));
        //    var dist = currentPoint.GetShortestDistance(best.Name, new());
        //    minutesLeft -= (dist + 1);
        //    pressureReleased += GetPossibleProfit(dist, best);
        //    best.IsReleased = true;
        //    currentPoint = best;
        //    unreleased.Remove(best);
        //}
        //while (unreleased.Count > 0);

        //resultFirst = GetMaxProfit(currentPoint, valves, 30, 0);
        //resultSecond = SolvingTwo(currentPoint);

        //CalculateBestRoutes(currentPoint, valves, minutesLeftTwo);

        //var resultSecond = EvaluateCombo(new List<Valve>
        //{
        //    valves.Find(v => v.Name == "JJ"),
        //    valves.Find(v => v.Name == "BB"),
        //    valves.Find(v => v.Name == "CC"),
        //    valves.Find(v => v.Name == "DD"),
        //    valves.Find(v => v.Name == "HH"),
        //    valves.Find(v => v.Name == "EE"),
        //}, currentPoint, currentPoint, minutesLeftTwo, minutesLeftTwo);

        resultSecond = CalculateBestRoutes(currentPoint, valves, minutesLeftTwo);

        stopwatch.Stop();

        Console.WriteLine($"Part one: {resultFirst}\nPart two: {resultSecond}\nTime elapsed: {stopwatch.Elapsed}");

        static int GetPossibleProfit(int distance, Valve valve, int time)
        {
            var usefulMinutes = time - 1 - distance;
            var profit = valve.Rate * usefulMinutes;
            return profit;
        }

        static int GetMaxProfit(Valve valve, List<Valve> unreleased, int timeRemains, int profit)
        {
            var profitableValves = valves.Where(v => v.Rate > 0 && unreleased.Contains(v)).ToList();
            var possibleProfits = new List<int>();
            if (timeRemains <= 0) return profit;
            if (valve.Rate > 0)
            {
                timeRemains -= 1;
                profit += valve.Rate * timeRemains;
                profitableValves.Remove(valve);
            }
            if (profitableValves.Count == 0)
            {
                return profit;
            }
            foreach (var profitableValve in profitableValves)
            {
                possibleProfits.Add(GetMaxProfit(profitableValve
                    , profitableValves
                    , timeRemains - valve.GetShortestDistance(profitableValve.Name, new()),
                    profit));
            }
            return possibleProfits.Max();
        }

        //static int SolvingTwo(Valve startPoint)
        //{
        //    var profitableValves = valves.Where(valve => valve.Rate > 0).ToList();
        //    var result = 0;

        //    var you = new Actor("MaBoi", startPoint);
        //    var elephant = new Actor("Dumbo", startPoint);
        //    var thaTeam = new List<Actor>()
        //    {
        //        you,
        //        elephant
        //    };

        //    while (minutesLeftTwo > 0)
        //    {
        //        foreach (var actor in thaTeam)
        //        {
        //            if (profitableValves.Count() == 0) break;
        //            if (actor.IsFree)
        //            {
        //                profitableValves = profitableValves.OrderByDescending(v => GetPossibleProfit(actor.CurrentValve.GetShortestDistance(v.Name, new()), v)).ToList();

        //                Valve target = new("PP");
        //                Actor opposing = new("temp", startPoint);

        //                if (actor.Name == "MaBoi") opposing = elephant;
        //                else opposing = you;

        //                var yourDistance = actor.CurrentValve.GetShortestDistance(profitableValves[0].Name, new());
        //                var opposingDistance = opposing.CurrentValve.GetShortestDistance(profitableValves[0].Name, new());

        //                if (profitableValves.Count() == 1
        //                    && yourDistance > opposingDistance)
        //                {
        //                    if (opposing.IsFree) break;
        //                    target = profitableValves[0];
        //                }
        //                else if (opposing.IsFree && yourDistance > opposingDistance)
        //                {
        //                    target = profitableValves[1];
        //                }
        //                else
        //                {
        //                    target = profitableValves[0];
        //                }

        //                var distanceToTarget = actor.CurrentValve.GetShortestDistance(target.Name, new());
        //                profitableValves.Remove(target);
        //                actor.TimeLeft = distanceToTarget + 1;
        //                actor.CurrentValve = target;
        //                actor.ProfitInWork = GetPossibleProfit(distanceToTarget, target, minutesLeftTwo);
        //                actor.IsFree = false;
        //            }
        //            else
        //            {
        //                result += actor.Act();
        //            }
        //        }

        //        minutesLeftTwo--;
        //    }

        //    return result;
        //}

        static int CalculateBestRoutes(Valve start, List<Valve> valves, int time)
        {
            var profitableValves = valves.Where(v => v.Rate > 0).ToList();
            profitableValves = profitableValves
                .OrderByDescending(v => GetPossibleProfit(start.GetShortestDistance(v.Name, new()), v, time))
                .ToList();

            var firstHalf = new List<Valve>();
            var secondHalf = new List<Valve>();

            var firstStarter = start;
            var secondStarter = start;

            var firstTime = time;
            var secondTime = time;

            var combos = new List<List<Valve>>();
            while (profitableValves.Count() >= 8)
            {
                var top = profitableValves.GetRange(0, 8);
                combos = GenerateCombos(top, 8);
                combos = combos.OrderByDescending(
                    c => EvaluateCombo(c, firstStarter, secondStarter, firstTime, secondTime)).ToList();
                //var combosWithVals = combos.ToDictionary(k => k, v => EvaluateCombo(v, firstStarter, secondStarter, firstTime, secondTime));

                firstTime = firstTime - firstStarter.GetShortestDistance(combos[0][0].Name, new()) - 1;
                firstStarter = combos[0][0];
                firstHalf.Add(firstStarter);
                profitableValves.Remove(firstStarter);

                Console.WriteLine($"Got {15 - profitableValves.Count()} item.");

                secondTime = secondTime - secondStarter.GetShortestDistance(combos[0][4].Name, new()) - 1;
                secondStarter = combos[0][4];
                secondHalf.Add(secondStarter);
                profitableValves.Remove(secondStarter);

                Console.WriteLine($"Got {15 - profitableValves.Count()} item.");
            }
            if (profitableValves.Count % 2 != 0)
            {
                profitableValves.Add(valves.First(v => v.Rate == 0));
            }
            combos = GenerateCombos(profitableValves, profitableValves.Count);
            combos = combos.OrderByDescending(
                    c => EvaluateCombo(c, firstStarter, secondStarter, firstTime, secondTime)).ToList();
            var topCombo = combos[0];
            var hf = topCombo.Count / 2;
            var ff = topCombo.GetRange(0, hf);
            var ss = topCombo.GetRange(hf, hf);
            firstHalf.AddRange(ff);
            secondHalf.AddRange(ss);
            firstHalf.AddRange(secondHalf);
            return EvaluateCombo(firstHalf, start, start, time, time);
        }

        static List<List<Valve>> GenerateCombos(List<Valve> valves, int count)
        {
            List<List<Valve>> combos = new();
            List<int> indices = new();
            for (int i = 0; i < count; i++)
            {
                indices.Add(i);
            }
            var permutations = Permutate(new(), indices);
            foreach (var permutation in permutations)
            {
                var combo = new List<Valve>();
                foreach (var index in permutation)
                {
                    combo.Add(valves[index]);
                }
                combos.Add(combo);
                //combos.Add(new List<Valve>()
                //{
                //    valves[permutation[0]],
                //    valves[permutation[1]],
                //    valves[permutation[2]],
                //    valves[permutation[3]],
                //    valves[permutation[4]],
                //    valves[permutation[5]],
                //});
            }
            return combos;
        }

        static int EvaluateCombo(List<Valve> combo, Valve start1, Valve start2, int time1, int time2)
        {
            var total = 0;

            var half = combo.Count / 2;

            var firstHalf = combo.GetRange(0, half);
            var dist1 = start1.GetShortestDistance(firstHalf[0].Name, new());
            total += GetPossibleProfit(dist1, firstHalf[0], time1);
            time1 = time1 - dist1 - 1;

            for (int i = 1; i < half; i++)
            {
                dist1 = firstHalf[i-1].GetShortestDistance(firstHalf[i].Name, new());
                total += GetPossibleProfit(dist1, firstHalf[i], time1);
                time1 = time1 - dist1 - 1;
            }

            //var dist1 = start.GetShortestDistance(combo[0].Name, new());
            //total += GetPossibleProfit(dist1, combo[0], time);
            //var dist11 = combo[0].GetShortestDistance(combo[1].Name, new());
            //total += GetPossibleProfit(dist11, combo[1], time - dist1 - 1);
            //var dist12 = combo[1].GetShortestDistance(combo[2].Name, new());
            //total += GetPossibleProfit(dist12, combo[2], time - dist1 - 1 - dist11 - 1);

            var secondHalf = combo.GetRange(half, half);
            var dist2 = start2.GetShortestDistance(secondHalf[0].Name, new());
            total += GetPossibleProfit(dist2, secondHalf[0], time2);
            time2 = time2 - dist2 - 1;

            for (int i = 1; i < half; i++)
            {
                dist2 = secondHalf[i - 1].GetShortestDistance(secondHalf[i].Name, new());
                total += GetPossibleProfit(dist2, secondHalf[i], time2);
                time2 = time2 - dist2 - 1;
            }

            //var dist2 = start.GetShortestDistance(combo[3].Name, new());
            //total += GetPossibleProfit(dist2, combo[3], time);
            //var dist21 = combo[3].GetShortestDistance(combo[4].Name, new());
            //total += GetPossibleProfit(dist21, combo[4], time - dist2 - 1);
            //var dist22 = combo[4].GetShortestDistance(combo[5].Name, new());
            //total += GetPossibleProfit(dist22, combo[5], time - dist2 - 1 - dist21 - 1);

            return total;
        }
    }

    private static List<List<T>> Permutate<T>(List<T> firstHalf, List<T> secondHalf)
    {
        List<List<T>> output = new();
            if (secondHalf.Count <= 1)
        {
            firstHalf.AddRange(secondHalf);
            output.Add(firstHalf);
            return output;
        };

        for (int i = 0; i < secondHalf.Count(); i++)
        {
            List<T> temp = new(firstHalf);
            List<T> temp2 = new(secondHalf);
            temp.Add(secondHalf[i]);
            temp2.Remove(secondHalf[i]);
            foreach (var permutation in Permutate(temp, temp2))
            {
                output.Add(permutation);
            }
        }
        //foreach (var item in secondHalf)
        //{
        //    List<T> temp = new(firstHalf);
        //    temp.Add(secondHalf[0]);
        //    foreach (var permutation in Permutate(temp, secondHalf.Skip(1).ToList()))
        //    {
        //        output.Add(permutation);
        //    }
        //}
        return output.Distinct().ToList() ;
    }

    class Actor
    {
        public string Name { get; set; }
        public bool IsFree { get; set; } = true;
        public int TimeLeft { get; set; } = 0;
        public int ProfitInWork { get; set; }
        public Valve CurrentValve { get; set; }

        public Actor(string name, Valve start)
        {
            Name = name;
            CurrentValve = start;
        }

        public int Act()
        {
            if (TimeLeft > 0)
            {
                TimeLeft--;
                return 0;
            }
            IsFree = true;
            return ProfitInWork;
        }
    }

    class Valve
    {
        public string Name { get; set; }
        public List<Valve> Neighbors { get; set; } = new();
        public int Rate { get; set; }
        public bool IsReleased { get; set; }
        public Valve(string name)
        {
            Name = name;
        }

        public int GetShortestDistance(string name, List<Valve> visited)
        {
            if (name == this.Name) return 0;
            var valve = Neighbors.Find(v => v.Name == name);
            if (valve is not null) return 1;
            var visitedFromHere = new List<Valve>(visited);
            visitedFromHere.Add(this);
            var distances = new List<int>() { valves.Count };
            foreach (var neighbor in Neighbors)
            {
                if (!visitedFromHere.Contains(neighbor))
                {
                    distances.Add(neighbor.GetShortestDistance(name, visitedFromHere) + 1);
                }

            }
            return distances.Min();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
