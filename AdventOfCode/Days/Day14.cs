using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    public class Day14 : ISolution
    {
        public string PartOne(string[] input)
        {
            var mask = Array.Empty<char>();
            var values = new Dictionary<int,long>();
            
            foreach (var str in input)
            {
                var maskMatch = Regex.Match(str, "mask = (?'mask'[0X1]+)");
                if (maskMatch.Success)
                {
                    mask = maskMatch.Groups["mask"].Value.ToCharArray();
                    continue;
                }

                var memMatch = Regex.Match(str, @"mem\[(?'memLoc'\d+)\] = (?'value'\d+)");
                var memLocation = int.Parse(memMatch.Groups["memLoc"].Value);
                var memValue = uint.Parse(memMatch.Groups["value"].Value);

                values[memLocation] = MaskValue(memValue, mask);
            }

            return values.Values.Sum().ToString();
        }

        private static long MaskValue(uint memValue, IReadOnlyList<char> mask)
        {
            var valueAsBinary = Convert.ToString(memValue, 2).PadLeft(mask.Count, '0').ToCharArray();

            for (var i = 0; i < mask.Count; i++)
            {
                valueAsBinary[i] = mask[i] switch
                {
                    '1' => '1',
                    '0' => '0',
                    _ => valueAsBinary[i]
                };
            }

            return Convert.ToInt64(string.Join("", valueAsBinary),2);
        }

        public string PartTwo(string[] input)
        {
            var mask = Array.Empty<char>();
            var values = new Dictionary<long,long>();
            
            foreach (var str in input)
            {
                var maskMatch = Regex.Match(str, "mask = (?'mask'[0X1]+)");
                if (maskMatch.Success)
                {
                    mask = maskMatch.Groups["mask"].Value.ToCharArray();
                    continue;
                }

                var memMatch = Regex.Match(str, @"mem\[(?'memLoc'\d+)\] = (?'value'\d+)");
                var memLocation = uint.Parse(memMatch.Groups["memLoc"].Value);
                var memValue = uint.Parse(memMatch.Groups["value"].Value);

                var memLocations= AllLocations(MaskValuePart2(memLocation, mask));

                foreach (var loc in memLocations)
                {
                    values[loc] = memValue;
                }
            }

            return values.Values.Sum().ToString();
        }
        
        private static char[] MaskValuePart2(uint memValue, IReadOnlyList<char> mask)
        {
            var valueAsBinary = Convert.ToString(memValue, 2).PadLeft(mask.Count, '0').ToCharArray();

            for (var i = 0; i < mask.Count; i++)
            {
                valueAsBinary[i] = mask[i] switch
                {
                    '1' => '1',
                    'X' => 'X',
                    _ => valueAsBinary[i]
                };
            }

            return valueAsBinary;
        }

        private static IEnumerable<long> AllLocations(IReadOnlyList<char> mask)
        {
            if (mask.All(x => x != 'X'))
            {
                yield return Convert.ToInt64(string.Join("", mask.ToArray()), 2);
            }
            else
            {
                List<long> toReturn = new();
                var loc = mask.ToList().IndexOf('X');

                var zero = mask.ToArray();
                zero[loc] = '0';
                toReturn.AddRange(AllLocations(zero));


                var one = mask.ToArray();
                one[loc] = '1';
                toReturn.AddRange(AllLocations(one));

                foreach (var item in toReturn)
                {
                    yield return item;
                }
            }
        }

        public int Day => 14;
    }
}