﻿using System;
using AdventOfCode.Days;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Tests.Days
{
    
    public class Day0Tests
    {
        private readonly ISolution _sut = new Day0();
        
        [Fact(Skip = "Scaffold")]
        public void PartOne_WhenCalled_DoesNotThrowNotImplementedException()
        {
            Action act = () =>  _sut.PartOne(_sut.Input());
            
            act.Should().NotThrow<NotImplementedException>();
        }
        
        [Fact(Skip = "Scaffold")]
        public void PartTwo_WhenCalled_DoesNotThrowNotImplementedException()
        {
            Action act = () =>  _sut.PartTwo(_sut.Input());
            
            act.Should().NotThrow<NotImplementedException>();
        }
    }
}