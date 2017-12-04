using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2017
{
    class Day4Solver : IAdventOfCodeSolver
    {
        public bool Solve(int part = 0)
        {
            string[] input = File.ReadAllLines("Day04/input.txt");

            int valid = 0;

            foreach (string passphrase in input)
            {
                bool foundDuplicate = false;
                HashSet<string> usedWords = new HashSet<string>();

                foreach (string word in passphrase.Split(' '))
                {
                    if (usedWords.Contains(word))
                    {
                        foundDuplicate = true;
                        break;
                    }

                    if (part == 2)
                    {
                        foreach (char[] anagram in EnumerateAnagrams(word.ToCharArray()))
                        {
                            usedWords.Add(new string(anagram));
                        }
                    }
                    else
                    {
                        usedWords.Add(word);
                    }
                }

                if (!foundDuplicate) valid++;
            }

            Console.WriteLine(valid);
            return true;
        }

        static IEnumerable<char[]> EnumerateAnagrams(char[] values, int startIdx = 0)
        {
            void Swap(ref char c1, ref char c2) => (c2, c1) = (c1, c2);

            if (startIdx + 1 == values.Length)
            {
                yield return values;
            }
            else
            {
                foreach (var a in EnumerateAnagrams(values, startIdx + 1))  yield return a;

                for (var i = startIdx + 1; i < values.Length; i++)
                {
                    Swap(ref values[startIdx], ref values[i]);
                    foreach (var a in EnumerateAnagrams(values, startIdx + 1)) yield return a;
                    Swap(ref values[startIdx], ref values[i]);
                }
            }
        }
    }
}
