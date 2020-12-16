using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day16 : ISolution
    {
        public string PartOne(string[] input)
        {
            var (rules, _, otherTickets) = ParseInput(input);
            
            return otherTickets
                .SelectMany(x => InvalidTicketNumbers(x, rules))
                .Sum()
                .ToString();

        }
        
        public string PartTwo(string[] input)
        {
            var (rules, myTicket, otherTickets) = ParseInput(input);

            var validTickets = (
                from ticket in otherTickets 
                let invalidTicketNumbers = InvalidTicketNumbers(ticket, rules).ToList() 
                where invalidTicketNumbers.Count == 0 
                select ticket).ToList();

            var groupedRules = rules
                .GroupBy(x => x.rule)
                .ToDictionary(x => x.Key, x => x.Select(y => (y.from, y.to)).ToList());

            var headings = new string[groupedRules.Keys.Count];
            while (groupedRules.Count > 0)
            {
                for (var i = 0; i < myTicket.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(headings[i]))
                        continue;

                    var potentialHeadings = new List<string>();
                    foreach (var (key, value) in groupedRules)
                    {
                        if (!validTickets.Any(x => value.All(y => x[i] < y.from || x[i] > y.to)))
                            potentialHeadings.Add(key);
                    }

                    if (potentialHeadings.Count != 1) continue;
                    
                    headings[i] = potentialHeadings.Single();
                    groupedRules.Remove(potentialHeadings.Single());
                }
            }

            var indices = headings
                .Select((x, idx) => (x, idx))
                .Where(x => x.x.StartsWith("departure"))
                .Select(x => x.idx);

            return indices
                .Select(index => myTicket[index])
                .Aggregate((long)1, (agg,nxt) => agg * (long)nxt)
                .ToString();
        }
        
        private static (List<(string rule, int from, int to)> rules, List<int> myTicket, List<List<int>> otherTickets) ParseInput(IEnumerable<string> input)
        {
            var rules = new List<(string rule, int from, int to)>();
            var myTicket = new List<int>();
            var otherTickets = new List<List<int>>();
            var readRules = false;
            var readMyTicket = false;

            foreach (var str in input)
            {
                if (!readRules)
                {
                    if (string.IsNullOrWhiteSpace(str))
                    {
                        readRules = true;
                    }
                    else
                    {
                        var split = str.Split(": ");
                        var rule = split[0];
                        var ranges = split[1].Split(" or ");
                        var range1 = ranges[0].Split("-");
                        var range2 = ranges[1].Split("-");
                        rules.Add((rule, int.Parse(range1[0]), int.Parse(range1[1])));
                        rules.Add((rule, int.Parse(range2[0]), int.Parse(range2[1])));
                    }

                    continue;
                }

                if (!readMyTicket)
                {
                    if (str == "your ticket:")
                        continue;

                    myTicket = str.Split(",").Select(int.Parse).ToList();
                    readMyTicket = true;

                    continue;
                }

                if (string.IsNullOrWhiteSpace(str) || str == "nearby tickets:")
                    continue;

                otherTickets.Add(str.Split(",").Select(int.Parse).ToList());
            }

            return (rules, myTicket, otherTickets);
        }

        private static IEnumerable<int> InvalidTicketNumbers(IEnumerable<int> ticket, IReadOnlyCollection<(string rule, int @from, int to)> rules)
        {
            return from number in ticket
                let inValid = rules.All(x => number < x.@from || number > x.to)
                where inValid
                select number;
        }

        public int Day => 16;
    }
}