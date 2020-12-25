using System;
using AdventOfCode.Days;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Tests.Days
{
    
    public class Day25Tests
    {
        private readonly ISolution _sut = new Day25();
        
        [Fact]
        public void PartOne_WhenCalled_DoesNotThrowNotImplementedException()
        {
            Action act = () =>  _sut.PartOne(_sut.Input());
            
            act.Should().NotThrow<NotImplementedException>();
        }

        private string[] example = new[]
        {
            "5764801",
            "17807724"
        };
        
        [Fact]
        public void PartOne_WhenCalled_WorksWithExample()
        {
            var results =  _sut.PartOne(example);
            
            results.Should().Be("14897079");
        }
        
        [Fact]
        public void PartTwo_WhenCalled_DoesNotThrowNotImplementedException()
        {
            Action act = () =>  _sut.PartTwo(_sut.Input());
            
            act.Should().NotThrow<NotImplementedException>();
        }
    }
}