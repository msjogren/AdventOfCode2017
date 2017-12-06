using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2017
{
    class Day6Solver : IAdventOfCodeSolver
    {
        public bool Solve(int part = 0)
        {
            int[] memoryBanks = File.ReadAllText("Day06/input.txt").Split('\t').Select(s => int.Parse(s)).ToArray();
            int numberOfBanks = memoryBanks.Length;
            int cycles = 0;
            int max = memoryBanks.Max();
            int maxIdx = Array.FindIndex(memoryBanks, i => i == max);
            Dictionary<string, int> seenStates = new Dictionary<string, int>() {
                { String.Join("", memoryBanks.Select(i => i.ToString())), 0 }
            };

            do
            {
                cycles++;

                int currentIdx = maxIdx;
                int blocksToDistribute = memoryBanks[currentIdx];
                memoryBanks[currentIdx] = 0;

                max = 0;
                maxIdx = int.MaxValue;

                for (int i = 0; i < numberOfBanks; i++)
                {
                    int blockIdx = (currentIdx + 1 + i) % numberOfBanks;
                    memoryBanks[blockIdx] += blocksToDistribute / numberOfBanks;
                    if (i < (blocksToDistribute % numberOfBanks)) memoryBanks[blockIdx]++;

                    if (memoryBanks[blockIdx] > max || (memoryBanks[blockIdx] == max && blockIdx < maxIdx))
                    {
                        max = memoryBanks[blockIdx];
                        maxIdx = blockIdx;
                    }
                }

                string state = String.Join("", memoryBanks.Select(i => i.ToString()));

                if (seenStates.TryGetValue(state, out int previousCycle))
                {
                    Console.WriteLine(part == 2 ? (cycles - previousCycle) : cycles);
                    break;
                }
                else
                {
                    seenStates.Add(state, cycles);
                }

            } while (true);

            return true;
        }

    }
}
