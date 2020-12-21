using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day21 : ISolution
    {
        public string PartOne(string[] input)
        {
            var (ingredientCount, allergenList, allergenSet) = ParseInput(input);

            HashSet<string> potentialAllergen = new();
            foreach (var allergen in allergenSet)
            {
                var toCheck = allergenList
                    .Where(x => x.allergen == allergen)
                    .Select(x => new HashSet<string>(x.ingredients))
                    .Aggregate((nxt, agg) =>
                        {
                            nxt.IntersectWith(agg);
                            return nxt;
                        });
                potentialAllergen.UnionWith(toCheck);
            }

            var num = ingredientCount
                .Where(kvp => !potentialAllergen.Contains(kvp.Key))
                .Sum(kvp => kvp.Value);
            
            return num.ToString();
        }
        
        public string PartTwo(string[] input)
        {
            var (_, allergenList, allergenSet) = ParseInput(input);

            Dictionary<string,string[]> potentialAllergen= new();
            foreach (var allergen in allergenSet)
            {
                var toCheck = allergenList.Where(x => x.allergen == allergen).ToList();
                var hmm = toCheck.Select(x => new HashSet<string>(x.ingredients)).Aggregate((nxt, agg) =>
                {
                    nxt.IntersectWith(agg);
                    return nxt;
                });

                potentialAllergen.Add(allergen, hmm.ToArray());
            }
            
            for(;;)
            {

                foreach (var allergen in allergenSet)
                {
                    var potAllergens = potentialAllergen[allergen];
                    if (potAllergens.Length != 1) continue;
                    
                    foreach (var (key, value) in potentialAllergen)
                    {
                        if (key == allergen)
                            continue;

                        var newList = value.Where(x => x != potAllergens.Single()).ToArray();
                        potentialAllergen[key] = newList;

                    }
                }
                    
                if (potentialAllergen.Values.Any(x => x.Length == 0))
                    break;
                if (potentialAllergen.Values.All(x => x.Length == 1))
                {
                    return string.Join(",", potentialAllergen.OrderBy(x => x.Key).Select(x => x.Value.Single()));
                }
            }


            return "-1";
        }

        private static (Dictionary<string, int> allergenCount, List<(string allergen, string[] ingredients)> allergenList, HashSet<string> allergenSet) ParseInput(IEnumerable<string> input)
        {
            Dictionary<string, int> ingredientCount = new();
            List<(string allergen, string[] ingredients)> allergenList = new();
            HashSet<string> allergenSet = new();

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
                    allergenSet.Add(allergen);
                    allergenList.Add((allergen, ingredients));
                }
            }

            return (ingredientCount, allergenList, allergenSet);
        }
        
        public int Day => 21;
    }
}