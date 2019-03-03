using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChuckNorrisJokesLive
{
    public class ChuckNorrisJokesService : BackgroundService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceScopeFactory _scopeFactory;

        public ChuckNorrisJokesService(IHttpClientFactory httpClientFactory, IServiceScopeFactory scopeFactory)
        {
            _httpClientFactory = httpClientFactory;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var joke = await GetJoke();
                var sendJokeCommand = new SendJokeCommand { Joke = joke };
                using (var scope = _scopeFactory.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetService<IMediator>();
                    await mediator.Send(sendJokeCommand, stoppingToken);
                }
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private async Task<string> GetJoke()
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var result = await httpClient.GetStringAsync("https://api.chucknorris.io/jokes/random");
                return result;
            }
        }
    }
}
