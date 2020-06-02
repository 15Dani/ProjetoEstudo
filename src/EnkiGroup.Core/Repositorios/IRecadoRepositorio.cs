using EnkiGroup.Core.Modelos;

namespace EnkiGroup.Core.Repositorios
{
    public interface IRecadoRepositorio : IRepositorio<Recado>
    {
        Recado ObterRecadoParaAgrupamento(string rementente, string destinatario, string assunto);
    }
}
