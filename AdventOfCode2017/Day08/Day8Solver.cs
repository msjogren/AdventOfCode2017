using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2017
{
    class Day8Solver : IAdventOfCodeSolver
    {
        public bool Solve(int part = 0)
        {
            string[] instructions = File.ReadAllLines("Day08/input.txt");
            Dictionary<string, int> registers = new Dictionary<string, int>();
            int maxIntermediateValue = 0;
            Regex regex = new Regex(@"^(\w+) (inc|dec) (-?\d+) if (\w+) ([<>=!]+) (-?\d+)$");


            int GetRegisterValue(string registerName)
            {
                if (registers.TryGetValue(registerName, out int v))
                {
                    return v;
                }
                else
                {
                    registers.Add(registerName, 0);
                    return 0;
                }
            }

            foreach (string instr in instructions)
            {
                Match m = regex.Match(instr);

                int conditionRegValue = GetRegisterValue(m.Groups[4].Value);
                int conditionRhsValue = int.Parse(m.Groups[6].Value);
                bool result = false;
                switch (m.Groups[5].Value)
                {
                    case ">": result = conditionRegValue > conditionRhsValue; break;
                    case "<": result = conditionRegValue < conditionRhsValue; break;
                    case ">=": result = conditionRegValue >= conditionRhsValue; break;
                    case "<=": result = conditionRegValue <= conditionRhsValue; break;
                    case "==": result = conditionRegValue == conditionRhsValue; break;
                    case "!=": result = conditionRegValue != conditionRhsValue; break;
                    default: throw new InvalidOperationException();
                }

                if (!result) continue;

                int newRegValue, changeValue = int.Parse(m.Groups[3].Value);
                registers[m.Groups[1].Value] = newRegValue = GetRegisterValue(m.Groups[1].Value) + (m.Groups[2].Value == "inc" ? changeValue : -changeValue);
                maxIntermediateValue = Math.Max(maxIntermediateValue, newRegValue);
            }

            Console.WriteLine(part == 2 ? maxIntermediateValue : registers.Values.Max());

            return true;
        }

    }
}
