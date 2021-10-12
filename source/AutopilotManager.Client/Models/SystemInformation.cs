using System;
using System.ComponentModel.DataAnnotations;

namespace AutopilotManager.Models
{
    public class SystemInformation
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Serial number")]
        public string SerialNumber { get; set; }

        [Required]
        [Display(Name = "Hardware hash")]
        public string HardwareHash { get; set; }

        [Required]
        [Display(Name = "Manufacturer")]
        public string Manufacturer { get; set; }

        [Required]
        [Display(Name = "Model")]
        public string Model { get; set; }

        [Display(Name = "Group Tag")]
        public string GroupTag { get; set; }

        [Display(Name = "Action")]
        public string Action { get; set; }

        public override string ToString()
        {
            return $"{Id},{GroupTag},{Manufacturer},{Model},{SerialNumber},{HardwareHash}";
        }
    }
}
