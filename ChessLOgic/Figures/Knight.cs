namespace ChessLogic.Figures;

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
                    if (IsUnderAttack(board, figure.Color))
                    {
                        board[startX][startY] = figure;
                        board[endX][endY] = null;
                        return false;
                    }
                    return true;
                }
            }
        }
        
        return false; // Если ни одно из возможных движений коня не подходит
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
    
    public Knight(char color): base(color,FigureType.Knight)
    {
        Type = FigureType.Knight;
    }
}