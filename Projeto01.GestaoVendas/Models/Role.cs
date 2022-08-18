using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projeto01.GestaoVendas.Models
{
    public class Role
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name ="Descrição")]
        public string Descricao{ get; set; }

    }
}