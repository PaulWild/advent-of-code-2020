using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Days
{
    public class Day17 : ISolution
    {
        public string PartOne(string[] input)
        {
            var liveStates = ParseInput(input);
            var initialCubeSize = input.Length/2 +1;

            var minBound=-initialCubeSize;
            var maxBound = initialCubeSize;
            for (var iter = 0; iter < 6; iter++)
            {
                var newStates = new HashSet<(int x, int y, int z, int w)>();
                
                for (var z = minBound; z <= maxBound; z++)
                for (var y = minBound; y <= maxBound; y++)
                for (var x = minBound; x <= maxBound; x++)
                {
                    var neighbourCount = Grid.Neighbours3(x, y, z)
                        .Count(coord => liveStates.Contains((coord.x, coord.y, coord.z, 0)));

                    if (ActiveAfterGeneration(liveStates.Contains((x, y, z, 0)), neighbourCount))
                    {
                        newStates.Add((x, y, z, 0));
                    }
                }
                liveStates = newStates;
                minBound--;
                maxBound++;
            }

            return liveStates.Count.ToString();
        }
        
        public string PartTwo(string[] input)
        {
            var liveStates = ParseInput(input);
            var initialCubeSize = (input.Length/2 +1);

            var minBound=-initialCubeSize;
            var maxBound = initialCubeSize;
            for (var iter = 0; iter < 6; iter++)
            {
                var newStates = new HashSet<(int x, int y, int z, int w)>();
                
                for (var w = minBound; w <= maxBound; w++)
                for (var z = minBound; z <= maxBound; z++)
                for (var y = minBound; y <= maxBound; y++)
                for (var x = minBound; x <= maxBound; x++)
                {
                    var neighbourCount = Grid.Neighbours4(x, y, z, w)
                        .Count(coord => liveStates.Contains((coord.x, coord.y, coord.z, coord.w)));

                    if (ActiveAfterGeneration(liveStates.Contains((x, y, z, w)), neighbourCount))
                    {
                        newStates.Add((x, y, z, w));
                    }
                }
                liveStates = newStates;
                minBound--;
                maxBound++;
            }

            return liveStates.Count.ToString();
        }

        private bool ActiveAfterGeneration(bool active, int neighbours)
        {
            switch (active)
            {
                case true when (neighbours == 2 || neighbours == 3):
                case false when (neighbours == 3):
                    return true;
            }

            return false;
        }
        
        private static HashSet<(int x, int y, int z, int w)> ParseInput(string[] input)
        {
            var initialCubeSize = input.Length/2;
            HashSet<(int x, int y, int z, int w)> liveStates = new(); 
            var asCharArray = input.Select(x => x.ToCharArray()).ToList();

            for (var y = 0; y < asCharArray[0].Length; y++)
            for (var x = 0; x < asCharArray[0].Length; x++)
            {
                if (asCharArray[y][x] == '#')
                    liveStates.Add((x-initialCubeSize,y-initialCubeSize,0,0)); // start around 0;
            }

            return liveStates;
        }

        public int Day => 17;
    }
}