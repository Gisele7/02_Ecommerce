using Projeto01.GestaoVendas.Models;
using Projeto01.GestaoVendas.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Projeto01.GestaoVendas.DataAccess
{
    public class DbPedidos
    {
        //Método para incluir um cliente no banco de dados
        public static void Incluir(Pedido pedido)
        {
            using (var ctx = new DBVendasEntities())
            {
                ctx.Pedidos.Add(pedido);
                ctx.SaveChanges();
            }
        }
        //referencie a lista da class de consulta
        //Se não passar parâmetro ele lista todos os clientes, sem filtro nenhum. Colocamos o parâmetro como opcional, colocando o "=null"
        public static IEnumerable<ClientePedidosVM> ListarPedidos(string doc = null)
        {
            using (var ctx = new DBVendasEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                //Primeiro parâmetro do Join é a coleção com a qual eu tô fazendo o Join ou seja a entidade secundária.
                //Segundo parâmetro seria a propriedade que representa a chave primária da primeira entidade.
                //Terceiro parâmetro seria a propriedade que representa a chave estrangeira na segunda entidade.
                //Quarto parâmetro é uma função, que irá definir um objeto.
                var pedidos = new List<ClientePedidosVM>(); //Nós so vamos pegar essa lista de clientes, se o doc for diferente de nulo
                //Ou seja, ou vai retornar uma lista vazia, ou vai retornar a lista com os pedidos daquele cliente
                if (!string.IsNullOrEmpty(doc))
                {
                    pedidos = ctx.Clientes
                                         .Join(
                                        ctx.Pedidos,
                                        cliente => cliente.Documento,
                                        pedido => pedido.DocCliente,
                                        (cliente, pedido) => new ClientePedidosVM()
                                        {
                                            DocCliente = cliente.Documento,
                                            NomeCliente = cliente.Nome,
                                            IdPedido = pedido.Id,
                                            NumeroPedido = pedido.NumeroPedido,
                                            DataPedido = pedido.Data,
                                            QtdItens = pedido.Itens.Count
                                        }
                                        ).Where(p => p.DocCliente == doc).ToList();
                }

                return pedidos;
                //Operdor ternário. Pedidos é = nulo?Se sim, traz todos os pedidos e se não, retorna o pedido que foi passado.
                //colocamos o isnullorempty para que quando o usuario tente buscar pelo "Todos os clientes" ele filtra por todos.
                //return string.IsNullOrEmpty(doc) ? pedidos : pedidos.Where(p => p.DocCliente == doc);
            }
        }

        public static ClientePedidosVM BuscarPedidoId(int idPedido)
        {
            using (var ctx = new DBVendasEntities())
            {
                var v_pedido = ctx.Clientes
                                         .Join(
                                        ctx.Pedidos,
                                        cliente => cliente.Documento,
                                        pedido => pedido.DocCliente,
                                        (cliente, pedido) => new ClientePedidosVM()
                                        {
                                            DocCliente = cliente.Documento,
                                            NomeCliente = cliente.Nome,
                                            IdPedido = pedido.Id,
                                            NumeroPedido = pedido.NumeroPedido,
                                            DataPedido = pedido.Data
                                        }
                                   ).FirstOrDefault(p => p.IdPedido == idPedido);
                return v_pedido;
            }

        }
        public static void Remover (Pedido pedido)
        {
            using (var ctx = new DBVendasEntities())
            {
                ctx.Entry<Pedido>(pedido).State = EntityState.Deleted;
                ctx.SaveChanges();
            }
        }
        public static Pedido BuscarPedido(int id)
        {
            using (var ctx = new DBVendasEntities())
            {
                return ctx.Pedidos.FirstOrDefault(p => p.Id == id);
            }
        }

        public static IEnumerable<Pedido> ListarTodosPedidos()
        {
            using (var ctx = new DBVendasEntities())
            {
                return ctx.Pedidos.ToList();
            }
        }

        public static int BuscarIdPedido(string numPedido)
        {
            using (var ctx = new DBVendasEntities())
            {
                return ctx.Pedidos.FirstOrDefault(p => p.NumeroPedido == numPedido).Id;
            }
        }
    }
}
