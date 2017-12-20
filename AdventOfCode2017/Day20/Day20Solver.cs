using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2017
{
    class Day20Solver : IAdventOfCodeSolver
    {
        class Particle
        {
            public int I;
            public Vector3 P;
            public Vector3 V;
            public Vector3 A;
            public float Distance => Math.Abs(P.X) + Math.Abs(P.Y) + Math.Abs(P.Z);

            public override bool Equals(object obj) => obj is Particle other && other.I == I;
            public override int GetHashCode() => I.GetHashCode();
        }

        public bool Solve(int part = 0)
        {
            string[] lines = File.ReadAllLines("Day20/input.txt");
            List<Particle> particles = new List<Particle>();

            for (int i = 0; i < lines.Length; i++)
            {
                particles.Add(new Particle()
                {
                    I = i,
                    P = ParseVector(lines[i], "p"),
                    V = ParseVector(lines[i], "v"),
                    A = ParseVector(lines[i], "a")
                });
            }

            int lastResult = -1;

            for (int tick = 0, sameResult = 0; sameResult < 1000; tick++)
            {
                int closest = -1;
                float minDistance = float.MaxValue;
                foreach (Particle p in particles)
                {
                    p.V += p.A;
                    p.P += p.V;

                    if (part == 1)
                    {
                        float d = p.Distance;
                        if (d < minDistance)
                        {
                            minDistance = d;
                            closest = p.I;
                        }
                    }
                }

                if (part == 2)
                {
                    var collisionsGroups =
                        from p in particles
                        group p by p.P into positionGroup
                        where positionGroup.Count() > 1
                        select positionGroup;
                    foreach (var g in collisionsGroups)
                    {
                        foreach (var p in g) particles.Remove(p);
                    }

                    sameResult = (lastResult - particles.Count) == 0 ? sameResult + 1 : 0;
                    lastResult = particles.Count;
                }
                else
                {
                    sameResult = closest == lastResult ? sameResult + 1 : 0;
                    lastResult = closest;
                }
            }

            Console.WriteLine(lastResult);

            return true;
        }

        private Vector3 ParseVector(string input, string param)
        {
            int start = input.IndexOf(param + "=<") + 3;
            int end = input.IndexOf('>', start);
            string[] values = input.Substring(start, end - start).Split(',');
            return new Vector3(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));
        }
    }
}
