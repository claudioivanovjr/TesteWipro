using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesteWebApi.Models
{
    public class ItemModel
    {
        public string moeda { get; set; }
        public DateTime data_inicio { get; set; }
        public DateTime data_fim { get; set; }
    }
}