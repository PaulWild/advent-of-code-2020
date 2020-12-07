using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day07 : ISolution
    {
        public string PartOne(string[] input)
        {
            var bags = ParseInput(input);

            var results = OuterBags("shiny gold", bags);
            
            return results.Count().ToString();
        }
        
        public string PartTwo(string[] input)
        {
            var bags = ParseInput(input);

            //we count the shiny gold bag 
            var result = BagCount("shiny gold", 1, bags) - 1;
            
            return result.ToString();
        }

        private static int BagCount(string bag, int count, List<Bag> bags)
        {
            var innerBags = bags
                .Where(x => x.Type == bag)
                .SelectMany(x => x.EnclosedBags)
                .ToList();

            if (!innerBags.Any())
            {
                return count;
            }

            var num = innerBags.Sum(nextBag => BagCount(nextBag.Key, nextBag.Value, bags));

            return num * count + count;
        }

        private static IEnumerable<string> OuterBags(string bag, List<Bag> bags)
        {
            var outerBags = bags
                .Where(x => x.EnclosedBags.ContainsKey(bag))
                .Select(x => x.Type)
                .ToList();

            if (!outerBags.Any())
            {
                return new List<string>();
            }

            var toReturn = outerBags.Select(x => x).ToList();
            var next = outerBags.SelectMany(x => OuterBags(x, bags)).ToList();
                
            toReturn.AddRange(next);

            return toReturn.Distinct().ToList();
        }

        private static List<Bag> ParseInput(IEnumerable<string> input)
        {
            var bags = new List<Bag>();
            foreach (var row in input)
            {
                var split = row.Replace(".", "").Split("contain");
                var bagType = split[0].Replace("bags", "").Trim();

                if (split[1].Contains("no other bags"))
                {
                    bags.Add(new Bag {Type = bagType, EnclosedBags = new Dictionary<string, int>()});
                }
                else
                {
                    var innerBags = split[1].Split(",");
                    var innerBagsDict = new Dictionary<string, int>();
                    foreach (var innerBag in innerBags)
                    {
                        var rawCount = innerBag.Trim().Substring(0, 1);
                        var count = int.Parse(rawCount); //assumption: all bags have less than ten items
                        var type = innerBag.Trim().Substring(1).Replace("bags", "").Replace("bag", "").Trim();

                        innerBagsDict.Add(type, count);
                    }

                    bags.Add(new Bag {Type = bagType, EnclosedBags = innerBagsDict});
                }
            }

            return bags;
        }
        
        private record Bag
        {
            public string Type { get; init; } 
            
            public Dictionary<string, int> EnclosedBags { get; init; }
        }
        
        public int Day => 07;
    }
}