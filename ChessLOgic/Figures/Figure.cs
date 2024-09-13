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
    public abstract bool IsUnderAttack(IFigure?[][] board, (int x, int y) position, char kingColor);
    public abstract bool IsUnderAttack(IFigure?[][] board,  char kingColor);
    
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

    public virtual void Moo(){}
}