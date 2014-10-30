using System;
using System.Collections.Generic;

namespace CleanCode
{
	public class Loc
	{
		public readonly int X, Y;

		public Loc(int x, int y)
		{
			X = x;
			Y = y;
		}

		public bool InBoard
		{
			get { return X >= 0 && X <= 7 && Y >= 0 && Y <= 7; }
		}

		public static IEnumerable<Loc> AllBoard()
		{
			for (int y = 0; y < 8; y++)
				for (int x = 0; x < 8; x++)
					yield return new Loc(x, y);
		}

		public override string ToString()
		{
			return new String((char) ('a' + X), 1) + Y;
		}

		public override bool Equals(object obj)
		{
			var other = obj as Loc;
			if (other == null) return false;
			return other.X == X && other.Y == Y;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (X*397) ^ Y;
			}
		}
	}
}