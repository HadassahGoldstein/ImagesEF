using System;
using System.Collections.Generic;
using System.Text;

namespace ImagesEF.Data
{
    public class Image
    {
        public string Address { get; set; }
        public string Title { get; set; }
        public DateTime DateUploaded { get; set; }
        public int Likes { get; set; }
        public int Id { get; set; }
    }
}
