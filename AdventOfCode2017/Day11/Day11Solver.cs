using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2017
{
    class Day11Solver : IAdventOfCodeSolver
    {
        public bool Solve(int part = 0)
        {
            Dictionary<string, int> moves = new Dictionary<string, int>() { { "n", 0 }, { "s", 0 }, { "se", 0 }, { "ne", 0 }, { "sw", 0 }, { "nw", 0 } };
            int longestPath = 0;

            foreach (string move in File.ReadAllText("Day11/input.txt").Split(','))
            {
                moves[move]++;

                if (part == 2)
                {
                    longestPath = Math.Max(longestPath, PathLength(new Dictionary<string, int>(moves)));
                }
            }

            Console.WriteLine(part == 2 ? longestPath : PathLength(moves));

            return true;
        }

        int PathLength(Dictionary<string, int> moves)
        {
            bool pathReduced;
            do
            {
                pathReduced = false;

                // n + se => ne
                int a = Math.Min(moves["n"], moves["se"]);
                moves["n"] -= a;
                moves["se"] -= a;
                moves["ne"] += a;

                // n + sw => nw
                int b = Math.Min(moves["n"], moves["sw"]);
                moves["n"] -= b;
                moves["sw"] -= b;
                moves["nw"] += b;

                // s + ne => se
                int c = Math.Min(moves["s"], moves["ne"]);
                moves["s"] -= c;
                moves["ne"] -= c;
                moves["se"] += c;

                // s + nw => sw
                int d = Math.Min(moves["s"], moves["nw"]);
                moves["s"] -= d;
                moves["nw"] -= d;
                moves["sw"] += d;

                // nw + ne => n
                int e = Math.Min(moves["nw"], moves["ne"]);
                moves["nw"] -= e;
                moves["ne"] -= e;
                moves["n"] += e;

                // sw + se => s
                int f = Math.Min(moves["sw"], moves["se"]);
                moves["sw"] -= f;
                moves["se"] -= f;
                moves["s"] += f;

                // n + s => 0
                int g = Math.Min(moves["n"], moves["s"]);
                moves["n"] -= g;
                moves["s"] -= g;

                // se + nw => 0
                int h = Math.Min(moves["nw"], moves["se"]);
                moves["nw"] -= h;
                moves["se"] -= h;

                // sw + ne => 0
                int i = Math.Min(moves["ne"], moves["sw"]);
                moves["ne"] -= i;
                moves["sw"] -= i;

                pathReduced = (a + b + c + d + e + f + g + h + i) > 0;
            } while (pathReduced);

            return moves.Sum(kvp => kvp.Value);
        }
    }
}
