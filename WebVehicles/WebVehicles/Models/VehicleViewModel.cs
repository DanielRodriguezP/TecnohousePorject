using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebVehicles.Models
{
    public class VehicleViewModel
    {
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [MaxLength(6, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Display(Name = "License Plate")]
        public string LicensePlate { get; set; }
        [Display(Name = "Color")]
        [MaxLength(12, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string Color { get; set; }
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Model")]
        public int Model { get; set; }
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Admission")]
        public DateTime DateAdmission { get; set; }
        public decimal Value { get; set; }
        [Display(Name = "Photo Url")]
        public string PhotoUrl { get; set; }
        [Display(Name = "Photo")]
        public Guid PhotoId { get; set; }
        [Display(Name = "Photo")]
        public string PhotoFullPath => PhotoId == Guid.Empty
           ? $"https://localhost:44353/images/noimage.png"
           : $"https://localhost:44353/images/{PhotoId}";

        [Display(Name = "Date Admission")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateLocal => DateAdmission.ToLocalTime();
    }
}
