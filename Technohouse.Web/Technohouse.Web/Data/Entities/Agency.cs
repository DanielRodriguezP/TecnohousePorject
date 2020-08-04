using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Technohouse.Web.Data.Entities
{
    public class Agency
    {
        [Key]
        public int AgencyId { get; set; }

        [Display(Name = "Name Agency")]
        [Required(ErrorMessage = "The Name Agency is required")]
        [RegularExpression(@"^[a-zA-Z]{50}$", ErrorMessage = "Characters are not allowed.")]
        public string NameAgency { get; set; }

        [Display(Name = "Address Agency")]
        [Required(ErrorMessage = "The Address is required")]
        [RegularExpression(@"^[a-zA-Z0-9#-]{50}$", ErrorMessage = "Characters are not allowed.")]
        public string AddressAgency { get; set; }

        [Display(Name = "Cell Phone")]
        [Required(ErrorMessage = "The Cell Phone is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Characters are not allowed.")]
        public int Cellphone { get; set; }

        public ActingZone ActingZones { get; set; }

        public ICollection<Property> Properties { get; set; }
    }
}
