using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Testing
{
    internal class Program
    {
        private static void Main()
        {
            //RunTest();
            //return;
            var board = new BoardOfChess();


            board.ReadFromConsole();

            bool isShah = false, isMat = false, isPat = false;

            isMat = board.CheckMat(ColorFigure.White);
            isPat = board.CheckPat(ColorFigure.White);
            isShah = board.CheckShah(ColorFigure.White);


            if (isMat)
                Console.Write("mate");
            else if (isPat)
                Console.Write("stalemate");
            else
                Console.Write(isShah ? "check" : "ok");
        }


        private static void RunTest()
        {
            var board = new BoardOfChess();
            var tests = new List<List<Figure>>();
            var correct = new List<bool[]>();
            var outdate = new List<bool[]>();
            var dontCorrectTest = new List<int>();
            //result :Mat,Pat,Shah

            //Test0
            tests.Add(new List<Figure>
                          {
                              new King(new Cell(0, 4), ColorFigure.White, board),
                              new Rook(new Cell(0, 0), ColorFigure.Black, board),
                              new Rook(new Cell(1, 1), ColorFigure.Black, board),
                              new Rook(new Cell(7, 0), ColorFigure.White, board),
                          });
            //Result
            correct.Add(new[] {false, false, true});

            //Test1
            tests.Add(new List<Figure>
                          {
                              new King(new Cell(0, 4), ColorFigure.White, board),
                              new Rook(new Cell(0, 0), ColorFigure.Black, board),
                              new Rook(new Cell(1, 1), ColorFigure.Black, board),
                              new Rook(new Cell(7, 3), ColorFigure.White, board),
                          });
            //Result
            correct.Add(new[] {false, false, true});

            //Test2
            tests.Add(new List<Figure>
                          {
                              new King(new Cell(0, 4), ColorFigure.White, board),
                              new Rook(new Cell(0, 0), ColorFigure.Black, board),
                          });
            //Result
            correct.Add(new[] {false, false, true});

            //Test3
            tests.Add(new List<Figure>
                          {
                              new Rook(new Cell(0, 0), ColorFigure.White, board),
                              new Nag(new Cell(1, 0), ColorFigure.White, board),
                              new Bishop(new Cell(2, 0), ColorFigure.White, board),
                              new King(new Cell(3, 0), ColorFigure.White, board),
                              new Quine(new Cell(4, 0), ColorFigure.White, board),
                              new Bishop(new Cell(5, 0), ColorFigure.White, board),
                              new Nag(new Cell(6, 0), ColorFigure.White, board),
                              new Rook(new Cell(7, 0), ColorFigure.White, board),
                              new Rook(new Cell(0, 7), ColorFigure.Black, board),
                              new Nag(new Cell(1, 7), ColorFigure.Black, board),
                              new Bishop(new Cell(2, 7), ColorFigure.Black, board),
                              new King(new Cell(3, 7), ColorFigure.Black, board),
                              new Quine(new Cell(4, 7), ColorFigure.Black, board),
                              new Bishop(new Cell(5, 7), ColorFigure.Black, board),
                              new Nag(new Cell(6, 7), ColorFigure.Black, board),
                              new Rook(new Cell(7, 7), ColorFigure.Black, board),
                          });
            //Result
            correct.Add(new[] {false, false, false});

            //Test4
            tests.Add(new List<Figure>
                          {
                              new King(new Cell(0, 0), ColorFigure.White, board),
                              new Rook(new Cell(7, 0), ColorFigure.Black, board),
                              new Rook(new Cell(6, 1), ColorFigure.Black, board),
                              new King(new Cell(7, 7), ColorFigure.Black, board),
                          });
            //Result
            correct.Add(new[] {true, false, true});

            //Test5
            tests.Add(new List<Figure>
                          {
                              new King(new Cell(0, 0), ColorFigure.White, board),
                              new Rook(new Cell(7, 1), ColorFigure.Black, board),
                              new Rook(new Cell(1, 7), ColorFigure.Black, board),
                              //new King(new Cell(7, 7), ColorFigure.Black, board),
                          });
            //Result
            correct.Add(new[] {false, true, false});

            //Test6
            tests.Add(new List<Figure>
                          {
                              new King(new Cell(0, 0), ColorFigure.White, board),
                              new Quine(new Cell(1, 1), ColorFigure.White, board),
                              new Rook(new Cell(7, 1), ColorFigure.Black, board),
                              new Rook(new Cell(1, 7), ColorFigure.Black, board),
                              //new King(new Cell(7, 7), ColorFigure.Black, board),
                          });
            //Result
            correct.Add(new[] {false, false, false});

            //Test7
            tests.Add(new List<Figure>
                          {
                              new King(new Cell(0, 0), ColorFigure.White, board),
                              new Quine(new Cell(1, 1), ColorFigure.Black, board),
                              new Rook(new Cell(7, 1), ColorFigure.Black, board),
                              new Rook(new Cell(1, 7), ColorFigure.Black, board),
                              //new King(new Cell(7, 7), ColorFigure.Black, board),
                          });
            //Result
            correct.Add(new[] {true, false, true});

            //Test8
            tests.Add(new List<Figure>
                          {
                              new King(new Cell(0, 0), ColorFigure.White, board),
                              new Bishop(new Cell(2, 2), ColorFigure.White, board),
                              new Quine(new Cell(1, 1), ColorFigure.Black, board),
                              new Rook(new Cell(7, 1), ColorFigure.Black, board),
                              new Rook(new Cell(1, 7), ColorFigure.Black, board),
                              //new King(new Cell(7, 7), ColorFigure.Black, board),
                          });
            //Result
            correct.Add(new[] {false, false, true});

            //Test9
            tests.Add(new List<Figure>
                          {
                              new King(new Cell(1, 0), ColorFigure.White, board),
                              new Bishop(new Cell(1, 1), ColorFigure.White, board),
                              new Quine(new Cell(0, 7), ColorFigure.Black, board),
                              new Rook(new Cell(1, 7), ColorFigure.Black, board),
                              new Rook(new Cell(2, 7), ColorFigure.Black, board),
                              //new King(new Cell(7, 7), ColorFigure.Black, board),
                          });
            //Result
            correct.Add(new[] {false, true, false});

            //Test10 Мат двумя слонами
            tests.Add(new List<Figure>
                          {
                              new King(new Cell(0, 0), ColorFigure.White, board),
                              new King(new Cell(1, 2), ColorFigure.Black, board),
                              new Bishop(new Cell(2, 2), ColorFigure.Black, board),
                              new Bishop(new Cell(3, 2), ColorFigure.Black, board),
                          });
            //Result
            correct.Add(new[] {true, false, true});

            //Test11 Мат двумя слонами
            tests.Add(new List<Figure>
                          {
                              new King(new Cell(0, 1), ColorFigure.White, board),
                              new King(new Cell(2, 1), ColorFigure.Black, board),
                              new Bishop(new Cell(1, 1), ColorFigure.Black, board),
                              new Bishop(new Cell(2, 3), ColorFigure.Black, board),
                          });
            //Result
            correct.Add(new[] {true, false, true});

            //Test12 Пат двумя слонами и королем
            tests.Add(new List<Figure>
                          {
                              new King(new Cell(0, 0), ColorFigure.White, board),
                              new King(new Cell(1, 2), ColorFigure.Black, board),
                              new Bishop(new Cell(3, 1), ColorFigure.Black, board),
                              new Bishop(new Cell(3, 2), ColorFigure.Black, board),
                          });
            //Result
            correct.Add(new[] {false, true, false});

            //Test13 Пат двумя слонами
            tests.Add(new List<Figure>
                          {
                              new King(new Cell(0, 1), ColorFigure.White, board),
                              new King(new Cell(2, 1), ColorFigure.Black, board),
                              new Bishop(new Cell(1, 1), ColorFigure.Black, board),
                              new Bishop(new Cell(3, 2), ColorFigure.Black, board),
                          });
            //Result
            correct.Add(new[] {false, true, false});

            //Test14 Пат двумя слонами
            tests.Add(new List<Figure>
                          {
                              new King(new Cell(2, 0), ColorFigure.White, board),
                              new King(new Cell(0, 1), ColorFigure.Black, board),
                              new Bishop(new Cell(2, 2), ColorFigure.Black, board),
                              new Bishop(new Cell(1, 2), ColorFigure.Black, board),
                          });
            //Result
            correct.Add(new[] {false, true, false});

            //Test14 Пат двумя слонами
            tests.Add(new List<Figure>
                          {
                              new King(new Cell(1, 0), ColorFigure.White, board),
                              new King(new Cell(0, 2), ColorFigure.Black, board),
                              new Bishop(new Cell(1, 1), ColorFigure.Black, board),
                              new Bishop(new Cell(1, 2), ColorFigure.Black, board),
                          });
            //Result
            correct.Add(new[] {false, true, false});
            //Test15 шах конем
            tests.Add(new List<Figure>
                          {
                              new King(new Cell(4, 5), ColorFigure.White, board),
                              new Nag(new Cell(3, 3), ColorFigure.Black, board)
                          });
            //Result
            correct.Add(new[] {false, false, true});

            //Test16 шах конем
            tests.Add(new List<Figure>
                          {
                              new King(new Cell(4, 5), ColorFigure.White, board),
                              new Nag(new Cell(5, 7), ColorFigure.Black, board)
                          });
            //Result
            correct.Add(new[] {false, false, true});
            //Test16 мат конем
            tests.Add(new List<Figure>
                          {
                              new King(new Cell(0, 0), ColorFigure.White, board),
                              new Rook(new Cell(1, 0), ColorFigure.White, board),
                              new Bishop(new Cell(0, 1), ColorFigure.White, board),
                              new Bishop(new Cell(1, 1), ColorFigure.White, board),
                              new Nag(new Cell(2, 1), ColorFigure.Black, board)
                          });
            //Result
            correct.Add(new[] {true, false, true});

            //Тесторование
            for (int i = 0; i < tests.Count; i++)
            {
                Console.Out.WriteLine(String.Format("Test:{0}", i));
                board.AddFigures(tests[i]);
                Console.Out.WriteLine(board.ToString());


                bool isMat = board.CheckMat(ColorFigure.White);
                bool isPat = board.CheckPat(ColorFigure.White);
                bool isShah = board.CheckShah(ColorFigure.White);

                outdate.Add(new[] {isMat, isPat, isShah});

                bool result = (isMat == correct[i][0] && isPat == correct[i][1] && isShah == correct[i][2]);
                if (result == false) dontCorrectTest.Add(i);
                board.RemoveAllFigures();
            }
            //Итог
            if (dontCorrectTest.Count == 0)
            {
                Console.WriteLine("OKEY!!!");
            }
            else


                dontCorrectTest.ForEach(
                    er => Console.WriteLine(
                        "ErrorTest:{0}.Mat:{1}({2}).Pat{3}({4}).Shah{5}({6})",
                        er,
                        outdate[er][0],
                        correct[er][0],
                        outdate[er][1],
                        correct[er][1],
                        outdate[er][2],
                        correct[er][2]));
            Console.ReadLine();
        }
    }
}

namespace Testing
{
    public class Cell
    {
        /// <summary>
        /// Задать координаты ячейки
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }

        public static bool operator ==(Cell curCell, Cell otherCell)
        {
            if ((object) curCell == null && (object) otherCell == null)
                return true;
            if ((object) curCell == null || (object) otherCell == null)
                return false;
            return curCell.X == otherCell.X && curCell.Y == otherCell.Y;
        }

        public static bool operator !=(Cell curCell, Cell otherCell)
        {
            if ((object) curCell == null || (object) otherCell == null)
                return (object) curCell != null || (object) otherCell != null;
            return curCell.X != otherCell.X || curCell.Y != otherCell.Y;
        }

        public static Cell operator *(Cell curCell, int alfa)
        {
            return new Cell(curCell.X*alfa, curCell.Y*alfa);
        }

        public Cell Apply(Cell offset)
        {
            return new Cell(X + offset.X, Y + offset.Y);
        }

        public override string ToString()
        {
            return (X.ToString(CultureInfo.InvariantCulture) + ":" + Y.ToString(CultureInfo.InvariantCulture));
        }

        public bool Equals(Cell other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.X == X && other.Y == Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Cell)) return false;
            return Equals((Cell) obj);
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

namespace Testing
{
    public class BoardOfChess
    {
        /// <summary>
        /// Конструктор создающий доску со стандартными размерами (8 x 8)
        /// </summary>
        public BoardOfChess()
        {
            Figures = new List<Figure>();
            MaxSizeX = MaxSizeY = 8;
        }

        /// <summary>
        /// Конструктор задающий нестандратные размеры доски
        /// </summary>
        /// <param name="maxSizeX"></param>
        /// <param name="maxSizeY"></param>
        public BoardOfChess(int maxSizeX, int maxSizeY)
        {
            Figures = new List<Figure>();
            MaxSizeX = maxSizeX;
            MaxSizeY = maxSizeY;
        }

        /// <summary>
        /// Фигуры находящиеся на доске
        /// </summary>
        public List<Figure> Figures { get; private set; }

        /// <summary>
        /// Размер доски по оси OX
        /// </summary>
        public int MaxSizeX { get; private set; }

        /// <summary>
        /// Размер доски по оси OY
        /// </summary>
        public int MaxSizeY { get; private set; }

        /// <summary>
        /// Функция возьмет  шахматную комбпнацию из Console.In и напечатает в Console.Out белым мат\шах\пат\ок
        /// </summary>
        public void ReadFromConsole()
        {
            for (int i = 0; i < MaxSizeY; i++)
            {
                string str = Console.In.ReadLine();
                for (int j = 0; j < MaxSizeX; j++)
                {
                    if (str[j] == '.')
                        continue;

                    ColorFigure color = (str[j] >= 'A' && str[j] <= 'Z') ? ColorFigure.White : ColorFigure.Black;
                    switch (str[j].ToString().ToUpper())
                    {
                        case "N":
                            AddFigure(new Nag(new Cell(j, i), color, this));
                            break;
                        case "B":
                            AddFigure(new Bishop(new Cell(j, i), color, this));
                            break;
                        case "R":
                            AddFigure(new Rook(new Cell(j, i), color, this));
                            break;
                        case "Q":
                            AddFigure(new Quine(new Cell(j, i), color, this));
                            break;
                        case "K":
                            AddFigure(new King(new Cell(j, i), color, this));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Добавление новой фигуры в игру
        /// </summary>
        /// <param name="newFigure"></param>
        public void AddFigure(Figure newFigure)
        {
            //Проверка стоит ли на этой позиции другая фигура или нет
            foreach (Figure f in Figures)
                if (f.Location == newFigure.Location)
                {
                    Console.Out.WriteLine(String.Format("На позиции ({0},{1}) уже стоит {2}", f.Location.X, f.Location.Y,
                                                        f.Name));
                    return;
                }
            Figures.Add(newFigure);
        }

        /// <summary>
        /// Добавление новsх фигур
        /// </summary>
        /// <param name="figures">список фигур которые нужно добавить</param>
        public void AddFigures(List<Figure> figures)
        {
            //Проверка стоит ли на этой позиции другая фигура или нет
            foreach (Figure newFigure in figures)
            {
                bool isDontExist = true;
                foreach (Figure f in Figures)
                    if (f.Location == newFigure.Location)
                    {
                        Console.Out.WriteLine(String.Format("На позиции ({0},{1}) уже стоит {2}", f.Location.X,
                                                            f.Location.Y, f.Name));
                        isDontExist = false;
                    }
                if (isDontExist)
                    Figures.Add(newFigure);
            }
        }

        /// <summary>
        /// Удаление фигуры с доски
        /// </summary>
        /// <param name="possition"></param>
        public void RemoveFigure(Cell possition)
        {
            foreach (Figure f in Figures)
            {
                if (f.Location.X == possition.X && f.Location.Y == possition.Y)
                {
                    Console.Out.WriteLine(String.Format("Фигура <{0}> удалена.", f.Name));
                    Figures.Remove(f);
                    return;
                }
                Console.Out.WriteLine("На данной позиции нет фигур");
            }
        }

        /// <summary>
        /// Очистить доску
        /// </summary>
        public void RemoveAllFigures()
        {
            Figures = new List<Figure>();
        }

        /// <summary>
        /// Вывести расположение фигур на доске. (ASCII-art)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result = " |";
            for (int i = 0; i < MaxSizeX; i++)
                result += (i + 1).ToString(CultureInfo.InvariantCulture) + '|';
            result += Environment.NewLine;
            for (int i = 0; i < MaxSizeY; i++)
            {
                for (int z = 0; z < (MaxSizeX + 1)*2; z++)
                    result += '-';
                result += Environment.NewLine + (i + 1).ToString(CultureInfo.InvariantCulture) + "|";
                for (int j = 0; j < MaxSizeX; j++)
                {
                    Figure a = Figures.FirstOrDefault(f => f.Location.X == j && f.Location.Y == i);
                    if (a == null)
                        result += " |";
                    else
                    {
                        if (a.Color == ColorFigure.Black)
                            result += a.Name.ToString()[0].ToString(CultureInfo.InvariantCulture).ToLower() + '|';
                        else
                            result += a.Name.ToString()[0].ToString(CultureInfo.InvariantCulture).ToUpper() + '|';
                    }
                }
                result += Environment.NewLine;
            }
            return result;
        }

        /// <summary>
        /// Проверить находится ли король под шахом
        /// </summary>
        /// <param name="whom">цвет короля</param>
        /// <returns>true-шах\false-нет мата</returns>
        public bool CheckShah(ColorFigure whom)
        {
            //ищем короля
            Figure king = Figures.FirstOrDefault(f => f.Color == whom && f.Name == NameFigure.King);
            if (king == null)
                throw new Exception(whom + " king is dont exists on board");
            //перебираем вражеские фигуры    
            return
                Figures.Where(f => f.Color != whom).Any(
                    f => f.GetMoves().FirstOrDefault(cell => (cell == king.Location)) != null);
        }

        /// <summary>
        /// Проверить поставлен ли мат королю
        /// </summary>
        /// <param name="whom">Цвет короля</param>
        /// <returns>true-мат\false- мата нет</returns>
        public bool CheckMat(ColorFigure whom)
        {
            //ищем короля
            Figure king = Figures.FirstOrDefault(f => f.Color == whom && f.Name == NameFigure.King);
            if (king == null)
                throw new Exception(whom + " king is dont exists on board");

            if (CheckShah(whom)) //Если был шах, то возможен мат
            {
                bool isMat = true;
                foreach (Figure figure in Figures.Where(f => f.Color == whom).ToList())
                {
                    foreach (Cell newLocation in figure.GetMoves().ToList())
                    {
                        Cell oldLocation = figure.Location; //Запоминаем где раньше стояла фигура
                        Figure figureOnNewPlace = figure.Move(newLocation);
                        //передвигаем фигуру на новое возможное место(если там стояла другая фигура вернем ее

                        if (CheckShah(whom) == false)
                            //если при перемещении фигуры шаха больше нет.. то значит и мата нет
                            isMat = false;
                        //Возвращаем фигуру на прежнее место
                        figure.Move(
                            oldLocation);
                        if (figureOnNewPlace != null)
                            AddFigure(figureOnNewPlace);
                        if (!isMat)
                            return false;
                    }
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Проверить не поставлен ли пат королю
        /// </summary>
        /// <param name="whom">цвет короля</param>
        /// <returns>true-пат, false-пата нет</returns>
        public bool CheckPat(ColorFigure whom)
        {
            Figure king = Figures.FirstOrDefault(f => f.Color == whom && f.Name == NameFigure.King);
            if (king == null)
                throw new Exception(whom + " king is dont exists on board");
            //Проверим что ни одна фигура не насосит шах королю
            if (!CheckShah(whom))
                //Может ли хоть одна фигура нашего короля пойти
            {
                bool isPat = true;
                foreach (Figure figure in Figures.Where(f => f.Color == whom).ToList()) //перебираем свои фигуры
                {
                    foreach (Cell newLocation in figure.GetMoves()) //получаем список клеток куда фигура могла сходить
                    {
                        Cell oldLocation = figure.Location; //Запоминаем где раньше стояла фигура
                        Figure figureOnNewPlace = figure.Move(newLocation);
                        //передвигаем фигуру на новое возможное место(если там стояла другая фигура вернем ее

                        if (CheckShah(whom) == false)
                            //если при перемещении фигуры шаха нет значит все оки доки пата нет
                            isPat = false;
                        //Возвращаем фигуру на прежнее место
                        figure.Move(
                            oldLocation);
                        if (figureOnNewPlace != null)
                            AddFigure(figureOnNewPlace);
                        if (!isPat)
                            return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}

namespace Testing
{
    internal class Bishop : INMFigure
    {
        public Bishop(Cell location, ColorFigure color, BoardOfChess board)
            : base(location, NameFigure.Bishop, color, board)
        {
        }

        public override List<Cell> GetMoves()
        {
            var firstOffset = new[]
                                  {
                                      new Cell(-1, 1),
                                      new Cell(-1, -1),
                                      new Cell(1, 1),
                                      new Cell(1, -1)
                                  };
            return base.GetOffset(firstOffset.ToList());
        }
    }
}

namespace Testing
{
    /// <summary>
    /// Название фигур
    /// </summary>
    public enum NameFigure
    {
        King, //король
        Bishop, //Слон
        Nag, //Конь
        Quine, //Королева
        Rook //Ладья
    }

    /// <summary>
    /// Цвета игроков
    /// </summary>
    public enum ColorFigure
    {
        Black, //Черный
        White //Белый
    }

    /// <summary>
    /// Класс фигур для игр
    /// </summary>
    public abstract class Figure
    {
        protected Figure(Cell location, NameFigure name, ColorFigure color, BoardOfChess board)
        {
            Location = location;
            Name = name;
            Color = color;
            Board = board;
        }

        /// <summary>
        /// Позиция фигуры на доске
        /// </summary>
        public Cell Location { get; private set; }

        /// <summary>
        /// Название фигуры
        /// </summary>
        public NameFigure Name { get; private set; }

        /// <summary>
        /// Цвет фигуры
        /// </summary>
        public ColorFigure Color { get; private set; }

        /// <summary>
        /// Доска на которой стоит фигура
        /// Зачем?! вдруг мы играем на двух досках)
        /// </summary>
        public BoardOfChess Board { get; private set; }

        /// <summary>
        /// Метод возвращает возмодные перемещения фигуры по доске(Без проверки о возможном шахе)
        /// </summary>
        /// <returns>Список позиций куда может передвинутся фигура</returns>
        public abstract List<Cell> GetMoves();

        /// <summary>
        /// Метод используя начальное смещение фигуры способен вернуть куда фигура может  сместиться
        /// </summary>
        /// <param name="firstOffset"></param>
        /// <returns></returns>
        protected abstract List<Cell> GetOffset(List<Cell> firstOffset);

        /// <summary>
        /// Метод передвигает фигуру на новую позицию. Если на месте фигуры стояла другая фигура то она удаляется из списка фигур.. и возвращается методом
        /// </summary>
        /// <param name="newLocation"></param>
        /// <returns></returns>
        public Figure Move(Cell newLocation)
        {
            Figure figureOnNewLocaion = Board.Figures.FirstOrDefault(f => f.Location == newLocation);
            if (figureOnNewLocaion != null)
                Board.Figures.Remove(figureOnNewLocaion);
            Board.Figures.FirstOrDefault(f => this == f).Location = newLocation; //передвинули фигуру
            return figureOnNewLocaion;
        }

        /// <summary>
        /// Рисует доску и отмечает позиции cells *- места куда может сходить фигура
        /// Метод необходим для дебага
        /// </summary>
        /// <param name="cells">Места которые нужно закрасить *</param>
        /// <returns>Сама картинка ASCII-art</returns>
        public string Show(List<Cell> cells)
        {
            string result = " |";
            for (int i = 0; i < Board.MaxSizeX; i++)
                result += (i + 1).ToString(CultureInfo.InvariantCulture) + '|';
            result += Environment.NewLine;
            for (int i = 0; i < Board.MaxSizeY; i++)
            {
                for (int z = 0; z < (Board.MaxSizeX + 1)*2; z++)
                    result += '-';
                result += Environment.NewLine + (i + 1).ToString(CultureInfo.InvariantCulture) + "|";

                for (int j = 0; j < Board.MaxSizeX; j++)
                {
                    Figure a = Board.Figures.FirstOrDefault(f => f.Location.X == j && f.Location.Y == i);
                    if (a == null)
                    {
                        Cell cell = cells.FirstOrDefault(f => f.X == j && f.Y == i);
                        if (cell != null)
                            result += "*|";
                        else
                            result += " |";
                    }
                    else
                    {
                        if (a.Color == ColorFigure.Black)
                            result += a.Name.ToString()[0].ToString(CultureInfo.InvariantCulture).ToLower() + '|';
                        else
                            result += a.Name.ToString()[0].ToString(CultureInfo.InvariantCulture).ToUpper() + '|';
                    }
                }


                result += Environment.NewLine;
            }
            return result;
        }
    }

    /// <summary>
    /// Абстрактный класс фигур имеющих конечное количество шагов 
    /// </summary>
    public abstract class FNMFigure : Figure
    {
        protected FNMFigure(Cell location, NameFigure name, ColorFigure color, BoardOfChess board)
            : base(location, name, color, board)
        {
        }

        protected override List<Cell> GetOffset(List<Cell> firstOffset)
        {
            var allowableOffset = new List<Cell>();
            foreach (Cell t in firstOffset)
            {
                Cell newPos = t.Apply(Location);
                //Проверяем то что смещение лежит на доске
                if (newPos.X >= 0 && newPos.X < Board.MaxSizeX && newPos.Y >= 0 && newPos.Y < Board.MaxSizeY)
                {
                    //Проверяем не стоит ли там наша фигура
                    bool isAllowCell = true;
                    foreach (Figure f in Board.Figures)
                        if (f.Location == newPos)
                        {
                            if (f.Color == Color) //свою фигуру есть нельзя                         
                                isAllowCell = false;
                            break;
                        }
                    if (isAllowCell)
                        allowableOffset.Add(newPos);
                }
            }
            return allowableOffset;
        }
    }

    /// <summary>
    /// Абстрактный класс фигур, идущих бесконечно в определенном направлении. В firstOfSet -хранятся первый цикл таких шагов
    /// </summary>
    public abstract class INMFigure : Figure
    {
        protected INMFigure(Cell location, NameFigure name, ColorFigure color, BoardOfChess board)
            : base(location, name, color, board)
        {
        }

        protected override List<Cell> GetOffset(List<Cell> firstOffset)
        {
            var allowableOffset = new List<Cell>();
            int r = 1;
            while (firstOffset.Count != 0)
            {
                var deleteOffSet = new List<Cell>();
                foreach (Cell c in firstOffset)
                {
                    Cell newPos = (c*r).Apply(Location);
                    //Проверяем то что смещение лежит на доске
                    bool isCloseWay = false; // Флаг удалять ли из цикла данное  смещение
                    bool isAllowCell = true; //Добавлять ли ячейку
                    if (newPos.X >= 0 && newPos.X < Board.MaxSizeX && newPos.Y >= 0 && newPos.Y < Board.MaxSizeY)
                    {
                        //Не стоит ли на новой позиции фигуры
                        foreach (Figure f in Board.Figures)
                            if (f.Location == newPos)
                            {
                                isCloseWay = true;
                                if (f.Color == Color) //Свою фигуру есть нельзя
                                    isAllowCell = false;
                                break;
                            }
                        if (isAllowCell)
                            allowableOffset.Add(newPos);
                        if (isCloseWay)
                            deleteOffSet.Add(c);
                    }
                    else
                        deleteOffSet.Add(c);
                }
                deleteOffSet.ForEach(c => firstOffset.Remove(c));
                r++;
            }
            return allowableOffset;
        }
    }
}

namespace Testing
{
    internal class King : FNMFigure
    {
        public King(Cell location, ColorFigure color, BoardOfChess board)
            : base(location, NameFigure.King, color, board)
        {
        }

        public override List<Cell> GetMoves()
        {
            var firstOffset = new[]
                                  {
                                      new Cell(-1, 1),
                                      new Cell(1, 1),
                                      new Cell(1, 0),
                                      new Cell(1, -1),
                                      new Cell(0, -1),
                                      new Cell(0, 1),
                                      new Cell(-1, -1),
                                      new Cell(-1, 0)
                                  };
            return base.GetOffset(firstOffset.ToList());
        }
    }
}

namespace Testing
{
    internal class Nag : FNMFigure
    {
        public Nag(Cell location, ColorFigure color, BoardOfChess board) : base(location, NameFigure.Nag, color, board)
        {
        }

        public override List<Cell> GetMoves()
        {
            var firstOffset = new[]
                                  {
                                      new Cell(1, 2),
                                      new Cell(2, 1),
                                      new Cell(2, -1),
                                      new Cell(1, -2),
                                      new Cell(-1, -2),
                                      new Cell(-2, -1),
                                      new Cell(-2, 1),
                                      new Cell(-1, 2)
                                  };
            return base.GetOffset(firstOffset.ToList());
        }
    }
}

namespace Testing
{
    internal class Quine : INMFigure
    {
        public Quine(Cell location, ColorFigure color, BoardOfChess board)
            : base(location, NameFigure.Quine, color, board)
        {
        }

        public override List<Cell> GetMoves()
        {
            var firstOffset = new[]
                                  {
                                      new Cell(-1, 1),
                                      new Cell(1, 1),
                                      new Cell(1, 0),
                                      new Cell(0, 1),
                                      new Cell(1, -1),
                                      new Cell(0, -1),
                                      new Cell(-1, -1),
                                      new Cell(-1, 0)
                                  };
            return base.GetOffset(firstOffset.ToList());
        }
    }
}

namespace Testing
{
    public class Rook : INMFigure
    {
        public Rook(Cell location, ColorFigure color, BoardOfChess board)
            : base(location, NameFigure.Rook, color, board)
        {
        }

        public override List<Cell> GetMoves()
        {
            var firstOffset = new[]
                                  {
                                      new Cell(0, 1),
                                      new Cell(-1, 0),
                                      new Cell(1, 0),
                                      new Cell(0, -1)
                                  };

            return base.GetOffset(firstOffset.ToList());
        }
    }
}