using DataDomain.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using Services;

namespace ZeissUT
{
    public class MachinesPayloadServiceTests
    {
        private readonly Mock<IMachinesReponseRepository> _machinesReponseRepositoryMock;
        private readonly Mock<ILogger<MachinesPayloadService>> _loggerMock;

        public MachinesPayloadServiceTests()
        {
            _machinesReponseRepositoryMock = new Mock<IMachinesReponseRepository>();
            _loggerMock = new Mock<ILogger<MachinesPayloadService>>();
        }

        public MachinesPayloadService CreateService()
        {
            var service = new MachinesPayloadService(_machinesReponseRepositoryMock.Object, _loggerMock.Object);

            return service;
        }

        [Fact]
        public async Task TestMachinesSavePayload_WhenException_ReturnFalse()
        {
            // Arrange
            var payload = "{\"topic\":\"events\",\"ref\":null,\"payload\":{\"timestamp\":\"2022 - 05 - 18T13: 59:32Z\",\"status\":\"running\",\"machine_id\":\"555277c8 - 7b91 - 4275 - 9fb2 - 04735e8a88c6\",\"id\":\"11bc5bd0 - 7033 - 4fe8 - a499 - 2fce0772cf51\"},\"join_ref\":null,\"event\":\"new\"}";
            var service = CreateService();

            // Act
            _machinesReponseRepositoryMock.Setup(m => m.SaveMachinesReponsesAsync(It.IsAny<string>())).ThrowsAsync(new Exception("save failed."));

            var result = await service.SaveMachinesPayLoadByWebSocketAsync(payload);

            // Assert
            Assert.True(!result);
        }
    }
}