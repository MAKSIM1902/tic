using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _5
{
    public class FindHub : Hub
    {
        public ICollection<string> GetAllGames()
        {
            List<string> s = new List<string>();
            foreach (var e in GameHub.Games)
            {
                if (e.Value.visible)
                {
                    s.Add(e.Key);
                }
            }
            return s;
        }
    }
}
