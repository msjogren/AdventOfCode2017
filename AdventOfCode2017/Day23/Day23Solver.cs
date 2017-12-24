using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2017
{
    class Day23Solver : IAdventOfCodeSolver
    {
        public bool Solve(int part = 0)
        {
            return part == 2 ? SolvePart2() : SolvePart1();
        }

        private bool SolvePart1(int part = 0)
        {
            string[] code = File.ReadAllLines("Day23/input.txt");
            Dictionary<string, long> registers = new Dictionary<string, long>() {
                { "a", 0 }, { "b", 0 }, { "c", 0 }, { "d", 0 }, { "e", 0 }, { "f", 0 }, { "g", 0 }, { "h", 0 }};

            int muls = 0;
            int ip = 0;
            bool done = false;

            long GetValue(string regOrInt)
            {
                if (long.TryParse(regOrInt, out long i)) return i;
                if (!registers.ContainsKey(regOrInt)) registers.Add(regOrInt, 0L);
                return registers[regOrInt];
            }

            while (!done)
            {
                string[] asm = code[ip].Split(' ');
                int jmp = 1;
                switch (asm[0])
                {
                    case "set":
                        registers[asm[1]] = GetValue(asm[2]);
                        break;
                    case "sub":
                        registers[asm[1]] = registers[asm[1]] - GetValue(asm[2]);
                        break;
                    case "mul":
                        muls++;
                        registers[asm[1]] = registers[asm[1]] * GetValue(asm[2]);
                        break;
                    case "jnz":
                        {
                            var v = GetValue(asm[1]);
                            if (v != 0) jmp = (int)GetValue(asm[2]);
                            break;
                        }
                }

                ip += jmp;

                if (ip < 0 || ip >= code.Length)
                {
                    Console.WriteLine("Terminating");
                    break;
                }
            }

            Console.WriteLine(muls);

            return true;
        }

        private bool SolvePart2()
        {
            HashSet<int> primes = new HashSet<int>();
            foreach (string line in File.ReadAllLines("Day23/primes.txt"))
            {
                foreach (int i in line.Split('\t').Select(s => int.Parse(s))) primes.Add(i);
            }

            int h = 0;
            // Rough, manual reconstruction of assembly code
            for (int b = 107900; b <= 124900; b += 17)
            {
                /*
                int f = 1; // isprime = true

	            for (int d = 2; d != b ; d++) {
		            for (int e = 2; e != b; e++) {
			            if (b == d * e) {
				            f = 0 // isprime = false
			            }
		            }
	            }

                if (f == 0) // !isprime
                {
                    h++;
                }
                */

                if (!primes.Contains(b))
                {
                    h++;
                }
            }

            Console.WriteLine(h);

            return true;
        }
    }
}
