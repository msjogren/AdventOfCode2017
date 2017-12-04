using System;
using System.Collections.Generic;
using System.Drawing;

namespace AdventOfCode2017
{
    class Day3Solver : IAdventOfCodeSolver
    {
        public bool Solve(int part = 0)
        {
            const int input = 312051;
            return part < 2 ? SolvePart1(input) : SolvePart2(input);
        }

        private bool SolvePart1(int input)
        {
            int radius = 0;
            int maxNumber = 1;
            int edge = 1;

            while (maxNumber < input)
            {
                radius++;
                maxNumber += 8 * radius; // 1, 8 (4*2), 16 (4*4), 24 (4*6) ... 
                edge = 2 * radius;
            }

            int[] cornerValues = new[] { maxNumber - 4 * edge, maxNumber - 3 * edge, maxNumber - 2 * edge, maxNumber - edge, maxNumber };

            for (int c = 4; c > 1; c--)
            {
                if (input <= cornerValues[c] && input >= cornerValues[c-1])
                {
                    int midpoint = (cornerValues[c] + cornerValues[c-1]) / 2;
                    int distance = radius + Math.Abs(input - midpoint);
                    Console.WriteLine(distance);
                    return true;
                }
            }

            return false;
        }

        private bool SolvePart2(int input)
        {
            Dictionary<Point, int> gridValues = new Dictionary<Point, int>();

            foreach (Point pt in EnumerateSpiralPoints())
            {
                int value;

                if (gridValues.Count == 0)
                {
                    value = 1;
                }
                else
                {
                    value = 0;
                    foreach (Point adjacent in EnumerateAdjacentPoints(pt))
                    {
                        if (gridValues.TryGetValue(adjacent, out int v)) value += v;
                    }
                }

                gridValues.Add(pt, value);
                if (value > input)
                {
                    Console.WriteLine(value);
                    return true;
                }
            }

            return false;
        }

        private IEnumerable<Point> EnumerateSpiralPoints()
        {
            int x = 0, y = 0;
            Point previous;

            yield return previous = new Point(x, y);
            for (int radius = 1; ; radius++)
            {
                yield return previous = new Point(previous.X + 1, previous.Y);
                int edge = 2 * radius;
                for (int i = 1; i < edge; i++) yield return previous = new Point(previous.X, previous.Y - 1);
                for (int i = 0; i < edge; i++) yield return previous = new Point(previous.X - 1, previous.Y);
                for (int i = 0; i < edge; i++) yield return previous = new Point(previous.X, previous.Y + 1);
                for (int i = 0; i < edge; i++) yield return previous = new Point(previous.X + 1, previous.Y);
            }
        }

        private IEnumerable<Point> EnumerateAdjacentPoints(Point pt)
        {
            return new Point[] {
                new Point(pt.X - 1, pt.Y - 1),
                new Point(pt.X, pt.Y - 1),
                new Point(pt.X + 1, pt.Y - 1),
                new Point(pt.X - 1, pt.Y),
                new Point(pt.X + 1, pt.Y),
                new Point(pt.X - 1, pt.Y + 1),
                new Point(pt.X, pt.Y + 1),
                new Point(pt.X + 1, pt.Y + 1)
            };
        }
    }
}
