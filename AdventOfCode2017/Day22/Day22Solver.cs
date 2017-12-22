using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace AdventOfCode2017
{
    class Day22Solver : IAdventOfCodeSolver
    {
        static readonly Point Up = new Point(0, -1);
        static readonly Point Down = new Point(0, 1);
        static readonly Point Left = new Point(-1, 0);
        static readonly Point Right = new Point(1, 0);

        enum State
        {
            Infected,
            Weakened,
            Flagged
        }

        Dictionary<Point, State> _infectedPoints = new Dictionary<Point, State>();

        public bool Solve(int part = 0)
        {
            var input = File.ReadAllLines("Day22/input.txt");
            int initialSize = input[0].Length;
            Point currentPos = new Point(initialSize / 2, initialSize / 2);

            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < initialSize; x++)
                {
                    if (input[y][x] == '#') _infectedPoints.Add(new Point(x, y), State.Infected);
                }
            }

            int infections = part == 2 ? SolvePart2(currentPos) : SolvePart1(currentPos);

            Console.WriteLine(infections);

            return true;
        }

        private int SolvePart1(Point currentPos)
        {
            Point direction = Up;
            int infections = 0;

            for (int burst = 0; burst < 10_000; burst++)
            {
                if (_infectedPoints.ContainsKey(currentPos))
                {
                    direction = TurnRight(direction);
                    _infectedPoints.Remove(currentPos);
                }
                else
                {
                    direction = TurnLeft(direction);
                    _infectedPoints.Add(currentPos, State.Infected);
                    infections++;
                }

                currentPos = new Point(currentPos.X + direction.X, currentPos.Y + direction.Y);
            }

            return infections;
        }

        private int SolvePart2(Point currentPos)
        {
            Point direction = Up;
            int infections = 0;
            
            for (int burst = 0; burst < 10_000_000; burst++)
            {
                if (_infectedPoints.TryGetValue(currentPos, out State prevState))
                {
                    if (prevState == State.Infected)
                    {
                        direction = TurnRight(direction);
                        _infectedPoints[currentPos] = State.Flagged;
                    }
                    else if (prevState == State.Flagged)
                    {
                        direction = TurnRight(TurnRight(direction));

                        _infectedPoints.Remove(currentPos);
                    }
                    else if (prevState == State.Weakened)
                    {
                        _infectedPoints[currentPos] = State.Infected;
                        infections++;
                    }
                }
                else
                {
                    direction = TurnLeft(direction);
                    _infectedPoints.Add(currentPos, State.Weakened);
                }

                currentPos = new Point(currentPos.X + direction.X, currentPos.Y + direction.Y);
            }
            
            return infections;
        }

        private Point TurnRight(Point direction)
        {
            if (direction == Up) return Right;
            if (direction == Down) return Left;
            if (direction == Right) return Down;
            return Up;
        }

        private Point TurnLeft(Point direction)
        {
            if (direction == Up) return Left;
            if (direction == Down) return Right;
            if (direction == Right) return Up;
            return Down;
        }
    }
}
