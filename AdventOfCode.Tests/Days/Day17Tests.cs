using System;
using AdventOfCode.Days;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Tests.Days
{
    
    public class Day17Tests
    {
        private readonly ISolution _sut = new Day17();
        
        [Fact]
        public void PartOne_WhenCalled_DoesNotThrowNotImplementedException()
        {
            Action act = () =>  _sut.PartOne(_sut.Input());
            
            act.Should().NotThrow<NotImplementedException>();
        }
        
        [Fact]
        public void PartTwo_WhenCalled_DoesNotThrowNotImplementedException()
        {
            Action act = () =>  _sut.PartTwo(_sut.Input());
            
            act.Should().NotThrow<NotImplementedException>();
        }

        private string[] example = new[]
        {
            ".#.",
            "..#",
            "###"
        };
        
        [Fact]
        public void PartOne_WhenCalled_WorksWithExampleData()
        {
            var res  = _sut.PartOne(example);
            
            res.Should().Be("112");
        }
    }
}