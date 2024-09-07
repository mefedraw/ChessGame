using Fleck;

namespace ChessGame2;

public class WsChessClient
{
    
    public WsChessClient(IWebSocketConnection playerConnection)
    {
        PlayerConnection = playerConnection;
    }

    public IWebSocketConnection PlayerConnection { set; get; }
    public char Color { get; set; }
}