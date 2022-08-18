using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projeto01.GestaoVendas.ViewModels
{
    public class ItensPedidoVM
    {
        public int IdItem { get; set; }
        public string NumeroPedido { get; set; }
        public string DescricaoProduto{ get; set; }
        public double Quantidade { get; set; }
        public double Preco{ get; set; }
        public double ValorTotal { get; set; }

    }
}