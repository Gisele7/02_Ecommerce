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
    using System.Web.Mvc;

    public partial class Produto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Produto()
        {
            this.Itens = new HashSet<Item>();
        }
    
        public int Id { get; set; }

        [Required]
        [Display(Name ="Categoria")]
        public int IdCategoria { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Descri��o")]
        public string Descricao { get; set; }

        [Required]
        [StringLength(10)]
        public string Unidade { get; set; }
  
        [MaxLength]
        [Display(Name = "Imagem")]
        public byte[] Foto { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string MimeType { get; set; }
        
        [Required]
        [Display(Name = "Pre�o")]
        public double Preco { get; set; }
    
        public virtual Categoria CategoriaInfo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item> Itens { get; set; }
    }
}