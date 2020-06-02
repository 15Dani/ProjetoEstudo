using EnkiGroup.Core.Repositorios;
using EnkiGroup.Core.RequestHandlers;
using EnkiGroup.Shared.RequestModels;
using EnkiGroup.Shared.ViewModels;
using NSubstitute;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EnkiGroup.Core.Tests.RequestHandlers
{
    public class ObterRecadosRequestHandlerTest
    {
        private readonly IRecadoRepositorio _repo;
        private readonly ObterRecadosRequestHandler _sut;

        public ObterRecadosRequestHandlerTest()
        {
            _repo = Substitute.For<IRecadoRepositorio>();
            _sut = new ObterRecadosRequestHandler(_repo);
        }

        [Fact]
        public async Task ObterRecadosDevolveOsRecadosProjetadosEmRecadoViewModel()
        {
            //arrange
            var request = new ObterRecadosRequest();

            //act
            var (sucesso, resultado) = await _sut.Handle(request, CancellationToken.None);

            //assert
            Assert.True(sucesso);
            Assert.IsAssignableFrom<IQueryable<RecadoViewModel>>(resultado);
            _repo.Received().ObterTodosProjetado<RecadoViewModel>();
        }
    }
}
