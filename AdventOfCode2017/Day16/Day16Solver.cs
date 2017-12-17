using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2017
{
    class Day16Solver : IAdventOfCodeSolver
    {
        public bool Solve(int part = 0)
        {
            string[] moves = File.ReadAllText("Day16/input.txt").Split(',');
            char[] programs = "abcdefghijklmnop".ToCharArray();
            return part == 2 ? SolvePart2(moves, programs) : SolvePart1(moves, programs);
        }

        bool SolvePart1(IEnumerable<string> moves, char[] programs)
        {
            DoDanceMoves(moves, ref programs);
            Console.WriteLine(new string(programs));
            return true;
        }

        private bool SolvePart2(IEnumerable<string> moves, char[] programs)
        {
            const int FinalIteration = 1_000_000_000;
            Dictionary<string, int> results = new Dictionary<string, int>();

            for (int i = 1; ; i++)
            {
                DoDanceMoves(moves, ref programs);
                string result = new string(programs);

                if (results.TryGetValue(result, out int prev))
                {
                    int cycleLength = i - prev;
                    int sameAsFinalIteration  =  FinalIteration - (FinalIteration / cycleLength) * cycleLength;
                    string finalResult = results.First(kvp => kvp.Value == sameAsFinalIteration).Key;
                    Console.WriteLine(finalResult);
                    break;
                }
                else
                {
                    results.Add(result, i);
                }
            }

            return true;
        }


        void DoDanceMoves(IEnumerable<string> moves, ref char[] programs)
        {
            char[] tmp = new char[programs.Length];

            foreach (string move in moves)
            {
                switch (move[0])
                {
                    case 's':
                        int spinSteps = int.Parse(move.Substring(1)) % programs.Length;
                        Array.Copy(programs, programs.Length - spinSteps, tmp, 0, spinSteps);
                        Array.Copy(programs, 0, tmp, spinSteps, programs.Length - spinSteps);
                        (tmp, programs) = (programs, tmp);
                        break;

                    case 'x':
                        int slash = move.IndexOf('/');
                        int x1 = int.Parse(move.Substring(1, slash - 1));
                        int x2 = int.Parse(move.Substring(slash + 1));
                        (programs[x2], programs[x1]) = (programs[x1], programs[x2]);
                        break;

                    case 'p':
                        int p1 = Array.IndexOf(programs, move[1]);
                        int p2 = Array.IndexOf(programs, move[3]);
                        (programs[p2], programs[p1]) = (programs[p1], programs[p2]);
                        break;
                }
            }
        }
    }
}
