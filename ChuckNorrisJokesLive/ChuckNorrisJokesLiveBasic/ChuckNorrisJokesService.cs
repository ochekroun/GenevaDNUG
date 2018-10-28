using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace ChuckNorrisJokesLiveBasic
{
    public class ChuckNorrisJokesService : BackgroundService
    {
        private readonly IHubContext<ChuckNorrisJokesHub> _hubContext;

        public ChuckNorrisJokesService(IHubContext<ChuckNorrisJokesHub> hubContext)
        {
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var joke = await GetJoke();
                await _hubContext.Clients.All.SendAsync("ReceiveJoke", joke, stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private async Task<string> GetJoke()
        {
            using (var httpClient = new HttpClient())
            {
                var result = await httpClient.GetStringAsync("https://api.chucknorris.io/jokes/random");
                return result;
            }
        }
    }
}
