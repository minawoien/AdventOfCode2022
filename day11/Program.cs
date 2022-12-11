﻿string[] lines = System.IO.File.ReadAllLines(@"input.txt");
Dictionary<int, List<double>> worry_levels = new Dictionary<int, List<double>>();
Dictionary<int, double> inspected = new Dictionary<int, double>();
int current_monkey = 0;
List<double> divisors = new List<double>();
for (var i = 0; i < lines.Length; i += 7)
{
    // Find current monkey
    var item = lines[i].Split(" ");
    current_monkey = Int32.Parse(item[1].Remove(item[1].Length - 1, 1));

    // Find Starting items
    var start = lines[i + 1].Split(":");
    var starting_items = start[1].Split(",");
    List<double> start_items = new List<double>();
    foreach (var worry_level in starting_items)
    {
        start_items.Add(double.Parse(worry_level));
    }
    worry_levels.Add(current_monkey, start_items);
    inspected.Add(current_monkey, 0);

    // Find devisors
    var test = lines[i + 3].Split(" ");
    divisors.Add(double.Parse(test[test.Length - 1]));
}
double lcm_divisors = LCM(divisors);

for (var j = 0; j < 10000; j++)
{
    for (var i = 0; i < lines.Length; i += 7)
    {
        double count = 0;
        // Find current monkey and its items
        var item = lines[i].Split(" ");
        current_monkey = Int32.Parse(item[1].Remove(item[1].Length - 1, 1));
        var starting_items = worry_levels[current_monkey];

        // Find operation
        var operation = lines[i + 2].Split(":");
        var ch = operation[1].Split(" ");
        double x = 0;

        // Find test
        var test = lines[i + 3].Split(" ");
        double numb = double.Parse(test[test.Length - 1]);

        // Find next monkey
        var next_true = lines[i + 4].Split(" ");
        var t_next = Int32.Parse(next_true[next_true.Length - 1]);
        var next_false = lines[i + 5].Split(" ");
        var f_next = Int32.Parse(next_false[next_false.Length - 1]);
        foreach (var worry_level in starting_items)
        {
            double new_worry = calculate_worry_level(x, worry_level, ch);
            new_worry = new_worry % lcm_divisors; // LCM of the divisors

            if (new_worry % numb == 0)
            {
                worry_levels[t_next].Add(new_worry);
            }
            else
            {
                worry_levels[f_next].Add(new_worry);
            }
            count++;
        }
        worry_levels[current_monkey] = new List<double>();
        inspected[current_monkey] += count;
    }
}

Console.WriteLine(sortDict(inspected));

static double gcd(double n1, double n2)
{
    if (n2 == 0)
    {
        return n1;
    }
    else
    {
        return gcd(n2, n1 % n2);
    }
}

static double LCM(List<double> numbers)
{
    return numbers.Aggregate((S, val) => S * val / gcd(S, val));
}

static double calculate_worry_level(double x, double worry_level, string[] ch)
{
    if (double.TryParse(ch[5], out x))
    {
        if (ch[4] == "*") { return worry_level * double.Parse(ch[5]); }
        else { return worry_level + double.Parse(ch[5]); }
    }
    else
    {
        if (ch[4] == "*") { return worry_level * worry_level; }
        else { return worry_level + worry_level; }
    }
}

static double sortDict(Dictionary<int, double> dict)
{
    var inspected_list = dict.ToList();
    inspected_list.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
    inspected_list.Reverse();
    return inspected_list[0].Value * inspected_list[1].Value;
}