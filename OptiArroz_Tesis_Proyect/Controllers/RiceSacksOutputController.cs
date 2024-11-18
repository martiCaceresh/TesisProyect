using AutoMapper;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;
using OptiArroz_Tesis_Proyect.Models.ViewModels;
using iText.Layout;
using System.Text.Json;
using QRCoder;

namespace OptiArroz_Tesis_Proyect.Controllers
{
	public class RiceSacksOutputController : Controller
	{
        private readonly IRiceClassificationDA RiceClassificationDA;
        private readonly IZoneDA ZoneDA;
        private readonly IRiceLotDA RiceLotDA;
        private readonly IMapper Mapper;
        private readonly IConfiguration Configuration;
		private readonly IRiceSacksOutputDA RiceSacksOutputDA;
		private readonly IRiceSacksOutputTypeDA RiceSacksOutputTypeDA;
        public readonly UserManager<ApplicationUser> UserManager;




        public RiceSacksOutputController(IRiceSacksOutputTypeDA RiceSacksOutputTypeDA,IRiceSacksOutputDA RiceSacksOutputDA,IConfiguration Configuration, IRiceLotDA RiceLotDA, IZoneDA ZoneDA, IRiceClassificationDA RiceClassificationDA, IMapper Mapper, UserManager<ApplicationUser> UserManager)
        {

            this.UserManager = UserManager;
            this.Mapper = Mapper;
            this.RiceClassificationDA = RiceClassificationDA;
            this.ZoneDA = ZoneDA;
            this.RiceLotDA = RiceLotDA;
            this.Configuration = Configuration;
			this.RiceSacksOutputDA = RiceSacksOutputDA;
			this.RiceSacksOutputTypeDA = RiceSacksOutputTypeDA;
        }
        public async Task<IActionResult> Index(string Page = "")
		{

			if(Page == "RiceSacksOutput")
			{
				var Model = new RiceSacksOutputVM();
				var Outputs = await RiceSacksOutputDA.GetRiceSacksOutputs();
				Model.RiceSacksOutputTableDTOs = Outputs;
				return PartialView("_RiceSacksOutputPartial", Model);
			}

			else if (Page == "RiceSacksConsultation")
			{
				var Model = new RiceSacksConsultationVM();
				Model.RiceClassifications = await RiceClassificationDA.GetActiveRiceClassifications();
                Model.RiceSacksOutputTypes = await RiceSacksOutputTypeDA.GetRiceSacksOutputTypes();
				return PartialView("_RiceSacksConsultationPartial",Model);
			}
			else
			{
                var Model = new RiceSacksOutputVM();
                var Outputs = await RiceSacksOutputDA.GetRiceSacksOutputs();
                Model.RiceSacksOutputTableDTOs = Outputs;
                return View(Model);

            }
			
			
		}

		[HttpPost]
		public async Task<IActionResult> GetLotsConsultationResult([FromForm] RiceSacksConsultationDTO RiceSacksConsultationDTO)
		{
            var Model = new RiceSacksConsultationVM();
            var Lots = await RiceLotDA.GetRiceLotConsultation(RiceSacksConsultationDTO.IdClassification);
            Model.RiceSacksConsultationTableDTOs = SelectQuantities(Mapper.Map<List<RiceSacksConsultationTableDTO>>(Lots), RiceSacksConsultationDTO.QuantitySelected);
            Model.QuantitySelected = RiceSacksConsultationDTO.QuantitySelected;

            return PartialView("_RiceSacksConsultationTablePartial", Model);

		}

        public async Task<IActionResult> RiceLotDetails(int IdLot)
        {
            var Model = new RiceLotDetailVM();
            Model.Classifications = await RiceClassificationDA.GetActiveRiceClassifications();
            var RiceLot = await RiceLotDA.GetRiceLotById(IdLot);
            Model.RiceLotDetailDTO = Mapper.Map<RiceLotDetailDTO>(RiceLot);
            return View(Model);
        }


        private List<RiceSacksConsultationTableDTO> SelectQuantities(List<RiceSacksConsultationTableDTO> Lots, int requestedQuantity)
        {


            int remainingQuantity = requestedQuantity;

            // Iterar sobre la lista de productos
            foreach (var Lot in Lots)
            {
                if (remainingQuantity <= 0)
                    break;

                // Determinar cuánto podemos tomar de este producto
                int quantityToTake = Math.Min(Lot.LeftoverQuantity, remainingQuantity);

                // Actualizar la cantidad seleccionada
                Lot.QuantitySelected = quantityToTake;

                // Actualizar la cantidad restante por seleccionar
                remainingQuantity -= quantityToTake;
            }

            return Lots;
        }

        [HttpPost]
        public async Task<JsonResult> CreateRiceSacksOutput([FromForm] CreateRiceSacksOutputDTO newOutputDTO)
        {
            try
            {
                var CurrentUser = await UserManager.GetUserAsync(User);
                var Consultation = JsonSerializer.Deserialize<List<RiceSacksConsultationDTO>>(newOutputDTO.Consultation);

                if (newOutputDTO.Attachment == null || Consultation == null) throw new Exception();

                // Convertir el IFormFile a byte[]
                byte[] fileBytes;
                using (var ms = new MemoryStream())
                {
                    await newOutputDTO.Attachment.CopyToAsync(ms);
                    fileBytes = ms.ToArray();
                }

                var NewOutput = new RiceSacksOutput(newOutputDTO, fileBytes, CurrentUser.Id);

                var (SelectedLots,Responses) = await RiceSacksOutputDA.CreateRiceSacksOutput(NewOutput, Consultation);


                // Agrega un mensaje de éxito al TempData
                TempData["SuccessMessage"] = "Se registro la salida correctamente";

                return Json("Ok"); //FALTA LOGICA PARA CUANDO ALGUN PEDIDO NO SE PUEDA COMPLETAR
            }
            catch (Exception)
            {

                return Json("Se produjo un error en el registro, vuelva a intentar");
            }
        }


        [HttpGet]
        public async Task<IActionResult> DownloadDepartureOrder(int IdRiceSacksOutput)
        {
            try
            {
                // Buscar el salida
                var Output = await RiceSacksOutputDA.GetRiceSacksOutputById(IdRiceSacksOutput);

                if (Output == null)
                    return NotFound("Lote no encontrado.");

                if (Output.OutputEvidence == null)
                    return NotFound("El lote no tiene especificación técnica.");

                // Retornar el archivo PDF
                return File(
                    fileContents: Output.OutputEvidence,
                    contentType: "application/pdf",
                    fileDownloadName: $"EspecificacionTecnica_Lote_{Output.Code}.pdf"
                );
            }
            catch
            {
                // Loguear el error si tienes logging configurado
                return StatusCode(500, "Error al descargar la especificación técnica.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DownloadExitNote(int IdRiceSacksOutput)
        {
            try
            {
                // Obtener los detalles de la salida
                var output = await RiceSacksOutputDA.GetRiceSacksOutputDetailById(IdRiceSacksOutput);
                if (output == null)
                {
                    return NotFound();
                }

                // Generar el PDF
                var pdfBytes = GenerateExitNote(output);

                // Devolver el PDF como un archivo para descargar
                return File(pdfBytes, "application/pdf", $"NotaDeSalida_{output.Code}.pdf");
            }
            catch
            {
                // Log the error here
                return StatusCode(500);
            }
        }

        private byte[] GenerateExitNote(RiceSacksOutputTableDTO output)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var writer = new PdfWriter(ms);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                // Configurar tamaño de página
                pdf.SetDefaultPageSize(PageSize.A4);
                document.SetMargins(36, 36, 36, 36); // 1 inch margins

                // Título centrado
                document.Add(new Paragraph("NOTA DE SALIDA")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20)
                    .SetBold());

                document.Add(new Paragraph($"N° {output.Code}")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(12)
                    .SetBold()
                    .SetMarginBottom(20));

                // Información general
                var infoTable = new Table(2)
                    .SetWidth(UnitValue.CreatePercentValue(100));

                // Columna izquierda
                AddInfoCell(infoTable, "Fecha de Emisión:", output.CreatedAt.ToString("dd/MM/yyyy HH:mm"));
                AddInfoCell(infoTable, "Tipo de Salida:", output.RiceSacksOutputType);
                AddInfoCell(infoTable, "Creado Por:", output.CreatedBy);

                // Columna derecha
                AddInfoCell(infoTable, "Última Actualización:", output.UpdatedAt.ToString("dd/MM/yyyy HH:mm"));
                AddInfoCell(infoTable, "Actualizado Por:", output.UpdatedBy);
                AddInfoCell(infoTable, "Observaciones:", output.Observation);

                document.Add(infoTable);

                // Detalle de lotes
                document.Add(new Paragraph("\nDETALLE DE LOTES")
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontSize(14)
                    .SetBold());

                var detailsTable = new Table(new float[] { 2, 3, 2, 2, 2, 3 })
                    .SetWidth(UnitValue.CreatePercentValue(100));

                // Encabezados
                string[] headers = { "Código", "Clasificación", "Cantidad\nSacos", "Ubicación", "Fecha\nVencimiento", "Total Kg" };
                foreach (var header in headers)
                {
                    detailsTable.AddHeaderCell(new Cell()
                        .Add(new Paragraph(header))
                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(10)
                        .SetBold());
                }

                // Detalles
                int totalSacks = 0;
                decimal totalKg = 0;

                foreach (var detail in output.RiceSacksOutputDetailTableDTOs)
                {
                    // Extraer el peso del saco de la clasificación
                    int weightPerSack = ExtractWeightFromClassification(detail.Classification);
                    decimal totalWeightForLot = detail.SacksQuantity * weightPerSack;

                    detailsTable.AddCell(CreateDetailCell(detail.Code));
                    detailsTable.AddCell(CreateDetailCell(detail.Classification));
                    detailsTable.AddCell(CreateDetailCell(detail.SacksQuantity.ToString()));
                    detailsTable.AddCell(CreateDetailCell(detail.Ubication));
                    detailsTable.AddCell(CreateDetailCell(detail.ExpirationDate.ToString("dd/MM/yyyy")));
                    detailsTable.AddCell(CreateDetailCell($"{totalWeightForLot:N0} Kg"));

                    totalSacks += detail.SacksQuantity;
                    totalKg += totalWeightForLot;
                }

                // Totales
                detailsTable.AddCell(new Cell(1, 2).Add(new Paragraph("TOTALES:")).SetBold());
                detailsTable.AddCell(new Cell().Add(new Paragraph(totalSacks.ToString())).SetTextAlignment(TextAlignment.CENTER).SetBold());
                detailsTable.AddCell(new Cell());
                detailsTable.AddCell(new Cell());
                detailsTable.AddCell(new Cell().Add(new Paragraph($"{totalKg:N0} Kg")).SetTextAlignment(TextAlignment.CENTER).SetBold());

                document.Add(detailsTable);

                // Firmas
                document.Add(new Paragraph("\n\n"));
                var signatureTable = new Table(3)
                    .SetWidth(UnitValue.CreatePercentValue(100));

                string[] signatures = { "Entregado por", "Revisado por", "Recibido por" };
                foreach (var sig in signatures)
                {
                    signatureTable.AddCell(new Cell()
                        .Add(new Paragraph("\n\n"))
                        .Add(new Paragraph("_______________________").SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph(sig).SetTextAlignment(TextAlignment.CENTER))
                        .SetBorder(Border.NO_BORDER));
                }

                document.Add(signatureTable);

                document.Close();
                return ms.ToArray();
            }
        }

        private int ExtractWeightFromClassification(string classification)
        {
            try
            {
                // Buscar el patrón de peso en la clasificación (ej: EXTRA-LARGO-50KG)
                var weightPart = classification.Split('-').LastOrDefault()?.ToUpper().Replace("KG", "");
                if (int.TryParse(weightPart, out int weight))
                {
                    return weight;
                }
                return 0; // Valor por defecto si no se puede extraer el peso
            }
            catch
            {
                return 0; // Valor por defecto en caso de error
            }
        }

        private void AddInfoCell(Table table, string label, string value)
        {
            table.AddCell(new Cell()
                .Add(new Paragraph(label).SetBold())
                .SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell()
                .Add(new Paragraph(value))
                .SetBorder(Border.NO_BORDER));
        }

        private Cell CreateDetailCell(string text)
        {
            return new Cell()
                .Add(new Paragraph(text))
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(10);
        }


        [HttpGet]
        public async Task<IActionResult> GenerateMobileQR(int idOutput)
        {
            try
            {
                // Verificar que el output existe
                var output = await RiceSacksOutputDA.GetRiceSacksOutputById(idOutput);
                if (output == null)
                {
                    return NotFound();
                }

                // Generar URL segura (podrías incluir un token temporal)
                var baseUrl = Configuration["BaseUrl"] ;
                var mobileUrl = $"{baseUrl}/mobile/output/{idOutput}";

                // Generar QR
                using (var qrGenerator = new QRCodeGenerator())
                {
                    var qrData = qrGenerator.CreateQrCode(mobileUrl, QRCodeGenerator.ECCLevel.Q);
                    var qrCode = new PngByteQRCode(qrData);
                    var qrImage = qrCode.GetGraphic(20); // Pixeles por módulo

                    return new JsonResult(new
                    {
                        success = true,
                        qrImage = Convert.ToBase64String(qrImage),
                        url = mobileUrl
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error generating QR code" });
            }
        }
    }

}
