namespace ChessLogic.Figures;

public interface IFigure
{
    public char Color { get; }
    public FigureType Type { get; }
    public  bool PossibleMove( ref IFigure?[][] board,(int,int) moveStartPosition, (int,int) moveEndPosition);
    protected bool IsUnderAttack(IFigure?[][] board, (int x, int y) position, char kingColor);
    protected bool IsUnderAttack(IFigure?[][] board, char kingColor);
    protected (int, int) FindKing(IFigure?[][] board, char kingColor);
}