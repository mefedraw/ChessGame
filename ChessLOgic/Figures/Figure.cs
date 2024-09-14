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
    

    public abstract List<(int, int)> GetPossibleMoves(ref IFigure?[][] board, (int, int) currentPos);
    
    public virtual bool SquareIsUnderAttack(ref IFigure?[][] board, (int, int) square, char pieceColor)
    {
        for (var column = 0; column < 8; column++)
        {
            for (var row = 0; row < 8; row++)
            {
                var figure = board[column][row];
                // Если фигура противника
                if (figure != null && figure.Color != pieceColor)
                {
                    // Проверяем, может ли фигура атаковать клетку
                    var tempFigure = board[square.Item1][square.Item2];
                    if (figure.PossibleMove(ref board, (column, row), square))
                    {
                        figure.PossibleMove(ref board, square, (column, row));
                        if (tempFigure != null)
                        {
                            board[square.Item1][square.Item2] = tempFigure;
                        }
                        return true; // Клетка под ударом
                    }
                }
            }
        }

        return false;
    }
    
    public bool IsCheckmate(ref IFigure?[][] board, char color)
    {
        // Шаг 1: Найти короля данного цвета
        (int kingX, int kingY) KingPos = FindKing(board,color);

        // Шаг 2: Проверить, под шахом ли король
        if (!KingIsUnderAttack(board, KingPos, color))
        {
            return false; // Если король не под шахом, мата нет
        }

        // Шаг 3: Проверить, может ли король сделать ход, чтобы выйти из шаха
        var kingMoves = GetPossibleMoves(ref board, KingPos);
        foreach (var move in kingMoves)
        {
            if (!SquareIsUnderAttack(ref board, move,color))
            {
                return false; // Если король может уйти из-под шаха, мата нет
            }
        }

        // Шаг 4: Проверить, могут ли другие фигуры защитить короля
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                var figure = board[x][y];
                if (figure != null && figure.Color == color)
                {
                    var possibleMoves = figure.GetPossibleMoves(ref board, (x, y));
                    foreach (var move in possibleMoves)
                    {
                        // Делаем временный ход для проверки
                        var tempBoard = (IFigure?[][])board.Clone();
                        tempBoard[move.Item1][move.Item2] = figure;
                        tempBoard[x][y] = null;

                        // Если после этого хода король больше не под шахом, мата нет
                        if (!KingIsUnderAttack(board, KingPos, color))
                        {
                            return false;
                        }
                    }
                }
            }
        }

        // Если никакой ход не спасает короля от шаха, это мат
        return true;
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