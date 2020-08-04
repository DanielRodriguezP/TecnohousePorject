using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Technohouse.Web.Data.Entities
{
    public class Property
    {
        [Key]
        public int PropertyId { get; set; }

        [Required(ErrorMessage = "The Price is Required")]
        [RegularExpression(@"^[0-9]{20}$", ErrorMessage = "Only twenty numbers are allowed")]
        [Display(Name = "Price")]
        public int Price { get; set; }

        public Owner Owners { get; set; }

        public PropertyType PropertyTypes { get; set; }

        public Agency Agencies { get; set; }
    }
}
