using Common;
using System.Text;

namespace Year2023;

internal class Day03 : Day
{
    protected override int TestSolutionOne { get; set; } = 4361;
    protected override int TestSolutionTwo { get; set; } = 467835;

    protected override int SolveOne(string input)
    {
        int output = 0;
        using var reader = new StreamReader(input);
        var buffer = new Queue<string>();
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (line == null) throw new NullReferenceException(nameof(line));
            var lengthI = line.Length;
            if (buffer.Count == 0) buffer.Enqueue(new string('.', lengthI));
            buffer.Enqueue(line);
            if (reader.EndOfStream) buffer.Enqueue(new string('.', lengthI));

            if (buffer.Count < 3) continue;

            var currentLineI = buffer.ToList()[1];
            var numbersI = new List<Numero>();
            var currentNumberI = new StringBuilder();
            for (int i = 0; i < lengthI; i++)
            {
                var ch = currentLineI[i];
                if (char.IsDigit(ch)) currentNumberI.Append(ch);
                if (!char.IsDigit(ch) && currentNumberI.Length != 0)
                {
                    numbersI.Add(new(int.Parse(currentNumberI.ToString()), i - currentNumberI.Length, i - 1));
                    currentNumberI.Clear();
                }
                if (i == lengthI - 1 && currentNumberI.Length != 0)
                {
                    numbersI.Add(new(int.Parse(currentNumberI.ToString()), i - currentNumberI.Length + 1, i));
                    currentNumberI.Clear();
                }
            }
            foreach (var number in numbersI)
            {
                bool isDetail = false;
                foreach (var row in buffer)
                {
                    for (int i = number.Start > 0 ? number.Start - 1 : 0;
                         i <= ((number.End < lengthI - 1) ? number.End + 1 : number.End);
                         i++)
                    {
                        if (!char.IsDigit(row[i]) && row[i] != '.')
                        {
                            isDetail = true;
                            output += number.Value;
                            break;
                        }
                    }
                    if (isDetail) break;
                }
            }

            buffer.Dequeue();
        }

        var currentLine = buffer.ToList()[1];
        var length = currentLine.Length;
        var numbers = new List<Numero>();
        var currentNumber = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            var ch = currentLine[i];
            if (char.IsDigit(ch)) currentNumber.Append(ch);
            if (!char.IsDigit(ch) && currentNumber.Length != 0)
            {
                numbers.Add(new(int.Parse(currentNumber.ToString()), i - currentNumber.Length, i - 1));
                currentNumber.Clear();
            }
            if (i == length - 1 && currentNumber.Length != 0)
            {
                numbers.Add(new(int.Parse(currentNumber.ToString()), i - currentNumber.Length + 1, i));
                currentNumber.Clear();
            }
        }
        foreach (var number in numbers)
        {
            bool isDetail = false;
            foreach (var row in buffer)
            {
                for (int i = number.Start > 0 ? number.Start - 1 : 0;
                     i <= ((number.End < length - 1) ? number.End + 1 : number.End);
                     i++)
                {
                    if (!char.IsDigit(row[i]) && row[i] != '.')
                    {
                        isDetail = true;
                        output += number.Value;
                        break;
                    }
                }
                if (isDetail) break;
            }
        }

        return output;
    }

    private class Numero
    {
        public int Value { get; }
        public int Start { get; }
        public int End { get; }

        public Numero(int value, int start, int end)
        {
            Value = value;
            Start = start;
            End = end;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }


    protected override int SolveTwo(string input)
    {
        int output = 0;
        using var reader = new StreamReader(input);
        var buffer = new Queue<string>();
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (line == null) throw new NullReferenceException(nameof(line));
            var length = line.Length;
            if (buffer.Count == 0) buffer.Enqueue(new string('.', length));
            buffer.Enqueue(line);
            if (reader.EndOfStream) buffer.Enqueue(new string('.', length));

            if (buffer.Count < 3) continue;

            var currentLine = buffer.ToList()[1];
            if (!currentLine.Contains('*'))
            {
                buffer.Dequeue();
                continue;
            }
            var numbers = new List<Numero>();
            var currentNumber = new StringBuilder();
            foreach (var row in buffer)
            {
                for (int i = 0; i < length; i++)
                {
                    var ch = row[i];
                    if (char.IsDigit(ch))
                    {
                        currentNumber.Append(ch);
                    }
                    if (!char.IsDigit(ch) && currentNumber.Length != 0)
                    {
                        numbers.Add(new(int.Parse(currentNumber.ToString()), i - currentNumber.Length, i - 1));
                        currentNumber.Clear();
                    }
                    if (i == length - 1 && currentNumber.Length != 0)
                    {
                        numbers.Add(new(int.Parse(currentNumber.ToString()), i - currentNumber.Length + 1, i));
                        currentNumber.Clear();
                    }
                }
            }

            for (int i = 0; i < length; i++)
            {
                if (currentLine[i] == '*')
                {
                    var neighbors = numbers.Where(n => n.Start <= i + 1 && n.End >= i - 1).ToList();
                    if (neighbors.Count == 2)
                    {
                        output += neighbors[0].Value * neighbors[1].Value;
                    }
                }
            }

            buffer.Dequeue();
        }

        var currentLineI = buffer.ToList()[1];
        if (currentLineI.Contains('*'))
        {
            var numbersI = new List<Numero>();
            var currentNumberI = new StringBuilder();
            var lengthI = currentLineI.Length;
            foreach (var row in buffer)
            {
                for (int i = 0; i < lengthI; i++)
                {
                    var ch = currentLineI[i];
                    if (char.IsDigit(ch))
                    {
                        currentNumberI.Append(ch);
                    }
                    if (!char.IsDigit(ch) && currentNumberI.Length != 0)
                    {
                        numbersI.Add(new(int.Parse(currentNumberI.ToString()), i - currentNumberI.Length, i - 1));
                        currentNumberI.Clear();
                    }
                    if (i == lengthI - 1 && currentNumberI.Length != 0)
                    {
                        numbersI.Add(new(int.Parse(currentNumberI.ToString()), i - currentNumberI.Length + 1, i));
                        currentNumberI.Clear();
                    }
                }
            }

            for (int i = 0; i < lengthI; i++)
            {
                if (currentLineI[i] == '*')
                {
                    var neighbors = numbersI.Where(n => n.Start <= i + 1 && n.End >= i - 1).ToList();
                    if (neighbors.Count == 2)
                    {
                        output += neighbors[0].Value * neighbors[1].Value;
                    }
                }
            }
        }

        return output;
    }
}
