using Projeto01.GestaoVendas.DataAccess;
using Projeto01.GestaoVendas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projeto01.GestaoVendas.Controllers
{
    //Colocando o authorize aqui, fazemos com que tudo do nosso sistema precise de que o usuário esteja logado.
    [Authorize]
    public class ProdutosController : Controller
    {
        // GET: Produtos
        //Estamos liberando apenas o index
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Incluir()
        {
            //case sensitive. pegamos da classe categorias o tem Id e Descricao

            //Vamos encaminhar p view a lista de categorias pra que quando categoria for selecionada, o id faça parte do produto.
            ViewBag.ListaCategorias = new SelectList(DbProdutos.ListarCategorias(), "Id", "Descricao");
            return View();
        }

        [HttpPost]
        // POSTED DA IMAGEM 
        public ActionResult Incluir(Produto produto, HttpPostedFileBase image)
        {
            if (!ModelState.IsValid)
            {
                return Incluir();
            }
            // IMAGEM 
            if (image != null)
            {
                produto.MimeType = image.ContentType;
                produto.Foto = new byte[image.ContentLength];

                //Copiando os bytes da imagem recebida como parâmetro, na propriedade foto.
                //Parâmetros: 1° Quem vai receber os bytes? | 2° A partir de posição que a gente vai começar a tratar esses bytes? | 3° Tratar até o final 
                image.InputStream.Read(produto.Foto, 0, image.ContentLength);
            }
            DbProdutos.Incluir(produto);
            return RedirectToAction("Listar");
        }
        //IMAGEM 
        public FileResult BuscarProduto(int id)
        {
            try
            {
                var produto = DbProdutos.Buscar(id);
                if (produto != null)
                {
                    if (produto.Foto != null)
                    {
                        return File(produto.Foto, produto.MimeType);
                    }
                }
                    return File("~/Images/ND.jfif", "image/jpeg");                
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public ActionResult Listar()
        {
            return View(DbProdutos.Listar());
        }
        public ActionResult ExibirDetalhes(int id)
        {
            return View(DbProdutos.Buscar(id));
        }
        public ActionResult Alterar(int id)
        {
            ViewBag.ListaCategorias = new SelectList(DbProdutos.ListarCategorias(), "Id", "Descricao");
            return View(DbProdutos.Buscar(id));
        }

        [HttpPost]
        public ActionResult Alterar(Produto produto, HttpPostedFileBase image)
        {
            if (!ModelState.IsValid)
            {
                return Alterar(produto.Id);
            }
            // IMAGEM 
            if (image != null)
            {
                produto.MimeType = image.ContentType;
                produto.Foto = new byte[image.ContentLength];

                //Copiando os bytes da imagem recebida como parâmetro, na propriedade foto.
                //Parâmetros: 1° Quem vai receber os bytes? | 2° A partir de posição que a gente vai começar a tratar esses bytes? | 3° Tratar até o final 
                image.InputStream.Read(produto.Foto, 0, image.ContentLength);
            }
            DbProdutos.Alterar(produto);
            return RedirectToAction("Listar");
        }
        public ActionResult Remover (int id)
        {
            return View(DbProdutos.Buscar(id));
        }
        [HttpPost]
        public ActionResult Remover(Produto produto)
        {
            try
            {
                DbProdutos.Remover(produto);
                return RedirectToAction("Listar");
            }
            catch (Exception ex)
            {
                ViewBag.MessageErro = ex.Message;
                return View("_Error");
                
            }
        }

    }
}