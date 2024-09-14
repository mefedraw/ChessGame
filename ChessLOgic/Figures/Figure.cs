namespace ChessLogic.Figures;

public abstract class Figure : IFigure
{
    protected Figure(char color, FigureType type)
    {
        Color = color;
        Type = type;    
    }

    public char Color { get; private set; }
    public FigureType Type { get; protected set; }
    public abstract bool PossibleMove( ref IFigure?[][] board,(int,int) moveStartPosition, (int,int) moveEndPosition);
    public virtual bool KingIsUnderAttack(IFigure?[][] board, (int x, int y) position, char kingColor)
    {
        for (var column = 0; column < 8; column++)
        {
            for (var row = 0; row < 8; row++)
            {
                var figure = board[column][row];
                // Если фигура противника
                if (figure != null && figure.Color != kingColor)
                {
                    // Проверяем, может ли фигура атаковать клетку
                    if (figure.PossibleMove(ref board, (column, row), position))
                    {
                        figure.PossibleMove(ref board, position, (column, row));
                        return true; // Клетка под ударом
                    }
                }
            }
        }

        return false;
    }
    public virtual bool KingIsUnderAttack(IFigure?[][] board, char pieceColor)
    {
        (int x, int y) kingPosition = FindKing(board, pieceColor);

        for (var column = 0; column < 8; column++)
        {
            for (var row = 0; row < 8; row++)
            {
                var figure = board[column][row];
                // Если фигура противника
                if (figure != null && figure.Color != pieceColor)
                {
                    var tempPiece = board[kingPosition.x][kingPosition.y];
                    // Проверяем, может ли фигура атаковать клетку
                    if (figure.PossibleMove(ref board, (column, row), kingPosition))
                    {
                        figure.PossibleMove(ref board, kingPosition, (column, row));
                        if (tempPiece != null)
                        {
                            board[kingPosition.x][kingPosition.y] = tempPiece;
                        }

                        return true; // Клетка под ударом
                    }
                }
            }
        }

        return false;
    }
    
    public virtual (int, int) FindKing(IFigure?[][] board, char kingColor)
    {
        for (var column = 0; column < 8; column++) // находим союзного короля
        {
            for (var row = 0; row < 8; row++)
            {
                if (board[column][row] != null)
                {
                    if (board[column][row].Type == FigureType.King && board[column][row].Color == kingColor)
                    {
                        return (column, row);
                    }
                }
            }
        }

        return (0, 0);
    }
    
}