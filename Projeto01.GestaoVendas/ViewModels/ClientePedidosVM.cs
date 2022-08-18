using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projeto01.GestaoVendas.ViewModels
{
    public class ClientePedidosVM
    {
        //Instanciando todos os campos que você deseja para a consulta. 
        //Aqui por exemplo, queremos dois campos da class Cliente e três campos da class Pedidos

        [Display(Name ="Documento")]
        public string DocCliente { get; set; }

        [Display(Name ="Cliente")]
        public string NomeCliente { get; set; }
        
        [Display(Name ="Id Pedido")]
        public int IdPedido { get; set; }
        [Display(Name ="Num. Pedido")]
        public string NumeroPedido { get; set; }
        [Display(Name ="Data Pedido")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DataPedido { get; set; }
        [Display(Name ="Qtde. Itens")]
        public int QtdItens { get; set; }
    }
}