using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Producer.API.Models
{
    public class Message
    {
        public int ID { get; set; }
        public string Body { get; set; }
    }
}