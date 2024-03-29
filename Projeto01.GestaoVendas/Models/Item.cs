//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Projeto01.GestaoVendas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Item
    {
        
        public int Id { get; set; }
        [Display(Name = "Pedido")]
        public int IdPedido { get; set; }
        [Display(Name ="Produto")]
        public int IdProduto { get; set; }
        [Required]
        //Minimo de compra � 1 produto e paximo � 100.
        [Range(1, 100)]
        public double Quantidade { get; set; }
    
        public virtual Pedido PedidoInfo { get; set; }
        public virtual Produto ProdutoInfo { get; set; }
    }
}
