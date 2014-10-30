using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanCode
{
	public class Figure
	{
		public static readonly Figure Rook = new Figure(true, 'R', new Loc(1, 0), new Loc(0, 1));
		public static readonly Figure King = new Figure(false, 'K', new Loc(1, 1), new Loc(1, 0), new Loc(0, 1));
		public static readonly Figure Queen = new Figure(true, 'Q', new Loc(1, 1), new Loc(1, 0), new Loc(0, 1));
		public static readonly Figure Bishop = new Figure(true, 'B', new Loc(1, 1));
		public static readonly Figure Knight = new Figure(false, 'N', new Loc(2, 1), new Loc(1, 2));
		private static readonly Figure[] map = new Figure[128];

		private readonly Loc[] ds;
		private readonly bool infinit;

		static Figure()
		{
			foreach (Figure f in new[] {King, Queen, Rook, Knight, Bishop})
				map[f.Sign] = f;
			map['.'] = null;
		}

		public Figure(bool infinit, char sign, params Loc[] ds)
		{
			this.infinit = infinit;
			this.ds = ds
				.Union(ds.Select(dd => new Loc(-dd.X, dd.Y)))
				.Union(ds.Select(dd => new Loc(dd.X, -dd.Y)))
				.Union(ds.Select(dd => new Loc(-dd.X, -dd.Y)))
				.ToArray();
			Sign = sign;
		}

		public char Sign { get; private set; }

		public static Figure FromChar(char c)
		{
			return map[Char.ToUpper(c)];
		}

		public IEnumerable<Loc> Moves(Loc loc, Board board)
		{
			return ds.SelectMany(d => MovesInOneDirection(loc, board, d, infinit));
		}
		
		private static IEnumerable<Loc> MovesInOneDirection(Loc from, Board board, Loc dir, bool infinit)
		{
			Cell fromCell = board.Get(from);
			for (int i = 1; i < (infinit ? 8 : 2); i++)
			{
				var to = new Loc(from.X + dir.X*i, from.Y + dir.Y*i);
				if (!to.InBoard) break;
				Cell toCell = board.Get(to);
				if (toCell.Figure == null) yield return to;
				else
				{
					if (toCell.Color != fromCell.Color) yield return to;
					yield break;
				}
			}
		}
	}
}