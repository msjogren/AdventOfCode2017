using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2017
{
    class Day10Solver : IAdventOfCodeSolver
    {
        class KnotHash
        {
            const int HashSize = 256;
            private int[] _hash = Enumerable.Range(0, HashSize).ToArray();
            private int _currentPos = 0;
            private int _skipSize = 0;

            public int HashRound(int[] lengths)
            {
                foreach (int length in lengths)
                {
                    for (int i = 0; i < length / 2; i++)
                    {
                        int j = (_currentPos + i) % HashSize, k = (_currentPos + length - 1 - i) % HashSize;
                        (_hash[j], _hash[k]) = (_hash[k], _hash[j]);
                    }

                    _currentPos = (_currentPos + length + _skipSize++) % HashSize;
                }

                return _hash[0] * _hash[1];
            }

            public string GetDenseHash()
            {
                int[] dense = new int[HashSize / 16];
                for (int i = 0; i < dense.Length; i++)
                {
                    dense[i] = _hash.Skip(16 * i).Take(16).Aggregate((i1, i2) => i1 ^ i2);
                }

                return dense.Select(i => i.ToString("x2")).Aggregate((s1, s2) => s1 + s2);
            }
            
        }

        public bool Solve(int part = 0)
        {
            string input = File.ReadAllText("Day10/input.txt");
            KnotHash knotHash = new KnotHash();

            if (part == 2)
            {
                int[] lengths = (input + "\x11\x1f\x49\x2f\x17").Select(c => (int)c).ToArray();

                for (int round = 0; round < 64; round++)
                {
                    knotHash.HashRound(lengths);
                }

                Console.WriteLine(knotHash.GetDenseHash());
            }
            else
            {
                int[] lengths = input.Split(',').Select(s => int.Parse(s)).ToArray();
                int result = knotHash.HashRound(lengths);
                Console.WriteLine(result);
            }

            return true;
        }
    }
}
