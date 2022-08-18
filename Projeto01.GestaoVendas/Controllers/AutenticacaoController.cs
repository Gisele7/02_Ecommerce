using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Projeto01.GestaoVendas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projeto01.GestaoVendas.Controllers
{
    public class AutenticacaoController : Controller
    {
        // GET: Autenticacao
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        //só pode cadastrar novos usuarios, somente quem tem nivel admin
        [Authorize(Roles = "ADMIN")]
        public ActionResult Cadastro()
        {
            var roleStore = new RoleStore<IdentityRole>();
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            //definindo uma lista de todas as roles
            var lista = roleManager.Roles.ToList();
            var listaRoles = new List<string>();

            foreach (var item in lista)
            {
                listaRoles.Add(item.Name);
            }
            ViewBag.Roles = new SelectList(listaRoles);     
            return View();
        }
        [HttpPost]
        public ActionResult Cadastro(NovoUsuario usuario)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            //IdentityUser é uma classe de usuário, tecnicamente ele representa uma classe que tem os dados do usuário prontas
            //UserStore é justamente o objeto que faz referência ao Banco de Dados 
            try
            {
                var usuarioStore = new UserStore<IdentityUser>();
                var usuarioManager = new UserManager<IdentityUser>(usuarioStore);

                //Criando uma identidade
                var usuarioInfo = new IdentityUser()
                {
                    UserName = usuario.Nome
                };

                //criando o usuário no banco de dados
                //IdentityResult é o tipo do objeto que vai dizer para nós o resultado. Se foi incluido com sucesso ou se houve algum erro.
                IdentityResult resultado = usuarioManager.Create(usuarioInfo, usuario.Senha);

                //Se usuário for criado corretamente, nós o autenticamos, ou seja, o usuario nao precisara fazer o login apos o CADASTRO
                if (resultado.Succeeded) //Só vamos ter essa propriedade succeeded configurada para verdadeiro se nenhum erro ocorrer
                {
                    //Usuário criado: adicionando um role.
                    usuarioManager.AddToRole(usuarioInfo.Id, usuario.RoleName);

                    //Autentica e retorna para a pagina inicial
                    //Current - significa que a gente está referenciando justamente o contexto atual da nossa aplicação.
                    //Estamos atribuindo para a variavel authManager justamente uma variavel do tipo Authentication Owin 
                    var authManager = System.Web.HttpContext.Current.GetOwinContext().Authentication;
                    //Obtendo a identidade especificando qual é o tipo de autenticação que vai ser feito, se vai ser armazenado em cookie...
                    var identidadeUsuario = usuarioManager.CreateIdentity(usuarioInfo, DefaultAuthenticationTypes.ApplicationCookie);
                    authManager.SignIn(new AuthenticationProperties() { }, identidadeUsuario);


                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    throw new Exception(resultado.Errors.FirstOrDefault());  //Só vamos ter essa propriedade error configurada para verdadeiro se algum erro ocorrer
                }
            }
            catch (Exception ex)
            {

                ViewBag.MensagemErro = ex.Message;
                return View("_Error");

            }
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        //colocamos o segundo paramtro para que quando, por exemplo: você quer cadastrar um novo produto mas nao está autenticado
        //assim que você clicar em cadastro, será direcionado para a tela de login. Quando feito o login, a gente quer que retorne para a tela de cadastro de produto
        //e não para o index da página. Em outras palavras, durante o processo de requisição se tiver esse parâmetro na url, aquele parametro é passado para o action
        
        public ActionResult Login(LoginUsuario usuario, string ReturnUrl)
        {
            //verificar se o modelo é valido
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                var usuarioStore = new UserStore<IdentityUser>();
                var usuarioManager = new UserManager<IdentityUser>(usuarioStore);

                //buscando o usuário com base no seu nome e na senha.
                var usuarioInfo = usuarioManager.Find(usuario.Nome, usuario.Senha);

                if (usuarioInfo == null)
                {
                    throw new Exception("Usuário ou senha inválidos");
                }
                    //Current - significa que a gente está referenciando justamente o contexto atual da nossa aplicação.
                    //Estamos atribuindo para a variavel authManager justamente uma variavel do tipo Authentication Owin 
                    var authManager = System.Web.HttpContext.Current.GetOwinContext().Authentication;
                    //Obtendo a identidade especificando qual é o tipo de autenticação que vai ser feito, se vai ser armazenado em cookie...
                    var identidadeUsuario = usuarioManager.CreateIdentity(usuarioInfo, DefaultAuthenticationTypes.ApplicationCookie);
                    authManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, identidadeUsuario);

                if (ReturnUrl ==  null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(ReturnUrl);
                }
                    

            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                return View("_Error");
            }
        }
        public ActionResult Logout()
        {
            var authManager = System.Web.HttpContext.Current.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Authorize(Roles ="ADMIN")]
        public ActionResult CriarRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CriarRole(Role role)
        {
            var roleStore = new RoleStore<IdentityRole>();
            var roleManager = new RoleManager<IdentityRole>(roleStore);


            //criando um novo role
            if (!roleManager.RoleExists(role.Descricao)) //se não existir
            {
                var identidade = new IdentityRole();
                identidade.Name = role.Descricao;
                roleManager.Create(identidade);

                ViewBag.RoleName = $"Role \"{role.Descricao}\" criada com sucesso!";
            }
            return View();
        }
    }
    

}
