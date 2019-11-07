using Microsoft.AspNetCore.SignalR;
using ServiceStack.Redis;
using SingnalrTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingnalrTest.Hubs
{
    public class ChatHub : Hub
    {
        public ChatHub()
        {
        }

        private RedisDB<ClientConnection> redisDB = new RedisDB<ClientConnection>();

        public async Task SendMessage(string userName, string message)
        {
            //await Clients.All.SendAsync("ReceiveMessage", user, message);

            //await Clients.Caller.SendAsync("ReceiveMessage", user, message);

            SignalrBackground.Clients = Clients;

            int eventId = 0;
            try
            {
                eventId = int.Parse(message);
            }
            catch (Exception)
            {
                return;
            }

            if(userName.Equals("server"))
            {
                await SignalrBackground.Notify(eventId);
            }
            else
            {
                var clientConnection = new ClientConnection();

                var connectionId = GetConnectionId();

                //redisDB.ClearAll();

                var clients = redisDB.GetAll() ?? new List<ClientConnection>();

                var user = clients.FirstOrDefault(c => c.UserName == userName);

                if (user != null)
                {
                    bool changed = false;

                    var connection = user.Connections.FirstOrDefault(c => c.Id == connectionId);

                    if (connection == null)
                    {
                        connection = new Connection() { Id = connectionId, Connected = true };

                        user.Connections.Add(connection);

                        changed = true;
                    }

                    if (user.EventId != eventId)
                    {
                        user.EventId = eventId;

                        changed = true;
                    }

                    if (changed) redisDB.Save(user);
                }
                else
                {
                    user = new ClientConnection(userName, int.Parse(message));

                    var connection = new Connection() { Id = connectionId, Connected = true };

                    user.Connections.Add(connection);

                    redisDB.Save(user);
                }

                await Clients.Caller.SendAsync("ReceiveMessage", "Server", "ok");

                //await Clients.Client(connectionId).SendAsync("ReceiveMessage", "Server", "Hello!");
            }
        }

        public override Task OnConnectedAsync()
        {

            var user = Context.User.Identity;

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception ex)
        {
            var connectionId = GetConnectionId();

            var clients = redisDB.GetAll() ?? new List<ClientConnection>();

            var user = clients.Where(c => c.Connections.Any(co => co.Id.Equals(connectionId))).FirstOrDefault();
            if(user != null)
            {
                user.Disconnect(connectionId);

                redisDB.Save(user);
            }

            return base.OnDisconnectedAsync(ex);
        }

        string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        async Task Test()
        {
            await Task.Delay(3000);

            await Clients.Others.SendAsync("ReceiveMessage", "Tiago", "sua resposta apos 3 segundos");
        }
    }
}
