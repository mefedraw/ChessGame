namespace ChessLogic;

public enum FigureType
{
    Pawn,
    King,
    Queen,
    Bishop,
    Knight,
    Rook,
    None
}

public interface IFigure
{
    public char Color { get; }
    public FigureType Type { get; }
    public  bool PossibleMove( ref IFigure[][] board,(int,int) moveStartPosition, (int,int) moveEndPosition);
}

public abstract class Figure : IFigure
{
    protected Figure(char color, FigureType type)
    {
        Color = color;
        Type = type;    
    }

    public char Color { get; private set; }
    public FigureType Type { get; protected set; }
    public abstract bool PossibleMove( ref IFigure[][] board,(int,int) moveStartPosition, (int,int) moveEndPosition);
    
    public virtual void Moo(){}
}



public  class Pawn : Figure
{
    public override bool PossibleMove(ref IFigure[][] board,(int,int) moveStartPosition, (int,int) moveEndPosition)
    {
        int startX = moveStartPosition.Item1;
        int startY = moveStartPosition.Item2;
        int endX = moveEndPosition.Item1;
        int endY = moveEndPosition.Item2;
        IFigure figure = board[startX][startY];
        if (figure == null || figure.Type != FigureType.Pawn)
        {
            return false; // Если на начальной позиции нет фигуры или это не пешка
        }
        int direction = figure.Color == 'w' ? 1 : -1; // Для белых пешек движение вверх (координаты уменьшаются), для черных вниз
        // Ход вперед на одну клетку
        if (endX == startX + direction && endY == startY && board[endX][endY] == null)
        {
            board[startX][startY] = null;
            board[endX][endY] = figure;
            return true;
        }
        // Ход вперед на две клетки, если пешка на своей начальной позиции
        bool isStartingPosition = (figure.Color == 'w' && startX == 1) || (figure.Color == 'b' && startX == 6);
        if (isStartingPosition && endX == startX + 2 * direction && (endY == startY) && (board[endX][endY] == null) && (board[startY][startX+direction] == null))
        {
            board[startX][startY] = null;
            board[endX][endY] = figure;
            return true;
        }
        // Взятие фигуры по диагонали
        if ( endX== startX + direction && (endY == startY - 1 || endY == startY + 1) && board[endX][endY] != null && board[endX][endY].Color != figure.Color)
        {
            board[startX][startY] = null;
            board[endX][endY] = figure;
            return true;
        }
        return false; // Все другие ходы недопустимы для пешки
    }

    public Pawn(char color) : base(color, FigureType.Pawn)
    {
    }
}




public class King : Figure
{
    public override bool PossibleMove(ref IFigure[][] board, (int, int) moveStartPosition, (int, int) moveEndPosition)
    {
        int startX = moveStartPosition.Item1; // Горизонтальная координата (столбец)
        int startY = moveStartPosition.Item2; // Вертикальная координата (строка)
        int endX = moveEndPosition.Item1;     // Горизонтальная координата (столбец)
        int endY = moveEndPosition.Item2;     // Вертикальная координата (строка)

        IFigure figure = board[startX][startY];

        if (figure == null || figure.Type != FigureType.King)
        {
            return false; // Если на начальной позиции нет фигуры или это не король
        }

        // Король может двигаться на одну клетку в любом направлении
        int deltaX = Math.Abs(endX - startX);
        int deltaY = Math.Abs(endY - startY);

        if ((deltaX <= 1 && deltaY <= 1) && !(deltaX == 0 && deltaY == 0)) // Движение на 1 клетку по горизонтали, вертикали или диагонали
        {
            // Если конечная клетка пуста или там фигура противника
            if (board[endX][endY] == null || board[endX][endY].Color != figure.Color)
            {
                board[startX][startY] = null;
                board[endX][endY] = figure;
                return true;
            }
        }

        return false; // Любое другое движение недопустимо для короля
    }


    public King(char color) : base(color, FigureType.King)
    {
    }
}




public class Queen : Figure
{
public override bool PossibleMove(ref IFigure[][] board, (int, int) moveStartPosition, (int, int) moveEndPosition)
{
    int startX = moveStartPosition.Item1; // Горизонтальная координата (столбец)
    int startY = moveStartPosition.Item2; // Вертикальная координата (строка)
    int endX = moveEndPosition.Item1;     // Горизонтальная координата (столбец)
    int endY = moveEndPosition.Item2;     // Вертикальная координата (строка)

    IFigure figure = board[startX][startY];

    if (figure == null || figure.Type != FigureType.Queen)
    {
        return false; // Если на начальной позиции нет фигуры или это не ферзь
    }

    // Ферзь может двигаться как по диагонали, так и по горизонтали/вертикали
    int deltaX = Math.Abs(endX - startX);
    int deltaY = Math.Abs(endY - startY);

    // Проверка: если ход по диагонали
    if (deltaX == deltaY)
    {
        // Логика для движения по диагонали (как у слона)
        int stepX = (endX - startX) > 0 ? 1 : -1;
        int stepY = (endY - startY) > 0 ? 1 : -1;

        int x = startX + stepX;
        int y = startY + stepY;
        while (x != endX && y != endY)
        {
            if (board[x][y] != null)
            {
                return false; // Если на пути есть фигура, ход невозможен
            }
            x += stepX;
            y += stepY;
        }

        // Если конечная клетка пуста или там фигура противника, ход возможен
        if (board[endX][endY] == null || board[endX][endY].Color != figure.Color)
        {
            board[startX][startY] = null;
            board[endX][endY] = figure;
            return true;
        }
    }

    // Проверка: если ход по горизонтали или вертикали
    if (startX == endX || startY == endY)
    {
        // Логика для движения по прямой линии (как у ладьи)
        if (startX == endX) // Вертикальное движение
        {
            int stepY = startY < endY ? 1 : -1;
            for (int y = startY + stepY; y != endY; y += stepY)
            {
                if (board[startX][y] != null)
                {
                    return false; // Если на пути есть фигура, ход невозможен
                }
            }
        }
        else if (startY == endY) // Горизонтальное движение
        {
            int stepX = startX < endX ? 1 : -1;
            for (int x = startX + stepX; x != endX; x += stepX)
            {
                if (board[x][startY] != null)
                {
                    return false; // Если на пути есть фигура, ход невозможен
                }
            }
        }

        // Если конечная клетка пуста или там фигура противника, ход возможен
        if (board[endX][endY] == null || board[endX][endY].Color != figure.Color)
        {
            board[startX][startY] = null;
            board[endX][endY] = figure;
            return true;
        }
    }

    return false; // Все другие ходы недопустимы для ферзя
}

    public Queen(char color) : base(color, FigureType.Queen)
    {
    }
    
}




public class Bishop : Figure
{
    public override bool PossibleMove(ref IFigure[][] board, (int, int) moveStartPosition, (int, int) moveEndPosition)
    {
        int startX = moveStartPosition.Item1; // Горизонтальная координата (столбец)
        int startY = moveStartPosition.Item2; // Вертикальная координата (строка)
        int endX = moveEndPosition.Item1;     // Горизонтальная координата (столбец)
        int endY = moveEndPosition.Item2;     // Вертикальная координата (строка)

        IFigure figure = board[startX][startY];

        if (figure == null || figure.Type != FigureType.Bishop)
        {
            return false; // Если на начальной позиции нет фигуры или это не слон
        }

        // Слон может двигаться только по диагоналям, то есть |dx| == |dy|
        int deltaX = Math.Abs(endX - startX);
        int deltaY = Math.Abs(endY - startY);
    
        if (deltaX != deltaY)
        {
            return false; // Если движение не по диагонали
        }

        // Определяем направление движения
        int stepX = (endX - startX) > 0 ? 1 : -1;
        int stepY = (endY - startY) > 0 ? 1 : -1;

        // Проверяем весь путь на наличие препятствий
        int x = startX + stepX;
        int y = startY + stepY;
        while (x != endX && y != endY)
        {
            if (board[x][y] != null)
            {
                return false; // Если на пути есть фигура, ход невозможен
            }
            x += stepX;
            y += stepY;
        }

        // Если в конечной клетке стоит фигура противника, её можно съесть
        if (board[endX][endY] == null || board[endX][endY].Color != figure.Color)
        {
            board[startX][startY] = null;
            board[endX][endY] = figure;
            return true;
        }

        return false; // Если в конечной клетке фигура того же цвета, ход невозможен
    }

    public Bishop(char color) : base(color, FigureType.Bishop)
    {
        Type = FigureType.Bishop;
    }
}



public class Knight : Figure
{

    public override bool PossibleMove(ref IFigure[][] board, (int, int) moveStartPosition, (int, int) moveEndPosition)
    {
        int startX = moveStartPosition.Item1; // Горизонтальная координата (столбец)
        int startY = moveStartPosition.Item2; // Вертикальная координата (строка)
        int endX = moveEndPosition.Item1;     // Горизонтальная координата (столбец)
        int endY = moveEndPosition.Item2;     // Вертикальная координата (строка)

        IFigure figure = board[startX][startY];

        if (figure == null || figure.Type != FigureType.Knight)
        {
            return false; // Если на начальной позиции нет фигуры или это не конь
        }

        // Варианты возможных движений коня
        int[] dx = { 2, 2, -2, -2, 1, 1, -1, -1 };
        int[] dy = { 1, -1, 1, -1, 2, -2, 2, -2 };

        for (int i = 0; i < 8; i++)
        {
            int newX = startX + dx[i];
            int newY = startY + dy[i];

            if (newX == endX && newY == endY)
            {
                // Если клетка пуста или там фигура противника, ход возможен
                if (board[endX][endY] == null || board[endX][endY].Color != figure.Color)
                {
                    board[startX][startY] = null;
                    board[endX][endY] = figure;
                    return true;
                }
            }
        }

        return false; // Если ни одно из возможных движений коня не подходит
    }
    
    public Knight(char color): base(color,FigureType.Knight)
    {
        Type = FigureType.Knight;
    }
}



public class Rook : Figure
{
    public override bool PossibleMove(ref IFigure[][] board, (int, int) moveStartPosition, (int, int) moveEndPosition)
    {
        int startX = moveStartPosition.Item1; // Горизонтальная координата (столбец)
        int startY = moveStartPosition.Item2; // Вертикальная координата (строка)
        int endX = moveEndPosition.Item1;     // Горизонтальная координата (столбец)
        int endY = moveEndPosition.Item2;     // Вертикальная координата (строка)

        IFigure figure = board[startX][startY];

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
            return true;
        }

        return false; // Если в конечной клетке фигура того же цвета, ход невозможен
    }
    

    public Rook(char color) : base(color,FigureType.Rook)
    {
        Type = FigureType.Rook;
    }
}
