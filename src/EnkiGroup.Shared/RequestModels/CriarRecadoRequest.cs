using MediatR;

namespace EnkiGroup.Shared.RequestModels
{
    public class CriarRecadoRequest : IRequest<OperationResult>, IValidatable
    {
        public string Remetente  { get; set; }
        public string Destinatario { get; set; }
        public string Assunto { get; set; }
        public string Mensagem { get; set; }
    }
}
