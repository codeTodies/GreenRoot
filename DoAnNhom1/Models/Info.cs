using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnNhom1.Models
{
    public class Info
    {
        public Info()
        {
            this.ImgInfo = "~/image/BatTay.jpg";
        }
        [Key]
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string ImgInfo { get; set; }
        [Required]
        public string Content { get; set; }
        [NotMapped]
        public HttpPostedFileBase UploadImage { get; set; }
    }
}