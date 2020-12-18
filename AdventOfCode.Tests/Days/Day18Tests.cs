using System;
using AdventOfCode.Days;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Tests.Days
{
    
    public class Day18Tests
    {
        private readonly ISolution _sut = new Day18();
        
        [Fact]
        public void PartOne_WhenCalled_DoesNotThrowNotImplementedException()
        {
            Action act = () =>  _sut.PartOne(_sut.Input());
            
            act.Should().NotThrow<NotImplementedException>();
        }

        private string[] exampleOne = new[]
        {
            "1 + (2 * 3) + (4 * (5 + 6))"
        };
        
        [Fact]
        public void PartOne_WhenCalled_WorksWithExampleOne()
        {
            var res =   _sut.PartOne(exampleOne);
            
            res.Should().Be("51");
        }
        
        private string[] exampleTwo = new[]
        {
            "((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2"
        };
        
        [Fact]
        public void PartOne_WhenCalled_WorksWithExampleTwo()
        {
            var res =  _sut.PartOne(exampleTwo);
            
            res.Should().Be("13632");
        }
        
        [Fact]
        public void PartTwo_WhenCalled_DoesNotThrowNotImplementedException()
        {
            Action act = () =>  _sut.PartTwo(_sut.Input());
            
            act.Should().NotThrow<NotImplementedException>();
        }
        
        [Fact]
        public void PartTwo_WhenCalled_WorksWithExampleTwo()
        {
            var res =  _sut.PartTwo(exampleTwo);
            
            res.Should().Be("23340");
        }
        
        [Fact]
        public void PartTwo_WhenCalled_WorksWithExampleOne()
        {
            var res =   _sut.PartTwo(exampleOne);
            
            res.Should().Be("51");
        }
        
        private string[] exampleFour = new[]
        {
            "1 + 2 * 3 + 4 * 5 + 6",
            "1 + (2 * 3) + (4 * (5 + 6))",
            "2 * 3 + (4 * 5)",
            "5 + (8 * 3 + 9 + 3 * 4 * 3)",
            "5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))",
            "((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2"
        };

        [Fact]
        public void PartTwo_WhenCalled_WorksWithExampleFour()
        {
            var res =   _sut.PartTwo(exampleFour);
            
            res.Should().Be("694173");
        }
        
        private string[] exampleThree = new[]
        {
            "5 * 2 * (4 + 8) + 8 * 3 + (2 + 8 + 8 * 6 * 9)"
        };

        [Fact]
        public void PartTwo_WhenCalled_WorksWithExampleThree()
        {
            var res =   _sut.PartTwo(exampleThree);
            
            res.Should().Be("195000");
        }

    }
}