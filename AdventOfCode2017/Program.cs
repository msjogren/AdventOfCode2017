﻿using System;

namespace AdventOfCode2017
{
    class Program
    {
        static void Main(string[] args)
        {
            IAdventOfCodeSolver solver = new Day5Solver();
            solver.Solve(2);
            Console.ReadLine();
        }
    }
}
