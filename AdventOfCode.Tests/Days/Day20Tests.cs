using System;
using AdventOfCode.Days;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Tests.Days
{
    public class Day20Tests
    {
        private string[] example =
        {
"Tile 2311:",
"..##.#..#.",
"##..#.....",
"#...##..#.",
"####.#...#",
"##.##.###.",
"##...#.###",
".#.#.#..##",
"..#....#..",
"###...#.#.",
"..###..###",
"",
"Tile 1951:",
"#.##...##.",
"#.####...#",
".....#..##",
"#...######",
".##.#....#",
".###.#####",
"###.##.##.",
".###....#.",
"..#.#..#.#",
"#...##.#..",
"",
"Tile 1171:",
"####...##.",
"#..##.#..#",
"##.#..#.#.",
".###.####.",
"..###.####",
".##....##.",
".#...####.",
"#.##.####.",
"####..#...",
".....##...",
"",
"Tile 1427:",
"###.##.#..",
".#..#.##..",
".#.##.#..#",
"#.#.#.##.#",
"....#...##",
"...##..##.",
"...#.#####",
".#.####.#.",
"..#..###.#",
"..##.#..#.",
"",
"Tile 1489:",
"##.#.#....",
"..##...#..",
".##..##...",
"..#...#...",
"#####...#.",
"#..#.#.#.#",
"...#.#.#..",
"##.#...##.",
"..##.##.##",
"###.##.#..",
"",
"Tile 2473:",
"#....####.",
"#..#.##...",
"#.##..#...",
"######.#.#",
".#...#.#.#",
".#########",
".###.#..#.",
"########.#",
"##...##.#.",
"..###.#.#.",
"",
"Tile 2971:",
"..#.#....#",
"#...###...",
"#.#.###...",
"##.##..#..",
".#####..##",
".#..####.#",
"#..#.#..#.",
"..####.###",
"..#.#.###.",
"...#.#.#.#",
"",
"Tile 2729:",
"...#.#.#.#",
"####.#....",
"..#.#.....",
"....#..#.#",
".##..##.#.",
".#.####...",
"####.#.#..",
"##.####...",
"##..#.##..",
"#.##...##.",
"",
"Tile 3079:",
"#.#.#####.",
".#..######",
"..#.......",
"######....",
"####.#..#.",
".#...#.##.",
"#.#####.##",
"..#.###...",
"..#.......",
"..#.###...",
"",
        };
        
        private readonly ISolution _sut = new Day20();
        
        [Fact]
        public void PartOne_WhenCalled_DoesNotThrowNotImplementedException()
        {
            Action act = () =>  _sut.PartOne(_sut.Input());
            
            act.Should().NotThrow<NotImplementedException>();
        }
        
                
        [Fact]
        public void PartOne_WhenCalled_WorksWithExample()
        {
            var res =  _sut.PartOne(example);

            res.Should().Be("20899048083289");
        }
        
        [Fact]
        public void PartTwo_WhenCalled_DoesNotThrowNotImplementedException()
        {
            Action act = () =>  _sut.PartTwo(_sut.Input());
            
            act.Should().NotThrow<NotImplementedException>();
        }
        
        [Fact]
        public void PartTwo_WhenCalled_WorksWithExample()
        {
            var res =  _sut.PartTwo(example);

            res.Should().Be("273");
        }
    }
}