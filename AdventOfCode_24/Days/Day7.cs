using AdventOfCode_24.Model.Days;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode_24.Days
{
    internal class Day7 : Day
    {
        public override int Year => 2024;

        public override int DayNumber => 7;

        private string Part1()
        {
            long total = 0;
            foreach (var line in Input)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                var s1 = line.Split(':');
                long target = long.Parse(s1[0]);
                var strValues = s1[1].Trim().Split(' ');
                var values = strValues.Select(s => long.Parse(s)).ToArray();

                if (CanSolve(target, values))
                    total += target;
            }


            return total.ToString();
        }

        private string Part2()
        {
            int longest = 0;
            foreach (var line in Input)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                var s1 = line.Split(':');
                long target = long.Parse(s1[0]);
                var strValues = s1[1].Trim().Split(' ');
                if (strValues.Length > longest)
                    longest = strValues.Length;
            }

            Dictionary<int, List<byte[]>> permutationDictionary = GetAllPermutations(longest - 1);

            long total = 0;
            foreach (var line in Input)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                var s1 = line.Split(':');
                long target = long.Parse(s1[0]);
                var strValues = s1[1].Trim().Split(' ');
                var values = strValues.Select(s => long.Parse(s)).ToArray();

                if (CanSolve(target, values, permutationDictionary[values.Length - 1]))
                {
                    Log.Success(line + " Solvable!");
                    total += target;
                }
                else
                    Log.Error(line + " Unsolvable!");
            }


            return total.ToString();
        }

        private Dictionary<int, List<byte[]>> GetAllPermutations(int mostOperators)
        {
            Dictionary<int, List<byte[]>> permutationDictionary = [];
            List<byte[]> previousPermutations = [[0], [1], [2]];
            permutationDictionary.Add(1, previousPermutations);

            for (int i = 2; i <= mostOperators; i++)
            {
                List<byte[]> currentPermutations = [];
                foreach (var p in previousPermutations)
                    for (int j = 0; j < 3; j++)
                        currentPermutations.Add(p.Concat([(byte)j]).ToArray());
                previousPermutations = currentPermutations;
                permutationDictionary.Add(i, currentPermutations);
            }

            return permutationDictionary;
        }

        private bool CanSolve(long target, long[] numbers, List<byte[]> permutations)
        {
            foreach (var p in permutations)
            {
                long total = numbers[0];
                for (int i = 0; i < p.Length; i++)
                {
                    byte op = p[i];
                    switch (op)
                    {
                        case 0:
                            total += numbers[i + 1];
                            break;
                        case 1:
                            total *= numbers[i + 1];
                            break;
                        case 2:
                            total = long.Parse(total.ToString() + numbers[i + 1].ToString());
                            break;
                    }
                }

                if (total == target)
                    return true;
            }

            return false;
        }

        private bool CanSolve(long target, long[] numbers)
        {
            int operators = numbers.Length - 1;

            int counter = 0;
            while (true)
            {
                BitArray ba = new BitArray(new int[]{counter});

                long total = numbers[0];
                for (int i = 0; i < operators; i++)
                {
                    if (ba[i])
                        total += numbers[i + 1];
                    else total *= numbers[i + 1];
                }

                if (total == target)
                    return true;
                counter++;

                // Stop when value is at max
                if (ba[operators] == true)
                    break;
            }

            return false;
        }
    }
}