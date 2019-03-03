using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ChuckNorrisJokesLive
{
    public interface IChuckNorrisJokesHub
    {
        Task ReceiveJoke(string joke);
    }


    public class ChuckNorrisJokesHub : Hub<IChuckNorrisJokesHub>
    {
        public async Task SendJoke(string joke)
        {
            await Clients.All.ReceiveJoke(joke);
        }
    }
}