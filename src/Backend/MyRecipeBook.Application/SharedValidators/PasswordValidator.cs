using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.SharedValidators
{
    //109
    public class PasswordValidator<T> : PropertyValidator<T, string>
    {
        public override string Name => "PasswordValidator";

        public override bool IsValid(ValidationContext<T> context, string password)
        {
            if(string.IsNullOrEmpty(password))
            {
                context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.PASSWORD_EMPTY);
                return false;
            }

            if(password.Length < 6)
            {
                context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.PASSWORD_MUST_BE_LONGER_THAN_6_CHARACTERS);
                return false;
            }

            return true;
        }

        protected override string GetDefaultMessageTemplate(string errorCode) => "{ErrorMessage}";
    }
}
