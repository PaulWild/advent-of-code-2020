using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day01 : ISolution
    {
        public string PartOne(string[] input)
        {
            var numbers = input.Select(int.Parse).ToList();

            return (from num in numbers 
                from other in numbers 
                where num + other == 2020
                select num * other).First().ToString();
        }

        public string PartTwo(string[] input)
        {
            var numbers = input.Select(int.Parse).ToList();

            return (from num in numbers
                from other in numbers
                from third in numbers
                where num + other + third == 2020
                select num * other * third).First().ToString();
        }

        public int Day => 01;
    }
}