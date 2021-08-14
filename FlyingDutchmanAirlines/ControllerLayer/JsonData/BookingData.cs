using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlyingDutchmanAirlines.ControllerLayer.JsonData
{
    public class BookingData : IValidatableObject
    {
        private string _firstName;
        private string _lastName;

        public string FirstName
        {
            get => _firstName;
            set => _firstName = ValidateName(value, nameof(FirstName));
        }

        public string LastName
        {
            get => _lastName;
            set => _lastName = ValidateName(value, nameof(LastName));
        }

        private string ValidateName(string name, string propertyName)
        {
            return string.IsNullOrEmpty(name) 
                ? throw new ArgumentException("Could not set " + propertyName) 
                : name;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new();

            if (FirstName == null && LastName == null)
            {
                validationResults.Add(new ValidationResult("Both names are null"));
            }
            else if (FirstName == null || LastName == null)
            {
                validationResults.Add(new ValidationResult("One of the names are null"));
            }

            return validationResults;
        }
    }
}