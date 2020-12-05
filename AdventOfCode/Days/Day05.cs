using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Days
{
    public class Day05 : ISolution
    {
        public string PartOne(string[] input)
        {
            var results = CalculateSeatIdsPostMegaThreadRead(input);

            return results.Max().ToString();
        }
        
        public string PartTwo(string[] input)
        {
            var results = CalculateSeatIdsPostMegaThreadRead(input);
            var min = results.Min();
            var max = results.Max();

            return Enumerable.Range(min, (max - min))
                .First(x => !results.Contains(x))
                .ToString();
        }

        private static List<int> CalculateSeatIds(IEnumerable<string> input)
        {
            var results = new List<int>();
            foreach (var str in input)
            {
                var items = str.ToCharArray();

                var lower = 0;
                var higher = 127;

                var lowerSeat = 0;
                var higherSeat = 8;
                foreach (var t in items)
                {
                    var middle = (higher + lower) / 2.0;
                    var middleSeat = (higherSeat + lowerSeat) / 2.0;
                    switch (t)
                    {
                        case 'F':
                            higher = (int) Math.Floor(middle);
                            break;
                        case 'B':
                            lower = (int) Math.Ceiling(middle);
                            break;
                        case 'L':
                            higherSeat = (int) Math.Floor(middleSeat);
                            break;
                        case 'R':
                            lowerSeat = (int) Math.Ceiling(middleSeat);
                            break;
                    }
                }

                results.Add((lower * 8) + lowerSeat);
            }

            return results;
        }
        
        private static IEnumerable<int> CalculateSeatIdsPostMegaThreadRead(IEnumerable<string> input)
        {
            return input
                .Select(str => str.ToCharArray())
                .Select(chrs => chrs.Select(chr => chr switch
                {
                    'F' => 0,
                    'B' => 1,
                    'L' => 0,
                    'R' => 1
                }))
                .Select(x => string.Join("", x))
                .Select(x => Convert.ToInt32(x.ToString(), 2));
        }
        
        public int Day => 05;
    }
}