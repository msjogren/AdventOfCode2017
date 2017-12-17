using System;
using System.Collections.Generic;


namespace AdventOfCode2017
{
    class Day17Solver : IAdventOfCodeSolver
    {
        public bool Solve(int part = 0)
        {
            const int input = 354;

            return part == 2 ? SolvePart2(input) : SolvePart1(input);
        }

        bool SolvePart1(int input)
        {
            List<int> buffer = new List<int>() { 0 };
            int currentPos = 0;

            for (int i = 1; i <= 2017; i++)
            {
                currentPos = ((currentPos + input) % i) + 1;
                buffer.Insert(currentPos, i);
            }

            Console.WriteLine(buffer[currentPos + 1]);
            return true;
        }

        bool SolvePart2(int input)
        {
            int currentPos = 0;
            int valueAtPos1 = -1;

            for (int i = 1; i <= 50_000_000; i++)
            {
                currentPos = ((currentPos + input) % i) + 1;
                if (currentPos == 1) valueAtPos1 = i;
            }

            Console.WriteLine(valueAtPos1);
            return true;
        }

    }
}
