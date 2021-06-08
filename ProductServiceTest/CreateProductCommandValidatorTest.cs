using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductWebService.Command;
using ProductWebService.Validator;
using System;

namespace ProductServiceTest
{
    [TestClass]
    public class CreateProductCommandValidatorTest
    {
        private readonly CreateProductCommandValidator _validator;
        private readonly CreateProductCommand _command;

        public CreateProductCommandValidatorTest()
        {
           
            _validator = new CreateProductCommandValidator();
            _command = new CreateProductCommand() { };
        }

        [TestMethod]
        public void PassingInvalidCommandThrowsException()
        {
            _validator.ShouldNotHaveValidationErrorFor(l => l.Id, _command);
            _validator.ShouldHaveValidationErrorFor(l => l.Name, _command);
            _validator.ShouldHaveValidationErrorFor(l => l.Description, _command);
        }
        [TestMethod]
        public void PassingInvalidDataForDescription_ThrowsException()
        {
            _command.Id = Guid.NewGuid();
            _command.Name = "TestData";
            _validator.ShouldHaveValidationErrorFor(l => l.Description, _command);
        }
        [TestMethod]
        public void PassingExceedingLimit_ThrowsException()
        {
            _command.Id = Guid.NewGuid();
            _command.Name = "TestData";
            _command.Description = "TestDataTestDataTestDataTestDataTestDataTestData";
            _validator.ShouldHaveValidationErrorFor(l => l.Description, _command);
        }
        [TestMethod]
        public void PassingValidData_ThrowsNoException()
        {
            _command.Id = Guid.NewGuid();
            _command.Name = "TestData";
            _command.Description = "TestData";
            _validator.ShouldNotHaveValidationErrorFor(l => l.Description, _command);
            _validator.ShouldNotHaveValidationErrorFor(l => l.Name, _command);
            _validator.ShouldNotHaveValidationErrorFor(l => l.Id, _command);
        }
    }
}
