using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingnalrTest.Models
{
    public class ClientConnection
    {
        public ClientConnection()
        {
            Id = Guid.NewGuid().ToString();
        }

        public ClientConnection(string userName, int eventId) : this()
        {
            UserName = userName;
            EventId = eventId;
            Connections = new List<Connection>();
            CreatedOn = DateTime.Now;
        }

        public string Id { get; set; }

        public string UserName { get; set; }

        public IList<Connection> Connections { get; set; }

        public int EventId { get; set; }

        public DateTime CreatedOn { get; set; }

        public void Disconnect(string connectionId)
        {
            var connection = Connections.FirstOrDefault(c => c.Id.Equals(connectionId));

            Connections.Remove(connection);
        }
    }

    public class Connection
    {
        public string  Id { get; set; }

        public string UserAgent { get; set; }

        public bool Connected { get; set; }
    }
}
