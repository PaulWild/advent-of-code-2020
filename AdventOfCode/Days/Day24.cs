using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    public class Day24 : ISolution
    {

        private const decimal R = 1M;
        private readonly Dictionary<string, (decimal x, decimal y)> _delta;

        public Day24()
        {
            var x = 1.5M * R * (decimal)Math.Cos(Math.PI / 3);
            var y = 1.5M * R * (decimal)Math.Sin(Math.PI / 3);
            
            _delta = new Dictionary<string, (decimal x, decimal y)>()
            {
                {"e", (1.5M * R, 0)},
                {"w", (-1.5M * R, 0)},
                {"ne", (x, y * -1)},
                {"nw", (x * -1, y * -1)},
                {"se", (x, y)},
                {"sw", (x * -1, y)},
            };
        }
        
        public string PartOne(string[] input)
        {
            var tiles = GetStartingTiles(input);
            
            return tiles.Values.Count(x => !x).ToString();
        }

        private Dictionary<(decimal x, decimal y), bool> GetStartingTiles(string[] input)
        {
            var tiles = new Dictionary<(decimal x, decimal y), bool>();
            
            foreach (var instructions in ParseInput(input))
            {
                (decimal x, decimal y) pos = (0, 0);
                foreach (var (x, y) in instructions.Select(instruction => _delta[instruction]))
                {
                    pos.x += x;
                    pos.y += y;
                }

                if (tiles.ContainsKey(pos))
                {
                    tiles[pos] = !tiles[pos];
                }
                else
                {
                    tiles[pos] = false;
                }
            }

            return tiles;
        }

        private static IEnumerable<List<string>> ParseInput(IEnumerable<string> input)
        {
            var regex = new Regex(@"e|se|sw|w|nw|ne");
            foreach (var row in input)
            {
                var matches = regex.Matches(row);

                var instructions = new List<string>();
                foreach (Match match in matches)
                {
                    instructions.Add(match.Value);
                }

                yield return instructions;
            }
        }

        public string PartTwo(string[] input)
        {
            var tiles = GetStartingTiles(input);

            for (var i = 0; i < 100; i++)
            {
                var tileCounts = new Dictionary<(decimal x, decimal y), (bool colour, int blackNeighbours)>();

                foreach (var (pos, colour) in tiles)
                {
                    foreach (var (_, (dx, dy)) in _delta)
                    {
                        var neighbour = pos;
                        neighbour.x += dx;
                        neighbour.y += dy;

                        if (!tileCounts.ContainsKey(neighbour))
                        {
                            var newColour = true;
                            if (tiles.ContainsKey(neighbour))
                                newColour = tiles[neighbour];
                            tileCounts[neighbour] = (newColour, 0);
                        }

                        var val = tileCounts[neighbour];

                        if (!colour)
                        {
                            val.blackNeighbours++;
                        }

                        tileCounts[neighbour] = val;
                    }
                }

                var newTiles = new Dictionary<(decimal x, decimal y), bool>();
                foreach (var (pos, (colour, blackNeighbours)) in tileCounts)
                {
                    switch (colour)
                    {
                        case false when blackNeighbours == 0 || blackNeighbours > 2:
                            break;
                        case true when blackNeighbours == 2:
                        case false:
                            newTiles.Add(pos, false);
                            break;
                    }
                }

                tiles = newTiles;


            }

            return tiles.Values.Count(x => !x).ToString();
        }

        public int Day => 24;
    }
}