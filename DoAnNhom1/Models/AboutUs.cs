using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoAnNhom1.Models
{
    public class AboutUs
    {
        public AboutUs()
        {
            this.ImgUs = "~/image/vphs.jpg";
        }
        [Key]
        public int ID { get; set; }
        [Required]
        public string ImgUs { get; set; }
        [Required]
        public string NameUs { get; set; }
        [Required]
        public string RoleUs { get; set; }
        [Required]
        public string DescriptionUs { get; set; }
        [NotMapped]
        public HttpPostedFileBase UploadImage { get; set; }
    }
}