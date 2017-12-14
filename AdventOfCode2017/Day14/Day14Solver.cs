using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2017
{
    class Day14Solver : IAdventOfCodeSolver
    {
        public bool Solve(int part = 0)
        {
            const string input = "wenycdww";
            Day10Solver.KnotHash knotHash;
            const int GridSize = 128;
            int?[,] groupGrid = new int?[GridSize, GridSize];
            int bitsSet = 0;

            for (int row = 0; row < 128; row++)
            {
                knotHash = new Day10Solver.KnotHash();
                string rowInput = input + "-" + row;
                int[] lengths = (rowInput + "\x11\x1f\x49\x2f\x17").Select(c => (int)c).ToArray();

                for (int round = 0; round < 64; round++)
                {
                    knotHash.HashRound(lengths);
                }
                
                byte[] hash = knotHash.GetDenseHashBytes();

                for (int byteIdx = 0; byteIdx < hash.Length; byteIdx++)
                {
                    byte hashbyte = hash[byteIdx];
                    for (int bit = 7; bit >= 0; bit--)
                    {
                        bool isUsed = ((hashbyte >> bit) & 0b1) != 0;
                        if (isUsed) bitsSet += 1;
                        groupGrid[8 * byteIdx + (7 - bit), row] = isUsed ? 0 : (int?)null;
                    }
                }
            }

            void FloodFillGroup(int groupNumber, int startX, int startY)
            {
                Stack<(int x, int y)> adjacent = new Stack<(int x, int y)>();
                adjacent.Push((startX, startY));

                while (adjacent.Count > 0)
                {
                    (int x, int y) = adjacent.Pop();
                    if (groupGrid[x, y].HasValue && groupGrid[x, y].Value == 0)
                    {
                        groupGrid[x, y] = groupNumber;
                        if (x > 0) adjacent.Push((x - 1, y));
                        if (x < (GridSize-1)) adjacent.Push((x + 1, y));
                        if (y > 0) adjacent.Push((x, y - 1));
                        if (y < (GridSize-1)) adjacent.Push((x, y + 1));
                    }
                }
            }

            if (part == 2)
            {
                int groups = 0;
                for (int y = 0; y < GridSize; y++)
                {
                    for (int x = 0; x < GridSize; x++)
                    {
                        if (groupGrid[x, y].HasValue && groupGrid[x, y].Value == 0)
                        {
                            FloodFillGroup(++groups, x, y);
                        }
                    }
                }

                Console.WriteLine(groups);
            }
            else
            {
                Console.WriteLine(bitsSet);
            }

            return true;
        }
    }
}
