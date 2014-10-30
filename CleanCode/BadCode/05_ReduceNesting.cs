using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanCode.Tasks.IsCheck
{
    public struct Position
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public Position(int Row, int Column)
            : this()
        {
            this.Row = Row;
            this.Column = Column;
        }
        public bool IsValid()
        {
            return this.Row >= 0 && this.Column >= 0 && this.Row < 8 && this.Column < 8;
        }
    }

    public enum FigureColor
    {
        BLACK,
        WHITE
    }

    public abstract class Figure
    {
        public FigureColor Color;
        public ChessBoard Board;
        public abstract List<Position> GetMoves(int x, int y);
        protected abstract int[] GetDx() ;
        protected abstract int[] GetDy() ;
    }

    // more than one field
    public abstract class Figure1: Figure
    {
        public override List<Position> GetMoves(int x, int y)
        {
            int[] dx = GetDx();
            int[] dy = GetDy();
            List<Position> Result = new List<Position>();
            Position curpos = new Position(x, y);
            for (int dir = 0; dir < dx.Count(); ++dir)
            {
                for (int i = 1; ; ++i)
                {
                    Position newpos = new Position(curpos.Row + dx[dir] * i, curpos.Column + dy[dir] * i);
                    if (!newpos.IsValid())
                        break;
                    if (Board.Cells[newpos.Row, newpos.Column] == null || Board.Cells[newpos.Row, newpos.Column].Color != Board.Cells[x, y].Color)
                    {
                        Result.Add(newpos);
                    }
                    if (Board.Cells[newpos.Row, newpos.Column] != null)
                    {
                        break;
                    }
                }
            }
            return Result;
        }
    }
    // one field
    public abstract class Figure2: Figure
    {
        public override List<Position> GetMoves(int x, int y)
        {
            int[] dx = GetDx();
            int[] dy = GetDy();
            List<Position> Result = new List<Position>();
            Position curpos = new Position(x, y);
            for (int dir = 0; dir < dx.Count(); ++dir)
            {
                Position newpos = new Position(curpos.Row + dx[dir], curpos.Column + dy[dir]);
                if (!newpos.IsValid())
                    continue;
                if (Board.Cells[newpos.Row, newpos.Column] == null || Board.Cells[newpos.Row, newpos.Column].Color != Board.Cells[x, y].Color)
                {
                    Result.Add(newpos);
                }
            }
            return Result;
        }
    }

    class Rook : Figure1
    {
        protected override int[] GetDx()
        {
            return new int[] { -1, 0, 0, 1 };
        }
        protected override int[] GetDy()
        {
            return new int[] { 0, -1, 1, 0 };
        }
    }
    class Bishop : Figure1
    {
        protected override int[] GetDx()
        {
            return new int[] { -1, -1, 1, 1 };
        }

        protected override int[] GetDy()
        {
            return new int[] { -1, 1, -1, 1 };
        }
    }
    class Queen : Figure1
    {
        protected override int[] GetDx()
        {
            return new int[] { -1, -1, -1, 0, 0, 1, 1, 1 };
        }
        protected override int[] GetDy()
        {
            return new int[] { -1, 0, 1, -1, 1, -1, 0, 1 };
        }
    }


    class Knight : Figure2
    {
        protected override int[] GetDx()
        {
            return new int[] { -2, -2, -1, -1, 1, 1, 2, 2 };
        }
        protected override int[] GetDy()
        {
            return new int[] { -1, 1, -2, 2, -2, 2, -1, 1 };
        }
    }

    public class King : Figure2
    {
        protected override int[] GetDx()
        {
            return new int[] { -1, -1, -1, 0, 0, 1, 1, 1 };
        }
        protected override int[] GetDy()
        {
            return new int[] { -1, 0, 1, -1, 1, -1, 0, 1 };
        }
    }


    public class ChessBoard
    {
        private static Figure GetFigure(char c, int x, int y)
        {
            if (c == '.') return null;
            Figure figure;
            switch (char.ToLower(c))
            {
                case 'n':
                    figure = new Knight();
                    break;
                case 'b':
                    figure = new Bishop();
                    break;
                case 'r':
                    figure = new Rook();
                    break;
                case 'q':
                    figure = new Queen();
                    break;
                case 'k':
                    figure = new King();
                    break;
                default:
                    return null;
            }
            figure.Color = char.IsUpper(c)?FigureColor.WHITE:FigureColor.BLACK;
            return figure;
        }

        public Figure[,] Cells;
        
        public void ReadFromConsole()
        {
            Cells = new Figure[8, 8];
            for (int i = 0; i < 8; ++i)
            {
                string s = Console.ReadLine();
                for (int j = 0; j < 8; ++j)
                {
                    Cells[i, j] = GetFigure(s[j], i, j);
                    if (Cells[i, j] != null)
                    {
                        Cells[i, j].Board = this;
                    }
                }
            }
        }

		private bool IsCheck()
		{
			for (int i = 0; i < 8; ++i)
			{
				for (int j = 0; j < 8; ++j)
				{
					if (Cells[i, j] == null)
						continue;
					if (Cells[i, j].Color == FigureColor.WHITE)
					{
						continue;
					}
					List<Position> moves = Cells[i, j].GetMoves(i, j);
					foreach (Position pos in moves)
					{
						if (Cells[pos.Row, pos.Column] != null && Cells[pos.Row, pos.Column].Color == FigureColor.WHITE && Cells[pos.Row, pos.Column] is King)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

        private bool IsValid()
        {
            int KingCnt = 0;
            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    if (Cells[i, j] != null && Cells[i, j] is King)
                    {
                        ++KingCnt;
                        List<Position> moves = Cells[i, j].GetMoves(i, j);
                        foreach(Position pos in moves){
                            if (Cells[pos.Row, pos.Column] != null && Cells[pos.Row, pos.Column] is King)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return KingCnt == 2;
        }

        public string Solve()
        {
            bool CanSave = false;
            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    if (Cells[i, j] != null && Cells[i, j].Color == FigureColor.WHITE)
                    {
                        Figure CurFig = Cells[i, j];
                        List<Position> moves = Cells[i, j].GetMoves(i, j);
                        foreach (Position move in moves)
                        {
                            int x = move.Row;
                            int y = move.Column;
                            Figure CurFig2 = Cells[x, y];
                            Cells[x, y] = CurFig;
                            Cells[i, j] = null;
                                if (IsValid() && !IsCheck())
                            {
                                CanSave = true;
                            }
                            Cells[i, j] = CurFig;
                            Cells[x, y] = CurFig2;
                        }
                    }
                }
            }
            if (IsCheck())
            {
                return CanSave ? "check" : "mate";
            }
            else
            {
                return CanSave ? "ok" : "stalemate";
            }
        }
    }

    class Program
    {
        static void Main1(string[] args)
        {
            ChessBoard board = new ChessBoard();
            board.ReadFromConsole();
            Console.WriteLine(board.Solve());
//            System.Threading.Thread.Sleep(500); 
        }
    }
}