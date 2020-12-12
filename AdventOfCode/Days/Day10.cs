using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day10 : ISolution
    {
        public string PartOne(string[] input)
        {
            var jolts =  input.Select(int.Parse).OrderBy(x => x).ToList();
            var joltDifferences = jolts
                .Select((x, idx) => idx == 0 ? x : (x - jolts[idx - 1]))
                .GroupBy(x => x)
                .ToDictionary(key => key.Key, value => value.Count());


            //add one for the final jolt of 3 
            return (joltDifferences[1] * (joltDifferences[3]+1)).ToString();
        }
        
        public string PartTwo(string[] input)
        {
            //Assumption: Only ever gaps of 1 or 3 in the input space.
            var jolts =  input.Select(int.Parse).OrderBy(x => x).ToList();

            //***DP***//
            jolts.Insert(0,0);
            seensValues[jolts.Count - 1] = 1;
            
            return Part2DynamicProgramming(0, jolts).ToString();
            
            //My Original Solution
            return PartTwoOriginal(jolts);
        }

        private string PartTwoOriginal(List<int> jolts)
        {
            var joltDifferences = jolts
                .Select((x, idx) => idx == 0 ? x : (x - jolts[idx - 1]));

            return string.Join("", joltDifferences)
                .Split("3").Select(x => gapCombos[x.Length])
                .Aggregate((agg, nxt) => agg * nxt)
                .ToString();
        }

        //Calculated these by hand. There probably is a sequence here to generate any combo
        private Dictionary<int, long> gapCombos = new()
        {
            {0, 1},
            {1, 1},
            {2, 2},
            {3, 4},
            {4, 7}
        };

        private Dictionary<int,long> seensValues = new();

        private long Part2DynamicProgramming(int idx, List<int> list)
        {
            var j = idx+1;

            if (seensValues.ContainsKey(idx))
            {
                return seensValues[idx];
            }
            long pathsAtIdx = 0;
            while (j < list.Count && list[j] - list[idx] <= 3)
            {
                pathsAtIdx += Part2DynamicProgramming(j, list);
                j++;
            }

            seensValues.Add(idx, pathsAtIdx);
            return pathsAtIdx;

        }
        
        
        public int Day => 10;
    }
}