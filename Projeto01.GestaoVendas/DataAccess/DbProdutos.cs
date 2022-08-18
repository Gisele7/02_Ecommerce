using Projeto01.GestaoVendas.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Projeto01.GestaoVendas.DataAccess
{
    public static class DbProdutos
    {
        public static void Incluir(Produto produto)
        {
            using (var ctx = new DBVendasEntities())
            {
                ctx.Produtos.Add(produto);
                ctx.SaveChanges();
            }
        }
        public static Produto Buscar(int id)
        {
            using (var ctx = new DBVendasEntities())
            {
                return ctx.Produtos.FirstOrDefault(p => p.Id == id);
            }
            
        }
        public static IEnumerable<Categoria> ListarCategorias()
        {
            using (var ctx = new DBVendasEntities())
            {
                return ctx.Categorias.ToList();
            }
        }
        public static IEnumerable<Produto> Listar()
        {
            using (var ctx = new DBVendasEntities())
            {
                return ctx.Produtos.ToList();
            }
        }
        public static void Alterar(Produto produto)
        {
            using (var ctx = new DBVendasEntities())
            {
                ctx.Entry(produto).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }
        public static void Remover (Produto produto)
        {
            using (var ctx = new DBVendasEntities())
            {
                ctx.Entry(produto).State = EntityState.Deleted;
                ctx.SaveChanges();
            }
        }
    }
}