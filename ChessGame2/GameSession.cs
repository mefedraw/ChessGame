using Fleck;
using ChessLogic;

namespace ChessGame2;

public class GameSession
{
    public GameSession(WsChessClient player1, WsChessClient player2, Game boardState, bool botGame)
    {
        Player1 = player1;
        Player2 = player2;
        BoardState = boardState;
        BotGame = botGame;

        var random = new Random();
        if (random.Next(2) == 0)
        {
            Player1.Color = 'w';
            Player2.Color = 'b';
        }
        else
        {
            Player1.Color = 'b';
            Player2.Color = 'w';
        }
    }

    public GameSession(WsChessClient player1, WsChessClient player2, Game boardState)
    {
        // для тестов 
        Player1 = player1;
        Player2 = player2;
        BoardState = boardState;
        BotGame = false;

        Player1.Color = 'w';
        Player2.Color = 'b';
    }

    public GameSession(WsChessClient player1, Game boardState, bool botGame)
    {
        Player1 = player1;
        Player2 = new WsChessClient();
        BoardState = boardState;
        BotGame = botGame;

        Player1.Color = 'w';
        Player2.Color = 'b';
    }

    public WsChessClient Player1 { get; set; }
    public WsChessClient Player2 { get; set; }
    public Game BoardState { get; set; }

    public bool BotGame { get; set; }

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

                Player1.PlayerConnection.Send($"LOGS:Сейчас ход {turnString}" + '\n' +
                                              $"Вы играете {colorMessage} фигурами");

                Player1.PlayerConnection.Send($"FEN:{GetBoardState()}:{Player1.Color}");
                if (!BotGame)
                {
                    colorMessage = Player2.Color == 'w' ? "белыми" : "черными";
                    Player2.PlayerConnection.Send($"LOGS:Сейчас ход {turnString}" + '\n' +
                                                  $"Вы играете {colorMessage} фигурами");

                    Player2.PlayerConnection.Send($"FEN:{GetBoardState()}:{Player2.Color}");
                }
            }
        }
    }

    public void ApplyBotMove(string move) // игрок всегда player1
    {
        var num = BoardState.CharToCoord(move[1]);
        var letter = BoardState.CharToCoord(move[0]);

        var whitePieceMove = char.IsUpper(
            BoardState.GetFigureSymbol(
                BoardState.Board[num][letter]));

        var successfulMove = BoardState.DoMove(move);
        if (successfulMove)
        { 
            Player1.PlayerConnection.Send($"FEN:{GetBoardState()}:{Player1.Color}");
        }
    }

    private string GetBoardState() 
    {
        return BoardState.GetBoardAsFEN();
    }
}