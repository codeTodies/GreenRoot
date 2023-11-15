using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnNhom1.ViewModels
{
    public class CombineViewModel
    {
        public IEnumerable<DoAnNhom1.Models.AboutUs> AboutUsData { get; set; }
        public IEnumerable<DoAnNhom1.Models.Region> RegionsData { get; set; }
    }
}