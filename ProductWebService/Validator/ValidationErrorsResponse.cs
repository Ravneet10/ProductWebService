using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFNValidationApp.Validator
{
    public class ValidationErrorsResponse
    {
        public IList<ValidationError> Errors { get; set; }

        /// <summary>
        /// Fluent Validation errors
        /// </summary>
        public ValidationErrorsResponse(ValidationResult validationResults)
        {
            Errors = Map(validationResults);
        }

        /// <summary>
        /// Custom errors (Not Fluent Validations)
        /// </summary>
        public ValidationErrorsResponse(ValidationError error)
        {
            Errors = new List<ValidationError> { error };
        }

        public List<ValidationError> Map(ValidationResult validationResult)
        {
            return validationResult.Errors
                .Select(e => new ValidationError
                {
                    Code = e.ErrorCode,
                    PropertyName = e.PropertyName,
                    Description = e.ErrorMessage
                }).ToList();
        }
    }

    public class ValidationError
    {
        public ValidationError() { }

        public ValidationError(string code, string propertyName, string description)
        {
            Code = code;
            PropertyName = propertyName;
            Description = description;
        }
        public string Code { get; set; }
        public string PropertyName { get; set; }
        public string Description { get; set; }
    }
}
