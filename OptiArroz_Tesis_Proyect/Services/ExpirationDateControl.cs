using Microsoft.AspNetCore.SignalR;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Services
{
    public class ExpirationDateControl : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ExpirationDateControl> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(5); // Cada 5 minutos

        public ExpirationDateControl(
            IServiceScopeFactory scopeFactory,
            ILogger<ExpirationDateControl> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        //FUNCION QUE SE EJECUTA EN SEGUNDO PLANO Y QUE EJECUTA LA FUNCION DE MONITOREO DE ACUERDO AL CHECK INTERVAL
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine($"[{DateTime.Now}] Servicio de verificación de vencimientos iniciado");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    Console.WriteLine($"\n[{DateTime.Now}] Iniciando verificación de lotes...");
                    await CheckExpiringLots(stoppingToken);

                    Console.WriteLine($"[{DateTime.Now}] Verificación completada. Próxima ejecución en 5 minutos.");
                    await Task.Delay(_checkInterval, stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[{DateTime.Now}] ERROR: {ex.Message}");
                    _logger.LogError(ex, "Error en la verificación de vencimientos");
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Espera 1 minuto antes de reintentar
                }
            }
        }

        //FUNCION QUE MONITOREA EL VENCIMIENTO DE LOS LOTES
        private async Task CheckExpiringLots(CancellationToken stoppingToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    var lotRepository = scope.ServiceProvider.GetRequiredService<IRiceLotDA>();
                    var notificationTypeRepository = scope.ServiceProvider.GetRequiredService<INotificationTypeDA>();
                    var notificationDA = scope.ServiceProvider.GetRequiredService<INotificationSenderDA>();
                    var signalRHub = scope.ServiceProvider.GetRequiredService<SignalRHub>();

                    Console.WriteLine($"[{DateTime.Now}] Obteniendo configuración de notificaciones...");
                    var expiryNotificationTypes = await notificationTypeRepository.GetNotificationTypes("FECHA VENCIMIENTO");

                    Console.WriteLine($"[{DateTime.Now}] Buscando lotes próximos a vencer (PriorNotificationDays: {expiryNotificationTypes.PriorNotificationDays})...");
                    var expiringLots = await lotRepository.GetLotsExpiringInDays(expiryNotificationTypes.PriorNotificationDays);
                    Console.WriteLine($"[{DateTime.Now}] Se encontraron {expiringLots.Count()} lotes próximos a vencer");

                    foreach (var lot in expiringLots)
                    {
                        if (stoppingToken.IsCancellationRequested) break;

                        Console.WriteLine($"[{DateTime.Now}] Procesando lote: {lot.Code}");
                        var lastNotification = await notificationDA.GetLastNotificationForLot(
                            lot.IdLot,
                            expiryNotificationTypes.IdNotificationType);

                        if (ShouldSendNotification(lastNotification, expiryNotificationTypes.FrecuencyAfterFirstNotification))
                        {
                            var daysUntilExpiry = (lot.ExpirationDate - DateTime.Now).Days;
                            var messageType = GetMessageType(daysUntilExpiry, expiryNotificationTypes.PriorNotificationDays);

                            Console.WriteLine($"[{DateTime.Now}] Enviando notificación para lote {lot.Code} - Días para vencer: {daysUntilExpiry} - Tipo: {messageType}");

                            try
                            {
                                var title = "¡Alerta de Vencimiento!";
                                var message = $"Lote {lot.Code} vencerá en {daysUntilExpiry} días (Fecha: {lot.ExpirationDate:dd/MM/yyyy})";
                                var link = $"/RiceLot/RiceLotDetails?IdLot={lot.IdLot}";

                                await signalRHub.SendToRole(
                                    "ADMINISTRADOR",
                                    title,
                                    message,
                                    messageType,
                                    expiryNotificationTypes.IdNotificationType,
                                    link,
                                    "");

                                await signalRHub.SendToRole(
                                    "COLABORADOR",
                                    title,
                                    message,
                                    messageType,
                                    expiryNotificationTypes.IdNotificationType,
                                    link,
                                    "");

                                Console.WriteLine($"[{DateTime.Now}] Notificación enviada exitosamente para lote {lot.Code}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"[{DateTime.Now}] Error al enviar notificación para lote {lot.Code}: {ex.Message}");
                                throw new Exception($"No se pudo enviar la notificación para el lote {lot.Code}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"[{DateTime.Now}] No se requiere enviar notificación para lote {lot.Code} (última notificación muy reciente)");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[{DateTime.Now}] Error procesando lotes: {ex.Message}");
                    _logger.LogError(ex, "Error procesando los lotes por vencer");
                    throw;
                }
            }
        }

        //FUNCION PARA EMITIR NIVELES DE MENSAJES DE ACUERDO A QUE TAN CERCA SE ENCUENTRA DEL NIVEL DE STOCK 
        private string GetMessageType(int daysUntilExpiry, int priorNotificationDays)
        {
            var rangeSize = (double)priorNotificationDays / 3;
            var dangerLimit = (int)Math.Ceiling(rangeSize);
            var warningLimit = (int)Math.Ceiling(rangeSize * 2);

            Console.WriteLine($"[{DateTime.Now}] Calculando tipo de mensaje - Rangos: ERROR(0-{dangerLimit}), WARNING({dangerLimit + 1}-{warningLimit}), INFO({warningLimit + 1}-{priorNotificationDays})");

            return daysUntilExpiry <= dangerLimit ? "ERROR" :
                   daysUntilExpiry <= warningLimit ? "WARNING" : "INFO";
        }

        //FUNCION PARA VALIDAR SI SE DEBERIA DE VOLVER A ENVIAR LA NOTIFICACION DE VENCIMIENTO DEL LOTE
        private bool ShouldSendNotification(Notification lastNotification, int frecuencyDays)
        {
            if (lastNotification.IdNotification == 0)
            {
                Console.WriteLine($"[{DateTime.Now}] No hay notificación previa, se debe enviar");
                return true;
            }

            var daysSinceLastNotification = (DateTime.Now - lastNotification.CreatedAt).Days;
            var shouldSend = daysSinceLastNotification >= frecuencyDays;

            Console.WriteLine($"[{DateTime.Now}] Última notificación hace {daysSinceLastNotification} días (frecuencia configurada: {frecuencyDays} días) - {(shouldSend ? "Se debe enviar" : "No se debe enviar aún")}");

            return shouldSend;
        }
    }
}
