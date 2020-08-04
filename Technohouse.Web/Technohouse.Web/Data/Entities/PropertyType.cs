using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Technohouse.Web.Data.Entities
{
    public class PropertyType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name is Required")]
        [RegularExpression(@"^[a-zA-Z]{50}$", ErrorMessage = "Characters are not allowed.")]
        [Display(Name = "Name")]
        public int Name { get; set; }

        public ICollection<Property> Properties { get; set; }
    }
}
