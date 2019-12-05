using System;
using System.IO;

namespace Day1
{
    class Program {
        static void Main(string[] args) {
            StreamReader Reader = new StreamReader(@"/Work/AoC/Day 1/day1.in");
            StreamWriter Writer = new StreamWriter(@"/Work/AoC/Day 1/day1.out");
            String Line;
            int Fuel = 0;
            while ((Line = Reader.ReadLine()) != null) {
                int Mass;
                Int32.TryParse(Line, out Mass);
                Fuel += CalculateFuel(Mass);
            }
            Console.WriteLine("{0} fuel is required",Fuel);
            Writer.WriteLine(Fuel);
            Writer.Close();
            Reader.Close();
        }

        static int CalculateFuel(int Mass) {
            int Fuel = Mass;
            Fuel /= 3;
            Fuel -= 2;
            if (Fuel < 0)
                return 0;
            return Fuel + CalculateFuel(Fuel);
        }
    }
}
