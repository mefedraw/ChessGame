namespace ChessLogic.Figures;

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

public override bool IsUnderAttack(IFigure[][] board, (int x, int y) position, char kingColor)
{
    throw new NotImplementedException();
}

public override bool IsUnderAttack(IFigure[][] board, char kingColor)
{
    (int x, int y) kingPosition = (0, 0);

    for (var column = 0; column < 8; column++) // находим союзного короля
    {
        for (var row = 0; row < 8; row++)
        {
            if (board[column][row] != null)
            {
                if (board[column][row].Type == FigureType.King && board[column][row].Color == kingColor)
                {
                    kingPosition = (column, row);
                    column = 8; 
                    break;
                }
            }
        }
    }

    for (var column = 0; column < 8; column++)
    {
        for (var row = 0; row < 8; row++)
        {
            var figure = board[column][row];
            // Если фигура противника
            if (figure != null && figure.Color != kingColor)
            {
                // Проверяем, может ли фигура атаковать клетку
                if (figure.PossibleMove(ref board, (column, row), kingPosition))
                {
                    figure.PossibleMove(ref board, kingPosition, (column, row));
                    board[kingPosition.x][kingPosition.y] = new King(kingColor);
                    return true; // Клетка под ударом
                }
            }
        }
    }

    return false;
}

public Queen(char color) : base(color, FigureType.Queen)
{
}
    
}