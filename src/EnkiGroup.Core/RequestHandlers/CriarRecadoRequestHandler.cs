using EnkiGroup.Core.Modelos;
using EnkiGroup.Core.Repositorios;
using EnkiGroup.Shared;
using EnkiGroup.Shared.RequestModels;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EnkiGroup.Core.RequestHandlers
{
    public class CriarRecadoRequestHandler : IRequestHandler<CriarRecadoRequest, OperationResult>
    {
        private readonly IRecadoRepositorio _recados;

        public CriarRecadoRequestHandler(IRecadoRepositorio recados) 
            => _recados = recados;

        public Task<OperationResult> Handle(CriarRecadoRequest request, CancellationToken cancellationToken)
        {
            var recadoPai = _recados.ObterRecadoParaAgrupamento(request.Remetente, request.Destinatario, request.Assunto);

            var recado = new Recado(request.Remetente, request.Destinatario, request.Assunto, request.Mensagem, recadoPai?.AgrupadoComId ?? recadoPai?.Id);

            _recados.Adicionar(recado);

            return OperationResult.Success().AsTask;
        }
    }
}
