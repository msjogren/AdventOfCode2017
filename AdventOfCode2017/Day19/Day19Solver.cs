using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2017
{
    class Day19Solver : IAdventOfCodeSolver
    {
        enum Direction {  Down, Up, Left, Right };

        public bool Solve(int part = 0)
        {
            string[] lines = File.ReadAllLines("Day19/input.txt");
            char[,] grid = new char[lines.Max(s => s.Length), lines.Length];

            for (int yy = 0; yy < lines.Length; yy++)
            {
                for (int xx = 0; xx < lines[yy].Length; xx++)
                {
                    grid[xx, yy] = lines[yy][xx];
                }
            }

            string path = "";
            int steps = 0;
            Direction dir = Direction.Down;
            int y = 0;
            int x = lines[0].IndexOf('|');

            while (true)
            {
                steps++;

                switch (dir)
                {
                    case Direction.Down: y += 1; break;
                    case Direction.Up: y -= 1; break;
                    case Direction.Left: x -= 1; break;
                    case Direction.Right: x += 1; break;
                }

                if (grid[x, y] == '+')
                {
                    if (dir == Direction.Down || dir == Direction.Up)
                    {
                        if (grid[x - 1, y] == '-' || Char.IsLetter(grid[x - 1, y]))
                        {
                            dir = Direction.Left;
                        }
                        else if (grid[x + 1, y] == '-' || Char.IsLetter(grid[x + 1, y]))
                        {
                            dir = Direction.Right;
                        }
                    }
                    else
                    {
                        if (grid[x, y - 1] == '|' || Char.IsLetter(grid[x, y - 1]))
                        {
                            dir = Direction.Up;
                        }
                        else if (grid[x, y + 1] == '|' || Char.IsLetter(grid[x, y + 1]))
                        {
                            dir = Direction.Down;
                        }
                    }
                }
                else if (Char.IsLetter(grid[x, y]))
                {
                    path += grid[x, y];
                }
                else if (grid[x, y] == ' ')
                {
                    break;
                }
            }

            Console.WriteLine(part == 2 ? steps.ToString() : path);

            return true;
        }
    }
}
