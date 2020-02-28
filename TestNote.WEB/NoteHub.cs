using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace TestNote.WEB.Hubs
{
    public class NoteHub : Hub
    {
        public async Task SendNotification(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}