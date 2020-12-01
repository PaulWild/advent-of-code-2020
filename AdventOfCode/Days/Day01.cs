using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day01 : ISolution
    {
        public string PartOne(string[] input)
        {
            var numbers = input.Select(item => Convert.ToInt32(item)).ToList();

            List<(int fst, int snd)> pairs = (from num in numbers 
                from other in numbers 
                where num != other
                select (num, other)).ToList();
            
            return pairs.Where(x => x.fst + x.snd == 2020)
                .Select(x => x.fst * x.snd)
                .First()
                .ToString();
        }

        public string PartTwo(string[] input)
        {
            var numbers = input.Select(item => Convert.ToInt32(item)).ToList();
            
            List<(int fst, int snd, int thd)> triples = (from num in numbers 
                from other in numbers 
                from third in numbers
                where num != other && num != third
                select (num, other, third)).ToList();
            
            return triples.Where(x => x.fst + x.snd + x.thd == 2020)
                .Select(x => x.fst * x.snd * x.thd)
                .First()
                .ToString();
        }

        public int Day => 01;
    }
}