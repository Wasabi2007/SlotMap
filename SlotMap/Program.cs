using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotMap
{
	class Program
	{
		private static readonly Random r = new Random();

		public struct Color
		{
			public double r;
			public double g;
			public double b;
			public double a;

			public Color(double r, double g, double b, double a)
			{
				this.r = r;
				this.g = g;
				this.b = b;
				this.a = a;
			}

			public static Color operator* (Color col, double rh)
			{
				col.r += rh;
				col.g += rh;
				col.b += rh;
				col.a += rh;

				return col;
			}
		}

		public struct Color_class
		{
			public double r;
			public double g;
			public double b;
			public double a;

			public Color_class(double r, double g, double b, double a)
			{
				this.r = r;
				this.g = g;
				this.b = b;
				this.a = a;
			}

			public static Color_class operator *(Color_class col, double rh)
			{
				col.r += rh;
				col.g += rh;
				col.b += rh;
				col.a += rh;

				return col;
			}
		}

		public static Color NextColor()
		{
			return new Color(r.NextDouble(), r.NextDouble(), r.NextDouble(), r.NextDouble());
		}

		public static Color_class NextColor_class()
		{
			return new Color_class(r.NextDouble(), r.NextDouble(), r.NextDouble(), r.NextDouble());
		}

		static void Main(string[] args)
		{
			int numbers = 1000000;

			Dictionary<int, Color> randomDoublesDic = new Dictionary<int, Color>();
			int[] DicKeys = new int[numbers];


			SlotMap<Color> randomDoublesSlotMap = new SlotMap<Color>();
			SlotMap<Color>.Key[] slotmapKeys = new SlotMap<Color>.Key[numbers];

			Stopwatch insertionSlotMapWatch = Stopwatch.StartNew();
			for (int i = 0; i < numbers; i++)
			{
				slotmapKeys[i] = randomDoublesSlotMap.Add(NextColor());
			}
			insertionSlotMapWatch.Stop();
			Console.WriteLine($"SlotMap Insertion for {numbers} elements took {insertionSlotMapWatch.ElapsedMilliseconds} ms");

			Stopwatch insertionDicWatch = Stopwatch.StartNew();
			for(int i = 0; i < numbers; i++)
			{
				randomDoublesDic.Add(i, NextColor());
				DicKeys[i] = i;
			}
			insertionDicWatch.Stop();
			Console.WriteLine($"Dictonary Insertion for {numbers} elements took {insertionDicWatch.ElapsedMilliseconds} ms");



			Stopwatch itterationSlotMapWatch = Stopwatch.StartNew();
			for(int i = 0; i < slotmapKeys.Length; i++)
			{
				randomDoublesSlotMap[slotmapKeys[i]] *= 2;
			}
			itterationSlotMapWatch.Stop();
			Console.WriteLine($"SlotMap keyed Itteration over {numbers} elements took {itterationSlotMapWatch.ElapsedMilliseconds} ms");

			Stopwatch itterationDicWatch = Stopwatch.StartNew();
			for(int i = 0; i < DicKeys.Length; i++)
			{
				randomDoublesDic[DicKeys[i]] *= 2;
			}
			insertionDicWatch.Stop();
			Console.WriteLine($"Dictonary keyed Itteration over {numbers} elements took {itterationDicWatch.ElapsedMilliseconds} ms");


			itterationSlotMapWatch = Stopwatch.StartNew();
			for(int i = 0; i < randomDoublesSlotMap.Count; i++)
			{
				var result = randomDoublesSlotMap[i] * 2;
			}
			itterationSlotMapWatch.Stop();
			Console.WriteLine($"SlotMap Itteration over {numbers} elements took {itterationSlotMapWatch.ElapsedMilliseconds} ms");

			itterationDicWatch = Stopwatch.StartNew();
			foreach (var d in randomDoublesDic)
			{
				var result = d.Value * 2;
			}
			insertionDicWatch.Stop();
			Console.WriteLine($"Dictonary Itteration over {numbers} elements took {itterationDicWatch.ElapsedMilliseconds} ms");


			Stopwatch delsInsertSlotMapWatch = Stopwatch.StartNew();
			for(int i = 0; i < numbers/4; i++)
			{
				var keyindex = r.Next(0, numbers);
				var key = slotmapKeys[keyindex];
				randomDoublesSlotMap.Remove(key);
				slotmapKeys[keyindex] = randomDoublesSlotMap.Add(NextColor());
			}
			delsInsertSlotMapWatch.Stop();
			Console.WriteLine($"SlotMap deleting and adding {numbers / 4} elements took {delsInsertSlotMapWatch.ElapsedMilliseconds} ms");

			Stopwatch delsInsertDicWatch = Stopwatch.StartNew();
			for(int i = 0; i < numbers / 4; i++)
			{
				var keyindex = r.Next(0, numbers);
				randomDoublesDic.Remove(DicKeys[keyindex]);
				randomDoublesDic.Add(DicKeys[keyindex],NextColor());
			}
			delsInsertDicWatch.Stop();
			Console.WriteLine($"Dictonary deleting and adding {numbers / 4} elements took {delsInsertDicWatch.ElapsedMilliseconds} ms");

			itterationSlotMapWatch = Stopwatch.StartNew();
			for(int i = 0; i < slotmapKeys.Length; i++)
			{
				randomDoublesSlotMap[slotmapKeys[i]] *= 2;
			}
			itterationSlotMapWatch.Stop();
			Console.WriteLine($"SlotMap keyed Itteration 2 over {numbers} elements took {itterationSlotMapWatch.ElapsedMilliseconds} ms");

			itterationDicWatch = Stopwatch.StartNew();
			for(int i = 0; i < DicKeys.Length; i++)
			{
				randomDoublesDic[DicKeys[i]] *= 2;
			}
			insertionDicWatch.Stop();
			Console.WriteLine($"Dictonary keyed Itteration 2 over {numbers} elements took {itterationDicWatch.ElapsedMilliseconds} ms");


			itterationSlotMapWatch = Stopwatch.StartNew();
			for(int i = 0; i < randomDoublesSlotMap.Count; i++)
			{
				var result = randomDoublesSlotMap[i] * 2;
			}
			itterationSlotMapWatch.Stop();
			Console.WriteLine($"SlotMap Itteration 2 over {numbers} elements took {itterationSlotMapWatch.ElapsedMilliseconds} ms");

			itterationDicWatch = Stopwatch.StartNew();
			foreach(var d in randomDoublesDic)
			{
				var result = d.Value * 2;
			}
			insertionDicWatch.Stop();
			Console.WriteLine($"Dictonary Itteration 2 over {numbers} elements took {itterationDicWatch.ElapsedMilliseconds} ms");


			Stopwatch randomAccessSlotMapWatch = Stopwatch.StartNew();
			for(int i = 0; i < numbers / 4; i++)
			{
				var keyindex = r.Next(0, numbers);
				var result = randomDoublesSlotMap[slotmapKeys[keyindex]] * 2;
			}
			randomAccessSlotMapWatch.Stop();
			Console.WriteLine($"SlotMap random key access with {numbers / 4} elements took {randomAccessSlotMapWatch.ElapsedMilliseconds} ms");

			Stopwatch randomAccessDicWatch = Stopwatch.StartNew();
			for(int i = 0; i < DicKeys.Length; i++)
			{
				var keyindex = r.Next(0, numbers);
				var result = randomDoublesDic[DicKeys[keyindex]] * 2;
			}
			randomAccessDicWatch.Stop();
			Console.WriteLine($"Dictonary random key access with {numbers / 4} elements took {randomAccessDicWatch.ElapsedMilliseconds} ms");

			Console.ReadLine();
		}
	}
}
