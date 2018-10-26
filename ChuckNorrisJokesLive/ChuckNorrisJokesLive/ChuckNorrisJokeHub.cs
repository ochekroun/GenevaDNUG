using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ChuckNorrisJokesLive
{
    public interface IChuckNorrisJokeHub
    {
        Task ReceiveJoke(string joke);
    }

    public class ChuckNorrisJokeHub : Hub<IChuckNorrisJokeHub>
    {
        public async Task SendJoke(string joke)
        {
            await Clients.All.ReceiveJoke(joke);
        }
    }
}