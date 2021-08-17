using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class ItemController : Controller
    {
        // GET: Item
        public ActionResult Index()
        {
            var itens = (List<ItemModel>)HttpContext.Session["Itens"];
            if (itens != null && itens.Count > 0)
            {
                return View(itens);
            }

            return View();
        }

        [HttpPost]
        [WebMethod]
        public ActionResult AddItemFila(string jsonEntrada)
        {
            var itens = (List<ItemModel>)HttpContext.Session["Itens"];
            if (itens == null)
            {
                itens = new List<ItemModel>();
            }
            
            if (string.IsNullOrEmpty(jsonEntrada))
            {
                ViewBag.msg = "Favor informar o json com itens a serem cadastrados.";
                return View("index");
                
            }

            var novosItens = JsonConvert.DeserializeObject<List<ItemModel>>(jsonEntrada);

            itens.AddRange(novosItens.Where(w => !string.IsNullOrEmpty(w.moeda)));

            HttpContext.Session["Itens"] = itens;

            return View("index", itens);
        }

        [WebMethod]
        public ActionResult GetItemFila()
        {
            var itens = (List<ItemModel>)HttpContext.Session["Itens"];

            if(itens == null || itens.Count == 0)
            {
                ViewBag.msgBusca = "Nenhum item na lista.";
                return View("index");
            }

            ViewBag.UltimoItem = itens.LastOrDefault();
            itens.Remove(ViewBag.UltimoItem);

            return View("index", itens);
        }
    }
}
