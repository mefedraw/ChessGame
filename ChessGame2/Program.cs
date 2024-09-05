using System.Collections.Immutable;
using ChessGame2;
using Fleck;
using ChessLogic;

var server = new WebSocketServer("ws://0.0.0.0:8181");

var wsConnections = new List<IWebSocketConnection>();

var wsConnectionsQueue = new Dictionary<string, IWebSocketConnection>();

var games = new Dictionary<string, GameSession>();

server.Start(ws =>
{
    ws.OnOpen = () => { wsConnections.Add(ws); };

    ws.OnMessage = message =>
    {
        if (message.Contains("challenge"))
        {
            var gameId = message[10..];
            if (!wsConnectionsQueue.ContainsKey(gameId)) // no session with such ID yet
            {
                wsConnectionsQueue[gameId] = ws; // player1 init game
            }
            else // player2 accepts invitation by providing same ID
            {
                games[gameId] = new GameSession(wsConnectionsQueue[gameId], ws, new Game());
                wsConnectionsQueue[gameId].Send($"Партия {gameId} началась!");
                ws.Send($"Партия {gameId} началась!");
            }
        }
        else if (message.Contains("resign"))
        {
            var gameId = message[7..];
            wsConnectionsQueue.Remove(gameId);
            games[gameId].Player1.Send($"Партия {gameId} завершилась так как пользователь сдался :(");
            games[gameId].Player2.Send($"Партия {gameId} завершилась так как пользователь сдался :(");
            games.Remove(gameId);
        }
        else // moves handler
        {
            var parts = message.Split(':');
            var gameId = parts[0];
            var currentMove = parts[1];
            var currentSession = games[gameId];
            currentSession.ApplyMove(currentMove);
        }
    };
});

WebApplication.CreateBuilder(args).Build().Run();