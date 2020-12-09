using System;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day09 : ISolution
    {
        public string PartOne(string[] input)
        {
            var allNumbers = input.Select(item => Convert.ToInt64(item)).ToList();

            for (var i = 25; i < allNumbers.Count; i++)
            {
                var numbers = allNumbers.GetRange(i - 25, 25);

                var valid = (from number in numbers
                    from other in numbers
                    let sum = other + number
                    where number != other
                    where sum == allNumbers[i]
                    select true).Any();
                
                if (!valid)
                    return allNumbers[i].ToString();
            }

            return "-1";
        }

        public string PartTwo(string[] input)
        {
            var toFind = Convert.ToInt64(PartOne(input));
            var allNumbers = input.Select(item => Convert.ToInt64(item)).ToList();

            for (var i = 0; i < allNumbers.Count; i++)
            {
                var j = 0;
                long sum = 0;
                while (sum < toFind)
                {
                    var range = allNumbers.GetRange(i, j);
                    sum = range.Sum();

                    if (sum == toFind)
                        return (range.Min() + range.Max()).ToString();

                    j++;
                }
            }

            return "-1";
        }

        public int Day => 09;
    }
}