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
    public abstract bool PossibleMove( ref IFigure[][] board,(int,int) moveStartPosition, (int,int) moveEndPosition);
    public abstract bool IsUnderAttack(IFigure[][] board, (int x, int y) position, char kingColor);
    public abstract bool IsUnderAttack(IFigure[][] board,  char kingColor);

    public virtual void Moo(){}
}