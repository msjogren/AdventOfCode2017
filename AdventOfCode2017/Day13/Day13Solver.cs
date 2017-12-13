using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2017
{
    class Day13Solver : IAdventOfCodeSolver
    {
        public bool Solve(int part = 0)
        {
            Dictionary<int, int> scanners = new Dictionary<int, int>();

            foreach (string line in File.ReadAllLines("Day13/input.txt"))
            {
                string[] parts = line.Split(": ");
                scanners.Add(int.Parse(parts[0]), int.Parse(parts[1]));
            }

            if (part == 2)
            {
                return SolveStep2(scanners);
            }
            else
            {
                return SolveStep1(scanners);
            }
        }

        private bool SolveStep1(Dictionary<int, int> scanners)
        {
            int maxDepth = scanners.Keys.Max();
            int severity = 0;

            for (int packetDepth = 0; packetDepth <= maxDepth; packetDepth++)
            {
                if (scanners.TryGetValue(packetDepth, out int range) && WillGetCaughtByScannerAtTime(packetDepth, range))
                {
                    severity += packetDepth * range;
                }
            }

            Console.WriteLine(severity);

            return true;
        }

        private bool SolveStep2(Dictionary<int, int> scanners)
        {
            for (int delay = 1; ; delay++)
            {
                if (!scanners.Any(kvp => WillGetCaughtByScannerAtTime(delay + kvp.Key, kvp.Value)))
                {
                    Console.WriteLine(delay);
                    break;
                }
            }

            return true;
        }

        private bool WillGetCaughtByScannerAtTime(int time, int range)
        {
            return time % (2 * (range - 1)) == 0;
        }
    }
}
