using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Technohouse.Web.Data.Entities
{
    public class Owner
    {
        [Key]
        public int CodeId { get; set; }

        [Required(ErrorMessage = "The Name Property is Required")]
        [RegularExpression(@"^[a-zA-Z]{50}$", ErrorMessage = "Characters are not allowed.")]
        [Display(Name = "Name Property")]
        public string NameProperty { get; set; }

        [Required(ErrorMessage = "The Address Property is Required")]
        [RegularExpression(@"^[a-zA-Z]{50}$", ErrorMessage = "Characters are not allowed.")]
        [Display(Name = "Name Property")]
        public string AddressProperty { get; set; }

        [Required(ErrorMessage = "The Surface is Required")]
        [RegularExpression(@"^[a-zA-Z]{50}$", ErrorMessage = "Characters are not allowed.")]
        [Display(Name = "Name Property")]
        public string Surface { get; set; }

        public ICollection<Property> Properties { get; set; }
    }
}
