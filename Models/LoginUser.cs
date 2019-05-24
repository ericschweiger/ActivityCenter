using System;
using System.ComponentModel.DataAnnotations;

namespace ActivityCenter.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage="Email cant be blank")]
        [EmailAddress]
        public string LoginEmail {get;set;}
        [Required(ErrorMessage="You forgot to enter your password")]
        [DataType(DataType.Password)]
        public string LoginPassword {get;set;}
    }
}
