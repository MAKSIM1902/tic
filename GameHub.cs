using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System;
using _5.Models;
using System.Collections.Generic;

namespace _5
{
    public class GameHub : Hub
    {
        public static ConcurrentDictionary<string, Game> Games = new ConcurrentDictionary<string, Game>();
        public static ConcurrentDictionary<string, string> IdAndGames = new ConcurrentDictionary<string, string>();
     
        public string GetConnectionId => Context.ConnectionId;

        public void CreateGame()
        {
            Games[Context.ConnectionId] = new Game(Context.ConnectionId);
            IdAndGames[Context.ConnectionId] = Context.ConnectionId;
        }

        public async Task ConnectUser(string id)
        {
            IdAndGames[Context.ConnectionId] = id;
            await Clients.Client(id).SendAsync("ConnectUser", Context.ConnectionId);
            Games[id].AddId(Context.ConnectionId);
            if (Games[id].firstMoveId == 0)
            {
                await Clients.Client(id).SendAsync("GetNumber", 0);
                await Clients.Client(Context.ConnectionId).SendAsync("GetNumber", 1);
            }
            else
            {
                await Clients.Client(id).SendAsync("GetNumber", 1);
                await Clients.Client(Context.ConnectionId).SendAsync("GetNumber", 0);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            foreach(var k in Games[IdAndGames[Context.ConnectionId]].id)
            {
                await Clients.Client(k).SendAsync("OpponentDisconect");
            }

            Game res;
            Games.TryRemove(IdAndGames[Context.ConnectionId], out res);
            string s;
            IdAndGames.TryRemove(Context.ConnectionId, out s);
            await base.OnDisconnectedAsync(exception);
        }

        public bool Move(int i, int j, string id, int number)
        {
            if(Games[IdAndGames[Context.ConnectionId]].Move(i, j, number))
            {
                Clients.Client(id).SendAsync("Move", i, j, number);
                int endGame = Games[IdAndGames[Context.ConnectionId]].WhoIsWin();
                if (endGame != -1)
                {
                    if(endGame == 1)
                    {
                        Clients.Client(id).SendAsync("EndGame", -1);
                        Clients.Client(Context.ConnectionId).SendAsync("EndGame", 1);
                    }
                    else
                    {
                        Clients.Client(id).SendAsync("EndGame", 0);
                        Clients.Client(Context.ConnectionId).SendAsync("EndGame", 0);
                    }

                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
