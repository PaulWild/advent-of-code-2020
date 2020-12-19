using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day19 : ISolution
    {
        public string PartOne(string[] input)
        {
            var (receivedMessages, rules) = ProcessInput(input);

            return receivedMessages
                .Select(message => GetPotentialStrings(0, rules, message).Contains(message))
                .Count(valid => valid).ToString();
        }
        
        public string PartTwo(string[] input)
        {
            var (receivedMessages, rules) = ProcessInput(input);
            
            rules[8] = new DayRule
            {
                Rules = new List<List<int>>
                {
                    new() {42},
                    new() {42, 8}
                }
            };
                
            rules[11] = new DayRule
            {
                Rules = new List<List<int>>
                {
                    new() {42, 31},
                    new() {42, 11, 31}
                }
            };

            return receivedMessages
                .Select(message => GetPotentialStrings(0, rules, message).Contains(message))
                .Count(valid => valid).ToString();
        }
        
        private static (List<string> receivedMessages,Dictionary<int, DayRule> rules)  ProcessInput(IEnumerable<string> input)
        {
            var processedRules = false;
            var receivedMessages = new List<string>();
            var ruleDict = new Dictionary<int, DayRule>();

            foreach (var row in input)
            {
                if (string.IsNullOrWhiteSpace(row))
                {
                    processedRules = true;
                    continue;
                }

                if (!processedRules)
                {
                    var split = row.Split(": ");
                    var ruleNumber = int.Parse(split[0]);

                    if (split[1].Contains("\""))
                    {
                        ruleDict.Add(ruleNumber, new DayRule {Value = split[1].Replace("\"", "")});
                    }
                    else
                    {
                        var rulesStr = split[1].Split("|");
                        var rules = rulesStr
                            .Select(rule =>
                                rule.Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToList())
                            .ToList();

                        ruleDict.Add(ruleNumber, new DayRule {Rules = rules});
                    }
                }
                else
                {
                    receivedMessages.Add(row);
                }
            }

            return (receivedMessages, ruleDict);
        }

        private static IEnumerable<string> GetPotentialStrings(int ruleNumber, IReadOnlyDictionary<int, DayRule> rules, string compare)
        {
            var r = rules[ruleNumber];

            if (r.IsCharacter)
            {
                yield return r.Value;
            }
            else
            {
                foreach (var rule in r.Rules)
                {
                    switch (rule.Count)
                    {
                        case 1:
                        {
                            var strings = GetPotentialStrings(rule[0], rules, compare);
                            foreach (var s in strings)
                            {
                                yield return s;
                            }

                            break;
                        }
                        case 2:
                        {
                            var leftStrings = GetPotentialStrings(rule[0], rules, compare);

                            foreach (var left in leftStrings)
                            {
                                if (!compare.StartsWith(left))
                                {
                                    continue;
                                }
                                
                                var rightStrings = GetPotentialStrings(rule[1], rules, compare.Substring(left.Length));
                                foreach (var right in rightStrings)
                                {
                                    
                                    yield return left + right;
                                }
                            }

                            break;
                        }
                        case 3:
                        {
                            var first = GetPotentialStrings(rule[0], rules,compare);

                            foreach (var f in first)
                            {
                                if (!compare.StartsWith(f))
                                {
                                    continue;
                                }

                                var second = GetPotentialStrings(rule[1], rules, compare.Substring(f.Length));
                                foreach (var s in second)
                                {
                                    if (!compare.StartsWith(f + s))
                                    {
                                        continue;
                                    }

                                    var third = GetPotentialStrings(rule[2], rules, compare.Substring(f.Length + s.Length));
                                    foreach (var t in third)
                                    {
                                        yield return f + s + t;
                                    }
                                }
                            }

                            break;
                        }
                        default:
                            throw new ArgumentOutOfRangeException(nameof(rule.Count),"uh-oh");
                    }
                }
            }
        }

        private record DayRule
        {
            public List<List<int>> Rules { get; init; }
            
            public string Value { get; init; }

            public bool IsCharacter => !string.IsNullOrWhiteSpace(Value);
        }
        
        public int Day => 19;
    }
}