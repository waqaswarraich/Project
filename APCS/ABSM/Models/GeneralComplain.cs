using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABSM.Models
{
    public class GeneralComplain
    {
        public int ID { get; set; }

        [Required]
        public string Subject { get; set; }

        [Display(Name ="Recipt")]
        public string ImageUrl { get; set; }
        

        public string Email { get; set; }

        public string Status { get; set; }

        [Required]
        public string Description { get; set; }

    }
}