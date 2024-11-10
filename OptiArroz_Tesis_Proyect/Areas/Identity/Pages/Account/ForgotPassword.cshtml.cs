// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using OptiArroz_Tesis_Proyect.Models.Entities;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models;
using Microsoft.EntityFrameworkCore;

namespace OptiArroz_Tesis_Proyect.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationSenderDA _notificationSender;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, INotificationSenderDA notificationSender)
        {
            _userManager = userManager;
            _notificationSender = notificationSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "Campo obligatorio")]
            [RegularExpression(@"^\d{8,10}$", ErrorMessage = "El DNI o carnet de extranjería debe tener entre 8 y 10 dígitos.")]
            [Display(Name = "Dni o Carnet de extranjería")]
            public string Username { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(Input.Username);

                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // Generar un código de 6 dígitos aleatorio
                var verificationCode = new Random().Next(100000, 999999).ToString();
                // Guardar el código temporal y su fecha de expiración (por ejemplo, 2 minutos)
                user.PasswordResetCode = verificationCode;
                user.CodeExpiration = DateTime.UtcNow.AddMinutes(2); // el código expira en 2 minutos
                await _userManager.UpdateAsync(user);

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code, Input.Username },
                    protocol: Request.Scheme);

                // Enviar el código por email al usuario
                _notificationSender.SendNotification(new List<string> { Input.Username },
                    $"Estimado {user.Name},<br /><br />Tu código de verificación para restablecer la contraseña es: <b>{verificationCode}</b>. Este código es válido por 2 minutos.<br /><br />Saludos cordiales."
                );

                return Redirect(callbackUrl);
            }

            return Page();
        }
    }
}
