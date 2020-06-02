using EnkiGroup.Shared.ViewModels;
using MediatR;
using System.Linq;

namespace EnkiGroup.Shared.RequestModels
{
    public class ObterRecadosRequest : IRequest<OperationResult<IQueryable<RecadoViewModel>>>
    {

    }
}
