namespace ChessLogic.Figures;

public interface IFigure
{
    public char Color { get; }
    public FigureType Type { get; }
    public  bool PossibleMove( ref IFigure?[][] board,(int,int) moveStartPosition, (int,int) moveEndPosition);
    
    public  bool SquareIsUnderAttack( ref IFigure?[][] board,(int,int) square, char pieceColor);
    protected bool KingIsUnderAttack(IFigure?[][] board, char pieceColor);
    protected (int, int) FindKing(IFigure?[][] board, char kingColor);
}