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
            int[] code = File.ReadAllText(@"day7a-input.txt").Split(",").Select(int.Parse).ToArray();

            code = new[] { 3, 26, 1001, 26, -4, 26, 3, 27, 1002, 27, 2, 27, 1, 27, 26, 27, 4, 27, 1001, 28, -1, 28, 1005, 28, 6, 99, 0, 0, 5 };

            List<Intcode> amplifiers = new List<Intcode>
            {
                new Intcode(code).BufferInput(new List<int>{9}),
                new Intcode(code).BufferInput(new List<int>{8}),
                new Intcode(code).BufferInput(new List<int>{7}),
                new Intcode(code).BufferInput(new List<int>{6}),
                new Intcode(code).BufferInput(new List<int>{5})
            };

            (bool, int) input = (false, 0);
            int index = 0;
            while (!input.Item1)
            {
                input = amplifiers[index].Process(new List<int> { input.Item2 });
                index = ++index >= amplifiers.Count ? 0 : index;
                Console.Write(".");
            }
            Console.WriteLine($"Final output: {input.Item2}");
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
