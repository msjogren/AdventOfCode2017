using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2017
{
    class Day25Solver : IAdventOfCodeSolver
    {
        class State
        {
            public State(IEnumerable<string> definition)
            {
                int lineno = 0;
                bool writeValue0 = false, writeValue1 = false;
                int moveSteps0 = 0, moveSteps1 = 0;
                string nextState0 = null, nextState1 = null;

                foreach (string line in definition)
                {
                    switch (lineno)
                    {
                        case 0: Name = line.Substring(9, 1); break;
                        case 2: writeValue0 = line.EndsWith("1."); break;
                        case 3: moveSteps0 = line.EndsWith("right.") ? 1 : -1; break;
                        case 4: nextState0 = line.Substring(line.Length - 2, 1); break;
                        case 6: writeValue1 = line.EndsWith("1."); break;
                        case 7: moveSteps1 = line.EndsWith("right.") ? 1 : -1; break;
                        case 8: nextState1 = line.Substring(line.Length - 2, 1); break;
                    }

                    lineno++;
                }

                GetWriteValue = (b) => b ? writeValue1 : writeValue0;
                GetMoveSteps = (b) => b ? moveSteps1 : moveSteps0;
                GetNextState = (b) => b ? nextState1 : nextState0;
            }

            public string Name { get; private set; }
            public Func<bool, bool> GetWriteValue;
            public Func<bool, int> GetMoveSteps;
            public Func<bool, string> GetNextState;
        }

        public bool Solve(int part = 0)
        {
            var input = File.ReadAllLines("Day25/input.txt");
            string currentState = input[0].Substring(input[0].Length - 2, 1);
            int afterSteps = input[1].LastIndexOf(' ');
            int stepsStart = input[1].IndexOf("after ") + 6;
            int steps = int.Parse(input[1].Substring(stepsStart, afterSteps - stepsStart));
            Dictionary<string, State> states = new Dictionary<string, State>();

            for (int i = 0; i < (input.Length - 2) / 10; i++)
            {
                var s = new State(input.Skip(3 + 10 * i).Take(9));
                states.Add(s.Name, s);
            }

            const int TapeWidth = 100_000;
            int cursor = TapeWidth / 2;
            bool[] tape = new bool[TapeWidth];
            for (int i = 0; i < steps; i++)
            {
                State s = states[currentState];
                bool currentValue = tape[cursor];
                tape[cursor] = s.GetWriteValue(currentValue);
                cursor += s.GetMoveSteps(currentValue);
                currentState = s.GetNextState(currentValue);
            }

            Console.WriteLine(tape.Count(b => b));

            return true;
        }

    }
}
