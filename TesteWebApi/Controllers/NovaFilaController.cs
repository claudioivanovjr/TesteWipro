using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using TesteWebApi.Models;

namespace TesteWebApi.Controllers
{
    public class NovaFilaController : Controller
    {
        List<ItemModel> itens = new List<ItemModel>();

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult AddItemFila(string itensJson)
        {
            var itensModel = JsonConvert.DeserializeObject<List<ItemModel>>(itensJson);

            itens.AddRange(itensModel.Where(w => !string.IsNullOrEmpty(w.moeda)));

            return PartialView(itensModel);
        }

        public ActionResult GetItemFila()
        {
            var ultimoItem = itens.LastOrDefault();

            if (ultimoItem != null)
            {
                itens.Remove(ultimoItem);
                return PartialView(ultimoItem);
            }
            else
            {
                ViewBag.msgErro = "Nenhum item na fila.";
                return PartialView();
            }
        }
    }
}
