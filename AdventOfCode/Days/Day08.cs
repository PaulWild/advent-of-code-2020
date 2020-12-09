using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day08 : ISolution
    {
        public string PartOne(string[] input)
        {
            var instructions = ParseInstructions(input);

            return ProcessInstructions(instructions).value.ToString();
        }

        
        public string PartTwo(string[] input)
        {
            var instructions = ParseInstructions(input);

            for (var i = 0; i < instructions.Count; i++)
            {
                var newIns = instructions.Select(x => x).ToList();
                switch (newIns[i].ins)
                {
                    case "acc":
                        continue;
                    case "jmp":
                        newIns[i] = ("nop", newIns[i].val);
                        break;
                    case "nop":
                        newIns[i] = ("jmp", newIns[i].val);
                        break;
                }

                try
                {
                    var (value, infLoop) = ProcessInstructions(newIns);
                    
                    if (!infLoop)
                        return value.ToString();
                }
                catch (ArgumentOutOfRangeException)
                {
                    //ignore 
                }
            }

            return "-1";
        }
        
        private static List<(string ins, int val)> ParseInstructions(string[] input)
        {
            List<(string ins, int val)> instructions = input
                .Select(x => x.Split(" "))
                .Select(x => (x[0], int.Parse(x[1])))
                .ToList();
            return instructions;
        }

        private static (int value, bool infLoop) ProcessInstructions(List<(string ins, int val)> instructions)
        {
            var seenInstructions = new HashSet<int>();
            var instruction = 0;
            var accumulator = 0;
            var looped = false;
            
            while (instruction != instructions.Count)
            {
                if (seenInstructions.Contains(instruction))
                {
                    looped = true;
                    break;
                }
                
                seenInstructions.Add(instruction);
                
                switch (instructions[instruction].ins)
                {
                    case "acc":
                        accumulator += instructions[instruction].val;
                        instruction++;
                        break;
                    case "jmp":
                        instruction += instructions[instruction].val;
                        break;
                    case "nop":
                        instruction++;
                        break;
                }
            }

            return (accumulator, looped);
        }

        public int Day => 08;
    }
}