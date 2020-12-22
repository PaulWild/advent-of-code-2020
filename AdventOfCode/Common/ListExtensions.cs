using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Common
{
    public static class ListExtensions
    {
        public static int GetSequenceHashCode<T>(this IList<T> sequence)
        {
            const int seed = 487;
            const int modifier = 31;

            unchecked
            {
                return sequence.Aggregate(seed, (current, item) =>
                    (current*modifier) + item.GetHashCode());
            }            
        }

    }
}