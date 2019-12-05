using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day4 {
    class Program {
        static void Main(string[] args) {
            StreamReader Reader = new StreamReader(@"/Work/AoC/Day 4/day4.in");
            StreamWriter Writer = new StreamWriter(@"/Work/AoC/Day 4/day4.out");
            string[] Data = Reader.ReadToEnd().Trim('\n').Split("-");
            int Min;
            int Max;
            int Count = 0;
            Int32.TryParse(Data[0], out Min);
            Int32.TryParse(Data[1], out Max);
            for(int i = Min; i <= Max; ++i) {
                if (IsAllowed(i))
                    ++Count;
            }
            Console.WriteLine(Count);
            Writer.WriteLine(Count);
            Writer.Close();
            Reader.Close();
        }

        static bool IsAllowed(int Number) {
            List<int> Digits = GetDigits(Number);
            for (int i = 0; i < Digits.Count - 1; ++i) {
                if (Digits[i] > Digits[i + 1])
                    return false;
                if (!Digits.GroupBy(d => d).Any(d => d.Count() == 2))
                    return false;
            }
            return true;
        }

        static List<int> GetDigits(int Number) {
            List<int> List = new List<int>();
            while(Number > 0) {
                int digit = Number % 10;
                List.Add(digit);
                Number /= 10;
            }
            List.Reverse();
            return List;
        }
    }
}
