using Fleck;
using ChessLogic;

namespace ChessGame2;

public class GameSession
{
    public GameSession(WsChessClient player1, WsChessClient player2, Game boardState)
    {
        Player1 = player1;
        Player2 = player2;
        BoardState = boardState;

        var random = new Random();
        if (random.Next(2) == 0)
        {
            Player1.Color = 'w'; // White for Player1
            Player2.Color = 'b'; // Black for Player2
        }
        else
        {
            Player1.Color = 'b'; // Black for Player1
            Player2.Color = 'w'; // White for Player2
        }
    }

    public WsChessClient Player1 { get; set; }
    public WsChessClient Player2 { get; set; }
    public Game BoardState { get; set; }

    public void ApplyMove(string move, WsChessClient player)
    {
        var num = BoardState.CharToCoord(move[1]);
        var letter = BoardState.CharToCoord(move[0]);

        var whitePieceMove = char.IsUpper(
            BoardState.GetFigureSymbol(
                BoardState.Board[num][letter]));
        // we are checking the color of piece on specified cell (upper case means white)

        if ((whitePieceMove && player.Color == 'w') || (!whitePieceMove && player.Color == 'b'))
        {
            var successfulMove = BoardState.DoMove(move);
            if (successfulMove)
            {
                string colorMessage = Player1.Color == 'w' ? "белыми" : "черными";
                
                string turnString = BoardState.WhitesTurn ? "белых" : "черных";
                
                Player1.PlayerConnection.Send($"Сейчас ход {turnString}" + '\n' + GetBoardState() + '\n' +
                                              $"Вы играете {colorMessage} фигурами");

                colorMessage = Player2.Color == 'w' ? "белыми" : "черными";
                Player2.PlayerConnection.Send($"Сейчас ход {turnString}" + '\n' + GetBoardState() + '\n' +
                                              $"Вы играете {colorMessage} фигурами");
            }
        }
    }

    private string GetBoardState()
    {
        return BoardState.GetBoardAsFEN();
    }
}