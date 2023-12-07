using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Sources;

namespace ThirteenthDay;

class Program
{
    const string inputPath = @"..\..\..\input.txt";

    static int resultFirst = 0;
    static int resultSecond = 0;

    static void Main()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        int index = 0;

        string firstDivider = "[[2]]";
        string secondDivider = "[[6]]";

        List<string> packets = new()
        {
            firstDivider,
            secondDivider
        };

        using var reader = new StreamReader(inputPath);
        while (!reader.EndOfStream)
        {
            var firstString = reader.ReadLine();
            if (string.IsNullOrEmpty(firstString)) continue;
            packets.Add(firstString);

            var secondString = reader.ReadLine() ?? throw new NullReferenceException();
            packets.Add(secondString);

            index++;
            var comparer = new MyComparer();
            if (comparer.Compare(firstString, secondString) == -1) resultFirst += index;
        }

        packets.Sort(new MyComparer());
        resultSecond = (packets.IndexOf(firstDivider)+1) * (packets.IndexOf(secondDivider)+1);

        stopwatch.Stop();

        Console.WriteLine($"Part one: {resultFirst}\nPart two: {resultSecond}\nTime elapsed: {stopwatch.Elapsed}");


    }

    public class MyComparer : IComparer<string>
    {
        public int Compare(string? first, string? second)
        {
            var output = 0;
            bool isFinal = false;

            if (first is null || second is null) throw new NullReferenceException();

            var firstMatch = Regex.Match(first, "(?<=\\[).*(?=\\])");
            var secondMatch = Regex.Match(second, "(?<=\\[).*(?=\\])");

            string[]? firstItems = new string[1];
            string[]? secondItems = new string[1];

            if (firstMatch.Success)
            {
                firstItems = firstMatch.Value.Split(',');

            }
            else
            {
                firstItems[0] = first;
            }

            if (secondMatch.Success)
            {
                secondItems = secondMatch.Value.Split(',');

            }
            else
            {
                secondItems[0] = second;
            }

            var legitFirst = new List<string>();
            var legitSecond = new List<string>();

            var sb = new StringBuilder();

            bool isNotComplete = false;

            foreach (var item in firstItems)
            {
                if (item.EndsWith("]"))
                {
                    sb.Append(item);
                    if (sb.ToString().Count(x => x == '[') == sb.ToString().Count(x => x == ']'))
                    {
                        isNotComplete = false;

                        legitFirst.Add(sb.ToString());
                        sb.Clear();
                        continue;
                    }
                    else
                    {
                        isNotComplete = true;
                        sb.Append(','); continue;
                    }
                }

                if (isNotComplete) { sb.Append(item); sb.Append(','); continue; }
                if (item.StartsWith("[")) { isNotComplete = true; sb.Append(item); sb.Append(','); continue; }
                if (item == "") continue;
                legitFirst.Add(item);
            }
            foreach (var item in secondItems)
            {
                if (item.EndsWith("]"))
                {
                    sb.Append(item);
                    if (sb.ToString().Count(x => x == '[') == sb.ToString().Count(x => x == ']'))
                    {
                        isNotComplete = false;
                        legitSecond.Add(sb.ToString());
                        sb.Clear();
                        continue;
                    }
                    else
                    {
                        isNotComplete = true;
                        sb.Append(','); continue;
                    }
                }

                if (isNotComplete) { sb.Append(item); sb.Append(','); continue; }
                if (item.StartsWith("[")) { isNotComplete = true; sb.Append(item); sb.Append(','); continue; }
                if (item == "") continue;
                legitSecond.Add(item);
            }

            for (int i = 0; i < Math.Max(legitFirst.Count, legitSecond.Count); i++)
            {
                string firstItem;
                string secondItem;

                try
                {
                    firstItem = legitFirst[i];
                }
                catch (Exception)
                {
                    output = -1;
                    isFinal = true;
                    break;
                }
                try
                {
                    secondItem = legitSecond[i];
                }
                catch (Exception)
                {
                    output = 1;
                    isFinal = true;
                    break;
                }

                if (int.TryParse(firstItem, out int firstInt) && int.TryParse(secondItem, out int secondInt))
                {
                    if (firstInt > secondInt)
                    {
                        output = -1;
                        isFinal = true;
                        break;
                    }

                    if (firstInt < secondInt)
                    {
                        output = -1;
                        isFinal = true;
                        break;
                    }

                    continue;
                }

                output = Compare(firstItem, secondItem, out isFinal);
                if (isFinal) return output;
            }

            return output;
        }

        public static int Compare(string? first, string? second, out bool isFinal)
        {
            var output = 0;
            isFinal = false;

            if (first is null || second is null) throw new NullReferenceException();

            var firstMatch = Regex.Match(first, "(?<=\\[).*(?=\\])");
            var secondMatch = Regex.Match(second, "(?<=\\[).*(?=\\])");

            string[]? firstItems = new string[1];
            string[]? secondItems = new string[1];

            if (firstMatch.Success)
            {
                firstItems = firstMatch.Value.Split(',');

            }
            else
            {
                firstItems[0] = first;
            }

            if (secondMatch.Success)
            {
                secondItems = secondMatch.Value.Split(',');

            }
            else
            {
                secondItems[0] = second;
            }

            var legitFirst = new List<string>();
            var legitSecond = new List<string>();

            var sb = new StringBuilder();

            bool isNotComplete = false;

            foreach (var item in firstItems)
            {
                if (item.EndsWith("]"))
                {
                    sb.Append(item);
                    if (sb.ToString().Count(x => x == '[') == sb.ToString().Count(x => x == ']'))
                    {
                        isNotComplete = false;

                        legitFirst.Add(sb.ToString());
                        sb.Clear();
                        continue;
                    }
                    else
                    {
                        isNotComplete = true;
                        sb.Append(','); continue;
                    }
                }

                if (isNotComplete) { sb.Append(item); sb.Append(','); continue; }
                if (item.StartsWith("[")) { isNotComplete = true; sb.Append(item); sb.Append(','); continue; }
                if (item == "") continue;
                legitFirst.Add(item);
            }
            foreach (var item in secondItems)
            {
                if (item.EndsWith("]"))
                {
                    sb.Append(item);
                    if (sb.ToString().Count(x => x == '[') == sb.ToString().Count(x => x == ']'))
                    {
                        isNotComplete = false;
                        legitSecond.Add(sb.ToString());
                        sb.Clear();
                        continue;
                    }
                    else
                    {
                        isNotComplete = true;
                        sb.Append(','); continue;
                    }
                }

                if (isNotComplete) { sb.Append(item); sb.Append(','); continue; }
                if (item.StartsWith("[")) { isNotComplete = true; sb.Append(item); sb.Append(','); continue; }
                if (item == "") continue;
                legitSecond.Add(item);
            }

            for (int i = 0; i < Math.Max(legitFirst.Count, legitSecond.Count); i++)
            {
                string firstItem;
                string secondItem;

                try
                {
                    firstItem = legitFirst[i];
                }
                catch (Exception)
                {
                    output = -1;
                    isFinal = true;
                    break;
                }
                try
                {
                    secondItem = legitSecond[i];
                }
                catch (Exception)
                {
                    output = 1;
                    isFinal = true;
                    break;
                }

                if (int.TryParse(firstItem, out int firstInt) && int.TryParse(secondItem, out int secondInt))
                {
                    if (firstInt > secondInt)
                    {
                        output = 1;
                        isFinal = true;
                        break;
                    }

                    if (firstInt < secondInt)
                    {
                        output = -1;
                        isFinal = true;
                        break;
                    }

                    continue;
                }

                output = Compare(firstItem, secondItem, out isFinal);
                if (isFinal) return output;
            }

            return output;
        }
    }
}
