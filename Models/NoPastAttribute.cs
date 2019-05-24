using System;
using System.ComponentModel.DataAnnotations;
public class NoPastAttribute : ValidationAttribute{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext){
        if(value != null && (DateTime)value < DateTime.Now){
            return new ValidationResult("No Past Dates Allowed!");
        }
        return ValidationResult.Success;
    }
}