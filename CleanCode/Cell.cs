namespace CleanCode
{
	public class Cell
	{
		public const int Black = -1;
		public const int White = 1;

		public static readonly Cell Empty = new Cell(null, White);
		public readonly int Color;
		public readonly Figure Figure;

		public Cell(Figure figure, int color)
		{
			Figure = figure;
			Color = color;
		}

		public bool IsWhiteKing
		{
			get { return Figure == Figure.King && Color == White; }
		}

		public override string ToString()
		{
			string c = Figure == null ? " ." : " " + Figure.Sign;
			return Color == Black ? c.ToLower() : c;
		}
	}
}