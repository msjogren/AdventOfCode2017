using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2017
{
    class Day5Solver : IAdventOfCodeSolver
    {
        public bool Solve(int part = 0)
        {
            int[] maze = File.ReadAllLines("Day05/input.txt").Select(s => int.Parse(s)).ToArray();
            int ip = 0;
            int steps = 0;

            while (ip >= 0 && ip < maze.Length)
            {
                steps++;
                if (part == 2 && maze[ip] >= 3)
                {
                    ip += maze[ip]--;
                }
                else
                {
                    ip += maze[ip]++;
                }
            }

            Console.WriteLine(steps);
            return true;
        }

    }
}
