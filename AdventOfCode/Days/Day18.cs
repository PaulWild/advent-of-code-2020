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
            return RunSolution(input, Reduce);
        }
        
        public string PartTwo(string[] input)
        {
            return RunSolution(input, ReducePart2);
        }

        private static string RunSolution(string[] input, Func<string ,string> reducer)
        {
            long accum = 0L;
            foreach (var str in input)
            {
                var opStr = str;
                var brackets = new Regex(@"\([\d +*]+\)");


                while (brackets.IsMatch(opStr))
                {
                    var toReduce = brackets.Match(opStr).Value;
                    var toReplace = reducer(toReduce.Substring(1, toReduce.Length - 2));
                    opStr = opStr.Replace(toReduce, toReplace);
                }


                accum += long.Parse(reducer(opStr));
            }


            return accum.ToString();
        }

        private static string Reduce(string input)
        {
            var op = new Regex(@"(?'left'\d+) (?'op'[+*]) (?'right'\d+)");

            while (op.IsMatch(input))
            {
                var match = op.Match(input);
                var replacement  = match.Groups["op"].Value == "+" 
                    ? (long.Parse(match.Groups["left"].Value) + long.Parse(match.Groups["right"].Value)).ToString() 
                    : (long.Parse(match.Groups["left"].Value) * long.Parse(match.Groups["right"].Value)).ToString();
                input = op.Replace(input, replacement, 1);
            }

            return input;
        }
        
        private static string ReducePart2(string input)
        {
            var addition = new Regex(@"(?'left'\d+) \+ (?'right'\d+)");
            var multiply = new Regex(@"(?'left'\d+) \* (?'right'\d+)");
            
            while (addition.IsMatch(input))
            {
                var match = addition.Match(input);
                var replacement = (long.Parse(match.Groups["left"].Value) + long.Parse(match.Groups["right"].Value)).ToString();
                input = addition.Replace(input, replacement, 1);
            }
            
            while (multiply.IsMatch(input))
            {
                var match = multiply.Match(input);
                var replacement = (long.Parse(match.Groups["left"].Value) * long.Parse(match.Groups["right"].Value)).ToString();
                input = multiply.Replace(input, replacement, 1);
            }

            return input;
        }


        public int Day => 18;
    }
}