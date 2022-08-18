using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projeto01.GestaoVendas.Models
{
    public class NovoUsuario
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
        [Required]
        [Compare("Senha")] //para comparar se é igual a senha
        [DataType(DataType.Password)]//para mascarar com senha e nao ficar visivel
        public string Confirma { get; set; }
        
        [Display(Name ="Perfil")]
        public string RoleName { get; set; }
    }
}