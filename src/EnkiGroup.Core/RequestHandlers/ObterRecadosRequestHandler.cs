using EnkiGroup.Core.Repositorios;
using EnkiGroup.Shared;
using EnkiGroup.Shared.RequestModels;
using EnkiGroup.Shared.ViewModels;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EnkiGroup.Core.RequestHandlers
{
    public class ObterRecadosRequestHandler : IRequestHandler<ObterRecadosRequest, OperationResult<IQueryable<RecadoViewModel>>>
    {
        private readonly IRecadoRepositorio _recados;

        public ObterRecadosRequestHandler(IRecadoRepositorio recados)
            => _recados = recados;

        public Task<OperationResult<IQueryable<RecadoViewModel>>> Handle(ObterRecadosRequest request, CancellationToken cancellationToken) 
            => OperationResult.Success(_recados.ObterTodosProjetado<RecadoViewModel>()).AsTask;
    }
}
