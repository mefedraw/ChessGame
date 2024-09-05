using Fleck;
using ChessLogic;

namespace ChessGame2;

public class GameSession
{
    public GameSession(IWebSocketConnection player1, IWebSocketConnection player2, Game boardState)
    {
        Player1 = player1;
        Player2 = player2;
        BoardState = boardState;
    }
    
    public IWebSocketConnection Player1 { get; }
    public IWebSocketConnection Player2 { get; }
    private Game BoardState { get; set; }
    
    public void ApplyMove(string move)
    {
        var successfulMove = BoardState.DoMove(move);
        if (successfulMove)
        {
            Player1.Send(GetBoardState());
            Player2.Send(GetBoardState());
        }
    }
    
    private string GetBoardState()
    {
        return BoardState.GetBoardAsString();
    }
}