using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2017
{
    class Day18Solver : IAdventOfCodeSolver
    {
        public bool Solve(int part = 0)
        {
            string[] code = File.ReadAllLines("Day18/input.txt");
            return part == 2 ? SolvePart2(code) : SolvePart1(code);
        }

        private bool SolvePart1(string[] code)
        {
            Dictionary<string, long> registers = new Dictionary<string, long>();
            long lastSoundFreq = 0;
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
                    case "snd":
                        lastSoundFreq = GetValue(asm[1]);
                        break;
                    case "rcv":
                        {
                            var v = GetValue(asm[1]);
                            if (v != 0)
                            {
                                Console.WriteLine("rcv " + lastSoundFreq);
                                done = true;
                            }
                            break;
                        }
                    case "set":
                        registers[asm[1]] = GetValue(asm[2]);
                        break;
                    case "add":
                        registers[asm[1]] = GetValue(asm[1]) + GetValue(asm[2]);
                        break;
                    case "mul":
                        registers[asm[1]] = GetValue(asm[1]) * GetValue(asm[2]);
                        break;
                    case "mod":
                        registers[asm[1]] = GetValue(asm[1]) % GetValue(asm[2]);
                        break;
                    case "jgz":
                        {
                            var v = GetValue(asm[1]);
                            if (v > 0) jmp = (int)GetValue(asm[2]);
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

            return true;
        }


        class Computer
        {
            int _id;
            Dictionary<string, long> _registers = new Dictionary<string, long>();
            string[] _code;
            Queue<long> _messageQueue = new Queue<long>();
            int _ip = 0;

            public event EventHandler<long> Send;

            public bool Waiting { get; private set; }
            public bool Terminated { get; set; }

            public Computer(string[] code, int p)
            {
                _code = code;
                _id = p;
                _registers["p"] = p;
            }

            public void Receive(long msg) => _messageQueue.Enqueue(msg);

            private long GetValue(string regOrInt)
            {
                if (long.TryParse(regOrInt, out long i)) return i;
                if (!_registers.ContainsKey(regOrInt)) _registers.Add(regOrInt, 0L);
                return _registers[regOrInt];
            }

            public bool Tick()
            {
                if (Terminated) return false;

                string[] asm = _code[_ip].Split(' ');
                int jmp = 1;
                switch (asm[0])
                {
                    case "snd":
                        Send(this, GetValue(asm[1]));
                        break;
                    case "rcv":
                        {
                            if (_messageQueue.TryDequeue(out long i))
                            {
                                _registers[asm[1]] = i;
                                Waiting = false;
                            }
                            else
                            {
                                Waiting = true;
                                jmp = 0;
                            }
                            break;
                        }
                    case "set":
                        _registers[asm[1]] = GetValue(asm[2]);
                        break;
                    case "add":
                        _registers[asm[1]] = GetValue(asm[1]) + GetValue(asm[2]);
                        break;
                    case "mul":
                        _registers[asm[1]] = GetValue(asm[1]) * GetValue(asm[2]);
                        break;
                    case "mod":
                        _registers[asm[1]] = GetValue(asm[1]) % GetValue(asm[2]);
                        break;
                    case "jgz":
                        {
                            var v = GetValue(asm[1]);
                            if (v > 0) jmp = (int)GetValue(asm[2]);
                            break;
                        }
                }

                _ip += jmp;

                if (_ip < 0 || _ip >= _code.Length)
                {
                    Console.WriteLine("Terminating " + _id);
                    Terminated = true;
                    return false;
                }

                return true;
            }
        }

        bool SolvePart2(string[] code)
        {
            Computer c1 = new Computer(code, 0);
            Computer c2 = new Computer(code, 1);
            int c2Sends = 0;

            c1.Send += (s, i) => c2.Receive(i);
            c2.Send += (s, i) => { c2Sends++; c1.Receive(i); };

            while (!c1.Terminated || !c2.Terminated)
            {
                c1.Tick();
                c2.Tick();
                if (c1.Waiting && c2.Waiting)
                {
                    c1.Terminated = true;
                    c2.Terminated = true;
                    Console.WriteLine("Deadlock detected");
                }
            }

            Console.WriteLine(c2Sends);

            return true;
        }

    }
}
