using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    public class Day04 : ISolution
    {
        public string PartOne(string[] input)
        {
            var allItems = ParsePassports(input);

            return allItems.Count(HasRequiredFields).ToString();
        }

        private static bool HasRequiredFields(Dictionary<string, string> x)
        {
            return x.ContainsKey("byr") 
                   && x.ContainsKey("iyr")
                   && x.ContainsKey("eyr")
                   && x.ContainsKey("hgt") 
                   && x.ContainsKey("hcl") 
                   && x.ContainsKey("ecl") 
                   && x.ContainsKey("pid");
        }

        private static IEnumerable<Dictionary<string, string>> ParsePassports(IEnumerable<string> input)
        {
            var allItems = new List<Dictionary<string, string>>();
            var currentItem = new Dictionary<string, string>();
            foreach (var str in input)
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    allItems.Add(currentItem);
                    currentItem = new Dictionary<string, string>();
                    continue;
                }

                var pairs = str.Split(' ');
                var items = pairs.Select(x => x.Split(':'));

                foreach (var item in items)
                {
                    currentItem.Add(item[0], item[1]);
                }
            }

            //cheeky
            if (currentItem.Count > 0)
                allItems.Add(currentItem);
            
            return allItems;
        }

        public string PartTwo(string[] input)
        {
            var allItems = ParsePassports(input);

            var valid = 0;
            foreach (var item in allItems)
            {
                if (!HasRequiredFields(item))
                {
                    continue;
                }

                //byr (Birth Year) - four digits; at least 1920 and at most 2002.
                var byr = int.Parse(item["byr"]);
                if (!(byr >= 1920 && byr <= 2002))
                    continue;

                //iyr (Issue Year) - four digits; at least 2010 and at most 2020.
                var iyr = int.Parse(item["iyr"]);
                if (!(iyr >= 2010 && iyr <= 2020))
                    continue;
                
                //eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
                var eyr = int.Parse(item["eyr"]);
                if (!(eyr >= 2020 && eyr <= 2030))
                    continue;
                
                var hgt = item["hgt"];
                var hgtUnit = hgt.EndsWith("cm") ? "cm" : "in";
                var hgtValue = int.Parse(hgt.Replace(hgtUnit, ""));
                
                // hgt (Height) - a number followed by either cm or in:
                //If cm, the number must be at least 150 and at most 193.
                //    If in, the number must be at least 59 and at most 76.
                switch (hgtUnit)
                {
                    case "cm" when !(hgtValue >= 150 && hgtValue <=193):
                    case "in" when !(hgtValue >= 59 && hgtValue <=76):
                        continue;
                }

                //hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
                var hcl = item["hcl"];
                if (!Regex.IsMatch(hcl, @"^#[0-9,a-f]{6}$")) 
                    continue;

                //ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
                var ecl = item["ecl"];
                if (!(new [] {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"}.Contains(ecl)))
                    continue;

                //  pid (Passport ID) - a nine-digit number, including leading zeroes.
                var pid = item["pid"];
                if (!Regex.IsMatch(pid, @"^[0-9]{9}$")) 
                    continue;

                valid++;
            }

            return valid.ToString();
        }

        public int Day => 04;
    }
}