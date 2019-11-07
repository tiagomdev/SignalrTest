using Microsoft.AspNetCore.SignalR;
using SingnalrTest.Hubs;
using SingnalrTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingnalrTest
{
    public static class SignalrBackground
    {
        static RedisDB<ClientConnection> redisDB = new RedisDB<ClientConnection>();

        public static IHubCallerClients Clients;

        public static async Task Notify(int eventId)
        {
            if (Clients != null)
            {
                try
                {
                    var clients = redisDB.GetAll();


                    var userConnections = clients.Where(c => c.EventId == eventId).Select(c => c.Connections.Select(i => i.Id)).ToList();

                    foreach (var connections in userConnections)
                    {
                        foreach (var connectionId in connections)
                        {
                            await Clients.Client(connectionId).SendAsync("ReceiveMessage", "Server", "notify");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
