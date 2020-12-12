using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    public class Day12 : ISolution
    {
        public string PartOne(string[] input)
        {
            var instructions = ParseInstructions(input);
            
            var ship = new Ship() {Location = (0, 0), Direction = (1, 0)};
            ship = instructions.Aggregate(ship, (current, instruction) => ProcessDirection(instruction, current));

            return (Math.Abs(ship.Location.x) + Math.Abs(ship.Location.y)).ToString();
        }

        private static IEnumerable<Instruction> ParseInstructions(string[] input)
        {
            var instructions = input
                .Select(x => Regex.Match(x, @"(?'direction'[A-Z])(?'distance'\d+)"))
                .Select(x => new Instruction
                {
                    Direction = x.Groups["direction"].Value,
                    Distance = int.Parse(x.Groups["distance"].Value)
                });
            return instructions;
        }

        private Ship ProcessDirection(Instruction instruction, Ship currentShip)
        {
            return instruction.Direction switch
            {
                "N" => currentShip with { Location =
                    (currentShip.Location.x, currentShip.Location.y - instruction.Distance) },
                "E" => currentShip with { Location =
                    (currentShip.Location.x + instruction.Distance, currentShip.Location.y) },
                "S" => currentShip with { Location =
                    (currentShip.Location.x, currentShip.Location.y + instruction.Distance) },
                "W" => currentShip with { Location =
                    (currentShip.Location.x - instruction.Distance, currentShip.Location.y) },
                "L" => currentShip with { Direction = Turn(currentShip.Direction, instruction)},
                "R" => currentShip with { Direction = Turn(currentShip.Direction, instruction)},
                "F" => currentShip with { Location = (
                    currentShip.Location.x + instruction.Distance * currentShip.Direction.dx,
                    currentShip.Location.y + instruction.Distance * currentShip.Direction.dy) },
                _ => throw new Exception("invalid direction")
            };
        }
        
        private Ship ProcessDirectionPartTwo(Instruction instruction, Ship currentShip)
        {
            return instruction.Direction switch
            {
                "N" => currentShip with { Direction =
                    (currentShip.Direction.dx, currentShip.Direction.dy - instruction.Distance) },
                "E" => currentShip with { Direction =
                    (currentShip.Direction.dx + instruction.Distance, currentShip.Direction.dy) },
                "S" => currentShip with { Direction =
                    (currentShip.Direction.dx, currentShip.Direction.dy + instruction.Distance) },
                "W" => currentShip with { Direction =
                    (currentShip.Direction.dx - instruction.Distance, currentShip.Direction.dy) },
                "L" => currentShip with { Direction = Turn(currentShip.Direction, instruction)},
                "R" => currentShip with { Direction = Turn(currentShip.Direction, instruction)},
                "F" => currentShip with { Location = (
                    currentShip.Location.x + instruction.Distance * currentShip.Direction.dx,
                    currentShip.Location.y + instruction.Distance * currentShip.Direction.dy) },
                _ => throw new Exception("invalid direction")
            };
        }

        private (int dx, int dy) Turn((int dx, int dy) currentDirection, Instruction instruction)
        {
            for (var i = 1; i <= instruction.Distance / 90; i++)
            {
                currentDirection = instruction.Direction switch
                {
                    "R" => (currentDirection.dy * -1, currentDirection.dx),
                    "L" => (currentDirection.dy, currentDirection.dx * -1),
                    _ => currentDirection
                };
            }

            return currentDirection;
        }

        public string PartTwo(string[] input)
        {
            var instructions = ParseInstructions(input);
            
            var ship = new Ship() {Location = (0, 0), Direction = (10, -1)};
            ship = instructions.Aggregate(ship, (current, instruction) => ProcessDirectionPartTwo(instruction, current));

            return (Math.Abs(ship.Location.x) + Math.Abs(ship.Location.y)).ToString();
        }
        

        public int Day => 12;
        
        private record Instruction
        {
            public string Direction { get; init; }
            
            public int Distance { get; init; }
        }

        private record Ship
        {
            public (int x, int y) Location { get; init; }
            
            public (int dx, int dy) Direction { get; init; }
        }
    }
}