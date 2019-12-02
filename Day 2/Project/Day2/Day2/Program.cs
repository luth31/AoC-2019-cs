using System;
using System.IO;

namespace Day2 {
    class Program {
        static void Main(string[] args) {
            StreamReader Reader = new StreamReader(@"/Work/AoC/Day 2/day2.in");
            StreamWriter Writer = new StreamWriter(@"/Work/AoC/Day 2/day2.out");
            String[] FileBuffer = Reader.ReadToEnd().Split(',');
            int[] Arr = Array.ConvertAll(FileBuffer, int.Parse);
            String Buffer = "";
            int? Result = CheckMatch(Arr);
            foreach (int n in Arr)
                Buffer += n + ", ";
            if (Result == null)
                return;
            Console.WriteLine("{0}", Result.Value);
            Writer.WriteLine("{0}", Result.Value);
            Writer.Close();
            Reader.Close();
        }

        static void HandleOpcodes(int[] Arr) {
            int index = 0;
            while (true) {
                switch (Arr[index]) {
                    case 1:
                        Arr[Arr[index + 3]] = Arr[Arr[index + 1]] + Arr[Arr[index + 2]];
                        break;
                    case 2:
                        Arr[Arr[index + 3]] = Arr[Arr[index + 1]] * Arr[Arr[index + 2]];
                        break;
                    case 99:
                        return;
                }
                index += 4;
            }
        }

        static int? CheckMatch(int[] Arr) {
            int[] TempArr = new int[Arr.Length];
            Array.Copy(Arr, TempArr, Arr.Length);
            for (int i = 0; i < 100; ++i)
                for (int j = 0; j < 100; ++j) {
                    TempArr[1] = i;
                    TempArr[2] = j;
                    HandleOpcodes(TempArr);
                    if (TempArr[0] == 19690720) {
                        return 100 * TempArr[1] + TempArr[2];
                    }
                    TempArr = new int[Arr.Length];
                    Array.Copy(Arr, TempArr, Arr.Length);
                }
            return null;
        }
    }
}
