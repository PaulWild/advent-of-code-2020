using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Program
    {
        private const string Year = "2020";
        
        static Task<int> Main(string[] args)
        {
            var aoc = new Command("aoc", "run advent of code solutions")
            {
                new Argument<int?>("day", "Day to run."),
                new Option<int?>(new[] { "--part", "-p" }, "The part to run"),
            };
            aoc.Handler = CommandHandler.Create<int?, int?, IConsole>(RunSolutionForDay);

            var bootstrap = new Command("bootstrap", "bootstrap a new advent of code day")
            {
                new Argument<int>("day", "day to bootstrap")
            };
            bootstrap.Handler = CommandHandler.Create<int, IConsole>(BootStrap);
            
            var root = new RootCommand
            {
                aoc,
                bootstrap
            };
            
            return root.InvokeAsync(args);
        }

        static void RunSolutionForDay(int? day, int? part, IConsole console)
        {
            var solutions = GetSolutionsToRun(day);
            
            foreach (var solution in solutions)
            {
                console.Out.WriteLine($"Advent of code - Day {solution.Day}");

                if (!part.HasValue || part == 1)
                {
                    RunSolutionPart(() => solution.PartOne(solution.Input()), 1, console);
                }
                if (!part.HasValue || part == 2)
                {
                    RunSolutionPart(() => solution.PartTwo(solution.Input()), 2, console);
                }
            }
        }

        private static IEnumerable<ISolution> GetSolutionsToRun(int? day)
        {
            List<ISolution> solutions = new();
            if (day.HasValue)
            {
                var sol = Solutions().SingleOrDefault(x => x.Day == day);
                if (sol != null)
                {
                    solutions.Add(sol);
                }
            }
            else
            {
                solutions.AddRange(Solutions());
            }

            return solutions;
        }

        private static void RunSolutionPart(Func<string> solutionFunc, int part, IConsole console)
        {
            try
            {
                var answer = solutionFunc();
                console.Out.WriteLine(SolutionText(part, answer));
            }
            catch (NotImplementedException)
            {
                console.Error.WriteLine(ErrorText(part));
            }
        }

        private static string SolutionText(int part, string answer) => $"\tAnswer to Part {part} is: {answer}";

        private static string ErrorText(int part) => $"\tPart {part} has not been solved";

        private static IEnumerable<ISolution> Solutions()
        {
            var type = typeof(ISolution);

            return Assembly.GetExecutingAssembly()?.DefinedTypes
                .Where(x => x.ImplementedInterfaces.Contains(type))
                .Select(impl => (ISolution)Activator.CreateInstance(impl))
                .OrderBy(sol => sol?.Day);
        }

        private static async Task BootStrap(int day, IConsole console)
        {
            var padding = day > 9 ? "" : "0";
            
            //Main File
            var text = await File.ReadAllTextAsync("./AdventOfCode/Days/Day00.cs");
            var newText = text.Replace("00", padding + day);
            await File.WriteAllTextAsync($"./AdventOfCode/Days/Day{padding}{day}.cs", newText);
           
            //Test File
            var testText = await File.ReadAllTextAsync("./AdventOfCode.Tests/Days/Day00Tests.cs");
            var newTestText = testText.Replace("00", padding + day).Replace("(Skip = \"Scaffold\")", "");
            await File.WriteAllTextAsync($"./AdventOfCode.Tests/Days/Day{padding}{day}Tests.cs", newTestText);
            
            //Input Data
            var session = Environment.GetEnvironmentVariable("AOC_SESSION_ID");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("cookie", $"session={session}");
            
            var inputString = await client.GetStringAsync($"https://adventofcode.com/{Year}/day/{day}/input");
            await File.WriteAllTextAsync($"./AdventOfCode/Input/day_{padding}{day}.txt", inputString);

        }
    }
}
