using ImagesEF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagesEF.Web.Models
{
    public class ImagesViewModel
    {
        public List<Image> Images { get; set; }
    }
    public class ViewImageViewModel
    {
        public Image Image { get; set; }
        public bool Liked { get; set; }
    }
}
