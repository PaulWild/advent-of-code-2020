using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day20 : ISolution
    {
        public string PartOne(string[] input)
        {
            var tiles = ParseInput(input);
            var tileSides = TilesWithMatchingEdges(tiles);
            var distinctTiles = tiles.Select(x => x.Id.Id).Distinct().Count();
            var images = BuildImage(new List<TileSides>(), tileSides, (int) Math.Sqrt(distinctTiles)).ToList();
            
            var first = images.Last().Select(x => x.Id.Id).ToList();
            
            return ((long)first[0] * first.Last() * first[(int)Math.Sqrt(distinctTiles)-1] *
                first[^((int) Math.Sqrt(distinctTiles))]).ToString();
        }

        private static List<TileSides> TilesWithMatchingEdges(List<Tile> tiles)
        {
            var tileSides = new List<TileSides>();
            foreach (var tile in tiles)
            {
                List<(int Id, int Rotation, bool Flipped)> top = new();
                List<(int Id, int Rotation, bool Flipped)> left = new();
                foreach (var other in tiles.Where(other => tile.Id.Id != other.Id.Id))
                {
                    if (tile.TopMatch(other))
                        top.Add(other.Id);
                    if (tile.LeftMatch(other))
                        left.Add(other.Id);
                }

                tileSides.Add(new TileSides()
                {
                    Id = tile.Id,
                    Left = left,

                    Top = top
                });
            }

            return tileSides;
        }


        public IEnumerable<List<TileSides>> BuildImage(List<TileSides> currentImage, List<TileSides> potentialTiles, int imageWidth)
        {
            if (currentImage.Count == imageWidth*imageWidth)
            {
                yield return currentImage;
            }
            else
            {
                var position = currentImage.Count + 1;

                var foo = potentialTiles.Select(x => x);
                if ((position - 1) % imageWidth != 0)
                {
                    //needs a left hand side check
                    var toCheck = currentImage[^1];
                    foo = foo.Where(x => x.Left.Contains(toCheck.Id)).ToList();
                }

                if (position - imageWidth > 0)
                {
                    //need a top check
                    var toCheck = currentImage[^imageWidth];
                    foo = foo.Where(x => x.Top.Contains(toCheck.Id)).ToList();
                }


                foreach (var potential in foo)
                {
                    var newCurrent = currentImage.Select(x => x).ToList();
                    newCurrent.Add(potential);

                    var newPotential = potentialTiles.Where(x => x.Id.Id != potential.Id.Id).ToList();

                    var blah = BuildImage(newCurrent, newPotential, imageWidth);
                    foreach (var image in blah)
                    {
                        yield return image;
                    }
                }
            }
        }
        

        public string PartTwo(string[] input)
        {
            var tiles = ParseInput(input);
            var tileSides = TilesWithMatchingEdges(tiles);
            var distinctTiles = tiles.Select(x => x.Id.Id).Distinct().Count();
            var images = BuildImage(new List<TileSides>(), tileSides, (int) Math.Sqrt(distinctTiles)).ToList();
            
            var tileWidth = tiles.First().Length;
            var unpaddedTileWidth = tileWidth - 2;
            var width = Math.Sqrt(distinctTiles);
            
            
            foreach (var image in images)
            {
                var resultsOne = image
                    .Select(x => tiles.Single(y => y.Id == x.Id))
                    .Select(x => x.Data.Where(point => point.x != 0 && point.x != (tileWidth-1) && point.y != 0 && point.y != tileWidth-1)).ToList();
                    
                    var results = resultsOne.Select((x, idx) => x.Select(point =>
                    {
                        int dx;
                        if (idx < width)
                        {
                            dx = idx * unpaddedTileWidth;
                        }
                        else
                        {
                            dx = (int)(idx % width) * unpaddedTileWidth;
                        }
                        var dy = (int)(idx / width) * unpaddedTileWidth;
                        return (point.x -1 + dx,
                            (int) (point.y-1 +dy));
                    }).ToList());
                
                var dataPoints = results.Aggregate((agg, nxt) =>
                {
                    agg.AddRange(nxt);
                    return agg;
                }).ToHashSet();
                
                var count = dataPoints.Count(dataPoint => IsSeaMonsterAt(dataPoint, dataPoints));

                if (count > 0)
                    return (dataPoints.Count - (count * 15)).ToString();
            }

            return "-1";
        }

        public bool IsSeaMonsterAt((int x, int y) startingPoint, HashSet<(int x, int y)> allPoints)
        {
            var (x, y) = startingPoint;
            return allPoints.Contains((x + 5, y)) &&
                   allPoints.Contains((x + 6, y)) &&
                   allPoints.Contains((x + 11, y)) &&
                   allPoints.Contains((x + 12, y)) &&
                   allPoints.Contains((x + 17, y)) &&
                   allPoints.Contains((x + 18, y)) &&
                   allPoints.Contains((x + 19, y)) &&
                   allPoints.Contains((x + 18, y - 1)) &&
                   allPoints.Contains((x + 1, y + 1)) &&
                   allPoints.Contains((x + 4, y + 1)) &&
                   allPoints.Contains((x + 7, y + 1)) &&
                   allPoints.Contains((x + 10, y + 1)) &&
                   allPoints.Contains((x + 13, y + 1)) &&
                   allPoints.Contains((x + 16, y + 1));
        }

        public List<Tile> ParseInput(string[] input)
        {
            List<Tile> tiles = new();
            Tile tile = new();
            
            var y = 0;
            var length = 0;
            HashSet<(int x, int y)> data = new();
            foreach (var row in input)
            {
                if (row.StartsWith("Tile "))
                {
                    tile = tile with {
                        Id = (int.Parse(row.Split(" ")[1].Replace(":", "")), 0, false),
                        Data = data
                    };
                }
                else if (string.IsNullOrWhiteSpace(row))
                {
                    tile = tile with { Length = length};
                    tiles.Add(tile);
                    tiles.Add(tile.Rotate(90));
                    tiles.Add(tile.Rotate(180));
                    tiles.Add(tile.Rotate(270));

                    var flipped = tile.Flip();
                    tiles.Add(flipped);
                    tiles.Add(flipped.Rotate(90));
                    tiles.Add(flipped.Rotate(180));
                    tiles.Add(flipped.Rotate(270));      
                    
                    //reset 
                    y = 0;
                    data = new HashSet<(int x, int y)>();
                    tile = new Tile();
                }
                else
                {
                    var x = 0;
                    length = row.Length;
                    foreach (var chr in row.ToCharArray())
                    {
                        
                        if (chr == '#')
                            data.Add((x, y));
                        x++;
                    }

                    y++;
                }
              
            }

            return tiles;
        }

        public record Tile
        {
            public (int Id, int Rotation, bool Flipped) Id { get; init; }
            
            //Length of a side
            public int Length { get; init; }
            
            //HasSet of '#' points
            public HashSet<(int x, int y)> Data { get; init; }

            public bool TopMatch(Tile other)
            {
                var thisEdge = Data.Where(t => t.y == 0).Select(x => x.x).ToHashSet();
                var otherEdge = other.Data.Where(t => t.y == Length-1).Select(x => x.x).ToHashSet();

                return thisEdge.SetEquals(otherEdge);
            }

            public bool LeftMatch(Tile other)
            {
                var thisEdge = Data.Where(t => t.x == 0).Select(x => x.y).ToHashSet();
                var otherEdge = other.Data.Where(t => t.x == Length-1).Select(x => x.y).ToHashSet();

                return thisEdge.SetEquals(otherEdge);           
            }
            
            public Tile Rotate(int rotation)
            {
                var newData = new HashSet<(int x, int y)>();
                foreach (var point in Data)
                {
                    var x1 = point.x - (Length-1)/2d;
                    var y1 = point.y - (Length-1)/2d;

                    var angle = (rotation * Math.PI) / 180;

                    var newX = x1 * Math.Cos(angle) - y1 * Math.Sin(angle);
                    var newY = x1 * Math.Sin(angle) + y1 * Math.Cos(angle);
                    
                    var x = Math.Round(newX + (Length-1)/2d);
                    var y=  Math.Round(newY + (Length-1)/2d);

                    newData.Add(((int)x, (int)y));
                }

                return this with {Id = (Id.Id, rotation, Id.Flipped), Data = newData};
            }

            public Tile Flip()
            {
                var newData = new HashSet<(int x, int y)>();
                foreach (var (i, y) in Data)
                {
                    var x = Length - 1 - i;
                    newData.Add((x, y));
                }

                return this with {Id = (Id.Id, Id.Rotation, true), Data = newData};
            }
            
        }
        
        public record TileSides
        {
            public (int Id, int Rotation, bool Flipped) Id { get; init; }
            
            public List<(int Id, int Rotation, bool Flipped)> Top { get; init; }
            
            public List<(int Id, int Rotation, bool Flipped)> Left { get; init; }

        }

        public int Day => 20;
    }
    
}