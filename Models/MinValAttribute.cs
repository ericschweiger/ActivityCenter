using System;
using System.ComponentModel.DataAnnotations;
public class MinValAttribute : ValidationAttribute{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext){
        if(value != null && (int)value < 1){
            return new ValidationResult("Your event has to be at least 1 hour in length");
        }
        return ValidationResult.Success;
    }
}