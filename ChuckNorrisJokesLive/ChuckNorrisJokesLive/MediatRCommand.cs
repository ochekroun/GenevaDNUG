using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ChuckNorrisJokesLive
{
    public class SendJokeCommand : IRequest<Unit>
    {
        public string Joke { get; set; }
    }

    public class MediatRCommand : IRequestHandler<SendJokeCommand, Unit>
    {
        private readonly IHubContext<ChuckNorrisJokeHub, IChuckNorrisJokeHub> _hubContext;

        public MediatRCommand(IHubContext<ChuckNorrisJokeHub, IChuckNorrisJokeHub> hubContext)
        {
            _hubContext = hubContext;
        }


        public async Task<Unit> Handle(SendJokeCommand request, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All.ReceiveJoke(request.Joke);
            return Unit.Value;
        }
    }
}