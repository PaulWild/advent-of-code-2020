using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day06 : ISolution
    {
        public string PartOne(string[] input)
        {
            var results = ParseResults(input);

            return results.Select(res => res.SelectMany(x => x.ToCharArray())
                .Distinct()
                .Count())
                .Sum()
                .ToString();
        }
        
        
        public string PartTwo(string[] input)
        {
            var results = ParseResults(input);

            var answers = (from result in results
                let numberOfPeople = result.Count
                let dict = result.SelectMany(x => x.ToCharArray())
                    .GroupBy(x => x)
                    .ToDictionary(key => key, value => value.Count())
                select dict.Values.Count(x => x == numberOfPeople)).ToList();

            return answers.Sum().ToString();
        }

        private static IEnumerable<List<string>> ParseResults(IEnumerable<string> input)
        {
            var results = new List<List<string>>();
            var result = new List<string>();
            foreach (var str in input)
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    results.Add(result);
                    result = new List<string>();
                    continue;
                }

                result.Add(str);
            }

            if (result.Count > 0)
            {
                results.Add(result);
            }

            return results;
        }

        public int Day => 06;
    }
}