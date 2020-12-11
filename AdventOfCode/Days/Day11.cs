using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace AdventOfCode.Days
{
    public class Day11 : ISolution
    {
        public string PartOne(string[] input)
        {
            var board = input.Select(x => x.ToCharArray().ToList()).ToList();

            for (;;)
            {
                var (newBoard, hasChanged) = ProgressBoard(board);
                board = newBoard;

                if (!hasChanged)
                    return board.SelectMany(x => x.Where(y => y == '#')).Count().ToString();
            }
        }

        private static (List<List<char>>, bool) ProgressBoard(List<List<char>> board)
        {
            var jMax = board.Count;
            var iMax = board.First().Count;

            var newBoard = new List<List<char>>();
            var boardChanged = false;
            for (var j = 0; j < jMax; j++)
            {
                newBoard.Add(new List<char>());
                for (var i = 0; i < iMax; i++)
                {
                    var neighbourIMin = i == 0 ? 0 : i - 1;
                    var neighbourJMin = j == 0 ? 0 : j - 1;
                    var neighbourIMax = i + 1 == iMax ? i : i + 1;
                    var neighbourJMax = j + 1 == jMax ? j : j + 1;

                    var neighbourCount = 0;
                    for (var jNeighbour = neighbourJMin; jNeighbour <= neighbourJMax; jNeighbour++)
                    for (var iNeighbour = neighbourIMin; iNeighbour <= neighbourIMax; iNeighbour++)
                    {
                        if (jNeighbour == j && iNeighbour == i)
                        {
                        }
                        else if (board[jNeighbour][iNeighbour] == '#')
                        {
                            neighbourCount++;
                        }
                    }

                    switch (board[j][i])
                    {
                        case '#' when (neighbourCount >= 4):
                            newBoard[j].Add('L');
                            boardChanged = true;
                            break;
                        case '.':
                            newBoard[j].Add('.');
                            break;
                        case 'L' when neighbourCount == 0:
                            newBoard[j].Add('#');
                            boardChanged = true;
                            break;
                        default:
                            newBoard[j].Add(board[j][i]);
                            break;
                    }
                }
            }

            return (newBoard, boardChanged);
        }
        
        public string PartTwo(string[] input)
        {            
            var board = input.Select(x => x.ToCharArray().ToList()).ToList();
            
            for (;;)
            {
                var (newBoard, hasChanged) = ProgressBoardPartTwo(board);
                board = newBoard;

                if (!hasChanged)
                    return board.SelectMany(x => x.Where(y => y == '#')).Count().ToString();
            }
        }

        private static (List<List<char>>, bool) ProgressBoardPartTwo(List<List<char>> board)
        {
            var jMax = board.Count;
            var iMax = board.First().Count;

            var newBoard = new List<List<char>>();
            var boardChanged = false;
            for (var j = 0; j < jMax; j++)
            {
                newBoard.Add(new List<char>());
                for (var i = 0; i < iMax; i++)
                {
                    var neighbourCount = 0;
                    for (var jSearch = j-1; jSearch >=0; jSearch--)
                    {
                        if (board[jSearch][i] == '#')
                        {
                            neighbourCount++;
                            break;
                        }
                        if (board[jSearch][i] == 'L')
                        {
                            break;
                        }
                    }
                    
                    for (var jSearch = j+1; jSearch < jMax; jSearch++)
                    {
                        if (board[jSearch][i] == '#')
                        {
                            neighbourCount++;
                            break;
                        }
                        if (board[jSearch][i] == 'L')
                        {
                            break;
                        }
                    }
                    
                    for (var iSearch = i-1; iSearch >= 0; iSearch--)
                    {
                        if (board[j][iSearch] == '#')
                        {
                            neighbourCount++;
                            break;
                        }
                        if (board[j][iSearch] == 'L')
                        {
                            break;
                        }
                    }
                    
                    for (var iSearch = i+1; iSearch < iMax; iSearch++)
                    {
                        if (board[j][iSearch] == '#')
                        {
                            neighbourCount++;
                            break;
                        }

                        if (board[j][iSearch] == 'L')
                        {
                            break;
                        }
                    }

                    var jSearchD = j;
                    var iSearchD = i;
                    while(--jSearchD >=0 && --iSearchD >=0)
                    {
                        if (board[jSearchD][iSearchD] == '#')
                        {
                            neighbourCount++;
                            break;
                        }

                        if (board[jSearchD][iSearchD] == 'L')
                        {
                            break;
                        }
                    }
                    
                    jSearchD = j;
                    iSearchD = i;
                    while(--jSearchD >=0 && ++iSearchD < iMax)
                    {
                        if (board[jSearchD][iSearchD] == '#')
                        {
                            neighbourCount++;
                            break;
                        }

                        if (board[jSearchD][iSearchD] == 'L')
                        {
                            break;
                        }
                    }
                    
                    jSearchD = j;
                    iSearchD = i;
                    while(++jSearchD < jMax && ++iSearchD < iMax)
                    {
                        if (board[jSearchD][iSearchD] == '#')
                        {
                            neighbourCount++;
                            break;
                        }

                        if (board[jSearchD][iSearchD] == 'L')
                        {
                            break;
                        }
                    }
                    
                    jSearchD = j;
                    iSearchD = i;
                    while(++jSearchD <jMax && --iSearchD >= 0)
                    {
                        if (board[jSearchD][iSearchD] == '#')
                        {
                            neighbourCount++;
                            break;
                        }

                        if (board[jSearchD][iSearchD] == 'L')
                        {
                            break;
                        }
                    }
                    
                    switch (board[j][i])
                    {
                        case '#' when (neighbourCount >= 5):
                            newBoard[j].Add('L');
                            boardChanged = true;
                            break;
                        case '.':
                            newBoard[j].Add('.');
                            break;
                        case 'L' when neighbourCount == 0:
                            newBoard[j].Add('#');
                            boardChanged = true;
                            break;
                        default:
                            newBoard[j].Add(board[j][i]);
                            break;
                    }
                }
            }

            return (newBoard, boardChanged);
        }
        public int Day => 11;
    }
}