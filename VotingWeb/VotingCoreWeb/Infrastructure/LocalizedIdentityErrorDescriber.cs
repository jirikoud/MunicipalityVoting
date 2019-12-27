using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingCoreWeb.Infrastructure
{
    public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DefaultError() { return new IdentityError { Code = nameof(DefaultError), Description = $"Došlo k neočekávané chybě." }; }
        public override IdentityError ConcurrencyFailure() { return new IdentityError { Code = nameof(ConcurrencyFailure), Description = "Chyba při souběhu ukládání do databáze." }; }
        public override IdentityError PasswordMismatch() { return new IdentityError { Code = nameof(PasswordMismatch), Description = "Špatné heslo." }; }
        public override IdentityError InvalidToken() { return new IdentityError { Code = nameof(InvalidToken), Description = "Neplatný token." }; }
        public override IdentityError LoginAlreadyAssociated() { return new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = "Uživatel s tímto účtem již existuje." }; }
        public override IdentityError InvalidUserName(string userName) { return new IdentityError { Code = nameof(InvalidUserName), Description = $"Uživatelské jméno '{userName}' je neplatné. Použijte pouze písmena a číslice." }; }
        public override IdentityError InvalidEmail(string email) { return new IdentityError { Code = nameof(InvalidEmail), Description = $"E-mail '{email}' není platný." }; }
        public override IdentityError DuplicateUserName(string userName) { return new IdentityError { Code = nameof(DuplicateUserName), Description = $"Uživatelské jméno '{userName}' je již obsazené." }; }
        public override IdentityError DuplicateEmail(string email) { return new IdentityError { Code = nameof(DuplicateEmail), Description = $"E-mail '{email}' je již obsazený." }; }
        public override IdentityError InvalidRoleName(string role) { return new IdentityError { Code = nameof(InvalidRoleName), Description = $"Název role '{role}' je naplatný." }; }
        public override IdentityError DuplicateRoleName(string role) { return new IdentityError { Code = nameof(DuplicateRoleName), Description = $"Název role '{role}' je již obsazený." }; }
        public override IdentityError UserAlreadyHasPassword() { return new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = "Uživatel má již nastavené heslo." }; }
        public override IdentityError UserLockoutNotEnabled() { return new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = "Uzamčení účtu není u tohoto uživatele povoleno." }; }
        public override IdentityError UserAlreadyInRole(string role) { return new IdentityError { Code = nameof(UserAlreadyInRole), Description = $"Uživatel má roli '{role}' již nastavenou." }; }
        public override IdentityError UserNotInRole(string role) { return new IdentityError { Code = nameof(UserNotInRole), Description = $"Uživatel nemá přidělenou roli '{role}'." }; }
        public override IdentityError PasswordTooShort(int length) { return new IdentityError { Code = nameof(PasswordTooShort), Description = $"Heslo musí mít alespoň {length} znaků." }; }
        public override IdentityError PasswordRequiresNonAlphanumeric() { return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Heslo musí mít alespoň jedno písmeno nebo číslici." }; }
        public override IdentityError PasswordRequiresDigit() { return new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "Heslo musí obsahovat alespoň jednu číslici ('0'-'9')." }; }
        public override IdentityError PasswordRequiresLower() { return new IdentityError { Code = nameof(PasswordRequiresLower), Description = "Heslo musí obsahovat alespoň jedno malé písmeno ('a'-'z')." }; }
        public override IdentityError PasswordRequiresUpper() { return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "Heslo musí obsahovat alespoň jedno velké písmeno ('A'-'Z')." }; }
    }
}
