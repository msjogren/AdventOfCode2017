using System;

namespace AdventOfCode2017
{
    class Program
    {
        static void Main(string[] args)
        {
            IAdventOfCodeSolver solver = new Day25Solver();
            solver.Solve(1);
            Console.ReadLine();
        }
    }
}
