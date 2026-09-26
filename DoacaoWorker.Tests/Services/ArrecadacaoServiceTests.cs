using DoacaoWorker.Application.Services;
using DoacaoWorker.Domain.Interfaces.Repositories;
using Moq;

namespace DoacaoWorker.Tests.Services
{
    public class ArrecadacaoServiceTests
    {
        private readonly Mock<ICampanhaRepository> _repo = new();
        private readonly ArrecadacaoService _service;

        public ArrecadacaoServiceTests()
        {
            _service = new ArrecadacaoService(_repo.Object);
        }

        [Fact]
        public async Task AtualizarValorArrecadado_CampanhaExiste_RecalculaERetornaTrue()
        {
            _repo.Setup(r => r.RecalcularValorArrecadado(7)).ReturnsAsync(true);

            var resultado = await _service.AtualizarValorArrecadado(7);

            Assert.True(resultado);
            _repo.Verify(r => r.RecalcularValorArrecadado(7), Times.Once);
        }

        [Fact]
        public async Task AtualizarValorArrecadado_CampanhaInexistente_RetornaFalse()
        {
            _repo.Setup(r => r.RecalcularValorArrecadado(99)).ReturnsAsync(false);

            var resultado = await _service.AtualizarValorArrecadado(99);

            Assert.False(resultado);
        }
    }
}
