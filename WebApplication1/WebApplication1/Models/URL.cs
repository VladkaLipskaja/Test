using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class URL
    {
        public int ID { get; set; }
        public string URLPath { get; set; }
        public DateTime CreationDate { get; set; }
        public string LastTitle { get; set; }
        public int LastStatus { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public ICollection<URLUpdate> Updates { get; set; }
    }
}