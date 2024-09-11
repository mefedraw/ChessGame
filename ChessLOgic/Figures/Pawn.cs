namespace ChessLogic.Figures;

public class Pawn : Figure
{
    public override bool PossibleMove(ref IFigure[][] board, (int, int) moveStartPosition, (int, int) moveEndPosition)
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

        int direction =
            figure.Color == 'w' ? 1 : -1; // Для белых пешек движение вверх (координаты уменьшаются), для черных вниз
        // Ход вперед на одну клетку
        if (endX == startX + direction && endY == startY && board[endX][endY] == null)
        {
            board[startX][startY] = null;
            board[endX][endY] = figure;
            if (IsUnderAttack(board, figure.Color))
            {
                board[startX][startY] = figure;
                board[endX][endY] = null;
                return false;
            }

            return true;
        }

        // Ход вперед на две клетки, если пешка на своей начальной позиции
        bool isStartingPosition = (figure.Color == 'w' && startX == 1) || (figure.Color == 'b' && startX == 6);
        if (isStartingPosition && endX == startX + 2 * direction && (endY == startY) && (board[endX][endY] == null) &&
            (board[startY][startX + direction] == null))
        {
            board[startX][startY] = null;
            board[endX][endY] = figure;
            if (IsUnderAttack(board, figure.Color))
            {
                board[startX][startY] = figure;
                board[endX][endY] = null;
                return false;
            }

            return true;
        }

        // Взятие фигуры по диагонали
        if (endX == startX + direction && (endY == startY - 1 || endY == startY + 1) && board[endX][endY] != null &&
            board[endX][endY].Color != figure.Color)
        {
            board[startX][startY] = null;
            var tempFigure = board[endX][endY];
            board[endX][endY] = figure;
            if (IsUnderAttack(board, figure.Color))
            {
                board[startX][startY] = figure;
                board[endX][endY] = tempFigure;
                return false;
            }

            return true;
        }

        return false; // Все другие ходы недопустимы для пешки
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

    public Pawn(char color) : base(color, FigureType.Pawn)
    {
    }
}