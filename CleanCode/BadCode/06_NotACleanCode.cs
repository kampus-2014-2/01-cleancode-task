using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess2
{
    
    
    // Смотрим на ChessBoard.GetStatus



    public class Point : IEquatable<Point>
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }

        public bool Equals(Point other)
        {
            return other != null && X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Point);
        }

        public override int GetHashCode()
        {
            return X + (Y << 16);
        }
    }

    public class ChessBoard
    {
        public const int Ok = 0;
        public const int Check = 1;
        public const int Mate = 2;
        public const int Stalemate = 3;
        public const int FieldSize = 8;

        public ChessBoard()
        {
            content = new IPiece[FieldSize, FieldSize];
            blackDanger = new bool[FieldSize, FieldSize];

            for (var i = 0; i < FieldSize; ++i)
                for (var j = 0; j < FieldSize; ++j)
                    blackDanger[i, j] = false;

            whites = new List<IPiece>();
            blacks = new List<IPiece>();
        }

        public static bool CoordsCorrect(int x, int y)
        {
            return (x >= 0 && x <= FieldSize - 1 && y >= 0 && y <= FieldSize - 1);
        }

        public static bool CoordsCorrect(Point p)
        {
            return (p.X >= 0 && p.X <= FieldSize - 1 && p.Y >= 0 && p.Y <= FieldSize - 1);
        }

        public King WhiteKing { get; private set; }

        public bool Add(IPiece piece)
        {
            if (piece == null)
                return false;

            int x = piece.X, y = piece.Y;

            if (content[x, y] != null)
                return false;

            content[x, y] = piece;

            if (piece.Black)
                blacks.Add(piece);
            else {
                whites.Add(piece);
                var whiteKing = piece as King;
                if (whiteKing != null)
                    WhiteKing = whiteKing;
            }
            return true;
        }

        public IPiece GetPiece(int x, int y)
        {
            if (!CoordsCorrect(x, y))
                throw new ArgumentException();

            if (content[x, y] == null)
                return null;

            return content[x, y];
        }

        public bool IsDangerousForWhite(int x, int y)
        {
            return blackDanger[x, y];
        }

        public int GetStatus()
        {
            foreach (var blackPiece in blacks)
            {
                // Fill a map of danger.
                List<List<Point>> paths = blackPiece.GetPaths();

                foreach (var path in paths) {
                    // For each possible path (geometrically, to the border)
                    // fill dangerous cells in blackDanger

                    IPiece whitePiece = null;
                    bool dangerous = true;
                    for (int i = 1; i < path.Count; ++i) {
                        if (dangerous)
                            blackDanger[path[i].X, path[i].Y] = true;

                        IPiece piece = GetPiece(path[i].X, path[i].Y);

                        if (piece != null) {
                            if (piece.Black)
                                break;

                            if (piece == WhiteKing) {
                                // If we've met the white king, add path as a threat to him
                                if (dangerous)
                                    WhiteKing.AddThreat(new Threat(path, i));

                                // If there is only one white piece protecting the white king from this black piece,
                                // add path as a threat to it
                                if (whitePiece != null) {
                                    whitePiece.AddThreat(new Threat(path, i));
                                }
                                break;
                            }

                            dangerous = false;

                            if (whitePiece == null) {
                                whitePiece = piece;
                            }
                            else {
                                break;
                            }
                        }
                    }
                }
            }

            if (IsDangerousForWhite(WhiteKing.X, WhiteKing.Y)) {
                if (WhiteKing.CanMove())
                    // If the white king is on a dangerous cell, but he can make a move, that's check.
                    return Check;

                if(WhiteKing.Threats.Count > 1)
                    // If he can't move and there's more than one threat to him, that's mate.
                    return Mate;

                // There's one threat.
                foreach (var piece in whites) {
                    // For each white piece check if it can eliminate the threat
                    if (WhiteKing.Threats[0].CellsToEliminate.Any(p => piece.CanMoveTo(p.X, p.Y))) {
                        return Check;
                    }
                }

                // If nobody can help, that's mate.
                return Mate;
            }

            // Check for stalemate.

            // If there's a white piece, that can move, that's OK.
            if (whites.Any(piece => piece.CanMove())) {
                return Ok;
            }

            // Otherwise, that's stalemate.
            return Stalemate;
        }

        private readonly IPiece[,] content;
        private readonly bool[,] blackDanger;
        private readonly List<IPiece> blacks;
        private readonly List<IPiece> whites;
    }

    public class Threat
    {
        public readonly HashSet<Point> CellsToEliminate;
        public readonly HashSet<Point> Cells; // All the cells from the attacker to the border.
        public readonly Point Attacker;

        public Threat(List<Point> path, int n)
        {
            Attacker = new Point(path[0].X, path[0].Y);
            Cells = new HashSet<Point>();
            CellsToEliminate = new HashSet<Point>();

            for(int i = 0; i < path.Count; ++i) {
                Cells.Add(path[i]);

                if (i < n)
                    CellsToEliminate.Add(path[i]);
            }
        }
    }

    public interface IPiece
    {
        bool Black { get; }
        int X { get; }
        int Y { get; }
        List<Threat> Threats { get; } 

        bool CorrectToMoveTo(int x, int y); // geometrically
        List<Point> GetPathTo(int x, int y); // geometrical path, empty if there's none
        List<List<Point>> GetPaths(); // all geometrically possible paths
        bool CanMoveTo(int x, int y); // considering other pieces and situations when it guards the king
        bool CanMove(); // CanMoveTo anywhere
        void AddThreat(Threat threat);
    }

    public abstract class Piece : IPiece
    {
        protected Piece(ChessBoard board, int x, int y, bool black)
        {
            Board = board;
            X = x;
            Y = y;
            Black = black;
            threats = new List<Threat>();
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        public bool Black { get; private set; }

        public abstract bool CorrectToMoveTo(int x, int y);
        public abstract List<Point> GetPathTo(int x, int y);
        public abstract List<List<Point>> GetPaths();
        public abstract List<Point> NearestCells();

        public bool CanMoveOnCells(List<Point> cells)
        {
            return cells.Any(p => CanMoveTo(p.X, p.Y));
        }

        public bool CanMove()
        {
            List<Point> cells = NearestCells();
            return cells.Any(p => CanMoveTo(p.X, p.Y));
        }

        public virtual bool CanMoveTo(int x, int y)
        {
            List<Point> path = GetPathTo(x, y);
            if (path.Count == 0)
                // Can't reach geometrically.
                return false;

            IPiece piece = Board.GetPiece(x, y);
            if (piece != null && piece.Black == Black)
                // Allied piece at the end.
                return false;

            for (int i = 1; i < path.Count - 1; ++i) {
                if (Board.GetPiece(path[i].X, path[i].Y) != null)
                    // Someone on the way.
                    return false;
            }

            foreach (var threat in threats)
            {
                if (!threat.CellsToEliminate.Contains(new Point(x, y)))
                    // If we're guarding the king from a threat, can't move anywhere except the cells of this threat path
                    return false;
            }

            return true;
        }

        public void AddThreat(Threat threat)
        {
            threats.Add(threat);
        }

        private readonly List<Threat> threats;
        protected ChessBoard Board;
        public List<Threat> Threats
        {
            get { return threats; }
        }
    }

    public sealed class Knight : Piece
    {
        public Knight(ChessBoard board, int x, int y, bool black) : base(board, x, y, black) { }

        public override bool CorrectToMoveTo(int x, int y)
        {
            if (!ChessBoard.CoordsCorrect(x, y))
                return false;

            int diffX = Math.Abs(x - X), diffY = Math.Abs(y - Y);

            if (diffX > 2 ||
                diffY > 2 ||
                diffX == 0 ||
                diffY == 0 ||
                diffX == diffY)
                return false;

            return true;
        }

        public override List<Point> GetPathTo(int x, int y)
        {
            var path = new List<Point>();

            if (!CorrectToMoveTo(x, y))
                return path;

            path.Add(new Point(X, Y));
            path.Add(new Point(x, y));

            return path;
        }

        public override List<List<Point>> GetPaths()
        {
            var paths = new List<List<Point>>();
            List<Point> cells = NearestCells();

            foreach (var p in cells) {
                paths.Add(new List<Point>());
                paths[paths.Count - 1].Add(new Point(X, Y));
                paths[paths.Count - 1].Add(p);
            }

            return paths;
        }

        public override List<Point> NearestCells()
        {
            int[] dx = { -1, -2, 1, 2, -1, -2, 1, 2 };
            int[] dy = { -2, -1, 2, 1, 2, 1, -2, -1 };

            var cells = new List<Point>();
            for (int i = 0; i < dx.Length; ++i) {
                if (ChessBoard.CoordsCorrect(X + dx[i], Y + dy[i])) {
                    cells.Add(new Point(X + dx[i], Y + dy[i]));
                }
            }

            return cells;
        }
    }

    public sealed class King : Piece
    {
        public King(ChessBoard board, int x, int y, bool black) : base(board, x, y, black) {}

        public override bool CorrectToMoveTo(int x, int y)
        {
            if (!ChessBoard.CoordsCorrect(x, y))
                return false;

            if (x == X && y == Y)
                return false;

            int diffX = Math.Abs(x - X), diffY = Math.Abs(y - Y);

            if (Math.Max(diffX, diffY) > 1)
                return false;

            return true;
        }

        public override List<Point> GetPathTo(int x, int y)
        {
            var path = new List<Point>();

            if (!CorrectToMoveTo(x, y))
                return path;

            path.Add(new Point(X, Y));
            path.Add(new Point(x, y));

            return path;
        }

        public override List<List<Point>> GetPaths()
        {
            var paths = new List<List<Point>>();
            List<Point> cells = NearestCells();

            foreach (var p in cells) {
                paths.Add(new List<Point>());
                paths[paths.Count - 1].Add(new Point(X, Y));
                paths[paths.Count - 1].Add(p);
            }

            return paths;
        }

        public override List<Point> NearestCells()
        {
            int[] dx = { 1, 1, 1, 0, 0, -1, -1, -1 };
            int[] dy = { 1, 0, -1, 1, -1, 1, 0, -1 };

            var cells = new List<Point>();
            for (int i = 0; i < dx.Length; ++i) {
                if (ChessBoard.CoordsCorrect(X + dx[i], Y + dy[i])) {
                    cells.Add(new Point(X + dx[i], Y + dy[i]));
                }
            }

            return cells;
        }

        public override bool CanMoveTo(int x, int y)
        {
            List<Point> path = GetPathTo(x, y);
            if (path.Count == 0)
                // Can't reach geometrically.
                return false;

            if (!Black && Board.IsDangerousForWhite(x, y))
                // Can't move on a dangerous place.
                return false;

            IPiece piece = Board.GetPiece(x, y);

            if (piece != null && piece.Black == Black)
                // Allied piece at the end.
                return false;

            foreach (var threat in Threats) {
                Point p = new Point(x, y);
                if (threat.Cells.Contains(p) && !p.Equals(threat.Attacker))
                    // Can't move on a cell of a threat path if we're not capturing the attacker.
                    return false;
            }

            for (int i = 1; i < path.Count - 1; ++i) {
                if (Board.GetPiece(path[i].X, path[i].Y) != null)
                    // Someone on the way.
                    return false;
            }



            return true;
        }
    }

    public sealed class Rook : Piece
    {
        public Rook(ChessBoard board, int x, int y, bool black) : base(board, x, y, black) { }

        public override bool CorrectToMoveTo(int x, int y)
        {
            if (!ChessBoard.CoordsCorrect(x, y))
                return false;

            if (x != X && y != Y)
                return false;

            if (x == X && y == Y)
                return false;

            return true;
        }

        public override List<Point> GetPathTo(int x, int y)
        {
            var path = new List<Point>();
            if (!CorrectToMoveTo(x, y)) return path;
            path.Add(new Point(X, Y));
            if (y < Y)
                for (int i = Y - 1; i > y; --i)
                    path.Add(new Point(x, i));
            else if (y > Y)
                for (int i = Y + 1; i < y; ++i)
                    path.Add(new Point(x, i));
            else if (x < X)
                for (int i = X - 1; i > x; --i)
                    path.Add(new Point(i, y));
            else 
                for (int i = X + 1; i < x; ++i)
                    path.Add(new Point(i, y));
            path.Add(new Point(x, y));
            return path;
        }

        public override List<List<Point>> GetPaths()
        {
            var paths = new List<List<Point>>();
            const int max = ChessBoard.FieldSize - 1;

            if (X != 0)
                paths.Add(GetPathTo(0, Y));
            if (X != max)
                paths.Add(GetPathTo(max, Y));
            if (Y != 0)
                paths.Add(GetPathTo(X, 0));
            if (Y != max)
                paths.Add(GetPathTo(X, max));

            return paths;
        }

        public override List<Point> NearestCells()
        {
            var cells = new List<Point>();
            const int max = ChessBoard.FieldSize - 1;

            if (X != 0)
                cells.Add(new Point(X - 1, Y));
            if (X != max)
                cells.Add(new Point(X + 1, Y));
            if (Y != 0)
                cells.Add(new Point(X, Y - 1));
            if (Y != max)
                cells.Add(new Point(X, Y + 1));

            return cells;
        }
    }

    public sealed class Bishop : Piece
    {
        public Bishop(ChessBoard board, int x, int y, bool black) : base(board, x, y, black) { }

        public override bool CorrectToMoveTo(int x, int y)
        {
            if (!ChessBoard.CoordsCorrect(x, y))
                return false;

            int diffX = Math.Abs(x - X), diffY = Math.Abs(y - Y);

            if (diffX != diffY)
                return false;

            if (diffX == 0)
                return false;

            return true;
        }

        public override List<Point> GetPathTo(int x, int y)
        {
            var path = new List<Point>();

            if (!CorrectToMoveTo(x, y))
                return path;

            path.Add(new Point(X, Y));
            int diff = Math.Abs(x - X);

            if (x > X) {
                if (y > Y) {
                    // Down-right
                    for (int i = 1; i < diff; ++i) {
                        path.Add(new Point(X + i, Y + i));
                    }
                }
                else {
                    // Up-right
                    for (int i = 1; i < diff; ++i) {
                        path.Add(new Point(X + i, Y - i));
                    }
                }
            }
            else {
                if (y > Y) {
                    // Down-left
                    for (int i = 1; i < diff; ++i) {
                        path.Add(new Point(X - i, Y + i));
                    }
                }
                else {
                    // Up-left
                    for (int i = 1; i < diff; ++i) {
                        path.Add(new Point(X - i, Y - i));
                    }
                }
            }

            path.Add(new Point(x, y));

            return path;
        }



        public override List<List<Point>> GetPaths()
        {
            var paths = new List<List<Point>>();
            const int max = ChessBoard.FieldSize - 1;

            if (X != 0 && Y != 0)
                paths.Add(ChessBoard.CoordsCorrect(0, Y - X) ? GetPathTo(0, Y - X) : GetPathTo(X - Y, 0));
            if (X != 0 && Y != max)
                paths.Add(ChessBoard.CoordsCorrect(X - max + Y, max) ? GetPathTo(X - max + Y, max) : GetPathTo(0, Y + X));
            if (X != max && Y != 0)
                paths.Add(ChessBoard.CoordsCorrect(max, Y - max + X) ? GetPathTo(max, Y - max + X) : GetPathTo(X + Y, 0));
            if (X != max && Y != max)
                paths.Add(ChessBoard.CoordsCorrect(max, Y + max - X) ? GetPathTo(max, Y + max - X) : GetPathTo(X + max - Y, max));

            return paths;
        }

        public override List<Point> NearestCells()
        {
            var cells = new List<Point>();

            const int max = ChessBoard.FieldSize - 1;

            if (X != max && Y != max)
                cells.Add(new Point(X + 1, Y + 1));
            if (X != max && Y != 0)
                cells.Add(new Point(X + 1, Y - 1));
            if (X != 0 && Y != max)
                cells.Add(new Point(X - 1, Y + 1));
            if (X != 0 && Y != 0)
                cells.Add(new Point(X - 1, Y - 1));

            return cells;
        }
    }

    public sealed class Queen : Piece
    {
        public Queen(ChessBoard board, int x, int y, bool black) : base(board, x, y, black) { }

        public override bool CorrectToMoveTo(int x, int y)
        {
            if (!ChessBoard.CoordsCorrect(x, y))
                return false;

            if (x == X && y == Y)
                return false;

            int diffX = Math.Abs(x - X), diffY = Math.Abs(y - Y);

            if (diffX != diffY && (diffX > 0 && diffY > 0))
                return false;

            return true;
        }

        public override List<Point> GetPathTo(int x, int y)
        {
            var path = new List<Point>();

            if (!CorrectToMoveTo(x, y))
                return path;

            path.Add(new Point(X, Y));

            int diffX = Math.Abs(x - X), diffY = Math.Abs(y - Y);

            if (diffX == 0) {
                if (y > Y) {
                    // Up
                    for (int i = 1; i < diffY; ++i) {
                        path.Add(new Point(X, Y + i));
                    }
                }
                else {
                    // Down
                    for (int i = 1; i < diffY; ++i) {
                        path.Add(new Point(X, Y - i));
                    }
                }
                path.Add(new Point(x, y));
                return path;
            }

            if (diffY == 0) {
                if (x > X) {
                    // Right
                    for (int i = 1; i < diffX; ++i) {
                        path.Add(new Point(X + i, Y));
                    }
                }
                else {
                    // Left
                    for (int i = 1; i < diffX; ++i) {
                        path.Add(new Point(X - i, Y));
                    }
                }
                path.Add(new Point(x, y));
                return path;
            }

            if (x > X) {
                if (y > Y) {
                    // Down-right
                    for (int i = 1; i < diffY; ++i) {
                        path.Add(new Point(X + i, Y + i));
                    }
                }
                else {
                    // Up-right
                    for (int i = 1; i < diffY; ++i) {
                        path.Add(new Point(X + i, Y - i));
                    }
                }
            }
            else {
                if (y > Y) {
                    // Down-left
                    for (int i = 1; i < diffY; ++i) {
                        path.Add(new Point(X - i, Y + i));
                    }
                }
                else {
                    // Up-left
                    for (int i = 1; i < diffY; ++i) {
                        path.Add(new Point(X - i, Y - i));
                    }
                }
            }

            path.Add(new Point(x, y));
            return path;
        }

        public override List<List<Point>> GetPaths()
        {
            var paths = new List<List<Point>>();
            const int max = ChessBoard.FieldSize - 1;

            if (X != 0)
                paths.Add(GetPathTo(0, Y));
            if (X != max)
                paths.Add(GetPathTo(max, Y));
            if (Y != 0)
                paths.Add(GetPathTo(X, 0));
            if (Y != max)
                paths.Add(GetPathTo(X, max));

            if (X != 0 && Y != 0)
                paths.Add(ChessBoard.CoordsCorrect(0, Y - X) ? GetPathTo(0, Y - X) : GetPathTo(X - Y, 0));
            if (X != 0 && Y != max)
                paths.Add(ChessBoard.CoordsCorrect(X - max + Y, max) ? GetPathTo(X - max + Y, max) : GetPathTo(0, Y + X));
            if (X != max && Y != 0)
                paths.Add(ChessBoard.CoordsCorrect(max, Y - max + X) ? GetPathTo(max, Y - max + X) : GetPathTo(X + Y, 0));
            if (X != max && Y != max)
                paths.Add(ChessBoard.CoordsCorrect(max, Y + max - X) ? GetPathTo(max, Y + max - X) : GetPathTo(X + max - Y, max));

            return paths;
        }

        public override List<Point> NearestCells()
        {
            int[] dx = { 1, 1, 1, 0, 0, -1, -1, -1 };
            int[] dy = { 1, 0, -1, 1, -1, 1, 0, -1 };

            var cells = new List<Point>();
            for (int i = 0; i < dx.Length; ++i) {
                if (ChessBoard.CoordsCorrect(X + dx[i], Y + dy[i])) {
                    cells.Add(new Point(X + dx[i], Y + dy[i]));
                }
            }

            return cells;
        }
    }

    public class PieceFactory
    {
        private readonly IDictionary<char, Func<int, int, bool, IPiece>> factoryMap;

        public PieceFactory(ChessBoard board)
        {
            factoryMap = new Dictionary<char, Func<int, int, bool, IPiece>> {
                                                                                { 'N', (x, y, black) => new Knight(board, x, y, black) },
                                                                                { 'B', (x, y, black) => new Bishop(board, x, y, black) },
                                                                                { 'R', (x, y, black) => new Rook(board, x, y, black) },
                                                                                { 'K', (x, y, black) => new King(board, x, y, black) },
                                                                                { 'Q', (x, y, black) => new Queen(board, x, y, black) },
                                                                                { '.', (x, y, black) => null }
                                                                            };
        }

        public IPiece NewPiece(int x, int y, char symbol)
        {
            char key = GetKey(symbol);
            bool black = IsBlack(symbol);
            return factoryMap[key](x, y, black);
        }

        private static char GetKey(char symbol)
        {
            return Char.ToUpper(symbol);
        }

        private static bool IsBlack(char symbol)
        {
            return Char.IsLower(symbol);
        }
    }

    public sealed class EntryPoint
    {
        static void Main()
        {
            var board = new ChessBoard();
            var factory = new PieceFactory(board);

            for (int i = 0; i < ChessBoard.FieldSize; ++i)
            {
                string line = Console.ReadLine();
                for (int j = 0; j < ChessBoard.FieldSize; ++j)
                {
                    if (line == null)
                        throw new NullReferenceException();

                    board.Add(factory.NewPiece(j, i, line[j]));
                }
            }
            int status = board.GetStatus();

            switch (status)
            {
                case ChessBoard.Ok:
                    Console.WriteLine("ok");
                    break;
                case ChessBoard.Check:
                    Console.WriteLine("check");
                    break;
                case ChessBoard.Mate:
                    Console.WriteLine("mate");
                    break;
                case ChessBoard.Stalemate:
                    Console.WriteLine("stalemate");
                    break;
                default:
                    throw new Exception("unknown state");
            }

        }
    }
}

