using System;
using AdventOfCode.Days;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Tests.Days
{
    
    public class Day12Tests
    {
        private readonly ISolution _sut = new Day12();
        
        [Fact]
        public void PartOne_WhenCalled_DoesNotThrowNotImplementedException()
        {
            Action act = () =>  _sut.PartOne(_sut.Input());
            
            act.Should().NotThrow<NotImplementedException>();
        }
        
        [Fact]
        public void PartOne_WhenCalled_WorksWIthTestData()
        {
            var input = new[]
            {
                "F10",
                "N3",
                "F7",
                "R90",
                "F11"
            };
            
            var res =   _sut.PartOne(input);

            res.Should().Be("25");
        }
        
        [Fact]
        public void PartTwo_WhenCalled_DoesNotThrowNotImplementedException()
        {
            Action act = () =>  _sut.PartTwo(_sut.Input());
            
            act.Should().NotThrow<NotImplementedException>();
        }
        
        [Fact]
        public void PartTwo_WhenCalled_WorksWIthTestData()
        {
            var input = new[]
            {
                "F10",
                "N3",
                "F7",
                "R90",
                "F11"
            };
            
            var res =   _sut.PartTwo(input);

            res.Should().Be("286");
        }
    }
}