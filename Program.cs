using System.Diagnostics;

namespace ReadonlyStruct
{
    internal class Program
    {
		readonly struct ReadonlyPoint
		{
			public int X { get; }
			public int Y { get; }

			public ReadonlyPoint(int x, int y)
			{
				X = x;
				Y = y;
			}

			// This method ensures no copies of 'this' are created
			public override string ToString() => $"({X}, {Y})";
		}
		static void ProcessReadonlyPoint(in ReadonlyPoint p)
		{
			// No defensive copy is created
			Console.WriteLine(p);
		}

		struct Point
		{
			public int X { get; set; }
			public int Y { get; set; }

			public Point(int x, int y) => (X, Y) = (x, y);

			public override string ToString() => $"({X}, {Y})";
		}
		static void ProcessPoint(Point p)
		{
			// This method causes a defensive copy if Point is passed by value
			Console.WriteLine(p);
		}


		static void Main(string[] args)
        {
			var normalPoint = new Point(10, 20);
			var readonlyPoint = new ReadonlyPoint(30, 40);

			Console.WriteLine("Processing normal struct...");
			MeasureExecutionTime(() => {
				for (int i = 0; i < 100_000; i++) {
					ProcessPoint(normalPoint);
				}
			});
			Console.ReadLine();
			Console.WriteLine("Processing readonly struct...");
			MeasureExecutionTime(() => {
				for (int i = 0; i < 100_000; i++) {
					ProcessReadonlyPoint(in readonlyPoint);
				}
			});

			Console.WriteLine("Done!");
		}
		static void MeasureExecutionTime(Action action)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			action();
			stopwatch.Stop();
			Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");
		}
	}
}
