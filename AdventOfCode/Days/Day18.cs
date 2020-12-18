using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    public class Day18 : ISolution
    {
        public string PartOne(string[] input)
        {
            ulong accum = 0L;
            foreach (var str in input)
            {
                var opStr = str;
                var a = new Regex(@"\([\d +*]+\)");

                while (a.IsMatch(opStr))
                {
                    var toReduce = a.Match(opStr).Value;
                    var toReplace = Reduce(toReduce.Substring(1, toReduce.Length - 2));
                    opStr = opStr.Replace(toReduce, toReplace.ToString());
                }

                accum += Reduce(opStr);
            }


            return accum.ToString();
        }

        private static ulong Reduce(string input)
        {
            var chars = input.Split(" ").ToList();

            var accum = ulong.Parse(chars.First());

            var operations = new Dictionary<string, Func<ulong, ulong, ulong>>
            {
                {"+", (a, b) => a + b},
                {"*", (a, b) => a * b}
            };
            Func<ulong, ulong, ulong> opp = (_, _) => throw new NotImplementedException();

            foreach (var c in chars.Skip(1))
            {
                switch (c)
                {
                    case "+":
                        opp = operations["+"];
                        break;
                    case "*":
                        opp = operations["*"];
                        break;
                    default:
                        accum = opp(accum, ulong.Parse(c));
                        break;
                }
            }

            return accum;
        }

        public string PartTwo(string[] input)
        {
            ulong accum = 0L;

            foreach (var str in input)
            {
                var opStr = str;

                var a = new Regex(@"\([\d +*]+\)");
                var additionRegex = new Regex(@"\d+ \+ \d+");

                while (a.IsMatch(opStr))
                {
                    var toReduce = a.Match(opStr).Value;
                    var toReplace = toReduce.Substring(1, toReduce.Length - 2);

                    while (additionRegex.IsMatch(toReplace))
                    {
                        var additionReduce = additionRegex.Match(toReplace).Value;
                        toReplace = additionRegex.Replace(toReplace, Reduce(additionReduce).ToString(), 1);
                    }

                    opStr = opStr.Replace(toReduce, Reduce(toReplace).ToString());

                }

                while (additionRegex.IsMatch(opStr))
                {
                    var additionReduce = additionRegex.Match(opStr).Value;
                    opStr = additionRegex.Replace(opStr, Reduce(additionReduce).ToString(), 1);
                }
                
                accum += Reduce(opStr);
            }


            return accum.ToString();
        }

        public int Day => 18;
    }
}