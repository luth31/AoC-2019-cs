using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day5 {
    class Program {
        static void Main(string[] args) {
            Reader = new StreamReader(@"/Work/AoC/Day 5/day5.in");
            Writer = new StreamWriter(@"/Work/AoC/Day 5/day5.out");
            String[] FileBuffer = Reader.ReadToEnd().Split(',');
            Data = Array.ConvertAll(FileBuffer, int.Parse);
            int Index = 0;
            while(true) {
                if (!HandleOpcodes(ref Index))
                    break;
            }
            Writer.Close();
            Reader.Close();
        }

        static bool HandleOpcodes(ref int Index) {
            int OpcodePacked = Data[Index];
            int Opcode = OpcodePacked % 100;
            OpcodePacked /= 100;
            List<int> ParamMode = new List<int>();
            while (OpcodePacked > 0) {
                ParamMode.Add(OpcodePacked % 10);
                OpcodePacked /= 10;
            }
            switch (Opcode) {
                case 1:
                    int Sum = EvalParam(ParamMode, Index, 0) + EvalParam(ParamMode, Index, 1);
                    SetParam(ParamMode, Index, 2, Sum);
                    Index += 4;
                    break;
                case 2:
                    int Mul = EvalParam(ParamMode, Index, 0) * EvalParam(ParamMode, Index, 1);
                    SetParam(ParamMode, Index, 2, Mul);
                    Index += 4;
                    break;
                case 3:
                    int Input;
                    Int32.TryParse(Console.ReadLine(), out Input);
                    SetParam(ParamMode, Index, 0, Input);
                    Index += 2;
                    break;
                case 4:
                    int Value = EvalParam(ParamMode, Index, 0);
                    Console.WriteLine(Value);
                    Writer.WriteLine(Value);
                    Index += 2;
                    break;
                case 5:
                    if (EvalParam(ParamMode, Index, 0) != 0)
                        Index = EvalParam(ParamMode, Index, 1);
                    else
                        Index += 3;
                    break;
                case 6:
                    if (EvalParam(ParamMode, Index, 0) == 0)
                        Index = EvalParam(ParamMode, Index, 1);
                    else
                        Index += 3;
                    break;
                case 7:
                    if (EvalParam(ParamMode, Index, 0) < EvalParam(ParamMode, Index, 1))
                        Data[EvalParamIndex(ParamMode, Index, 2)] = 1;
                    else
                        Data[EvalParamIndex(ParamMode, Index, 2)] = 0;
                    Index += 4;
                    break;
                case 8:
                    if (EvalParam(ParamMode, Index, 0) == EvalParam(ParamMode, Index, 1))
                        Data[EvalParamIndex(ParamMode, Index, 2)] = 1;
                    else
                        Data[EvalParamIndex(ParamMode, Index, 2)] = 0;
                    Index += 4;
                    break;
                default:
                    return false;
            }
            return true;

        }

        static public int EvalParam(List<int> ParamMode, int Index, int Offset) {
            return Data[EvalParamIndex(ParamMode, Index, Offset)];
        }

        static public int EvalParamIndex(List<int> ParamMode, int Index, int Offset) {
            bool IsPositionMode = ParamMode.ElementAtOrDefault(Offset) == 0;
            return (IsPositionMode ? Data[Index + Offset + 1] : Index + Offset + 1);
        }

        static public void SetParam(List<int> ParamMode, int Index, int Offset, int Value) {
            int RealIndex = EvalParamIndex(ParamMode, Index, Offset);
            Data[RealIndex] = Value;
        }

        static int[] Data;
        static StreamReader Reader;
        static StreamWriter Writer;
    }
}
