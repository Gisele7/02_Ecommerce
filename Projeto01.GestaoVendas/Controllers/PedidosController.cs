using Projeto01.GestaoVendas.DataAccess;
using Projeto01.GestaoVendas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projeto01.GestaoVendas.Controllers
{
    public class PedidosController : Controller
    {
        // GET: Pedidos
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Incluir()
        {
            //Viewbag para a lista de clientes  2°param: o que iremos selecionar - 3°param: o que iremos exibir
            ViewBag.ListaClientes = new SelectList(DbClientes.Listar(), "Documento", "Nome");
            return View();
        }
        [HttpPost]
        public ActionResult Incluir(Pedido pedido)
        {
            pedido.Data = DateTime.Now; 
            if (!ModelState.IsValid)
            {
                return Incluir();
            }
            else
            {
                DbPedidos.Incluir(pedido);
                return RedirectToAction("Index");
            }
        }
        //n colocar o "id" fara com que o documento nao ficara na url?
        public ActionResult Listar(string doc)
        {
            ViewBag.ListaClientes = new SelectList(DbClientes.Listar(), "Documento", "Nome");
            return View(DbPedidos.ListarPedidos(doc));
        }
        //Método lista ajax
        public ActionResult ListarAjax(string id)
        {
            //A primeira vez que ele for executado, não vai ser ajax, iremos mandar a lista de clientes.

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListaPedidos", DbPedidos.ListarPedidos(id));
            }
            else
            {
                ViewBag.ListaClientes = new SelectList(DbClientes.Listar(), "Documento", "Nome");
                return View();
            }
            
        }
       
        public ActionResult RemoverPedido(int id)
        {
            return View(DbPedidos.BuscarPedido(id));
        }
        [HttpPost]
        public ActionResult RemoverPedido(Pedido pedido)
        {
            var documento = pedido.DocCliente;
            DbPedidos.Remover(pedido);
            //para retornar no mesmo cliente selecionado
            return RedirectToAction("Listar", new { doc = documento });
        }
    }

}