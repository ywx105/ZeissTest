using DataDomain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDomain.Repository
{
    public interface IMachinesReponseRepository
    {
        Task<IEnumerable<MachinesPayLoad>> GetMachinesReponsesAsync();

        Task<bool> SaveMachinesReponsesAsync(string payload);
    }

    public class MachinesReponseRepository : IMachinesReponseRepository
    {
        private ZeissDbContext _context;
        private readonly ILogger<MachinesReponseRepository> _logger;
        public MachinesReponseRepository(ZeissDbContext context, ILogger<MachinesReponseRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<MachinesPayLoad>> GetMachinesReponsesAsync()
        {
            var result = await _context.MachinesReponse.ToListAsync();

            return result;
        }

        public async Task<bool> SaveMachinesReponsesAsync(string payload)
        {
            try
            {
                var newEntity = new MachinesPayLoad
                {
                    Payload = payload
                };

                await _context.MachinesReponse.AddAsync(newEntity);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"RepositoryError - {ex.Message}");
            }

            return false;
        }
    }
}
