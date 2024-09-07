namespace ChessLogic.Figures;

public interface IFigure
{
    public char Color { get; }
    public FigureType Type { get; }
    public  bool PossibleMove( ref IFigure[][] board,(int,int) moveStartPosition, (int,int) moveEndPosition);
}