namespace CleanCode
{
	public class Chess
	{
		private readonly Board b;

		public Chess(Board b)
		{
		this.b = b;
		}

		public string getWhiteStatus() {
			bool bad=checkForWhite();
			bool ok=  false;
			foreach (Loc loc1 in b.Figures(Cell.White))
			{
				foreach (Loc loc2 in b.Get(loc1).Figure.Moves(loc1, b)){
				Cell old_dest = b.PerformMove(loc1, loc2);
				if (!checkForWhite( ))
					ok = true;
				b.PerformUndoMove(loc1, loc2, old_dest);
				}
				
				
				
			}
			if (bad)
				if (ok)
					return "check";
				else return "mate";
				if (ok)	return "ok";
			return "stalemate";
		}

		private bool checkForWhite()
		{
			bool bFlag = false;
			foreach (Loc loc in b.Figures(Cell.Black))
			{
				var cell = b.Get(loc);
				var moves = cell.Figure.Moves(loc, b);
				foreach (Loc to in moves)
				{
					if (b.Get(to).IsWhiteKing)
						bFlag = true;
				}
			}
			if (bFlag) return true;
			return false;
		}
	}
}