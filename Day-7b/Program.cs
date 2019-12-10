using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_7b
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<IEnumerable<int>> permutations = GetPermutations(Enumerable.Range(5, 5), 5);
            
            int highestThrust = 0;

            foreach (var inputs in permutations)
            {
                int[] code1 = File.ReadAllText(@"day7a-input.txt").Split(",").Select(int.Parse).ToArray();
                int[] code2 = File.ReadAllText(@"day7a-input.txt").Split(",").Select(int.Parse).ToArray();
                int[] code3 = File.ReadAllText(@"day7a-input.txt").Split(",").Select(int.Parse).ToArray();
                int[] code4 = File.ReadAllText(@"day7a-input.txt").Split(",").Select(int.Parse).ToArray();
                int[] code5 = File.ReadAllText(@"day7a-input.txt").Split(",").Select(int.Parse).ToArray();

                var inputArray = inputs.ToArray();

                List<Intcode> amplifiers = new List<Intcode>
                {
                    new Intcode(code1).BufferInput(new List<int> {inputArray[0]}),
                    new Intcode(code2).BufferInput(new List<int> {inputArray[1]}),
                    new Intcode(code3).BufferInput(new List<int> {inputArray[2]}),
                    new Intcode(code4).BufferInput(new List<int> {inputArray[3]}),
                    new Intcode(code5).BufferInput(new List<int> {inputArray[4]})
                };

                (bool, int) input = (false, 0);
                int index = 0;
                while (!input.Item1)
                {
                    input = amplifiers[0].Process(new List<int> {input.Item2});
                    input = amplifiers[1].Process(new List<int> {input.Item2});
                    input = amplifiers[2].Process(new List<int> {input.Item2});
                    input = amplifiers[3].Process(new List<int> {input.Item2});
                    input = amplifiers[4].Process(new List<int> {input.Item2});
                }

                Console.WriteLine($"Output: {input.Item2}");
                if (input.Item2 > highestThrust) highestThrust = input.Item2;
            }

            Console.WriteLine($"Highest thrust achieved: {highestThrust}");
        }

        static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }
    }
}
