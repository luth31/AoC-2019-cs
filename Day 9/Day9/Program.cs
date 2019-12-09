using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day9 {
    class Program {
        static void Main(string[] args) {
            Reader = new StreamReader(@"/Work/AoC/Day 9/day9.in");
            Writer = new StreamWriter(@"/Work/AoC/Day 9/day9.out");
            String[] FileBuffer = Reader.ReadToEnd().Split(',');
            Data = new Dictionary<Int64, Int64>();
            foreach (string Input in FileBuffer) {
                Data.Add(Data.Count, Int64.Parse(Input));
            }
            Index = 0;
            while (true) {
                if (!HandleOpcodes())
                    break;
            }
            Writer.Close();
            Reader.Close();
        }

        static bool HandleOpcodes() {
            int OpcodePacked = (int)Data[Index];
            int Opcode = OpcodePacked % 100;
            OpcodePacked /= 100;
            ParamMode = new List<int>();
            while (OpcodePacked > 0) {
                ParamMode.Add(OpcodePacked % 10);
                OpcodePacked /= 10;
            }
            switch (Opcode) {
                case 1:
                    Int64 Sum = EvalParam(0) + EvalParam(1);
                    SetParam(2, Sum);
                    Index += 4;
                    break;
                case 2:
                    Int64 Mul = EvalParam(0) * EvalParam(1);
                    SetParam(2, Mul);
                    Index += 4;
                    break;
                case 3:
                    int Input;
                    Int32.TryParse(Console.ReadLine(), out Input);
                    SetParam(0, Input);
                    Index += 2;
                    break;
                case 4:
                    Int64 Value = EvalParam(0);
                    Console.WriteLine(Value);
                    Writer.WriteLine(Value);
                    Index += 2;
                    break;
                case 5:
                    if (EvalParam(0) != 0)
                        Index = EvalParam(1);
                    else
                        Index += 3;
                    break;
                case 6:
                    if (EvalParam(0) == 0)
                        Index = EvalParam(1);
                    else
                        Index += 3;
                    break;
                case 7:
                    if (EvalParam(0) < EvalParam(1))
                        Data[EvalParamIndex(2)] = 1;
                    else
                        Data[EvalParamIndex(2)] = 0;
                    Index += 4;
                    break;
                case 8:
                    if (EvalParam(0) == EvalParam(1))
                        Data[EvalParamIndex(2)] = 1;
                    else
                        Data[EvalParamIndex(2)] = 0;
                    Index += 4;
                    break;
                case 9:
                    UpdateBase(EvalParam(0));
                    Index += 2;
                    break;
                default:
                    return false;
            }
            return true;

        }

        static public Int64 EvalParam(int Offset) {
            Int64 Value;
            Data.TryGetValue(EvalParamIndex(Offset), out Value);
            return Value;
        }

        static public Int64 EvalParamIndex(int Offset) {
            int PositionMode = ParamMode.ElementAtOrDefault(Offset);
            Int64 RealIndex;
            if (PositionMode == 0)
                RealIndex = Data[Index + Offset + 1];
            else if (PositionMode == 1)
                RealIndex = Index + Offset + 1;
            else {
                RealIndex = GetBase() + Data[Index + Offset + 1];
            }
            return RealIndex;
        }

        static public void SetParam(int Offset, Int64 Value) {
            Int64 RealIndex = EvalParamIndex(Offset);
            Data[RealIndex] = Value;
        }

        static public void UpdateBase(Int64 Value) {
            RelativeBase += Value;
        }

        static public Int64 GetBase() {
            return RelativeBase;
        }

        static List<int> ParamMode;
        static Int64 Index;
        static Int64 RelativeBase = 0;
        static Dictionary<Int64, Int64> Data;
        static StreamReader Reader;
        static StreamWriter Writer;
    }
}
