using AngleSharp.Text;
using AutoMapper.Execution;
using FlicoProject.DtoLayer;
using FluentValidation;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.BusinessLayer.Concrete.Validators.RegisterValidators
{
    public class RegisterValidators : AbstractValidator<RegisterUser>
    {
        public RegisterValidators()
        {
            RuleFor(user => user.Email).EmailAddress().WithMessage("This Email is not exist").NotEmpty().WithMessage("Email cannot be empty");

            RuleFor(user => user.Phone)
            .NotEmpty().WithMessage("Phone cannot be empty.");

            RuleFor(user => user.Name)
                .NotEmpty().WithMessage("Name cannot be empty.");

            RuleFor(user => user.Surname)
                .NotEmpty().WithMessage("Surname cannot be empty.");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password cannot be empty.").Must(Password=>IsHaveBigLetter(Password)).WithMessage("Password must contain at least 1 big letter,1 small letter,1 number and,1 special character").Must(Password => IsHaveSmallLetter(Password)).WithMessage("Password must contain at least 1 big letter,1 small letter,1 number and,1 special character")
                .Must(Password => IsHaveNumber(Password)).WithMessage("Password must contain at least 1 big letter,1 small letter,1 number and,1 special character").Must(Password => IsHaveSpecial(Password)).WithMessage("Password must contain at least 1 big letter,1 small letter,1 number and,1 special character");
        }
        private bool IsHaveSmallLetter(string password)
        {
            int i = 0;
            char[] smallletter = { 'a', 'b', 'c', 'd', 'e', 'f','g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'r', 's', 't', 'u', 'v', 'y', 'z', 'x', 'w', 'q' };
            foreach (char s in smallletter)
            {
                if (password.Has(s)) ;
                i++;
            }
            if (i == 0) return false;
            else return true;
        }
        private bool IsHaveBigLetter(string password)
        {
            int i = 0;
            char[] bigletter = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'R', 'S', 'T', 'U', 'V', 'Y', 'Z', 'X', 'W', 'Q' };
            foreach (char s in bigletter)
            {
                if (password.Has(s)) ;
                i++;
            }
            if (i == 0) return false;
            else return true; 

        }
            private bool IsHaveNumber(string password)
        {
            int i = 0;
            char[] number = { '0', '1', '2', '3', '4', '5','6','7','8','9' };
            foreach (char s in number)
            {
                if (password.Has(s)) ;
                i++;
            }
            if(i == 0) return false;
            else return true;
        }
        private bool IsHaveSpecial(string password)
        {
            int i = 0;
            char[] special = { '"', '!', '^', '+', '%', '&', '/', '(', ')', '=','?','_',';',':','>','*','-','.','<','#','$','{','[',']','}','~','¨','`','´','|','@','€','é' };
            foreach (char s in special)
            {
                if (password.Has(s)) ;
                i++;
            }
            if (i == 0) return false;
            else return true;
        }
    }
}
