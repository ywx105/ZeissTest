using DataDomain;
using DataDomain.Entity;
using Services;
using System.Net.WebSockets;
using System.Text;

namespace WebAPI.WebSocketJobs
{
    public class WebsocketService : BackgroundService
    {
        private readonly ILogger<WebsocketService> _logger;
        private readonly IConfiguration _configuration;

        private readonly IServiceScopeFactory _scopeFactory;

        public WebsocketService(ILogger<WebsocketService> logger
            , IConfiguration configuration
            , IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var webSocketConnection = _configuration["WebSocketServer"];
            while (!stoppingToken.IsCancellationRequested)
                using (var socket = new ClientWebSocket())
                    try
                    {
                        await socket.ConnectAsync(new Uri(webSocketConnection), stoppingToken);

                        await Receive(socket, stoppingToken);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"WebSocketError - {ex.Message}");
                    }
        }

        private async Task Receive(ClientWebSocket socket, CancellationToken stoppingToken)
        {
            var buffer = new ArraySegment<byte>(new byte[2048]);
            while (!stoppingToken.IsCancellationRequested)
            {
                WebSocketReceiveResult result;
                using (var ms = new MemoryStream())
                {
                    do
                    {
                        result = await socket.ReceiveAsync(buffer, stoppingToken);
                        ms.Write(buffer.Array, buffer.Offset, result.Count);
                    } while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Close)
                        break;

                    ms.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(ms, Encoding.UTF8))
                    {
                        var resultContext = await reader.ReadToEndAsync();
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var db = scope.ServiceProvider.GetRequiredService<ZeissDbContext>();

                            var machinePayload = new MachinesPayLoad
                            {
                                Payload = resultContext
                            };
                            await db.MachinesReponse.AddAsync(machinePayload);
                            await db.SaveChangesAsync();
                        }
                        
                        _logger.LogInformation($"Response - {resultContext}");
                    }
                }
            };
        }
    }
}
