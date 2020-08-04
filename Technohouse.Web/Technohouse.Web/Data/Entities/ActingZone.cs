using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Technohouse.Web.Data.Entities
{
    public class ActingZone
    {
        [Key]
        public int ZoneId { get; set; }

        [Required(ErrorMessage = "The Zone is Required")]
        [RegularExpression(@"^[a-zA-Z]{50}$", ErrorMessage = "Characters are not allowed.")]
        [Display(Name = "Name Zone")]
        public int NameZone { get; set; }


        public ICollection<Agency> Agencies { get; set; }
    }
}
