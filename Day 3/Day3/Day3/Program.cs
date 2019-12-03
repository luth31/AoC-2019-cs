using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day3 {
    class Program {
        static void Main(string[] args) {
            StreamReader Reader = new StreamReader(@"/Work/AoC/Day 3/day3.in");
            StreamWriter Writer = new StreamWriter(@"/Work/AoC/Day 3/day3.out");
            String[] FirstLine = Reader.ReadLine().Split(",");
            String[] SecondLine = Reader.ReadLine().Split(",");

            GetPath(out PathA, FirstLine);
            GetPath(out PathB, SecondLine);
            List<KeyValuePair<(int,int),int>> Intersect = PathA.Where(d => PathB.ContainsKey(d.Key)).ToList();
            int minIntersection = Intersect.Min(e => Math.Abs(e.Key.Item1) + Math.Abs(e.Key.Item2));
            int bestIntersection = Intersect.Min(e => PathA[e.Key] + PathB[e.Key]);
            Console.WriteLine("Minimum intersection: {0}", minIntersection);
            Console.WriteLine("Best intersection: {0}", bestIntersection);
            Writer.WriteLine(minIntersection);
            Writer.WriteLine(bestIntersection);
            Writer.Close();
            Reader.Close();
        }

        static void GetPath(out Dictionary<(int, int), int> Path, String[] Input) {
            Path = new Dictionary<(int, int), int>();
            (int, int) Pos = (0, 0);
            int Length = 0;
            foreach (string Str in Input) {
                char Dir = Str[0];
                int Steps;
                Int32.TryParse(Str.Substring(1),out Steps);
                for (int i = 0; i < Steps; ++i) {
                    Pos.Item1 += MX[Dir];
                    Pos.Item2 += MY[Dir];
                    Path.TryAdd(Pos, ++Length);
                }
            }
        }

        static Dictionary<(int, int), int> PathA;
        static Dictionary<(int, int), int> PathB;
        static Dictionary<char, int> MX = new Dictionary<char, int>() {
            { 'U', 0 },
            { 'D', 0 },
            { 'L', -1 },
            { 'R', +1 },
        };
        static Dictionary<char, int> MY = new Dictionary<char, int>() {
            { 'U', +1 },
            { 'D', -1 },
            { 'L', 0 },
            { 'R', 0 },
        };
    }
}
