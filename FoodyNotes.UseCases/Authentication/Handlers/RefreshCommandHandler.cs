using System.Threading;
using System.Threading.Tasks;
using FoodyNotes.UseCases.Authentication.Commands;
using MediatR;

namespace FoodyNotes.UseCases.Authentication.Handlers
{
  public class RefreshCommandHandler : IRequestHandler<RefreshCommand, string>
  {

    public Task<string> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
      throw new System.NotImplementedException();
    }
  }
}