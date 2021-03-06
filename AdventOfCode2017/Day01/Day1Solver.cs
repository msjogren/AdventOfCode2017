﻿using System;
using System.IO;

namespace AdventOfCode2017
{
    class Day1Solver : IAdventOfCodeSolver
    {
        public bool Solve(int part = 0)
        {
            string input = File.ReadAllText("Day01/input.txt");
            return part < 2 ? SolvePart1(input) : SolvePart2(input);
        }

        private bool SolvePart1(string input)
        {
            input = input + input[0];
            int sum = 0;

            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input[i] == input[i + 1])
                {
                    sum += (input[i] - '0');
                }
            }

            Console.WriteLine(sum);
            return true;
        }

        private bool SolvePart2(string input)
        {
            int oppositeOffset = input.Length / 2;
            int sum = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == input[(i + oppositeOffset) % input.Length])
                {
                    sum += (input[i] - '0');
                }
            }

            Console.WriteLine(sum);
            return true;
        }
    }
}
