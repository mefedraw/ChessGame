namespace ChessLogic.Figures;

public class King : Figure
{
    public override bool PossibleMove(ref IFigure[][] board, (int, int) moveStartPosition, (int, int) moveEndPosition)
    {
        int startX = moveStartPosition.Item1; // Горизонтальная координата (столбец)
        int startY = moveStartPosition.Item2; // Вертикальная координата (строка)
        int endX = moveEndPosition.Item1; // Горизонтальная координата (столбец)
        int endY = moveEndPosition.Item2; // Вертикальная координата (строка)

        IFigure figure = board[startX][startY];

        if (figure == null || figure.Type != FigureType.King)
        {
            return false; // Если на начальной позиции нет фигуры или это не король
        }

        // Король может двигаться на одну клетку в любом направлении
        int deltaX = Math.Abs(endX - startX);
        int deltaY = Math.Abs(endY - startY);


        // Рокировка (O-O) белых
        if (startX == 0 && startY == 4 && endX == 0 && endY == 6 && !KingDidMove) // Короткая рокировка
        {
            // Проверяем, что ладья находится на позиции (0, 7), и она не двигалась
            IFigure rook = board[0][7];

            // Приводим к типу Rook, чтобы получить доступ к RookDidMove
            if (rook is Rook castedRook && !castedRook.RookDidMove)
            {
                // Проверяем, что клетки между королем и ладьей свободны
                if (board[0][5] == null && board[0][6] == null)
                {
                    // Делаем рокировку: перемещаем короля и ладью
                    board[0][4] = null; // Король покидает исходную позицию
                    board[0][6] = figure; // Король перемещается на новую позицию
                    board[0][7] = null; // Ладья покидает исходную позицию
                    board[0][5] = castedRook; // Ладья становится на новое место

                    KingDidMove = true; // Обновляем флаг движения короля
                    castedRook.RookDidMove = true; // Обновляем флаг движения ладьи

                    return true;
                }
            }
        }

        // Длинная рокировка (O-O-O) белых
        if (endX == 0 && (endY == 2 || endY == 1) && startX == 0 && startY == 4 &&
            !KingDidMove) // Длинная рокировка
        {
            // Проверяем, что ладья находится на позиции (0, 0), и она не двигалась
            IFigure rook = board[0][0];

            // Приводим к типу Rook, чтобы получить доступ к RookDidMove
            if (rook is Rook castedRook && !castedRook.RookDidMove)
            {
                // Проверяем, что клетки между королем и ладьей свободны
                if (board[0][1] == null && board[0][2] == null && board[0][3] == null)
                {
                    // Делаем рокировку: перемещаем короля и ладью
                    board[0][4] = null; // Король покидает исходную позицию
                    board[0][2] = figure; // Король перемещается на новую позицию
                    board[0][0] = null; // Ладья покидает исходную позицию
                    board[0][3] = castedRook; // Ладья становится на новое место

                    KingDidMove = true; // Обновляем флаг движения короля
                    castedRook.RookDidMove = true; // Обновляем флаг движения ладьи

                    return true;
                }
            }
        }

        // Рокировка (o-o) черных
        if (startX == 7 && startY == 4 && endX == 7 && endY == 6 && !KingDidMove) // Короткая рокировка
        {
            // Проверяем, что ладья находится на позиции (0, 7), и она не двигалась
            IFigure rook = board[7][7];
            // Приводим к типу Rook, чтобы получить доступ к RookDidMove
            if (rook is Rook castedRook && !castedRook.RookDidMove)
            {
                // Проверяем, что клетки между королем и ладьей свободны
                if (board[7][5] == null && board[7][6] == null)
                {
                    // Делаем рокировку: перемещаем короля и ладью
                    board[7][4] = null; // Король покидает исходную позицию
                    board[7][6] = figure; // Король перемещается на новую позицию
                    board[7][7] = null; // Ладья покидает исходную позицию
                    board[7][5] = castedRook; // Ладья становится на новое место

                    KingDidMove = true; // Обновляем флаг движения короля
                    castedRook.RookDidMove = true; // Обновляем флаг движения ладьи
                    return true;
                }
            }
        }

        // Длинная рокировка (o-o-o) черных
        if (endX == 7 && (endY == 2 || endY == 1) && startX == 7 && startY == 4 &&
            !KingDidMove) // Длинная рокировка
        {
            // Проверяем, что ладья находится на позиции (0, 0), и она не двигалась
            IFigure rook = board[7][0];

            // Приводим к типу Rook, чтобы получить доступ к RookDidMove
            if (rook is Rook castedRook && !castedRook.RookDidMove)
            {
                // Проверяем, что клетки между королем и ладьей свободны
                if (board[7][1] == null && board[7][2] == null && board[7][3] == null)
                {
                    // Делаем рокировку: перемещаем короля и ладью
                    board[7][4] = null; // Король покидает исходную позицию
                    board[7][2] = figure; // Король перемещается на новую позицию
                    board[7][0] = null; // Ладья покидает исходную позицию
                    board[7][3] = castedRook; // Ладья становится на новое место

                    KingDidMove = true; // Обновляем флаг движения короля
                    castedRook.RookDidMove = true; // Обновляем флаг движения ладьи

                    return true;
                }
            }
        }

        if ((deltaX <= 1 && deltaY <= 1) &&
            !(deltaX == 0 && deltaY == 0)) // Движение на 1 клетку по горизонтали, вертикали или диагонали
        {
            // Если конечная клетка пуста или там фигура противника
            if (board[endX][endY] == null || board[endX][endY].Color != figure.Color)
            {
                // проверка то что поле на которое ходит король не находится под ударом вражеских фигур
                if (!IsUnderAttack(board, moveEndPosition, figure.Color))
                {
                    board[startX][startY] = null;
                    board[endX][endY] = figure;
                    KingDidMove = true;
                    return true;   
                }
            }
        }

        return false; // Любое другое движение недопустимо для короля
    }

    // Метод проверки шаха на указанной позиции
    public override bool IsUnderAttack(IFigure[][] board, (int x, int y) position, char kingColor)
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

    public override bool IsUnderAttack(IFigure[][] board, char kingColor)
    {
        throw new NotImplementedException();
    }


    public bool KingDidMove { get; set; }
    
    public King(char color) : base(color, FigureType.King)
    {
    }
}