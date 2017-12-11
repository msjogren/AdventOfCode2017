using System;
using System.Diagnostics;
using System.IO;

namespace AdventOfCode2017
{
    class Day9Solver : IAdventOfCodeSolver
    {
        public bool Solve(int part = 0)
        {
            string input = File.ReadAllText("Day09/input.txt");

            (int score, _, int garbageChars) = ParseAndScoreGroup(input, 0, 1);

            Console.WriteLine(part == 2 ? garbageChars : score);

            return true;
        }

        private (int score, int endPos, int garbageChars) ParseAndScoreGroup(string input, int startOffset, int groupScore)
        {
            Debug.Assert(input[startOffset] == '{');
            int score = groupScore;
            int i = startOffset + 1;
            bool inGarbage = false;
            int garbageChars = 0;

            do
            {
                switch (input[i])
                {
                    case '!':
                        i++;
                        break;

                    case '<':
                        if (inGarbage)
                        {
                            garbageChars++;
                        }
                        else
                        {
                            inGarbage = true;
                        }
                        break;

                    case '>':
                        if (inGarbage)
                        {
                            inGarbage = false;
                        }
                        break;

                    case '{':
                        if (inGarbage)
                        {
                            garbageChars++;
                        }
                        else
                        {
                            (int subscore, int endPos, int subgarbage) = ParseAndScoreGroup(input, i, groupScore + 1);
                            score += subscore;
                            garbageChars += subgarbage;
                            i = endPos;
                        }
                        break;

                    case '}':
                        if (inGarbage)
                        {
                            garbageChars++;
                        }
                        else
                        {
                            return (score, i, garbageChars);
                        }
                        break;

                    default:
                        if (inGarbage) garbageChars++;
                        break;
                }

                i++;
            } while (true);
        }
    }
}
