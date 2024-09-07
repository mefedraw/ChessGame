namespace ChessLogic.Figures;

public class King : Figure
{
    public override bool PossibleMove(ref IFigure[][] board, (int, int) moveStartPosition, (int, int) moveEndPosition)
    {
        int startX = moveStartPosition.Item1; // Горизонтальная координата (столбец)
        int startY = moveStartPosition.Item2; // Вертикальная координата (строка)
        int endX = moveEndPosition.Item1;     // Горизонтальная координата (столбец)
        int endY = moveEndPosition.Item2;     // Вертикальная координата (строка)

        IFigure figure = board[startX][startY];

        if (figure == null || figure.Type != FigureType.King)
        {
            return false; // Если на начальной позиции нет фигуры или это не король
        }

        // Король может двигаться на одну клетку в любом направлении
        int deltaX = Math.Abs(endX - startX);
        int deltaY = Math.Abs(endY - startY);

        if ((deltaX <= 1 && deltaY <= 1) && !(deltaX == 0 && deltaY == 0)) // Движение на 1 клетку по горизонтали, вертикали или диагонали
        {
            // Если конечная клетка пуста или там фигура противника
            if (board[endX][endY] == null || board[endX][endY].Color != figure.Color)
            {
                board[startX][startY] = null;
                board[endX][endY] = figure;
                return true;
            }
        }

        return false; // Любое другое движение недопустимо для короля
    }


    public King(char color) : base(color, FigureType.King)
    {
    }
}