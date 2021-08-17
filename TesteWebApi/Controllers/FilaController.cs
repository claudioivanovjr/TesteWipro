using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TesteWebApi.Models;

namespace TesteWebApi.Controllers
{
    public class FilaController : ApiController
    {
        List<ItemModel> itens = new List<ItemModel>(); 

        //public IEnumerable<ItemModel> Get()
        //{
        //    return itens.AsEnumerable();
        //}

        [HttpGet]
        public IHttpActionResult GetItemFila()
        {
            var ultimoItem = itens.LastOrDefault();
            
            if(ultimoItem != null)
            {
                itens.Remove(ultimoItem);
                return Ok(ultimoItem);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("api/fila/AddItemFila")]
        public HttpResponseMessage AddItemFila([FromBody] string itensJson)
        {
            if (string.IsNullOrEmpty(itensJson))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var novosItens = JsonConvert.DeserializeObject<List<ItemModel>>(itensJson);

            itens.AddRange(novosItens.Where(w => !string.IsNullOrEmpty(w.moeda)));

            var response = Request.CreateResponse(HttpStatusCode.OK, itens);

            return response;
        }
    }
}
