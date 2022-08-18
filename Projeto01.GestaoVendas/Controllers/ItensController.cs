using Projeto01.GestaoVendas.DataAccess;
using Projeto01.GestaoVendas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projeto01.GestaoVendas.Controllers
{
    public class ItensController : Controller
    {
        // GET: Itens
        public ActionResult Index()
        {
            return View();
        }
        //Action para adicionar novos itens
        public ActionResult AdicionarItem(int? idPedido)
        {
            try
            {
                if (idPedido == null)
                {
                    //throw new Exception("É necessário fornecer um código do pedido adequadamente.");
                    return RedirectToAction("Listar", "Pedidos");
                }
                //dados do pedido a serem exibidos
                var pedido = DbPedidos.BuscarPedidoId((int)idPedido);
                ViewBag.Pedido = pedido;

                //lista de produtos para o componente dropdownList
                var produtos = DbProdutos.Listar();
                ViewBag.ListaProdutos = new SelectList(produtos, "Id", "Descricao");

                //Lista de itens referente ao pedido informado
                var itens = DbItens.ListarPorPedido((int)idPedido);
                ViewBag.ListaItens = itens;

                Item item = new Item();
                item.IdPedido = pedido.IdPedido;

                return View(item);
            }
            catch (Exception ex)
            {
                var mensagem = ex.Message;
                if (ex.InnerException != null)
                {
                    mensagem += " - " + ex.InnerException.Message;
                }
                ViewBag.MensagemErro = mensagem;
                return View("_Error");
            }

        }
        [HttpPost]
        public ActionResult AdicionarItem(Item item)
        {
            if (!ModelState.IsValid)
            {
                return AdicionarItem(item.IdPedido);
            }
            try
            {
                DbItens.Incluir(item);
                return Redirect("/Itens/AdicionarItem?idPedido=" + item.IdPedido);
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.ToString();
                return View("_Error");
            }
            
        }
        public ActionResult Remover(int id)
        {
            Item item = DbItens.BuscarItem(id);
            int idPedido = item.IdPedido;
            DbItens.RemoverItem(item);
            return RedirectToAction("AdicionarItem", new { idPedido = item.IdPedido });
        }
    }
}