using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2017
{
    class Day24Solver : IAdventOfCodeSolver
    {
        class Component
        {
            public Component(int port1, int port2)
            {
                Port1 = port1;
                Port2 = port2;
            }

            public int Port1 { get; private set; }
            public int Port2 { get; private set; }
        }

        public bool Solve(int part = 0)
        {
            List<Component> allComponents = new List<Component>();
            foreach (string line in File.ReadAllLines("Day24/input.txt"))
            {
                var parts = line.Split('/');
                allComponents.Add(new Component(int.Parse(parts[0]), int.Parse(parts[1])));
            }

            int longest = -1;
            int strongest = -1;
            
            void BuildBridge(int nextPort, IEnumerable<Component> used, IEnumerable<Component> unused)
            {
                var nextParts = unused.Where(c => c.Port1 == nextPort || c.Port2 == nextPort);
                if (nextParts.Any())
                {
                    foreach (Component next in nextParts)
                    {
                        int otherPort = next.Port1 == nextPort ? next.Port2 : next.Port1;
                        List<Component> state = new List<Component>(used);
                        state.Add(next);
                        BuildBridge(otherPort, state, unused.Except(new[] { next }));
                    }
                }
                else
                {
                    if (part == 2)
                    {
                        int length = used.Count();
                        if (length >= longest)
                        {
                            int strength = used.Sum(c => c.Port1 + c.Port2);
                            if (length > longest || (length == longest && strength > strongest))
                            {
                                longest = length;
                                strongest = strength;
                            }
                        }
                    }
                    else
                    {
                        int strength = used.Sum(c => c.Port1 + c.Port2);
                        strongest = Math.Max(strongest, strength);
                    }
                }
            }

            BuildBridge(0, new List<Component>(), allComponents);

            Console.WriteLine(strongest);

            return true;
        }
    }
}
