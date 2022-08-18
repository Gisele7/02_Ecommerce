using Newtonsoft.Json;
using Projeto01.GestaoVendas.DataAccess;
using Projeto01.GestaoVendas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Projeto01.GestaoVendas.Controllers
{
    public class PagamentosController : Controller
    {
        HttpClient client;
        string uriPagamentos = "http://localhost:53610/"; //é da api.

        public PagamentosController()
        {
            if(client == null)
            {
                client = new HttpClient();
                //baseAdress é uma prorpriedade URI, URI é um obj, uma classe.
                client.BaseAddress = new Uri(uriPagamentos);
                //Configurar o formato de envio e de recebimento (JSON)
                client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));

            }
        }

        // GET: Pagamentos
        public ActionResult Index()
        {
            return View();
        }

        //GET 
        public ActionResult EfetuarPagamento()
        {
            ViewBag.ListaPedidos = new SelectList(
            DbPedidos.ListarTodosPedidos(), "NumeroPedido", "NumeroPedido");
            
            return View();
        }
        [HttpPost]
        //async quer dizer que ele é um método assíncrono
        public async Task<ActionResult> EfetuarPagamento(Pagamento pagamento)
        {
            if (!ModelState.IsValid)
            {
                return EfetuarPagamento();
            }
            else
            {
                try
                {
                    pagamento.DataPagamento = DateTime.Now;
                    pagamento.Status = 0;
                    pagamento.Valor = DbItens.SomarItensPedido(DbPedidos.BuscarIdPedido(pagamento.NumeroPedido));

                    //obtendo o objeto Json a partir da instância da classe
                    string json = JsonConvert.SerializeObject(pagamento);

                    //obtendo o stream de bytes a partir do Json gerado anteriormente
                    HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

                    //enviando o objeto que ja está em formato de stream para a api
                    var response = await client.PostAsync("pagamentos/efetuar", content);
                    //pode fazer desse jeito também
                    //var response = new StringContent(json, Encoding.Unicoce, "application/json").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.MensagemOK = "Pagamento efetuado com sucesso!";
                        return View("_Success");
                    }
                    else
                    {
                        string erro = "Status: " + response.StatusCode + " - " + response.ReasonPhrase;
                        throw new Exception(erro);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.MensagemErro = ex.Message;
                    return View("_Error");
                }
            }
        }

        //variavel
        static List<Pagamento> pagamentos = new List<Pagamento>();
        public async Task<ActionResult> ListarPagamentos()
        {
            try
            {
                //Obtendo o stream de bytes
                HttpResponseMessage response = client.GetAsync("pagamentos/lista").Result;

                if (response.IsSuccessStatusCode)
                {
                    //Obtendo a resposta como string (json)
                    var resultado = await response.Content.ReadAsStringAsync();

                    //obtenho em forma de array / pegando o json e transformando em objeto
                    var lista = JsonConvert
                        .DeserializeObject<Pagamento[]>(resultado).ToList();

                    //para eu ter uma lista local, em memória
                    pagamentos = lista;

                    return View(lista);
                }
                else
                {
                    string erro = "Status: " + response.StatusCode + " - " + response.ReasonPhrase;
                    throw new Exception(erro);
                }
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                return View("_Error");
            }
        }
        public ActionResult EfetivarPagamento(int id)
        {
            Pagamento pagamento = pagamentos.Find(p => p.PagamentoId == id);
            return View(pagamento);
        }
        [HttpPost]
        public async Task<ActionResult>EfetivarPagamento(Pagamento pagamento)
        {
            try
            {
                pagamento = pagamentos.Find(p => p.PagamentoId == pagamento.PagamentoId);
                pagamento.Status = 1;

                string json = JsonConvert.SerializeObject(pagamento);
                HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

                var response = await client.PutAsync("pagamentos/alterar", content);

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.MensagemOK = "Pagamento efetivado com sucesso!";
                    return View("_Success");
                }
                else
                {
                    string erro = "Status: " + response.StatusCode + " - " + response.ReasonPhrase;
                    throw new Exception(erro);
                }
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                return View("_Error");
            }
        }

        public ActionResult IncluirCartao()
        {
            return View();
        }
    }
}