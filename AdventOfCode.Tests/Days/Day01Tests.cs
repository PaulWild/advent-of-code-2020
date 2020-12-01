using System;
using AdventOfCode.Days;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Tests.Days
{
    
    public class Day01Tests
    {
        private readonly ISolution _sut = new Day01();
        
        [Fact]
        public void PartOne_WhenCalled_DoesNotThrowNotImplementedException()
        {
            Action act = () =>  _sut.PartOne(_sut.Input());
            
            act.Should().NotThrow<NotImplementedException>();
        }
        
        [Fact]
        public void PartOne_WhenCalled_Works()
        {
            var input = new [] {
                "1721",
                "979",
                "366",
                "299",
                "675",
                "1456",
            };
            
            var res =  _sut.PartOne(input);
            
            res.Should().Be("514579");
        }
        
        [Fact]
        public void PartTwo_WhenCalled_DoesNotThrowNotImplementedException()
        {
            Action act = () =>  _sut.PartTwo(_sut.Input());
            
            act.Should().NotThrow<NotImplementedException>();
        }
        
        
        [Fact]
        public void PartTwo_WhenCalled_Works()
        {
            var input = new [] {
                "1721",
                "979",
                "366",
                "299",
                "675",
                "1456",
            };
            
            var res =  _sut.PartTwo(input);
            
            res.Should().Be("241861950");
        }

    }
}