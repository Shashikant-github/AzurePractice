using System;
using System.Collections.Generic;
using System.Text;

namespace Image2Storage.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string[] Tags { get; set; }
        public string Photo { get; set; }
    }
}
