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
            var images = BuildImage(new List<TileSides>(), tileSides, (int) Math.Sqrt(distinctTiles));
            
            var first = images.First().Select(x => x.Id.Id).ToList();
            
            return (
                    (long)first[0] * 
                    first.Last() * 
                    first[(int)Math.Sqrt(distinctTiles)-1] *
                    first[^(int) Math.Sqrt(distinctTiles)]
                ).ToString();
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

                var nextPotentialTiles = potentialTiles.Select(x => x);
                if ((position - 1) % imageWidth != 0)
                {
                    //needs a left hand side check
                    var toCheck = currentImage[^1];
                    nextPotentialTiles = nextPotentialTiles.Where(x => x.Left.Contains(toCheck.Id)).ToList();
                }

                if (position - imageWidth > 0)
                {
                    //need a top check
                    var toCheck = currentImage[^imageWidth];
                    nextPotentialTiles = nextPotentialTiles.Where(x => x.Top.Contains(toCheck.Id)).ToList();
                }


                foreach (var potential in nextPotentialTiles)
                {
                    var newCurrent = currentImage.Select(x => x).ToList();
                    newCurrent.Add(potential);

                    var newPotential = potentialTiles.Where(x => x.Id.Id != potential.Id.Id).ToList();

                    var images = BuildImage(newCurrent, newPotential, imageWidth);
                    foreach (var image in images)
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
            
            foreach (var image in images)
            {
                var imageWidth = (int)Math.Sqrt(image.Count);
                var results = image
                    .Select(x => tiles.Single(y => y.Id == x.Id))
                    .Select(t => t.RemoveEdges())
                    .Select((t, idx) => t.ShiftBy(AsXYLocation(idx, imageWidth)))
                    .Select(t => t.Data)
                    .Aggregate((agg, nxt) =>
                    {
                        agg.UnionWith(nxt);
                        return agg;
                    });
                
                var count = results.Count(dataPoint => IsSeaMonsterAt(dataPoint, results));

                if (count > 0)
                    return (results.Count - (count * 15)).ToString();
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
            
            public int Length { get; init; }
            
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

            public Tile RemoveEdges()
            {
                var data = Data.Where(point =>
                    point.x != 0 &&
                    point.x != (Length - 1) 
                    && point.y != 0 && 
                    point.y != Length - 1).ToHashSet();

                return this with {
                    Data = data,
                    Length = Length - 2
                    };
            }

            public Tile ShiftBy((int x, int y) coord)
            {
                var (x, y) = coord;
                var data = Data.Select(point => (point.x + x * Length, point.y + y * Length)).ToHashSet();
                return this with { Data = data};
            }
        }

        //Assumes square
        public (int x, int y) AsXYLocation(int idx, int length)
        {
            if (idx < length)
            {
                return (idx, 0);
            }

            var x = idx % length;
            var y = idx / length;
            return (x, y);

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