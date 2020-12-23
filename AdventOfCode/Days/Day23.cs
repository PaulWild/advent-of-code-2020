using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Common;

namespace AdventOfCode.Days
{
    public class Day23 : ISolution
    {
        public string PartOne(string[] input)
        {
            var numbers = ParseInput(input);

            return CrabCups(numbers, 100, false);
        }
        
        public string PartTwo(string[] input)
        {
            var numbers = ParseInput(input);

            var next = numbers.Max() + 1;
            for (var i = numbers.Count; i < 1000000; i++)
            {
                numbers.AddLast(next);
                next++;
            }

            return CrabCups(numbers, 10000000, true);
        }

        private static LinkedList<int> ParseInput(string[] input)
        {
            return new(input.First().ToCharArray().Select(x => (int) char.GetNumericValue(x)));
        }
        private static string CrabCups(LinkedList<int> numbers, int numberOfTurns, bool partTwoOutput)
        {
            var min = numbers.Min();
            var max = numbers.Max();

            var lookup = BuildDictionaryOfListNodes(numbers);

            var current = numbers.First;
            for (int i = 0; i < numberOfTurns; i++)
            {
                var removed = current.RemoveNAfter(3);
                var destination = FindDestinationNode(current.Value, removed, min, max, lookup);
                destination.AddNAfter(removed);
                current = current.NextOrFirst();
            }

            if (partTwoOutput)
            {
                var one = numbers.Find(1);
                var nxt = one.NextOrFirst();
                var nxt2 = nxt.NextOrFirst();

                return ((long) nxt.Value * nxt2.Value).ToString();
            }
            else
            {
                var after = numbers.Find(1);
                var nxt = after.NextOrFirst();
                var sb = new StringBuilder();
                while (nxt != after)
                {
                    sb.Append(nxt.Value);
                    nxt = nxt.NextOrFirst();
                }

                return sb.ToString();
            }
        }

        private static LinkedListNode<int> FindDestinationNode(int current,
            List<LinkedListNode<int>> removed, int min, int max,
            Dictionary<int, LinkedListNode<int>> lookup)
        {
            var destinationNumber = current - 1;

            LinkedListNode<int> destination;
            var removedValues = removed.Select(x => x.Value).ToList();
            for(;;)
            {
                if (removedValues.Contains(destinationNumber))
                {
                    destinationNumber--;
                }
                else if (destinationNumber < min)
                {
                    destinationNumber = max;
                }
                else
                {
                    destination = lookup[destinationNumber];
                    break;
                }
            }

            return destination;
        }

        private static Dictionary<int, LinkedListNode<int>> BuildDictionaryOfListNodes(LinkedList<int> numbers)
        {
            var lookup = new Dictionary<int, LinkedListNode<int>>();
            var curr = numbers.First;
            do
            {
                lookup[curr.Value] = curr;
                curr = curr.Next;
            } while (curr != null);

            return lookup;
        }

        public int Day => 23;
    }
}