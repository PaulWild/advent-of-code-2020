using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day21 : ISolution
    {
        public string PartOne(string[] input)
        {
            var (ingredientCount ,potentialAllergen) = ParseInput(input);

            var allergens = potentialAllergen.Values.SelectMany(x => x).ToHashSet();
            var num = ingredientCount
                .Where(kvp => !allergens.Contains(kvp.Key))
                .Sum(kvp => kvp.Value);
            
            return num.ToString();
        }
        
        public string PartTwo(string[] input)
        {
            var (_, potentialAllergen) = ParseInput(input);
            
            while (potentialAllergen.Values.Any(x => x.Count != 1))
            {
                foreach (var allergen in potentialAllergen.Keys)
                {
                    var potAllergens = potentialAllergen[allergen];
                    if (potAllergens.Count != 1) continue;
                    
                    foreach (var (key, value) in potentialAllergen)
                    {
                        if (key == allergen)
                            continue;

                        var newList = value.Where(x => x != potAllergens.Single()).ToHashSet();
                        potentialAllergen[key] = newList;
                    }
                }
            }
            
            return string.Join(",", potentialAllergen.OrderBy(x => x.Key).Select(x => x.Value.Single()));
        }

        private static (Dictionary<string, int> allergenCount, Dictionary<string,HashSet<string>>) ParseInput(IEnumerable<string> input)
        {
            Dictionary<string, int> ingredientCount = new();
            Dictionary<string,HashSet<string>> potentialAllergen= new();

            foreach (var row in input)
            {
                var split = row.Split(" (contains ");
                var ingredients = split[0].Split(" ");
                var allergens = split[1].Replace(")", "").Split(", ");

                foreach (var ingredient in ingredients)
                {
                    if (ingredientCount.ContainsKey(ingredient))
                        ingredientCount[ingredient] += 1;
                    else
                        ingredientCount[ingredient] = 1;
                }

                foreach (var allergen in allergens)
                {
                    if (potentialAllergen.ContainsKey(allergen))
                    {
                        potentialAllergen[allergen].IntersectWith(ingredients);
                    }
                    else
                    {
                        potentialAllergen[allergen] = new HashSet<string>(ingredients);
                    }
                }
            }
            
            return (ingredientCount, potentialAllergen);
        }
        
        public int Day => 21;
    }
}