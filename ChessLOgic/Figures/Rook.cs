namespace ChessLogic.Figures;

public class Rook : Figure
{
    public override bool PossibleMove(ref IFigure?[][] board, (int, int) moveStartPosition, (int, int) moveEndPosition)
    {
        int startX = moveStartPosition.Item1; // Горизонтальная координата (столбец)
        int startY = moveStartPosition.Item2; // Вертикальная координата (строка)
        int endX = moveEndPosition.Item1; // Горизонтальная координата (столбец)
        int endY = moveEndPosition.Item2; // Вертикальная координата (строка)

        IFigure? figure = board[startX][startY];

        if (figure == null || figure.Type != FigureType.Rook)
        {
            return false; // Если на начальной позиции нет фигуры или это не ладья
        }

        // Ладья может двигаться только по прямой линии: либо по горизонтали, либо по вертикали
        if (startX != endX && startY != endY)
        {
            return false; // Ладья не может двигаться по диагонали
        }

        // Проверка пути: должен быть свободен весь путь от старта до конца (без препятствий)
        if (startX == endX) // Вертикальное движение  
        {
            int step = startY < endY ? 1 : -1; // Определяем направление движения
            for (int y = startY + step; y != endY; y += step)
            {
                if (board[startX][y] != null)
                {
                    return false; // Если на пути есть фигура, ход невозможен
                }
            }
        }
        else if (startY == endY) // Горизонтальное движение
        {
            int step = startX < endX ? 1 : -1; // Определяем направление движения
            for (int x = startX + step; x != endX; x += step)
            {
                if (board[x][startY] != null)
                {
                    return false; // Если на пути есть фигура, ход невозможен
                }
            }
        }

        // Если в конечной клетке стоит фигура противника, её можно съесть
        if (board[endX][endY] == null || board[endX][endY].Color != figure.Color)
        {
            board[startX][startY] = null;
            board[endX][endY] = figure;
            if (KingIsUnderAttack(board, figure.Color))
            {
                board[startX][startY] = figure;
                board[endX][endY] = null;
                return false;
            }
            RookDidMove = true;
            return true;
        }

        return false; // Если в конечной клетке фигура того же цвета, ход невозможен
    }
    
    public override List<(int, int)> GetPossibleMoves(ref IFigure?[][] board, (int, int) currentPos)
    {
        List<(int, int)> possibleMoves = new List<(int, int)>();
        int x = currentPos.Item1;
        int y = currentPos.Item2;

        // Двигаемся вверх
        for (int i = x + 1; i < 8; i++)
        {
            if (AddMoveIfValid(ref board, possibleMoves, i, y)) break;
        }

        // Двигаемся вниз
        for (int i = x - 1; i >= 0; i--)
        {
            if (AddMoveIfValid(ref board, possibleMoves, i, y)) break;
        }

        // Двигаемся вправо
        for (int i = y + 1; i < 8; i++)
        {
            if (AddMoveIfValid(ref board, possibleMoves, x, i)) break;
        }

        // Двигаемся влево
        for (int i = y - 1; i >= 0; i--)
        {
            if (AddMoveIfValid(ref board, possibleMoves, x, i)) break;
        }

        return possibleMoves;
    }

    private bool AddMoveIfValid(ref IFigure?[][] board, List<(int, int)> moves, int x, int y)
    {
        if (board[x][y] == null)
        {
            moves.Add((x, y));
            return false; // Продолжаем движение
        }
        else if (board[x][y].Color != this.Color)
        {
            moves.Add((x, y)); // Вражеская фигура, добавляем и прекращаем движение
            return true;
        }
        return true; // Своя фигура, прекращаем движение
    }


    public bool RookDidMove { get; set; }

    public Rook(char color) : base(color, FigureType.Rook)
    {
        Type = FigureType.Rook;
    }
}