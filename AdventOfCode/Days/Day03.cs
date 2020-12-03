using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day03 : ISolution
    {
        public string PartOne(string[] input)
        {
            var numberOfTrees = NumberOfTrees(input, 3, 1);

            return numberOfTrees.ToString();
        }

        private static long NumberOfTrees(IEnumerable<string> input, int xStep, int yStep)
        {
            var treeMap = input.Select(row => row.ToCharArray()).ToList();
            var width = treeMap.First().Length;

            var y = 0;
            var x = 0;
            
            y += yStep;
            long numberOfTrees = 0;
            while (y < treeMap.Count)
            {
                x = (x + xStep) < width ? x + xStep : xStep - (width - x);

                if (treeMap[y][x] == '#')
                    numberOfTrees++;

                y += yStep;
            }

            return numberOfTrees;
        }

        public string PartTwo(string[] input)
        {
            (int x, int y)[] inputs = {(1, 1), (3, 1), (5, 1), (7, 1), (1, 2)};
            
            var numberOfTrees = inputs
                .Select(steps => NumberOfTrees(input, steps.x, steps.y))
                .Aggregate((acc, next) => acc * next);

            return numberOfTrees.ToString();
        }

        public int Day => 03;
    }
}