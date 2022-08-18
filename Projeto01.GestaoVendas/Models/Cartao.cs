using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projeto01.GestaoVendas.Models
{
    public class Cartao
    {
        public int Id { get; set; }
        public string NumeroCartao { get; set; }
        public string Titular { get; set; }
        public double Limite { get; set; }
    }
}