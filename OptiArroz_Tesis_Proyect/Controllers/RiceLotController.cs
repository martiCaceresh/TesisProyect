using AutoMapper;
using QRCoder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;
using OptiArroz_Tesis_Proyect.Models.ViewModels;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout;

namespace OptiArroz_Tesis_Proyect.Controllers
{
    public class RiceLotController : Controller
    {

        private readonly IRiceClassificationDA RiceClassificationDA;
        private readonly IZoneDA ZoneDA;
        private readonly IRiceLotDA RiceLotDA;
        private readonly IMapper Mapper;
        private readonly IConfiguration Configuration;
        public readonly UserManager<ApplicationUser> UserManager;



        public RiceLotController(IConfiguration Configuration,IRiceLotDA RiceLotDA, IZoneDA ZoneDA, IRiceClassificationDA RiceClassificationDA, IMapper Mapper, UserManager<ApplicationUser> UserManager)
        {

            this.UserManager = UserManager;
            this.Mapper = Mapper;
            this.RiceClassificationDA = RiceClassificationDA;
            this.ZoneDA = ZoneDA;
            this.RiceLotDA = RiceLotDA;
            this.Configuration = Configuration;
        }
        public async Task<IActionResult> Index()
        {
            CreateNewLotVM Model = new CreateNewLotVM();
            Model.RiceClassifications = await RiceClassificationDA.GetActiveRiceClassifications();
            Model.Zones = await ZoneDA.GetActiveZones();
          
            return View(Model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRiceLot([FromForm] CreateNewLotDTO NewLot , IFormFile TechnicalEspecification)
        {
            try
            {
                var CurrentUser = await UserManager.GetUserAsync(User);

                // Convertir el IFormFile a byte[]
                byte[] fileBytes;
                using (var ms = new MemoryStream())
                {
                    await TechnicalEspecification.CopyToAsync(ms);
                    fileBytes = ms.ToArray();
                }

                var RiceLot = new RiceLot(NewLot, fileBytes, CurrentUser.Id);

                await RiceLotDA.CreateRiceLot(RiceLot);

                // Agrega un mensaje de éxito al TempData
                TempData["SuccessMessage"] = "Se registro el lote correctamente";

                return RedirectToAction("Index" , "Home");
            }
            catch (Exception)
            {
                // Agrega un mensaje de error al TempData
                TempData["ErrorMessage"] = "Se produjo un error en el registro, vuelva a intentar.";

                return RedirectToAction("Index");
            }
        }
    
        public async Task<IActionResult> RiceLotDetails(int IdLot)
        {
            var Model = new RiceLotDetailVM();
            Model.Classifications = await RiceClassificationDA.GetActiveRiceClassifications();
            var RiceLot = await RiceLotDA.GetRiceLotById(IdLot);
            Model.RiceLotDetailDTO = Mapper.Map<RiceLotDetailDTO>(RiceLot);
            return View(Model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRiceLot ([FromForm] UpdateLotDTO UpdateLotDTO, IFormFile TechnicalEspecification)
        {
            try
            {
                var CurrentUser = await UserManager.GetUserAsync(User);

                // Convertir el IFormFile a byte[]
                byte[] fileBytes;
                using (var ms = new MemoryStream())
                {
                    await TechnicalEspecification.CopyToAsync(ms);
                    fileBytes = ms.ToArray();
                }

                var UpdateRiceLot = new RiceLot(UpdateLotDTO, fileBytes, CurrentUser.Id);

                await RiceLotDA.UpdateRiceLot(UpdateRiceLot);

                // Agrega un mensaje de éxito al TempData
                TempData["SuccessMessage"] = "Se actualizo el lote correctamente";

                return RedirectToAction("RiceLotDetails" , new { IdLot = UpdateLotDTO.IdLot });
            }
            catch (Exception)
            {
                // Agrega un mensaje de error al TempData
                TempData["ErrorMessage"] = "Se produjo un error en la actualizacion, vuelva a intentar.";

                return RedirectToAction("RiceLotDetails", new { IdLot = UpdateLotDTO.IdLot });
            }
        }

        [HttpGet]
        public async Task<IActionResult> DownloadTechnicalSpecification(int IdLot)
        {
            try
            {
                // Buscar el lote
                var RiceLot = await RiceLotDA.GetRiceLotById(IdLot);

                if (RiceLot == null)
                    return NotFound("Lote no encontrado.");

                if (RiceLot.TechnicalSpecification == null)
                    return NotFound("El lote no tiene especificación técnica.");

                // Retornar el archivo PDF
                return File(
                    fileContents: RiceLot.TechnicalSpecification,
                    contentType: "application/pdf",
                    fileDownloadName: $"EspecificacionTecnica_Lote_{RiceLot.Code}.pdf"
                );
            }
            catch
            {
                // Loguear el error si tienes logging configurado
                return StatusCode(500, "Error al descargar la especificación técnica.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DownloadLotLabel(int IdLot)
        {
            try
            {
                // Obtener los detalles del lote
                var RiceLot = await RiceLotDA.GetRiceLotById(IdLot);
                var lotDetail = Mapper.Map<RiceLotDetailDTO>(RiceLot); 
                if (lotDetail == null)
                {
                    return NotFound();
                }

                // Generar el PDF
                var pdfBytes = GenerateLotLabel(lotDetail);

                // Devolver el PDF como un archivo para descargar
                return File(pdfBytes, "application/pdf", $"Etiqueta_Lote_{IdLot}.pdf");
            }
            catch 
            {
                // Log the error here
                return StatusCode(500);
            }
        }

        private byte[] GenerateLotLabel(RiceLotDetailDTO lotDetail)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var writer = new PdfWriter(ms);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                // Configurar tamaño de página para la etiqueta
                pdf.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A6);
                document.SetMargins(20, 20, 20, 20);

                // Título "Código de lote"
                document.Add(new Paragraph("Código de lote")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(12));

                // Código del lote
                document.Add(new Paragraph(lotDetail.Code)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(14)
                    .SetBold());

                // Fechas
                var dateTable = new Table(2)
                    .SetWidth(UnitValue.CreatePercentValue(100))
                    .SetMarginTop(10);

                dateTable.AddCell(CreateCell("Fecha de ingreso", TextAlignment.LEFT));
                dateTable.AddCell(CreateCell("Fecha de vencimiento", TextAlignment.RIGHT));

                dateTable.AddCell(CreateCell(lotDetail.ReceptionDate.ToString("dd/MM/yyyy"), TextAlignment.LEFT));
                dateTable.AddCell(CreateCell(lotDetail.ExpirationDate.ToString("dd/MM/yyyy"), TextAlignment.RIGHT));

                document.Add(dateTable);

                // Clasificación
                document.Add(new Paragraph("Clasificación")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginTop(10));

                document.Add(new Paragraph($"LARGO-EXTRA-{lotDetail.InitialQuantity}KG")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBold());

                // Generar QR
                string qrUrl = $"{Configuration["BaseUrl"]}/mobile/lot/{lotDetail.IdLot}";
                var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(qrUrl, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new PngByteQRCode(qrCodeData);
                var qrCodeImage = qrCode.GetGraphic(10);

                // Agregar QR al PDF
                var imageData = ImageDataFactory.Create(qrCodeImage);
                var image = new Image(imageData)
                    .SetWidth(100)
                    .SetHeight(100)
                    .SetHorizontalAlignment(HorizontalAlignment.CENTER);
                document.Add(image);

                // Texto debajo del QR
                document.Add(new Paragraph("Escanee el QR para más detalle")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(10)
                    .SetMarginTop(5));

                document.Close();
                return ms.ToArray();
            }
        }

        private Cell CreateCell(string text, TextAlignment alignment)
        {
            return new Cell()
                .Add(new Paragraph(text).SetTextAlignment(alignment))
                .SetBorder(iText.Layout.Borders.Border.NO_BORDER);
        }


    }
}
