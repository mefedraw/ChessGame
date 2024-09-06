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
        
        Random random = new Random();
        if (random.Next(2) == 0)
        {
            Player1Color = 'w'; // White for Player1
            Player2Color = 'b'; // Black for Player2
        }
        else
        {
            Player1Color = 'b'; // Black for Player1
            Player2Color = 'w'; // White for Player2
        }
    }
    
    public IWebSocketConnection Player1 { get; }
    public IWebSocketConnection Player2 { get; }
    private Game BoardState { get; set; }
    
    public char Player1Color { get; set; }
    
    public char Player2Color { get; set; }
    
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
        return BoardState.GetBoardAsFEN();
    }
}