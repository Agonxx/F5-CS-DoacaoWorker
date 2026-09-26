using Shared.Contracts.Events;

namespace DoacaoWorker.Tests.Events
{
    public class DoacaoRecebidaEventTests
    {
        [Fact]
        public void DoacaoRecebidaEvent_DeveManterNomeCompletoDoContrato()
        {
            // O MassTransit roteia pelo nome completo do tipo: publisher (CampanhasApi) e consumer precisam bater
            Assert.Equal("Shared.Contracts.Events.DoacaoRecebidaEvent", typeof(DoacaoRecebidaEvent).FullName);
        }
    }
}
