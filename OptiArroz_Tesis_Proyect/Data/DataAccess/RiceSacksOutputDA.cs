﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;
using OptiArroz_Tesis_Proyect.Models.Utils;
using OptiArroz_Tesis_Proyect.Services;


namespace OptiArroz_Tesis_Proyect.Data.DataAccess
{
    public class RiceSacksOutputDA : IRiceSacksOutputDA
    {
        private readonly ApplicationDbContext DbContext;
        private readonly IMapper Mapper;
        public readonly SignalRHub signalRHub;

        public RiceSacksOutputDA(SignalRHub signalRHub,IMapper Mapper, ApplicationDbContext DbContext)
        {
            this.DbContext = DbContext;
            this.Mapper = Mapper;
            this.signalRHub = signalRHub;
        }

        public async Task<(List<RiceSacksConsultationTableDTO> SelectedLots, List<RiceSacksConsultationResultDTO> Responses)> CreateRiceSacksOutput(RiceSacksOutput NewOutput, List<RiceSacksConsultationDTO> Consultation)
        {
            await StockSemaphore.Semaphore.WaitAsync();
            using var transaction = await DbContext.Database.BeginTransactionAsync();
            try
            {
                

                var (SelectedLots , Result) = await SelectLotsToExtractionFIFO(Consultation);

                if (Result.Count > 0) return (SelectedLots, Result);

                var OutputRegister = await RegisterOutput(NewOutput);

                await RegisterOutputDetails(OutputRegister, SelectedLots);

                await UpdateStocks(SelectedLots,Consultation);
                
                // Confirmar la transacción
                await transaction.CommitAsync();

                return (SelectedLots, Result);
            }
            catch (Exception ex)
            {
                // Revertir todos los cambios si ocurre un error
                await transaction.RollbackAsync();
                throw new Exception("Fallo al crear la salida", ex);
            }
            finally
            {
                StockSemaphore.Semaphore.Release();
            }
        }


        private async Task UpdateStocks(List<RiceSacksConsultationTableDTO> SelectedLots, List<RiceSacksConsultationDTO> Consultation)
        {
            foreach (var lot in SelectedLots)
            {
                var FoundLot = await DbContext.RiceLots.FindAsync(lot.IdLot) ?? throw new Exception("No se encontro el lote");
                FoundLot.LeftoverQuantity -= lot.QuantitySelected; //ACTUALIZACION DE STOCKS DE LOTE

                if(FoundLot.LeftoverQuantity == 0)
                {
                    FoundLot.State = 0;
                    var FoundUbication = DbContext.Ubications.Find(FoundLot.IdLastUbication) ?? throw new Exception("No se encontro la ubicacion del lote");
                    FoundUbication.State = 1; //LIBERA LA UBICACION
                    FoundUbication.IdCurrentRiceLot = null;
                    DbContext.Entry(FoundUbication).State = EntityState.Modified;
                }

                DbContext.Entry(FoundLot).State = EntityState.Modified;
            }

            var notificationType = await DbContext.NotificationTypes.Where(x => x.Name == "STOCKS").FirstOrDefaultAsync() ?? throw new Exception();

            foreach (var consultate in Consultation)
            {
                var FoundClassification = await DbContext.RiceClassifications.FindAsync(consultate.IdClassification) ?? throw new Exception("Clasificacion no encontrada");
                FoundClassification.CurrentStock -= consultate.QuantitySelected;
                DbContext.Entry(FoundClassification).State = EntityState.Modified;

                if(FoundClassification.CurrentStock <= FoundClassification.MinimunStock)
                {
                    //////////////////////////////////////////////////////ªªªªªªGENERAR ALERTA POR STOCK MINIMOªªªªªªª///////////////////////////////////////////////////////
                    //Notifications
                    var title = "¡Se llego al stock minimo!";
                    var message = "Stock minimo de " + FoundClassification.MinimunStock + " sacos en la clasificacion " + FoundClassification.Name + " ha sido alcanzado con stock actual de " + FoundClassification.CurrentStock;
                    var messageType = "WARNING";
                    var link = "/Home/Index";
                    try
                    {
                        await signalRHub.SendToRole("ADMINISTRADOR", title, message, messageType, 1, link, "");
                        await signalRHub.SendToRole("COLABORADOR", title, message, messageType, 1, link, "");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                else if ( Math.Abs( FoundClassification.CurrentStock - FoundClassification.MinimunStock) <=  notificationType.PriorNotificationDays)
                {
                    //////////////////////////////////////////////////////ªªªªªªGENERAR ALERTA POR STOCK MINIMOªªªªªªª///////////////////////////////////////////////////////
                    //Notifications
                    var title = "¡Nos encontramos proximos al stock minimo!";
                    var message = "Proximo al stock minimo de " + FoundClassification.MinimunStock +   " sacos de la clasificacion " + FoundClassification.Name + " con stock actual de " + FoundClassification.CurrentStock;
                    var messageType = "INFO";
                    var link = "/Home/Index";
                    try
                    {
                        await signalRHub.SendToRole("ADMINISTRADOR", title, message, messageType, 1, link, "");
                        await signalRHub.SendToRole("COLABORADOR", title, message, messageType, 1, link, "");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                
            }

            await DbContext.SaveChangesAsync();
        }


        private async Task UpdateStocks(int IdLot, int QuantitySelected)
        {

                var FoundLot = await DbContext.RiceLots.FindAsync(IdLot) ?? throw new Exception("No se encontro el lote");
                FoundLot.LeftoverQuantity -= QuantitySelected; //ACTUALIZACION DE STOCKS DE LOTE

                if (FoundLot.LeftoverQuantity == 0)
                {
                    FoundLot.State = 0;
                    var FoundUbication = DbContext.Ubications.Find(FoundLot.IdLastUbication) ?? throw new Exception("No se encontro la ubicacion del lote");
                    FoundUbication.State = 1; //LIBERA LA UBICACION
                    FoundUbication.IdCurrentRiceLot = null;
                    DbContext.Entry(FoundUbication).State = EntityState.Modified;
                }

                DbContext.Entry(FoundLot).State = EntityState.Modified;
            

            var notificationType = await DbContext.NotificationTypes.Where(x => x.Name == "STOCKS").FirstOrDefaultAsync() ?? throw new Exception();

                var FoundClassification = await DbContext.RiceClassifications.FindAsync(FoundLot.IdClassification) ?? throw new Exception("Clasificacion no encontrada");
                FoundClassification.CurrentStock -= QuantitySelected;
                DbContext.Entry(FoundClassification).State = EntityState.Modified;

                if (FoundClassification.CurrentStock <= FoundClassification.MinimunStock)
                {
                    //////////////////////////////////////////////////////ªªªªªªGENERAR ALERTA POR STOCK MINIMOªªªªªªª///////////////////////////////////////////////////////
                    //Notifications
                    var title = "¡Se llego al stock minimo!";
                    var message = "Stock minimo de " + FoundClassification.MinimunStock + " sacos en la clasificacion " + FoundClassification.Name + " ha sido alcanzado con stock actual de " + FoundClassification.CurrentStock;
                    var messageType = "WARNING";
                    var link = "/Home/Index";
                    try
                    {
                        await signalRHub.SendToRole("ADMINISTRADOR", title, message, messageType, 1, link, "");
                        await signalRHub.SendToRole("COLABORADOR", title, message, messageType, 1, link, "");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                else if (Math.Abs(FoundClassification.CurrentStock - FoundClassification.MinimunStock) <= notificationType.PriorNotificationDays)
                {
                    //////////////////////////////////////////////////////ªªªªªªGENERAR ALERTA POR STOCK MINIMOªªªªªªª///////////////////////////////////////////////////////
                    //Notifications
                    var title = "¡Nos encontramos proximos al stock minimo!";
                    var message = "Proximo al stock minimo de " + FoundClassification.MinimunStock + " sacos de la clasificacion " + FoundClassification.Name + " con stock actual de " + FoundClassification.CurrentStock;
                    var messageType = "INFO";
                    var link = "/Home/Index";
                    try
                    {
                        await signalRHub.SendToRole("ADMINISTRADOR", title, message, messageType, 1, link, "");
                        await signalRHub.SendToRole("COLABORADOR", title, message, messageType, 1, link, "");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

            await DbContext.SaveChangesAsync();
        }



        private async Task<(List<RiceSacksConsultationTableDTO> SelectedLots, List<RiceSacksConsultationResultDTO> Responses)> SelectLotsToExtractionFIFO(List<RiceSacksConsultationDTO> Consultation)
        {
            var result = new List<RiceSacksConsultationTableDTO>();
            var Responses = new List<RiceSacksConsultationResultDTO>();

            //PROCESA CADA CONSULTA SE DEBE ARMAR LA LISTA DE LOTES QUE VAN A EXTRAER Y EL RESULTADO
            foreach (var Consultate in Consultation)
            {
                //Obtener lotes segun la clasificacion solicitada y que este ordenados por la fecha de expiracion y luego por las cantidades que tiene cada lote y luego segun la ubicación
                var RiceLots = await DbContext.RiceLots.Where(x => x.IdClassification == Consultate.IdClassification && x.State == 1 && x.IdLastUbication != null)
                    .Include(x=>x.LastUbication)
                    .Include(x=>x.RiceClassification)
                    .OrderBy(x => x.ExpirationDate)
                    .ThenBy(x => x.LeftoverQuantity)
                    .ThenBy(x=>x.LastUbication.IdZone)
                    .ThenBy(x=>x.LastUbication.Row)
                    .ThenBy(x=>x.LastUbication.Column)
                    .ToListAsync();

                var (lots , response) = await SelectQuantities(Mapper.Map<List<RiceSacksConsultationTableDTO>>(RiceLots), Consultate.QuantitySelected , Consultate.IdClassification);
                result.AddRange(lots);

            }
            return (result , Responses);
        }


        private async Task<(List<RiceSacksConsultationTableDTO> lots, RiceSacksConsultationResultDTO response)> SelectQuantities(List<RiceSacksConsultationTableDTO> Lots, int requestedQuantity, int IdClassification)
        {
            int remainingQuantity = requestedQuantity;
            var selectedLots = new List<RiceSacksConsultationTableDTO>();
            var response = new RiceSacksConsultationResultDTO();

            // Calcular el stock total disponible
            var Classification = await DbContext.RiceClassifications.FindAsync(IdClassification) ?? throw new Exception("No se encontro la clasificacion");

            // Verificar si hay suficiente stock antes de comenzar
            if (Classification.CurrentStock < requestedQuantity)
            {

                response.TooltipMessage = $"⚠️ NO HAY SUFICIENTE STOCK\n\n" +
                           $"• Clasificacion: {Classification.Name} unidades\n" +
                           $"• Stock actual: {Classification.CurrentStock} unidades\n" +
                           $"• Cantidad solicitada: {requestedQuantity} unidades\n" +
                           $"• Déficit: {Math.Abs(Classification.CurrentStock - requestedQuantity)} unidades\n\n" +
                           "❌ No es posible atender la solicitud por falta de stock.";

                return (selectedLots, response);
            }

            foreach (var Lot in Lots)
            {
                if (remainingQuantity <= 0)
                    break;

                // Determinar cuánto podemos tomar de este producto
                int quantityToTake = Math.Min(Lot.LeftoverQuantity, remainingQuantity);

                // Solo si se tomó alguna cantidad, agregamos el lote a la lista de seleccionados
                if (quantityToTake > 0)
                {
                    Lot.QuantitySelected = quantityToTake;
                    selectedLots.Add(Lot);
                    remainingQuantity -= quantityToTake;
                }
            }

            return (selectedLots , response);
        }

        private async Task RegisterOutputDetails (RiceSacksOutput OutputRegister, List<RiceSacksConsultationTableDTO> SelectedLots)
        {

            var OutputDetails = new List<RiceSacksOutputDetail>();

            foreach (var Lot in SelectedLots)
            {
                var Detail = new RiceSacksOutputDetail();
                Detail.IdRiceSacksOutput = OutputRegister.IdRiceSacksOutput;
                Detail.SacksQuantity = Lot.QuantitySelected;
                Detail.IdRiceLot = Lot.IdLot;

                OutputDetails.Add(Detail);
            }

            await DbContext.RiceSacksOutputDetails.AddRangeAsync(OutputDetails);
            await DbContext.SaveChangesAsync();

        }

        private async Task RegisterOutputDetails(RiceSacksOutput OutputRegister, int IdLot ,  int QuantitySelected)
        {

            var Detail = new RiceSacksOutputDetail();
            Detail.IdRiceSacksOutput = OutputRegister.IdRiceSacksOutput;
            Detail.SacksQuantity = QuantitySelected;
            Detail.IdRiceLot = IdLot;

            await DbContext.RiceSacksOutputDetails.AddAsync(Detail);
            await DbContext.SaveChangesAsync();

        }

        private async Task<RiceSacksOutput> RegisterOutput(RiceSacksOutput NewOutput)
        {
            //var LastRiceLot = await DbContext.RiceLots.Where(x => x.CreatedAt.Date == DateTime.Now.Date.AddDays(1)).OrderByDescending(x => x.Code).FirstOrDefaultAsync();
            var Outputs = await DbContext.RiceSacksOutputs.ToListAsync();
            var LastOutput = Outputs.Where(x => x.CreatedAt.Date == DateTime.Now.Date).OrderByDescending(x => x.Code).FirstOrDefault();

            if (LastOutput == null)
            {
                //Crear el primer lote del dia
                NewOutput.Code = $"SL-{DateTime.Now:yyyyMMdd}-00001";
            }
            else
            {
                //Aumentar el correlativo del LastRiceLot
                // Obtener el número correlativo del último código
                string lastNumber = LastOutput.Code.Split('-')[2];
                int correlativo = int.Parse(lastNumber) + 1;

                // Crear nuevo código con correlativo aumentado
                NewOutput.Code = $"SL-{DateTime.Now:yyyyMMdd}-{correlativo:00000}";
            }

            await DbContext.RiceSacksOutputs.AddAsync(NewOutput);
            await DbContext.SaveChangesAsync();

            return NewOutput;
        }


        

        public async Task<List<RiceSacksOutputTableDTO>> GetRiceSacksOutputs()
        {
            var Outputs = await DbContext.RiceSacksOutputs
                .Include(x=>x.CreatedBy)
                .Include(x=>x.RiceSacksOutputType)
                .ToListAsync();


            //YA TENGO EL DTO DE LAS SALIDAS
            var OutputsDTO = Mapper.Map<List<RiceSacksOutputTableDTO>>(Outputs);


            //LLENAR LAS LISTAS DE DETALLE DE CADA SALIDA
            foreach(var OutputDTO in OutputsDTO)
            {
                var Details = await DbContext.RiceSacksOutputDetails.Include(x => x.RiceLot)
                                                                    .ThenInclude(x => x.RiceClassification)
                                                                    .Include(x=>x.RiceLot)
                                                                    .ThenInclude(x=>x.LastUbication)
                                                                    .Include(x=>x.RiceLot)
                                                                    .ThenInclude(x=>x.Zone)
                                                                    .Where(x => x.IdRiceSacksOutput == OutputDTO.IdRiceSacksOutput)
                                                                    .ToListAsync();

                var DetailsDTO = Mapper.Map<List<RiceSacksOutputDetailTableDTO>>(Details);

                OutputDTO.RiceSacksOutputDetailTableDTOs = DetailsDTO;
            }



            return OutputsDTO;
        }

        public async Task<RiceSacksOutputTableDTO> GetRiceSacksOutputDetailById(int IdRiceSackOutput)
        {
            var Outputs = await DbContext.RiceSacksOutputs
                .Include(x => x.CreatedBy)
                .Include(x => x.RiceSacksOutputType)
                .Where(x => x.IdRiceSacksOutput == IdRiceSackOutput)
                .FirstOrDefaultAsync() ?? throw new Exception("No se encontro la salida") ;


            //YA TENGO EL DTO DE LAS SALIDAS
            var OutputsDTO = Mapper.Map<RiceSacksOutputTableDTO>(Outputs);

                var Details = await DbContext.RiceSacksOutputDetails.Include(x => x.RiceLot)
                                                                    .ThenInclude(x => x.RiceClassification)
                                                                    .Include(x => x.RiceLot)
                                                                    .ThenInclude(x => x.LastUbication)
                                                                    .Include(x => x.RiceLot)
                                                                    .ThenInclude(x => x.Zone)
                                                                    .Where(x => x.IdRiceSacksOutput == Outputs.IdRiceSacksOutput)
                                                                    .ToListAsync();

                var DetailsDTO = Mapper.Map<List<RiceSacksOutputDetailTableDTO>>(Details);

            OutputsDTO.RiceSacksOutputDetailTableDTOs = DetailsDTO;
            



            return OutputsDTO;
        }

        public async Task<RiceSacksOutput> GetRiceSacksOutputById(int IdRiceSacksOutput)
        {
            try
            {
                return await DbContext.RiceSacksOutputs.FindAsync(IdRiceSacksOutput) ?? throw new Exception("No se encontró la salida");
            }
            catch
            {
                throw new Exception("Algo fallo al obtener la salida");
            }
        }

        public async Task<List<RiceSacksOutputTypeLotDTO>> GetRiceSackOutputTypeLot(string OutputCode)
        {
            var IdOutput = await DbContext.RiceSacksOutputs.Where(x => x.Code == OutputCode).Select(x => x.IdRiceSacksOutput).FirstOrDefaultAsync();

            var groupedDetails = await DbContext.RiceSacksOutputDetails
                                        .Include(x => x.RiceLot)
                                        .ThenInclude(x => x.RiceClassification)
                                        .Where(x => x.IdRiceSacksOutput == IdOutput)
                                        .GroupBy(x => new {
                                            IdClassification = x.RiceLot.RiceClassification.IdClassification, // Asumiendo que existe un campo Id
                                            Classification = x.RiceLot.RiceClassification.Name,
                                            ExpirationDate = x.RiceLot.ExpirationDate
                                        })
                                        .Select(g => new RiceSacksOutputTypeLotDTO
                                        {
                                            IdClassification = g.Key.IdClassification,
                                            Classification = g.Key.Classification,
                                            ExpirationDate = g.Key.ExpirationDate,
                                            QuantitySelected = g.Sum(x => x.SacksQuantity)
                                        })
                                        .ToListAsync();

            return groupedDetails;


        }

        public async Task CreateRiceSacksOutput(RiceSacksOutput NewOutput, int QuantitySacks, int IdLot)
        {
            await StockSemaphore.Semaphore.WaitAsync();
            using var transaction = await DbContext.Database.BeginTransactionAsync();
            try
            {

                //validar stock del lote

                var FoundLot = await DbContext.RiceLots.FindAsync(IdLot) ?? throw new Exception("No se encontro el lote");

                if (FoundLot.LeftoverQuantity == 0) throw new Exception("No se pudo atender su pedido, ya que se extrajeron los sacos del lote en su totalidad");
                if (FoundLot.LeftoverQuantity < QuantitySacks) throw new Exception("Algunos sacos fueron retirados recientemente, por ello no es posible antender su solicitud con el stock del lote actual : " + FoundLot.LeftoverQuantity);


                var OutputRegister = await RegisterOutput(NewOutput);

                await RegisterOutputDetails(OutputRegister, IdLot, QuantitySacks);

                await UpdateStocks(IdLot, QuantitySacks);

                // Confirmar la transacción
                await transaction.CommitAsync();

                return;
            }
            catch
            {
                // Revertir todos los cambios si ocurre un error
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                StockSemaphore.Semaphore.Release();
            }
        }
    }
}
