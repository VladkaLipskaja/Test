using System;

namespace WebApplication1.Models
{
    public class URLUpdate
    {
        public int ID { get; set; }
        public int URLId { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}