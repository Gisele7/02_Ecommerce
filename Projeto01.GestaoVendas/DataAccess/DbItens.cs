using Projeto01.GestaoVendas.Models;
using Projeto01.GestaoVendas.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Projeto01.GestaoVendas.DataAccess
{
    public static class DbItens
    {
        public static void Incluir(Item item)
        {
            using (var ctx = new DBVendasEntities())
            {
                ctx.Itens.Add(item);
                ctx.SaveChanges();
            }
        }
        //Linq - Language Integrated Query - Usamos apenas para consultas
        public static IEnumerable<ItensPedidoVM> ListarPorPedido(int idPedido)
        {
            using (var ctx = new DBVendasEntities())
            {
                //Estamos criando um objeto de ctx Produtos. Criando uma referencia para produtos
                //var p = (from prod in ctx.Produtos select prod).ToList();
                //Usamos equals quando vamos fazer uma comparação entre propriedades das entidades, internamente.
                //Usamos o == quando vamos fazer uma comparação com algum elemento externo

                var itens = from pedido in ctx.Pedidos
                            join item in ctx.Itens
                            on pedido.Id equals item.IdPedido
                            join produto in ctx.Produtos
                            on item.IdProduto equals produto.Id
                            where pedido.Id == idPedido
                            select new ItensPedidoVM
                            {
                                IdItem = item.Id,
                                NumeroPedido = pedido.NumeroPedido,
                                DescricaoProduto = produto.Descricao,
                                Preco = produto.Preco,
                                Quantidade = item.Quantidade,
                                ValorTotal = produto.Preco * item.Quantidade,

                                
                            };
                return itens.ToList();
            
            }
        }

        public static double SomarItensPedido(int idPedido)
        {
            return ListarPorPedido(idPedido).Sum(p => p.ValorTotal) ;
        }

        public static void RemoverItem(Item item)
        {
            using (var ctx = new DBVendasEntities())
            {
                ctx.Entry(item).State = EntityState.Deleted;
                ctx.SaveChanges();
            }
        }
        public static Item BuscarItem(int id)
        {
            using (var ctx = new DBVendasEntities())
            {
                return ctx.Itens.FirstOrDefault(p => p.Id == id);
            }
        }
    }
}