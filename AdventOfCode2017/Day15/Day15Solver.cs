using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017
{
    class Day15Solver : IAdventOfCodeSolver
    {
        public bool Solve(int part = 0)
        {
            const long SeedA = 699, SeedB = 124;
            const long FactorA = 16807, FactorB = 48271;
            int numbersToJudge = part == 2 ? 5_000_000 : 40_000_000;

            IEnumerable<long> genA = part == 2 ? Part2Generator(FactorA, SeedA, 4) : Part1Generator(FactorA, SeedA);
            IEnumerable<long> genB = part == 2 ? Part2Generator(FactorB, SeedB, 8) : Part1Generator(FactorB, SeedB);

            int matches = Enumerable.Zip(genA, genB, (a, b) => (a, b)).Take(numbersToJudge).Count((pair) => (pair.a & 0xffff) == (pair.b & 0xffff));
            Console.WriteLine(matches);

            return true;
        }

        IEnumerable<long> Part1Generator(long factor, long seed)
        {
            long prev = seed;
            while (true)
            {
                long next = (prev * factor) % 2147483647;
                yield return next;
                prev = next;
            }
        }

        IEnumerable<long> Part2Generator(long factor, long seed, int modCriteria)
        {
            long prev = seed;
            while (true)
            {
                long next = (prev * factor) % 2147483647;
                if (next % modCriteria == 0) yield return next;
                prev = next;
            }
        }

    }
}
