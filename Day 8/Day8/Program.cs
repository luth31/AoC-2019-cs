using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day8 {
    class Program {
        static void Main(string[] args) {
            StreamReader Reader = new StreamReader(@"/Work/AoC/Day 8/day8.in");
            StreamWriter Writer = new StreamWriter(@"/Work/AoC/Day 8/day8.out");
            int Width = 25, Height = 6;
            int NumOfLayers;
            List<int> Data = new List<int>();
            do {
                int N = Reader.Read();
                Data.Add(N - '0');
            } while (!Reader.EndOfStream);
            Reader.Close();
            NumOfLayers = Data.Count / (Width * Height);
            List<Layer> Layers = new List<Layer>();
            for (int i = 0; i < NumOfLayers; ++i) {
                Layers.Add(new Layer(Data.GetRange(0, Width * Height), Width, Height));
                Data.RemoveRange(0, Width * Height);
            }
            Layer MinLayer = Layers.OrderBy(e => e.CountZero()).FirstOrDefault();
            int Result = MinLayer.SumInts(1, 2);
            Console.WriteLine(Result);
            Writer.WriteLine(Result);
            Layer FinalLayer = new Layer(2, Width, Height);
            for(int i = Layers.Count - 1; i >= 0; --i) {
                FinalLayer = Layers[i].StackOn(FinalLayer);
            }
            string Message = FinalLayer.Decode();
            Console.Write(Message);
            Writer.Write(Message);
            Writer.Close();
        }
    }

    class Layer {
        public Layer(int Fill, int Width, int Height) {
            W = Width;
            H = Height;
            _Data = new int[H, W];
            for (int i = 0; i < H; ++i)
                for (int j = 0; j < W; ++j)
                    _Data[i, j] = Fill;
        }
        public Layer(List<int> Data, int Width, int Height) {
            W = Width;
            H = Height;
            _Data = new int[H, W];
            for (int i = 0; i < H; ++i) {
                for (int j = 0; j < W; ++j)
                    _Data[i, j] = Data[i*W + j];
            }
        }

        public int CountZero() {
            int Total = 0;
            for (int i = 0; i < H; ++i)
                for (int j = 0; j < W; ++j)
                    if (_Data[i, j] == 0)
                        ++Total;
            return Total;
        }

        public int SumInts(int First, int Second) {
            int[] Sum = { 0, 0 };
            int Total;
            for (int i = 0; i < H; ++i)
                for (int j = 0; j < W; ++j) {
                    if (_Data[i, j] == First)
                        ++Sum[0];
                    if (_Data[i, j] == Second)
                        ++Sum[1];
                }
            Total = Sum[0] * Sum[1];
            return Total;
        }

        public Layer StackOn(Layer BaseLayer) { 
            for (int i = 0; i < H; ++i) {
                for (int j = 0; j < W; ++j) {
                    if (_Data[i, j] == 2)
                        continue;
                    BaseLayer._Data[i, j] = _Data[i, j]; 
                }
            }
            return BaseLayer;
        }

        public string Decode() {
            string Message = "";
            for (int i = 0; i < H; ++i) {
                for (int j = 0; j < W; ++j) {
                    if (_Data[i, j] == 0)
                        Message += " ";
                    else
                        Message += "*";
                }
                Message += "\n";
            }
            return Message;
        }

        int[,] _Data;
        int H;
        int W;
    }
}
