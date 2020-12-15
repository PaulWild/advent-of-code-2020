using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day15 : ISolution
    {
        public string PartOne(string[] input)
        {
            return PlayTerribleGame(input, 2020).ToString();
        }

        private static int PlayTerribleGame(IEnumerable<string> input, int numberOfTimes)
        {
            var numbers = input.First()
                .Split(',')
                .Select(int.Parse)
                .ToList();

            var numberDictionary = numbers
                .Select((x, idx) => (x, ++idx))
                .SkipLast(1)
                .ToDictionary(x => x.x, x => x.Item2);

            var number = numbers.Last();

            for (var i = numbers.Count + 1; i <= numberOfTimes; i++)
            {
                var newNumber = 0;
                if (numberDictionary.TryGetValue(number, out var tmp))
                {
                    newNumber = i - 1 - tmp;
                }

                numberDictionary[number] = i - 1;
                number = newNumber;
            }

            return number;
        }

        public string PartTwo(string[] input)
        {
            return PlayTerribleGame(input, 30000000).ToString();
        }

        public int Day => 15;
    }
}