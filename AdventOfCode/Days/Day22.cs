using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Days
{
    public class Day22 : ISolution
    {
        public string PartOne(string[] input)
        {
            var (player1, player2) = ParseInput(input);

            while (player1.Count > 0 && player2.Count > 0)
            {
                PlayRound(player1, player2);
            }

            return player1.Count > 0 
                ? CalculateScore(player1).ToString() 
                : CalculateScore(player2).ToString();
        }

        private static (Queue<int>, Queue<int>) ParseInput(string[] input)
        {
            List<Queue<int>> hands = new();
            Queue<int> hand = new();
            foreach (var row in input)
            {
                if (row.StartsWith("Player"))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(row))
                {
                    hands.Add(hand);
                    hand = new Queue<int>();
                }
                else
                {
                    hand.Enqueue(int.Parse(row));
                }
            }
            
            return (hands.First(), hands.Last());
        }

        public int CalculateScore(Queue<int> hand)
        {
            var toReturn = 0;
            var handCount = hand.Count;
            
            for (int i = handCount; i > 0; i--)
            {
                toReturn += i * hand.Dequeue();
            }

            return toReturn;

        }
        

        public void PlayRound(Queue<int> player1, Queue<int> player2)
        {
            var card1 = player1.Dequeue();
            var card2 = player2.Dequeue();

            if (card1 > card2)
            {
                player1.Enqueue(card1);
                player1.Enqueue(card2);
            }
            if (card2 > card1)
            {
                player2.Enqueue(card2);
                player2.Enqueue(card1);

            }
        }

        public string PartTwo(string[] input)
        {
            var (player1, player2) = ParseInput(input);

            PlayRecursiveCombat(player1, player2);

            return player1.Count > 0 
                ? CalculateScore(player1).ToString() 
                : CalculateScore(player2).ToString();
        }

        public void PlayRecursiveCombat(Queue<int> player1, Queue<int> player2)
        {
            var dp = new HashSet<(int, int)>();
            while (player1.Count > 0 && player2.Count > 0)
            {
                PlayRecursiveCombatRound(player1, player2,dp);
            }
        }
        
        public void PlayRecursiveCombatRound(Queue<int> player1, Queue<int> player2, HashSet<(int, int)> seenHands)
        {
            var hashCodePlayer1 = player1.ToList().GetSequenceHashCode();
            var hashCodePlayer2 = player2.ToList().GetSequenceHashCode();
            var item = (hashCodePlayer1, hashCodePlayer2);
            
            bool player1Win = seenHands.Contains(item);

            seenHands.Add((hashCodePlayer1,hashCodePlayer2));
            

            if (player1Win)
            {
                player2.Clear(); 
                return;
            }

            var card1 = player1.Dequeue();
            var card2 = player2.Dequeue();

            if (player1.Count >= card1 && player2.Count >= card2)
            {
                var player1Sub = new Queue<int>(player1.Take(card1));
                var player2Sub = new Queue<int>(player2.Take(card2));

                PlayRecursiveCombat(player1Sub, player2Sub);
                if (player1Sub.Count > 1)
                {
                    player1.Enqueue(card1);
                    player1.Enqueue(card2);  
                }
                else
                {
                    player2.Enqueue(card2);
                    player2.Enqueue(card1);
                }
            }
            else if (card1 > card2 )
            {
                player1.Enqueue(card1);
                player1.Enqueue(card2);
            }
            else if (card2 > card1)
            {
                player2.Enqueue(card2);
                player2.Enqueue(card1);
            }
        }
        

        public int Day => 22;
    }
}