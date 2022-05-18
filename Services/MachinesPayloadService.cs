using Contracts;
using DataDomain.Repository;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json;

namespace Services
{
    public interface IMachinesPayloadService
    {
        Task<bool> SaveMachinesPayLoadByWebSocketAsync(string payload);

        Task<IEnumerable<MachineOutput>> GetMachinesPayLoadAsync();
    }

    public class MachinesPayloadService : IMachinesPayloadService
    {
        private readonly IMachinesReponseRepository _machinesReponseRepository;
        private readonly ILogger<MachinesPayloadService> _logger;

        public MachinesPayloadService(IMachinesReponseRepository machinesReponseRepository
            , ILogger<MachinesPayloadService> logger)
        {
            _machinesReponseRepository = machinesReponseRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<MachineOutput>> GetMachinesPayLoadAsync()
        {
            var data = await _machinesReponseRepository.GetMachinesReponsesAsync();

            var result = new List<MachineOutput>();

            foreach (var item in data)
            {
                var resultItem = JsonConvert.DeserializeObject<MachineOutput>(item.Payload);
                if (resultItem != null)
                {
                    result.Add(resultItem);
                }
            }

            return result;
        }

        public async Task<bool> SaveMachinesPayLoadByWebSocketAsync(string payload)
        {
            try
            {
                var result = await _machinesReponseRepository.SaveMachinesReponsesAsync(payload);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ServiceError - {ex.Message}");

                return false;
            }
            

            
        }
    }
}