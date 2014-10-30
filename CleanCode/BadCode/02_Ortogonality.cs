using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CleanCode.Tasks
{
	static class ExtractMethods
	{
		//TODO отрефакторить Main
        private static void _Main(string[] args)
		{
			// Simple arguments parsing for testing reasons
			var arguments = new ArrayList();
			foreach (var argument in args)
			{
				arguments.Add(argument);
			}
			var flagTest = arguments.Contains("-test");
			var flagTestAll = arguments.Contains("-all");
			var flagLog = arguments.Contains("-log");

			if (flagTestAll)
			{
				var files = from file in Directory.EnumerateFiles(@".\tests\") select file;
				var countPass = 0;
				var countFail = 0;
				foreach (var file in files)
				{
					Console.Write("{0, -30}", file);
					var testcase = new TestCase(File.OpenText(file));
                    Console.WriteLine(testcase.RealResult);
					if (testcase.Test(testResult: true))
					{
						Console.WriteLine("Pass");
						countPass++;
					}
					else
					{
						Console.WriteLine("Fail");
						countFail++;
						if (flagLog)
						{
							testcase.DebugInfo();
							Console.WriteLine("\tExpected result: \t{0}", testcase.ExpectedResult);
						}
					}
				}

				Console.WriteLine("Pass: \t\t{0}", countPass);
				Console.WriteLine("Fail: \t\t{0}", countFail);
				Console.WriteLine("Overall: \t{0}", countFail + countPass);
			}

			if (flagTest)
			{
				var testcase = new TestCase(Console.In);
				var status = testcase.Test(testResult: true);
                Console.WriteLine(testcase.RealResult);
                if (!status)
                {
                    testcase.DebugInfo();
                    Console.WriteLine("Expected result: \t{0}", testcase.ExpectedResult);
                }
				Console.WriteLine(status ? "Pass" : "Fail");
			}

			// Стандратное чтение из консоли, в соответствии с заданием
			if (!(flagTest || flagTestAll))
			{
				var testcase = new TestCase(Console.In);
				testcase.Test();
				if (flagLog) testcase.DebugInfo();
				Console.WriteLine(testcase.RealResult);
			}
		}

		private class TestCase
		{
            /// <summary>Can be null</summary>
            public string File { get; private set; }
            public string RealResult { get; private set; }
			public string ExpectedResult { get; private set; }
			public bool Status { get; private set; }
			private TextReader sr;

			private PositionAnalyser positionAnalyser;

            public TestCase(string file)
                : this(System.IO.File.OpenText(file))
            {
                File = file;
            }

			public TestCase(TextReader stream)
			{
				sr = stream;
				positionAnalyser = new PositionAnalyser();
			}

			public bool Test(bool testResult = false)
			{
				var buffer = "";
				while (buffer.Length < (BitBoard.FieldSize * BitBoard.FieldSize))
				{
					buffer += sr.ReadLine();
				}

				var result = positionAnalyser.Parse(buffer);

				switch (result)
				{
					case GameState.Check: RealResult = "check"; break;
					case GameState.Mate: RealResult = "mate"; break;
					case GameState.Stalemate: RealResult = "stalemate"; break;
					case GameState.Ok: RealResult = "ok"; break;
					default:
						RealResult = "unknown"; break;
				}

				if (testResult)
				{
					ExpectedResult = sr.ReadLine();
					Status = (ExpectedResult == RealResult);
				}
				else
				{
					Status = false;
				}
				return Status;
			}

			public void DebugInfo()
			{
				positionAnalyser.DebugInfo();
			}

		}
	};

    public enum GameState { Check, Mate, Stalemate, Ok };
    public enum Color { None, Black, White }
    public enum Piece { None, Knight, Bishop, Rook, Queen, King }

	public struct Figure
	{
        public static readonly Figure None = new Figure('.');

        public Color color { get; private set; }
	    public Piece piece { get; private set; }

        public Figure(char letter): this()
        {
            if (letter == '.')
            {
                color = Color.None;
                piece = Piece.None;
                return;
            }

            switch (char.ToUpper(letter))
            {
                case ('K'): piece = Piece.King;     break;
                case ('Q'): piece = Piece.Queen;    break;
                case ('R'): piece = Piece.Rook;     break;
                case ('B'): piece = Piece.Bishop;   break;
                case ('N'): piece = Piece.Knight;   break;
                default: throw new Exception("Figure(char): Incorrect symbol");
            }
            color = char.IsUpper(letter) ? Color.White : Color.Black;
        }
	};

    public class BitBoard: BitMatrix
    {
        public const int FieldSize = 8;
        public BitBoard() : base(FieldSize, FieldSize) { }
    }

    class MovesGenerator
    {
        private readonly BitBoard friends;
        private readonly BitBoard enemies;

        public MovesGenerator(ref BitBoard rFriends, ref BitBoard rEnemies)
        {
            friends = rFriends;
            enemies = rEnemies;
        }

        public BitBoard Get(Piece piece, Cell position)
        {
            switch(piece)
            {
                case Piece.Knight:	    return GetKnightMoves(position.M, position.N);
                case Piece.Bishop:      return GetBishopMoves(position.M, position.N);
                case Piece.Rook:        return GetRookMoves(position.M, position.N);
                case Piece.Queen:       return GetQueenMoves(position.M, position.N);
                case Piece.King:        return GetKingMoves(position.M, position.N);
                default: throw new Exception("Unknown chess piece");
            }
        }

        private BitBoard GetKnightMoves (int m, int n) 
        {
            var moves = new BitBoard();
            
            var list = new List<int> { 1, -1, 2, -2 };
            var combinations = from i in list from j in list 
                               where Math.Abs(i) != Math.Abs(j) 
                               select new { i, j };
            foreach (var comb in combinations)
                MoveInto(m + comb.i, n + comb.j, moves);

            return moves;
        }

        private BitBoard GetBishopMoves(int m, int n)
        {
            var moves = new BitBoard();
            for (int i = m + 1, j = n + 1; MoveInto(i, j, moves); i++, j++) {} // south-east
            for (int i = m + 1, j = n - 1; MoveInto(i, j, moves); i++, j--) {} // north-east
            for (int i = m - 1, j = n + 1; MoveInto(i, j, moves); i--, j++) {} // south-west
            for (int i = m - 1, j = n - 1; MoveInto(i, j, moves); i--, j--) {} // north-west
            return moves;
        }

        private BitBoard GetRookMoves(int m, int n)
        {
            var moves = new BitBoard();
            for (var i = m + 1; MoveInto(i, n, moves); i++) {} // east
            for (var i = m - 1; MoveInto(i, n, moves); i--) {} // west
            for (var i = n + 1; MoveInto(m, i, moves); i++) {} // south
            for (var i = n - 1; MoveInto(m, i, moves); i--) {} // north
            return moves;
        }

        private BitBoard GetQueenMoves(int m, int n)
        {
            var moves = new BitBoard();
            moves.Or(GetBishopMoves(m, n));
            moves.Or(GetRookMoves(m, n));
            return moves;
        }

        private BitBoard GetKingMoves(int m, int n)
        {
            var moves = new BitBoard();

            var list = new List<int> { 1, 0, -1 };
            var combinations = from i in list from j in list
                               where !(i == 0 && j == 0)
                               select new { i, j };
            foreach (var comb in combinations)
                MoveInto(m + comb.i, n + comb.j, moves);

            return moves;
        }

        private enum Obstacle { None, Friend, Enemy }

        private Obstacle ProbeCell(int m, int n)
        {
            if (friends.Get(m, n)) return Obstacle.Friend;
            if (enemies.Get(m, n)) return Obstacle.Enemy;
            return Obstacle.None;
        }

        private bool MoveInto(int m, int n, BitBoard output)
            // false - дальнейшее движение фигуры в данном направлении невозможно
        {
            if (BitMatrix.CheckBoundaries(m, n) == false)
                return false;

            switch (ProbeCell(m, n))
            {
                case Obstacle.Friend:
                    return false;

                case Obstacle.Enemy:
                    output.Set(m, n);
                    return false;

                case Obstacle.None:
                    output.Set(m, n);
                    return true;

                default: throw new Exception("Unknown Obstacle entery");
            }
        }
    };

    public class SimpleAnalyser
    {
        protected Figure[,] ChessBoard;
        protected Dictionary<Cell, BitBoard> BCellToMovesMap;
        protected Dictionary<Cell, BitBoard> WCellToMovesMap;
        protected BitBoard BMovesSum, WMovesSum;
        protected BitBoard BPositions, WPositions;
        private MovesGenerator BMovesGen; // friends = BlackPos, enemies = WhitePos
        private MovesGenerator WMovesGen; // friends = WhitePos, enemies = BlackPos

        protected Cell KingPosition;
        public bool isKingAttacked { get; private set; }

        public SimpleAnalyser()
        {
            ChessBoard = new Figure[BitBoard.FieldSize,BitBoard.FieldSize];
            BCellToMovesMap = new Dictionary<Cell, BitBoard>();
            WCellToMovesMap = new Dictionary<Cell, BitBoard>();
            BMovesSum = new BitBoard();
            WMovesSum = new BitBoard();
            BPositions = new BitBoard();
            WPositions = new BitBoard();

            BMovesGen = new MovesGenerator(ref BPositions, ref WPositions);
            WMovesGen = new MovesGenerator(ref WPositions, ref BPositions);
        }

        public SimpleAnalyser(Figure[,] chessBoard) : this()
        {
            Array.ConstrainedCopy(chessBoard, 0, ChessBoard, 0, BitBoard.FieldSize * BitBoard.FieldSize);
        }


        public void SimpleAnalyse()
        {
            ReadFiguresPositions();
            CalcFiguresMovesMaps();
            isKingAttacked = BMovesSum.Get(KingPosition.M, KingPosition.N);
        }

        public void MoveFigure(Cell from, Cell to)
        {
            var figure = ChessBoard[from.M, from.N];
            ChessBoard[from.M, from.N] = Figure.None;
            ChessBoard[to.M, to.N] = figure;
        }

        private void ReadFiguresPositions()
        {
            for (var i = 0; i < BitBoard.FieldSize; i++)
                for (var j = 0; j < BitBoard.FieldSize; j++)
                {
                    var figure = ChessBoard[i, j];
                    if (figure.Equals(Figure.None)) continue;

                    if (figure.color == Color.White)
                    {
                        WPositions.Set(i, j);
                        if (figure.piece == Piece.King)
                            KingPosition = new Cell(i, j); // Белый король
                    }
                    else
                        BPositions.Set(i, j);
                }
        }

        private void CalcFiguresMovesMaps()
        {
            for (var i = 0; i < BitBoard.FieldSize; i++)
            {
                for (var j = 0; j < BitBoard.FieldSize; j++)
                {
                    var figure = ChessBoard[i, j];
                    if (figure.Equals(Figure.None)) continue;
                    if (figure.color == Color.White)
                    {
                        BitBoard moves = WMovesGen.Get(figure.piece, new Cell(i, j));
                        WCellToMovesMap.Add(new Cell(i, j), moves);
                    }
                    else
                    {
                        BitBoard moves = BMovesGen.Get(figure.piece, new Cell(i, j));
                        BCellToMovesMap.Add(new Cell(i, j), moves);
                    }
                }
            }

            foreach (var movesMap in WCellToMovesMap)
            {
                WMovesSum.Or(movesMap.Value);
            }

            foreach (var movesMap in BCellToMovesMap)
            {
                BMovesSum.Or(movesMap.Value);
            }

        }
    }

    public class PositionAnalyser : SimpleAnalyser
	{
		public bool isNextMove { get; set; }

		public GameState Parse(string input)
		{
			var index = 0;
			for (var i = 0; i < BitBoard.FieldSize; i++)
			{
				for (var j = 0; j < BitBoard.FieldSize; j++)
				{
					ChessBoard[i, j] = new Figure(input[index]);
					index++;
				}
			}

			return Analyse();
		}

		private GameState Analyse()
		{
			base.SimpleAnalyse();
			isNextMove = FindNextMove();

			if (isKingAttacked)
				return isNextMove ? GameState.Check : GameState.Mate;
			else
				return isNextMove ? GameState.Ok : GameState.Stalemate;
		}

		// Проверка возможности контратаки или защиты
		private bool FindNextMove()
		{
			//	Ищем пересечения каждой фигуры с общим полем BMovesSum + BPositions
			//	Пробуем переместить фигуру поочередно в каждую точку пересечения
			//	Вызываем функцию анализа для временного поля
			
			//	Исключение в логике, добавляем ходы отступления короля как есть
			var posibleCellToMovesMap = new Dictionary<Cell, BitBoard> { { KingPosition, WCellToMovesMap[KingPosition] } };
		    WCellToMovesMap.Remove(KingPosition);

            var BPosAndMoves = new BitBoard();
			BPosAndMoves.Or(BPositions);
			BPosAndMoves.Or(BMovesSum);

			foreach (var cellMovesMap in WCellToMovesMap)
			{
			    var moves = new BitBoard();
			    moves.Or(BPosAndMoves);
			    moves.And(cellMovesMap.Value);
			    posibleCellToMovesMap.Add(cellMovesMap.Key, moves);
			}

			foreach (var cellMovesMap in posibleCellToMovesMap)
			{
				for (var i = 0; i < BitBoard.FieldSize; i++)
				{
					for (var j = 0; j < BitBoard.FieldSize; j++)
					{
					    if (cellMovesMap.Value.Get(i, j) == false)
					        continue;

					    var nextMove = new SimpleAnalyser(ChessBoard);
					    nextMove.MoveFigure(cellMovesMap.Key, new Cell(i, j));
					    nextMove.SimpleAnalyse();
					    if (!nextMove.isKingAttacked) return true; // Этим ходом король защищен
					}
				}
			}

			return false; // Никакой защиты не найдено
		}

	    public void DebugInfo()
		{
			Console.WriteLine("Game field: \tWhite: \t\tBlack: ");
			for(var i = 0; i < BitBoard.FieldSize; i++)
			{
				for (var j = 0; j < BitBoard.FieldSize; j++)
					Console.Write(ChessBoard[i, j]);

				Console.Write("\t");

				for (var j = 0; j < BitBoard.FieldSize; j++)
					Console.Write(CellInfo(new Cell(i, j), ref WPositions, ref WMovesSum));

				Console.Write("\t");

				for (var j = 0; j < BitBoard.FieldSize; j++)
					Console.Write(CellInfo(new Cell(i, j), ref BPositions, ref BMovesSum));

				Console.WriteLine();
			}
			Console.WriteLine("Positions(O), Moves(x)");
	    	Console.WriteLine("King attacked?: \t{0}", isKingAttacked);
	    	Console.WriteLine("Is there defend?: \t{0}", isNextMove);
		}

		private static char CellInfo(Cell x, ref BitBoard positions, ref BitBoard moves)
		{
		    if (positions.Get(x.M, x.N) && moves.Get(x.M, x.N))
		        return '?';
		    else if (positions.Get(x.M, x.N))
		        return 'O';
		    else if (moves.Get(x.M, x.N))
		        return 'x';
		    else
		        return '.';
		}
	};

public struct Cell
{
    public int M { get; private set; }
    public int N { get; private set; }

    public Cell(int m, int n)
        : this()
    {
        M = m;
        N = n;
    }
};

public class BitMatrix
{
    private BitArray data;
    public static int M { get; private set; }
    public static int N { get; private set; }

    public BitMatrix(int m, int n)
    {
        data = new BitArray(m * n);
        M = m;
        N = n;
    }

    public bool Get(int m, int n)
    {
        return data.Get(m * N + n);
    }

    public void Set(int m, int n, bool value = true)
    {
        data.Set(m * N + n, value);
    }

    public void Not()
    {
        data.Not();
    }

    public void And(BitMatrix other)
    {
        data.And(other.data);
    }

    public void Or(BitMatrix other)
    {
        data.Or(other.data);
    }

    public void Xor(BitMatrix other)
    {
        data.Xor(other.data);
    }

    public void Print()
    {
        for (var i = 0; i < M; i++)
        {
            for (var j = 0; j < N; j++)
            {
                Console.Write(Get(i, j) ? 1 : 0);
            }
            Console.WriteLine();
        }
    }

    public static bool CheckBoundaries(int m, int n)
    {
        return (m < M) && (n < N) && (m >= 0) && (n >= 0);
    }
}

}

