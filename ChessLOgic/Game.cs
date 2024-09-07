using System.Text;
using ChessLogic.Figures;

namespace ChessLogic;

public class Game
{
    public Game()
    {
        // Инициализация доски
        Board = new Figure[8][];
        for (int i = 0; i < 8; i++)
        {
            Board[i] = new Figure[8];
        }

        // Расстановка белых фигур
        Board[0][0] = new Rook('w'); // Ладья
        Board[0][1] = new Knight('w'); // Конь
        Board[0][2] = new Bishop('w'); // Слон
        Board[0][3] = new Queen('w'); // Ферзь
        Board[0][4] = new King('w'); // Король
        Board[0][5] = new Bishop('w'); // Слон
        Board[0][6] = new Knight('w'); // Конь
        Board[0][7] = new Rook('w'); // Ладья
        for (int i = 0; i < 8; i++)
        {
            Board[1][i] = new Pawn('w'); // Пешки на второй линии
        }

        for (int i = 2; i < 6; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Board[i][j] = null;
            }
        }

        // Расстановка черных фигур
        Board[7][0] = new Rook('b'); // Ладья
        Board[7][1] = new Knight('b'); // Конь
        Board[7][2] = new Bishop('b'); // Слон
        Board[7][3] = new Queen('b'); // Ферзь
        Board[7][4] = new King('b'); // Король
        Board[7][5] = new Bishop('b'); // Слон
        Board[7][6] = new Knight('b'); // Конь
        Board[7][7] = new Rook('b'); // Ладья
        for (int i = 0; i < 8; i++)
        {
            Board[6][i] = new Pawn('b'); // Пешки на седьмой линии
        }

        WhitesTurn = true;
    }

    public IFigure[][] Board;

    public bool WhitesTurn { get; set; }

    public bool DoMove(string move)
    {
        try
        {
            // Преобразуем ход (например, e2e4) в координаты на доске
            (int, int) moveStartCoords = (CharToCoord(move[1]), CharToCoord(move[0]));
            (int, int) moveEndCoords = (CharToCoord(move[3]), CharToCoord(move[2]));

            // Проверяем, есть ли фигура на начальной позиции
            IFigure figure = Board[moveStartCoords.Item1][moveStartCoords.Item2];
            if (figure == null)
            {
                Console.WriteLine("Нет фигуры на начальной позиции.");
                return false;
            }

            // Выполняем ход
            if (figure.PossibleMove(ref Board, moveStartCoords, moveEndCoords))
            {
                WhitesTurn = !WhitesTurn;
                return true;
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при выполнении хода: {ex.Message}");
            return false; // Возвращаем false при любой ошибке
        }

        return false;
    }

    public int CharToCoord(char c)
    {
        if (char.IsLetter(c))
        {
            var test1 = c - 'a';
            return c - 'a'; // Преобразуем буквы 'a'-'h' в индексы 0-7
        }

        if (char.IsDigit(c))
        {
            var test2 = (c - '0' - 1);
            return (c - '0' - 1); // Преобразуем цифры '1'-'8' в индексы 7-0
        }

        throw new ArgumentException("Некорректный символ для шахматных координат");
    }

    public void PrintBoardToConsole()
    {
        Console.WriteLine("  a b c d e f g h"); // Верхняя строка с буквами для колонок

        for (int x = 7; x >= 0; x--) // Начинаем с 7, чтобы выводить сверху вниз
        {
            Console.Write($"{x + 1} "); // Печатаем цифру для строки

            for (int y = 0; y < 8; y++)
            {
                if (Board[x][y] == null)
                {
                    Console.Write("- ");
                }
                else
                {
                    Console.Write($"{GetFigureSymbol(Board[x][y])} ");
                }
            }

            Console.WriteLine($"{x + 1}"); // Печатаем цифру для строки с правой стороны
        }

        Console.WriteLine("  a b c d e f g h"); // Нижняя строка с буквами для колонок
    }

    public string GetBoardAsString()
    {
        var sb = new StringBuilder();

        sb.AppendLine("  a b c d e f g h"); // Верхняя строка с буквами для колонок

        for (int x = 7; x >= 0; x--) // Начинаем с 7, чтобы выводить сверху вниз
        {
            sb.Append($"{x + 1} "); // Печатаем цифру для строки

            for (int y = 0; y < 8; y++)
            {
                if (Board[x][y] == null)
                {
                    sb.Append("- ");
                }
                else
                {
                    sb.Append($"{GetFigureSymbol(Board[x][y])} ");
                }
            }

            sb.AppendLine($"{x + 1}"); // Печатаем цифру для строки с правой стороны
        }

        sb.AppendLine("  a b c d e f g h"); // Нижняя строка с буквами для колонок

        return sb.ToString();
    }
    
    public string GetBoardAsFEN()
    {
        var sb = new StringBuilder();
    
        for (int x = 7; x >= 0; x--) // от 8-й линии к 1-й
        {
            int emptySquares = 0;
        
            for (int y = 0; y < 8; y++)
            {
                if (Board[x][y] == null)
                {
                    emptySquares++; // увеличиваем счетчик пустых клеток
                }
                else
                {
                    if (emptySquares > 0)
                    {
                        sb.Append(emptySquares); // добавляем число пустых клеток
                        emptySquares = 0;
                    }
                    sb.Append(GetFigureSymbol(Board[x][y])); // добавляем фигуру
                }
            }
        
            if (emptySquares > 0)
            {
                sb.Append(emptySquares); // добавляем оставшиеся пустые клетки в строке
            }

            if (x > 0)
            {
                sb.Append('/'); // разделитель между строками доски
            }
        }
    
        // Добавление оставшейся информации для FEN строки:
        sb.Append(" w "); // чей ход, например, всегда ход белых в этой реализации
    
        // Информация о доступности рокировки (пример: KQkq - все рокировки доступны)
        sb.Append("KQkq ");
    
        // Эн-пассан информация. Здесь пропускаем "-", если нет возможности взять на проходе
        sb.Append("- ");
    
        // Количество полуходов (для реализации правила 50 ходов)
        sb.Append("0 ");
    
        // Номер полного хода
        sb.Append("1");
    
        return sb.ToString();
    }



    // Метод для отображения символов фигур
    public char GetFigureSymbol(IFigure figure)
    {
        if (figure == null)
        {
            return '-';
        }
        switch (figure.Type)
        {
            case FigureType.Pawn:
                return figure.Color == 'w' ? 'P' : 'p';
            case FigureType.King:
                return figure.Color == 'w' ? 'K' : 'k';
            case FigureType.Queen:
                return figure.Color == 'w' ? 'Q' : 'q';
            case FigureType.Bishop:
                return figure.Color == 'w' ? 'B' : 'b';
            case FigureType.Knight:
                return figure.Color == 'w' ? 'N' : 'n';
            case FigureType.Rook:
                return figure.Color == 'w' ? 'R' : 'r';
            default:
                return '-';
        }
    }
}