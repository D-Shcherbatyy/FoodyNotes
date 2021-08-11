using System.Threading;
using System.Threading.Tasks;
using FoodyNotes.UseCases.Authentication.Commands;
using MediatR;

namespace FoodyNotes.UseCases.Authentication.Handlers
{
  public class RevokeCommandHandler : IRequestHandler<RevokeCommand>
  {

    public Task<Unit> Handle(RevokeCommand request, CancellationToken cancellationToken)
    {
      throw new System.NotImplementedException();
    }
  }
}