// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using OptiArroz_Tesis_Proyect.Models.Entities;
using OptiArroz_Tesis_Proyect.Models;
using Microsoft.EntityFrameworkCore;

namespace OptiArroz_Tesis_Proyect.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

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
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Campo obligatorio")]
            [RegularExpression(@"^\d{8,10}$", ErrorMessage = "El DNI o carnet de extranjería debe tener entre 8 y 10 dígitos.")]
            [Display(Name = "Dni o Carnet de extranjería")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Campo obligatorio")]
            [RegularExpression(@"^\d{9}$", ErrorMessage = "El número de celular debe tener entre 9 dígitos.")]
            [Display(Name = "Dni o Carnet de extranjería")]
            public string PhoneNumber { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            /// 

            [Required(ErrorMessage = "Campo obligatorio")]
            [StringLength(100, ErrorMessage = "La contraseña debe tener al menos {2} y como máximo {1} caracteres.", MinimumLength = 6)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "Campo obligatorio")]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "La contraseña y la confirmación no coinciden")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Campo obligatorio")]
            public string Name { get; set; }
            [Required(ErrorMessage = "Campo obligatorio")]
            public string LastName { get; set; }

            public static class ValidationMessages
            {
                public const string RequiredError = "Campo obligatorio";
                public const string StringLengthError = "El {0} debe tener al menos {2} y como máximo {1} caracteres.";
                // Define aquí otros mensajes personalizados si es necesario
            }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Username.ToUpper(),
                    Email = String.IsNullOrWhiteSpace(Input.Email) ? "" : Input.Email.ToUpper(),
                    Name = Input.Name.ToUpper(),
                    LastName = Input.LastName.ToUpper(),
                    RegisterDate = DateTime.Now,
                    PasswordHash = Input.Password,
                    PhoneNumber = Input.PhoneNumber.ToUpper(),
                };
                var result = await _userManager.CreateAsync(user, user.PasswordHash);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "COLABORADOR");
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ReturnUrl = returnUrl;
            return Page();
        }


    }
}
