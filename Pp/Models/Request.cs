using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pp.Models
{
    public class Request
    {
        public int Id { get; set; }
        public string Selection { get; set; }
        public string CustomMessage { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual ICollection<Window> Windows { get; set; }
        public virtual ICollection<Door> Doors { get; set; }
    }

    public class Window
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Material { get; set; }
        public string Color { get; set; }
        public string Type { get; set; }
        public string Wing { get; set; }
    }

    public class Door
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Material { get; set; }
        public string Color { get; set; }
        public string Type { get; set; }
    }
}