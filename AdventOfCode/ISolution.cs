﻿using System.IO;
using System.Reflection;

namespace AdventOfCode
{
    public interface ISolution
    {
        string PartOne(string[] input);

        string PartTwo(string[] input);
        
        int Day { get; }

        string[] Input()
        {
            var filePath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? string.Empty, 
                $"Input/day_{Day}.txt");
            return File.ReadAllLines(filePath);
        }
    }
}