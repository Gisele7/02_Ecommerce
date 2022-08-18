using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projeto01.GestaoVendas.Models
{
    public class Pagamento
    {
        public int PagamentoId { get; set; }
        [Display(Name = "Número Pedido")]

        public string NumeroPedido { get; set; }
        [Display(Name = "Número Cartão")]
        [StringLength(16, MinimumLength = 16)]
        public string NumeroCartao { get; set; }
        public DateTime DataPagamento { get; set; }
        [DataType(DataType.Currency)]
        public double Valor { get; set; }
        public int Status { get; set; }
    }
}