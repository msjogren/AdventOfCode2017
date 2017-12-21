using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2017
{
    class Day21Solver : IAdventOfCodeSolver
    {
        public bool Solve(int part = 0)
        {
            Dictionary<string, string> replacementRules = new Dictionary<string, string>();

            foreach (string line in File.ReadAllLines("Day21/input.txt"))
            {
                string[] parts = line.Split(" => ");
                string from = parts[0].Replace("/", "");
                string to = parts[1].Replace("/", "");
                replacementRules.TryAdd(from, to);
                char[] rule = from.ToCharArray();
                replacementRules.TryAdd(new string(FlipHorizontally(rule)), to);
                replacementRules.TryAdd(new string(FlipVertically(rule)), to);

                for (int rotations = 0; rotations < 3; rotations++)
                {
                    rule = RotateClockwise(rule);
                    replacementRules.TryAdd(new string(rule), to);
                    replacementRules.TryAdd(new string(FlipHorizontally(rule)), to);
                    replacementRules.TryAdd(new string(FlipVertically(rule)), to);
                }
            }

            char[,] grid = { { '.', '.', '#' }, { '#', '.', '#' }, { '.', '#', '#' } };
            for (int i = 0; i < (part == 2 ? 18 : 5); i++)
            {
                grid = EnhanceGrid(grid, replacementRules);
            }

            int on = 0;
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    on += grid[x, y] == '#' ? 1 : 0;
                }
            }

            Console.WriteLine(on);

            return true;
        }

        private char[,] EnhanceGrid(char[,] grid, Dictionary<string, string> replacementRules)
        {
            int chunks, chunkSize, replacementSize;
            char[,] result;
            char[] replacementKey;

            if (grid.GetLength(0) % 2 == 0)
            {
                chunks = grid.GetLength(0) / 2;
                chunkSize = 2;
                replacementSize = 3;
                replacementKey = new char[4];
            }
            else
            {
                chunks = grid.GetLength(0) / 3;
                chunkSize = 3;
                replacementSize = 4;
                replacementKey = new char[9];
            }

            result = new char[replacementSize * chunks, replacementSize * chunks];

            for (int ychunk = 0; ychunk < chunks; ychunk++)
            {
                for (int xchunk = 0; xchunk < chunks; xchunk++)
                {
                    int ch = 0;
                    for (int y = 0; y < chunkSize; y++)
                    {
                        for (int x = 0; x < chunkSize; x++)
                        {
                            replacementKey[ch++] = grid[xchunk * chunkSize + x, ychunk * chunkSize + y];
                        }
                    }

                    string enhancement = replacementRules[new string(replacementKey)];

                    ch = 0;
                    for (int y = 0; y < replacementSize; y++)
                    {
                        for (int x = 0; x < replacementSize; x++)
                        {
                            result[xchunk * replacementSize + x, ychunk * replacementSize + y] = enhancement[ch++];
                        }
                    }
                }
            }

            return result;
        }

        private char[] FlipVertically(char[] a)
        {
            if (a.Length == 9)
            {
                /*
                 *              012    210
                 * 012345678 -> 345 -> 543 -> 210543876
                 *              678    876
                 */
                return new[] { a[2], a[1], a[0], a[5], a[4], a[3], a[8], a[7], a[6] };
            }
            else
            {
                /*
                 *         01    10
                 * 0123 -> 23 -> 32 -> 1032
                 */
                return new[] { a[1], a[0], a[3], a[2] };
            }
        }

        private char[] FlipHorizontally(char[] a)
        {
            if (a.Length == 9)
            {
                /*
                 *              012    678
                 * 012345678 -> 345 -> 345 -> 678345012
                 *              678    012
                 */
                return new[] { a[6], a[7], a[8], a[3], a[4], a[5], a[0], a[1], a[2] };
            }
            else
            {
                /*
                 *         01    23
                 * 0123 -> 23 -> 01 -> 2301
                 */
                return new[] { a[2], a[3], a[0], a[1] };
            }
        }

        private char[] RotateClockwise(char[] a)
        {
            if (a.Length == 9)
            {
                /*
                 *              012    630
                 * 012345678 -> 345 -> 741 -> 630741852
                 *              678    852
                 */
                return new[] { a[6], a[3], a[0], a[7], a[4], a[1], a[8], a[5], a[2] };
            }
            else
            {
                /*
                 *         01    20
                 * 0123 -> 23 -> 31 -> 2031
                 */
                return new[] { a[2], a[0], a[3], a[1] };
            }
        }
    }
}
