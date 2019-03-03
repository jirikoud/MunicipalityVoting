using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VotingWeb.Properties;

namespace VotingWeb.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessageResourceName = "VALIDATION_REQUIRED_EMAIL", ErrorMessageResourceType = typeof(AccountRes))]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(ErrorMessageResourceName = "VALIDATION_REQUIRED_EMAIL", ErrorMessageResourceType = typeof(AccountRes))]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessageResourceName = "VALIDATION_REQUIRED_EMAIL", ErrorMessageResourceType = typeof(AccountRes))]
        [Display(Name = "LOGIN_EMAIL", ResourceType = typeof(AccountRes))]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "VALIDATION_REQUIRED_PASSWORD", ErrorMessageResourceType = typeof(AccountRes))]
        [DataType(DataType.Password)]
        [Display(Name = "LOGIN_PASSWORD", ResourceType = typeof(AccountRes))]
        public string Password { get; set; }

        [Display(Name = "LOGIN_REMEMBER_ME", ResourceType = typeof(AccountRes))]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessageResourceName = "VALIDATION_REQUIRED_EMAIL", ErrorMessageResourceType = typeof(AccountRes))]
        [EmailAddress]
        [Display(Name = "RESET_EMAIL", ResourceType = typeof(AccountRes))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "VALIDATION_REQUIRED_PASSWORD", ErrorMessageResourceType = typeof(AccountRes))]
        [StringLength(100, ErrorMessageResourceName = "VALIDATION_PASSWORD_LENGTH", MinimumLength = 6, ErrorMessageResourceType = typeof(AccountRes))]
        [DataType(DataType.Password)]
        [Display(Name = "RESET_PASSWORD", ResourceType = typeof(AccountRes))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessageResourceName = "VALIDATION_PASSWORD_CONFIRM", ErrorMessageResourceType = typeof(AccountRes))]
        [Display(Name = "RESET_PASSWORD_CONFIRM", ResourceType = typeof(AccountRes))]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessageResourceName = "VALIDATION_REQUIRED_EMAIL", ErrorMessageResourceType = typeof(AccountRes))]
        [EmailAddress]
        [Display(Name = "FORGOT_EMAIL", ResourceType = typeof(AccountRes))]
        public string Email { get; set; }
    }
}
