using PagedList;
using Projeto01.GestaoVendas.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Projeto01.GestaoVendas.DataAccess
{
    public static class DbClientes
    {
        //Método para incluir um cliente no banco de dados
        public static void Incluir(Cliente cliente)
        {
            using (var ctx = new DBVendasEntities())
            {
                ctx.Clientes.Add(cliente);
                ctx.SaveChanges();
            }
        }

        //Método para listar todos os clientes
        //Vamos colocar dentro dos parenteses justamente o número de páginas
        //para não tornar esse dado(pagina) não obrigatório colocamos o "=1", pois sempre quando chamarmos a listagem de clientes, ele trará sempre a primeira pagina
        public static IEnumerable<Cliente> Listar(int pagina = 1)
        {
            using (var ctx = new DBVendasEntities())
            {
                //return ctx.Clientes.ToList();
                //estamos listando a lista por ordem de NOME e colocamos 5 dados por pagina
                return ctx.Clientes.OrderBy(x => x.Nome).ToPagedList(pagina, 5);
            }
        }
        //Método para buscar um cliente pelo documento
        public static Cliente Buscar(string documento)
        {
            using (var ctx = new DBVendasEntities())
            {
                //FirstOrDefault se for encontrado na lista, na coleção de clientes, um cliente com o documento informado no método, esse obj cliente correspondente é retornado
                //E se não for encontrado ele retorna um valor default, que é nulo.
                return ctx.Clientes.FirstOrDefault(p => p.Documento == documento);
            }
        }
        //Método para alterar um cliente com base no documento
        public static void Alterar (Cliente cliente)
        {
            using (var ctx = new DBVendasEntities())
            {
                ctx.Entry<Cliente>(cliente).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }
        //Método para excluir um cliente, com base no documento
        public static void Excluir(Cliente cliente)
        {
            using (var ctx = new DBVendasEntities())
            {
                ctx.Entry<Cliente>(cliente).State = EntityState.Deleted;
                ctx.SaveChanges();
            }
        }
    }
}