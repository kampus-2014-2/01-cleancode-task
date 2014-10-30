using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CleanCode
{
	public class Board
	{
		private readonly Cell[,] cells = new Cell[8,8];

		public Board(TextReader inp)
		{
			for (int y = 0; y < 8; y++)
			{
				string line = inp.ReadLine();
				if (line == null) throw new Exception("incorrect input");
				for (int x = 0; x < 8; x++)
				{
					char figureSign = line[x];
					int color = Char.IsUpper(figureSign) ? Cell.White : Cell.Black;
					Set(new Loc(x, y), new Cell(Figure.FromChar(figureSign), color));
				}
			}
		}

		public IEnumerable<Loc> Figures(int color)
		{
			return Loc.AllBoard().Where(loc => Get(loc).Figure != null && Get(loc).Color == color);
		}

		public Cell Get(Loc loc)
		{
			return !loc.InBoard ? Cell.Empty : cells[loc.X, loc.Y];
		}

		public void Set(Loc loc, Cell cell)
		{
			cells[loc.X, loc.Y] = cell;
		}

		public override string ToString()
		{
			var b = new StringBuilder();
			for (int y = 0; y < 8; y++)
			{
				for (int x = 0; x < 8; x++)
					b.Append(Get(new Loc(x, y)));
				b.AppendLine();
			}
			return b.ToString();
		}

		public Cell PerformMove(Loc from, Loc to)
		{
			Cell old = Get(to);
			Set(to, Get(from));
			Set(from, Cell.Empty);
			return old;
		}

		public void PerformUndoMove(Loc from, Loc to, Cell old)
		{
			Set(from, Get(to));
			Set(to, old);
		}
	}
}