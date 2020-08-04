using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Technohouse.Web.Data.Entities
{
    public class User
    {
        [Key]
        public int Identification { get; set; }

        [Display(Name = "Firts Name")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [RegularExpression(@"^[A-Z-a-z]{50}$", ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string FirtsName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [RegularExpression(@"^[A-Z-a-z]{50}$", ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [RegularExpression(@"^[0-9]{100}$", ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [RegularExpression(@"^[A-Z-a-z]{50}$", ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string Profile { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [RegularExpression(@"^[A-Z-a-z]{50}$", ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [RegularExpression(@"^([A-Z-a-z0-9#\*])([^ ]){4,8}$", ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string Password { get; set; }
        
        [Display(Name = "Image Url")]
        public string ImageUrl { get; set; }
    }
}
