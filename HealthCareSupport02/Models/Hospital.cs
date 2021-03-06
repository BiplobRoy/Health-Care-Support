//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HealthCareSupport02.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Hospital
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Hospital Name")]
        public string Name { get; set; }

        [Display(Name = "City")]
        public int CityId { get; set; }

        [Display(Name = "Sub City")]
        public int SubCItyId { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Website")]
        [DataType(DataType.Url)]
        public string WebSite { get; set; }
        public string Specialization { get; set; }
        public string Details { get; set; }
    
        public virtual City City { get; set; }
        public virtual SubCity SubCity { get; set; }
    }
}
