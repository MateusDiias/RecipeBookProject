using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Exceptions;
using Shouldly;

namespace Validators.Test.User.Update
{
    public class UpdateUserValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new UpdateUserValidator();

            var request = RequestUpdateUserJson.Build();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Error_Name_Empty()
        {
            var validator = new UpdateUserValidator();

            var request = RequestUpdateUserJson.Build();
            request.Name = string.Empty;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
            errors => errors.ShouldHaveSingleItem(),
            error => error.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.NAME_EMPTY)));
        }

        [Fact]
        public void Error_Email_Empty()
        {
            var validator = new UpdateUserValidator();

            var request = RequestUpdateUserJson.Build();
            request.Email = string.Empty;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
            errors => errors.ShouldHaveSingleItem(),
            error => error.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.EMAIL_EMPTY)));
        }

        [Fact]
        public void Error_Email_Invalid()
        {
            var validator = new UpdateUserValidator();

            var request = RequestUpdateUserJson.Build();
            request.Email = "emailInvalido";

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();

            result.Errors.ShouldSatisfyAllConditions(
            errors => errors.ShouldHaveSingleItem(),
            error => error.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.EMAIL_INVALID)));
        }
    }
}
