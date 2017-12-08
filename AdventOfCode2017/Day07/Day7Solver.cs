using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2017
{
    class Day7Solver : IAdventOfCodeSolver
    {
        class Tower
        {
            private List<Tower> _subtowers = new List<Tower>();

            public Tower(string name) { Name = name; }

            public string Name { get; private set; }
            public IReadOnlyList<Tower> SubTowers => _subtowers;
            public bool HasParent { get; private set; }
            public int Weight { get; set; }

            public void AddSubtower(Tower subtower)
            {
                subtower.HasParent = true;
                _subtowers.Add(subtower);
            }

            public int GetCombinedWeight() => Weight + _subtowers.Sum(t => t.GetCombinedWeight());

            public bool IsBalanced => _subtowers.Count < 2 ? true : _subtowers.Select(t => t.GetCombinedWeight()).Distinct().Count() == 1;
        }

        public bool Solve(int part = 0)
        {
            string[] towerSpecs = File.ReadAllLines("Day07/input.txt");
            Dictionary<string, Tower> towers = new Dictionary<string, Tower>();
            Regex regex = new Regex(@"^(\w+) \((\d+)\)\s?(-> ([\w\s,]+))?$");

            Tower GetTower(string name)
            {
                if (towers.TryGetValue(name, out Tower t))
                {
                    return t;
                }
                else
                {
                    var newTower = new Tower(name);
                    towers.Add(name, newTower);
                    return newTower;
                }
            }

            foreach (string spec in towerSpecs)
            {
                Match m = regex.Match(spec);

                var tower = GetTower(m.Groups[1].Value);
                tower.Weight = int.Parse(m.Groups[2].Value);

                if (m.Groups[4].Success)
                {
                    foreach (var subTowerName in m.Groups[4].Value.Split(", "))
                    {
                        tower.AddSubtower(GetTower(subTowerName));
                    }
                }
            }

            var bottomTower = towers.Values.First(t => t.SubTowers.Count > 0 && !t.HasParent);

            if (part == 2)
            {
                var faultyTower = bottomTower;
                while (faultyTower != null)
                {
                    var unbalancedSubTower = faultyTower.SubTowers.FirstOrDefault(t => !t.IsBalanced);
                    if (unbalancedSubTower == null)
                    {
                        var weightGroups =
                            from sub in faultyTower.SubTowers
                            group sub by sub.GetCombinedWeight() into g
                            select g;

                        int correctWeight = weightGroups.Where(g => g.Count() > 1).Select(g => g.Key).First();
                        var wrongWeightTower = weightGroups.First(g => g.Count() == 1).First();
                        int weightDiff = wrongWeightTower.GetCombinedWeight() - correctWeight;

                        Console.WriteLine($"{wrongWeightTower.Name} weight is {wrongWeightTower.Weight}, should be {wrongWeightTower.Weight - weightDiff}");
                    }

                    faultyTower = unbalancedSubTower;
                }
            }
            else
            {
                Console.WriteLine(bottomTower.Name);
            }

            return true;
        }
    }
}
