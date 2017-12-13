using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2017
{
    class Day12Solver : IAdventOfCodeSolver
    {
        public bool Solve(int part = 0)
        {
            string[] input = File.ReadAllLines("Day12/input.txt");
            List<HashSet<int>> groups = new List<HashSet<int>>();

            for (int id = 0; id < input.Length; id++)
            {
                HashSet<int> group = new HashSet<int>() { id };
                groups.Add(group);

                foreach (int pipeToId in input[id].Substring(input[id].IndexOf("<-> ") + 4).Split(", ").Select(s => int.Parse(s)))
                {
                    if (pipeToId < id)
                    {
                        var mergeWith = groups.First(set => set.Contains(pipeToId));
                        if (group != mergeWith)
                        {
                            mergeWith.UnionWith(group);
                            groups.Remove(group);
                            group = mergeWith;
                        }
                    }
                }
            }

            if (part == 2)
            {
                Console.WriteLine(groups.Count);
            }
            else
            {
                Console.WriteLine(groups.First(set => set.Contains(0)).Count);
            }

            return true;
        }
    }
}
