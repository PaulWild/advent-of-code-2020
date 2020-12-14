using System.Linq;

namespace AdventOfCode.Days
{
    public class Day13 : ISolution
    {
        public string PartOne(string[] input)
        {
            var boatArrivalTime = int.Parse(input.First());

            var schedules = input
                .Skip(1)
                .Single()
                .Split(",")
                .Where(x => x != "x")
                .Select(int.Parse);

            var waitTime = schedules
                .Select(schedule => ((boatArrivalTime - (boatArrivalTime % schedule)) + schedule, schedule))
                .Select(item  => ((item.Item1 - boatArrivalTime), item.schedule))
                .OrderBy(x => x.Item1)
                .First();

            return (waitTime.Item1 * waitTime.schedule).ToString();
        }

        public string PartTwo(string[] input)
        {
            var schedules = input
                .Skip(1)
                .Single()
                .Split(",")
                .Select((time, idx) => (time, idx))
                .Where(x => x.time != "x")
                .Select(x => (int.Parse(x.time), x.idx)) 
                .OrderBy(x => x.Item1)
                .ToList();
            
            long increment = 1;
            long counter = 0;
            for (var i = 2; i <= schedules.Count; i++)
            {
                var toAttempt = schedules.Take(i).ToList();
                
                var shouldBreak = false;
                while (!shouldBreak)
                {
                    var found = toAttempt.All(x => (counter + x.Item2) % x.Item1 == 0);

                    if (found)
                    {
                        increment = toAttempt.Aggregate( (long)1, (agg, nxt) => agg * (long)nxt.Item1);
                        shouldBreak = true;
                    }
                    else
                    {
                        counter += increment;
                    }
                }
            }

            return counter.ToString();

        }

        public int Day => 13;
    }
}