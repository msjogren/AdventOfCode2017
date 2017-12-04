using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2017
{
    class Day2Solver : IAdventOfCodeSolver
    {
        public bool Solve(int part = 0)
        {
            string[] input = File.ReadAllLines("Day02/input.txt");
            return part < 2 ?  SolvePart1(input) : SolvePart2(input);
        }

        private bool SolvePart1(string[] input)
        {
            int checksum = 0;

            foreach (string line in input)
            {
                int min = int.MaxValue, max = int.MinValue;
                foreach (string val in line.Split('\t'))
                {
                    int i = int.Parse(val);
                    min = Math.Min(min, i);
                    max = Math.Max(max, i);
                }

                checksum += (max - min);
            }

            Console.WriteLine(checksum);
            return true;
        }

        private bool SolvePart2(string[] input)
        {
            int checksum = 0;

            foreach (string line in input)
            {
                int[] nums = line.Split('\t').Select(s => int.Parse(s)).ToArray();
                bool found = false;

                for (int i = 0; !found && i < nums.Length - 1; i++)
                {
                    for (int j = i + 1; !found && j < nums.Length; j++)
                    {
                        if (nums[i] % nums[j] == 0)
                        {
                            checksum += nums[i] / nums[j];
                            found = true;
                        }
                        else if (nums[j] % nums[i] == 0)
                        {
                            checksum += nums[j] / nums[i];
                            found = true;
                        }
                    }
                }
            }

            Console.WriteLine(checksum);
            return true;
        }
    }
}
