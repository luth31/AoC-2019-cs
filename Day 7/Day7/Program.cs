using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day7 {
    class Program {
        static void Main(string[] args) {
            Reader = new StreamReader(@"/Work/AoC/Day 7/day7.in");
            Writer = new StreamWriter(@"/Work/AoC/Day 7/day7.out");
            String[] FileBuffer = Reader.ReadToEnd().Split(',');
            Reader.Close();
            Data = Array.ConvertAll(FileBuffer, int.Parse);
            var Permutations = GetPermutations(Enumerable.Range(5, 5), 5).ToList();
            int Max = 0;
            foreach (var Permutation in Permutations) {
                int Output = 0;
                List<int> PermList = Permutation.ToList();
                ExecutionUnit[] Units = new ExecutionUnit[5];
                Queue<ExecutionUnit> ExecutionQueue = new Queue<ExecutionUnit>();
                for (int i = 0; i < 5; ++i) {
                    Units[i] = new ExecutionUnit(Data, PermList[i]);
                    ExecutionQueue.Enqueue(Units[i]);
                }
                while (ExecutionQueue.Count > 0) {
                    ExecutionQueue.Peek().InputData(Output);
                    ExecutionUnit Unit = ExecutionQueue.Dequeue();
                    Unit.StartOrContinue();
                    Output = Unit.GetOutput();
                    if (!Unit.isFinished)
                        ExecutionQueue.Enqueue(Unit);
                }
                Max = (Output > Max ? Output : Max);
                
            }
            Console.WriteLine(Max);
            Writer.WriteLine(Max);
            Writer.Close();
        }

        // Credits: https://stackoverflow.com/questions/756055/listing-all-permutations-of-a-string-integer
        static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> List, int Length) {
            if (Length == 1) return List.Select(t => new T[] { t });

            return GetPermutations(List, Length - 1)
                .SelectMany(t => List.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        static int[] Data;
        static StreamReader Reader;
        static StreamWriter Writer;
    }

    public class ExecutionUnit {
        public ExecutionUnit(int[] InitData, int InputValue) {
            Data = new int[InitData.Length];
            Array.Copy(InitData, Data, InitData.Length);
            Input = new Queue<int>();
            Input.Enqueue(InputValue);
            isFinished = false;
        }



        public void StartOrContinue() {
            HandleOpcode();
        }

        public void InputData(int Value) {
            Input.Enqueue(Value);
        }

        public int GetOutput() {
            return Output;
        }

        void HandleOpcode() {
            while (Cursor < Data.Length) {
                int OpcodePacked = Data[Cursor];
                int Opcode = OpcodePacked % 100;
                OpcodePacked /= 100;
                List<int> ParamMode = new List<int>();
                while (OpcodePacked > 0) {
                    ParamMode.Add(OpcodePacked % 10);
                    OpcodePacked /= 10;
                }
                switch (Opcode) {
                    case 1:
                        int Sum = EvalParam(ParamMode, Cursor, 0) + EvalParam(ParamMode, Cursor, 1);
                        SetParam(ParamMode, Cursor, 2, Sum);
                        Cursor += 4;
                        break;
                    case 2:
                        int Mul = EvalParam(ParamMode, Cursor, 0) * EvalParam(ParamMode, Cursor, 1);
                        SetParam(ParamMode, Cursor, 2, Mul);
                        Cursor += 4;
                        break;
                    case 3:
                        int InputValue;
                        if (Input.Count > 0)
                            InputValue = Input.Dequeue();
                        else
                            return;
                        SetParam(ParamMode, Cursor, 0, InputValue);
                        Cursor += 2;
                        break;
                    case 4:
                        int Value = EvalParam(ParamMode, Cursor, 0);
                        Output = Value;
                        Cursor += 2;
                        return;
                    case 5:
                        if (EvalParam(ParamMode, Cursor, 0) != 0)
                            Cursor = EvalParam(ParamMode, Cursor, 1);
                        else
                            Cursor += 3;
                        break;
                    case 6:
                        if (EvalParam(ParamMode, Cursor, 0) == 0)
                            Cursor = EvalParam(ParamMode, Cursor, 1);
                        else
                            Cursor += 3;
                        break;
                    case 7:
                        if (EvalParam(ParamMode, Cursor, 0) < EvalParam(ParamMode, Cursor, 1))
                            Data[EvalParamIndex(ParamMode, Cursor, 2)] = 1;
                        else
                            Data[EvalParamIndex(ParamMode, Cursor, 2)] = 0;
                        Cursor += 4;
                        break;
                    case 8:
                        if (EvalParam(ParamMode, Cursor, 0) == EvalParam(ParamMode, Cursor, 1))
                            Data[EvalParamIndex(ParamMode, Cursor, 2)] = 1;
                        else
                            Data[EvalParamIndex(ParamMode, Cursor, 2)] = 0;
                        Cursor += 4;
                        break;
                    default:
                        isFinished = true;
                        return;
                }
            }

        }

        public int EvalParam(List<int> ParamMode, int Index, int Offset) {
            return Data[EvalParamIndex(ParamMode, Index, Offset)];
        }

        public int EvalParamIndex(List<int> ParamMode, int Index, int Offset) {
            bool IsPositionMode = ParamMode.ElementAtOrDefault(Offset) == 0;
            return (IsPositionMode ? Data[Index + Offset + 1] : Index + Offset + 1);
        }

        public void SetParam(List<int> ParamMode, int Index, int Offset, int Value) {
            int RealIndex = EvalParamIndex(ParamMode, Index, Offset);
            Data[RealIndex] = Value;
        }

        public bool isFinished { get; private set; }
        Queue<int> Input;
        int Output = 0;
        int Cursor = 0;
        int[] Data;

    }
}

