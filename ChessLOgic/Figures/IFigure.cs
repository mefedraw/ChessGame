namespace ChessLogic.Figures;

public interface IFigure
{
    public char Color { get; }
    public FigureType Type { get; }
    public  bool PossibleMove( ref IFigure[][] board,(int,int) moveStartPosition, (int,int) moveEndPosition);
    public bool IsUnderAttack(IFigure[][] board, (int x, int y) position, char kingColor);
    public bool IsUnderAttack(IFigure[][] board, char kingColor);
}