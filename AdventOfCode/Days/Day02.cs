using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    public class Day02 : ISolution
    {
        public string PartOne(string[] input)
        {
           return input
                .Select(str => Regex.Match(str, @"(?'min'\d+)-(?'max'\d+) (?'char'[a-z]): (?'password'[a-z]+)"))
                .Select(res => new PasswordPolicy()
                {
                    Password = res.Groups["password"].Value, 
                    Minimum = Convert.ToInt32(res.Groups["min"].Value), 
                    Maximum = Convert.ToInt32(res.Groups["max"].Value), 
                    Required = res.Groups["char"].Value
                })
                .Count(IsValid)
                .ToString();
        }

        private static bool IsValid(PasswordPolicy password)
        {
            var items = password.Password
                .ToCharArray()
                .GroupBy(x => x)
                .ToDictionary(x => x.Key.ToString(), y=> y.Count());

            return items.ContainsKey(password.Required) 
                   && items[password.Required] >= password.Minimum 
                   && items[password.Required] <= password.Maximum;
        }
        
        public string PartTwo(string[] input)
        {
            return input
                .Select(str => Regex.Match(str, @"(?'min'\d+)-(?'max'\d+) (?'char'[a-z]): (?'password'[a-z]+)"))
                .Select(res => new PasswordPolicy()
                {
                    Password = res.Groups["password"].Value, 
                    Minimum = Convert.ToInt32(res.Groups["min"].Value), 
                    Maximum = Convert.ToInt32(res.Groups["max"].Value), 
                    Required = res.Groups["char"].Value
                })
                .Count(PartTwoIsValid)
                .ToString();
        }

        private static bool PartTwoIsValid(PasswordPolicy password)
        {
            var items = password.Password
                .ToCharArray()
                .Select(x => x.ToString()).ToArray();

            return (items[password.Minimum-1] == password.Required || items[password.Maximum-1] == password.Required) 
                   && !(items[password.Minimum-1] == password.Required && items[password.Maximum-1] == password.Required);
        }

        private record PasswordPolicy
        {
            public string Password { get; init; }
            
            public int Minimum { get; init; }
            
            public int Maximum { get; init; }
            
            public string Required { get; init; }
        }
        
        public int Day => 02;
    }
}