using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class ItemApiController : ApiController
    {
        List<ItemModel> itens = new List<ItemModel>();

        public ItemModel GetItemFila()
        {
            var ultimo = itens.LastOrDefault();

            itens.Remove(ultimo);

            return ultimo;
        }

        public void AddItemFila([FromBody] string value)
        {
            var itensJson = JsonConvert.DeserializeObject<List<ItemModel>>(value);

            itens.AddRange(itensJson);
        }

    }
}
