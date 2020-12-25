using System;

namespace AdventOfCode.Days
{
    public class Day25 : ISolution
    {
        public string PartOne(string[] input)
        {
            var cardPublicKey = long.Parse(input[0]);
            var doorPublicKey = long.Parse(input[1]);
            
            long cardLoopSize = CalculateLoopSize(cardPublicKey);

            long value = 1;
            for (long i = 0; i < cardLoopSize; i++)
                value = Transform(value, doorPublicKey);

            return value.ToString();
        }

        private static long CalculateLoopSize(long publicKey)
        {
            long loop = 1;
            long value = 1;
            for (;;)
            {
                value = Transform(value);

                if (value == publicKey)
                    return loop;

                loop++;
            }
        }

        private static long Transform(long value, long subjectNumber = 7)
        {
            var num = subjectNumber * value;
            return num % 20201227;
        }


        public string PartTwo(string[] input)
        {
            return "Merry Christmas!";
        }

        public int Day => 25;
    }
}