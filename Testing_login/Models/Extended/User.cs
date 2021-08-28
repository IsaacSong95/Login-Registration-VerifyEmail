using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Testing_login.Models
{

    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        public string ConfirmPassword { get; set; }
    }

    public class UserMetadata
    {
        [Display(Name ="Frist Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name required")]
        public string FristName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage ="Last Name required")]
        public string LastName { get; set; }

        [Display(Name = "Email ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email required")]
        public string EmailID { get; set; }

        [Display(Name = "Date of birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString ="{0:MM/dd/yyyy}")]
        public DateTime DateOfBirth { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage ="Minimum 6 characters required")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm Password and password do not match")]
        public string ConfirmPassword { get; set; }




    }

}