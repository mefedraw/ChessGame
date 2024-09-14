﻿namespace ChessLogic.Figures;

public class Knight : Figure
{

    public override bool PossibleMove(ref IFigure?[][] board, (int, int) moveStartPosition, (int, int) moveEndPosition)
    {
        int startX = moveStartPosition.Item1; // Горизонтальная координата (столбец)
        int startY = moveStartPosition.Item2; // Вертикальная координата (строка)
        int endX = moveEndPosition.Item1;     // Горизонтальная координата (столбец)
        int endY = moveEndPosition.Item2;     // Вертикальная координата (строка)

        IFigure? figure = board[startX][startY];

        if (figure == null || figure.Type != FigureType.Knight)
        {
            return false; // Если на начальной позиции нет фигуры или это не конь
        }

        // Варианты возможных движений коня
        int[] dx = { 2, 2, -2, -2, 1, 1, -1, -1 };
        int[] dy = { 1, -1, 1, -1, 2, -2, 2, -2 };

        for (int i = 0; i < 8; i++)
        {
            int newX = startX + dx[i];
            int newY = startY + dy[i];

            if (newX == endX && newY == endY)
            {
                // Если клетка пуста или там фигура противника, ход возможен
                if (board[endX][endY] == null || board[endX][endY].Color != figure.Color)
                {
                    board[startX][startY] = null;
                    board[endX][endY] = figure;
                    if (KingIsUnderAttack(board, figure.Color))
                    {
                        board[startX][startY] = figure;
                        board[endX][endY] = null;
                        return false;
                    }
                    return true;
                }
            }
        }
        
        return false; // Если ни одно из возможных движений коня не подходит
    }
    
    public override List<(int, int)> GetPossibleMoves(ref IFigure?[][] board, (int, int) currentPos)
    {
        List<(int, int)> possibleMoves = new List<(int, int)>();
        int x = currentPos.Item1;
        int y = currentPos.Item2;

        // Варианты хода коня (все возможные "Г"-образные ходы)
        int[][] moves = {
            new[] {x + 2, y + 1}, new[] {x + 2, y - 1},
            new[] {x - 2, y + 1}, new[] {x - 2, y - 1},
            new[] {x + 1, y + 2}, new[] {x + 1, y - 2},
            new[] {x - 1, y + 2}, new[] {x - 1, y - 2}
        };

        foreach (var move in moves)
        {
            if (IsInBounds(move[0], move[1]) && (board[move[0]][move[1]] == null || board[move[0]][move[1]].Color != this.Color))
            {
                possibleMoves.Add((move[0], move[1]));
            }
        }

        return possibleMoves;
    }
    
    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < 8 && y >= 0 && y < 8;
    }
    
    public Knight(char color): base(color,FigureType.Knight)
    {
        Type = FigureType.Knight;
    }
}