using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ProyectoFinal.Data;
using ProyectoFinal.Security;
using ProyectoFinal.Services;
using proyectofinal1_.Models;

namespace ProyectoFinal.Controllers
{
    public class UsuariosController : Controller
    {
        private ProyectoFinalContext db = new ProyectoFinalContext();
        private IPasswordEncripter _passwordEncripter=new PasswordEncripter();
        private IAuthenticationService _authenticationService=new AuthenticationService();


        public ActionResult Index()
        {
            return View(db.Usuarios.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NombreUsuario,ContraseñaHash,HashKey,HashIV,FechaCreacion")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {

            
                var key = new byte[32];
                var iv = new byte[16];
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(key);
                    rng.GetBytes(iv);
                }
                var passwordEncrypted = _passwordEncripter.Encript(usuario.ContraseñaHash, new List<byte[]> { key, iv });

                var nuevoUsuario = new Usuario 
                {
                    NombreUsuario = usuario.NombreUsuario,
                    ContraseñaHash = passwordEncrypted,
                    HashKey = key,
                    HashIV = iv,
                    FechaCreacion = DateTime.UtcNow
                };

              
                db.Usuarios.Add(nuevoUsuario);
             
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usuario);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Usuario usuarioData)
        {
            if (ModelState.IsValid)
            {
                Usuario usuario;
                var resultado = _authenticationService.Auth(usuarioData.NombreUsuario, usuarioData.ContraseñaHash, out usuario);

                switch (resultado)
                {
                    case AuthResults.Success:
                        if (Request.Cookies["AuthCookie"] != null)
                        {
                            var existingCookie = new HttpCookie("AuthCookie")
                            {
                                Expires = DateTime.Now.AddDays(-1)
                            };
                            Response.Cookies.Add(existingCookie);
                        }
                        var cookie = new HttpCookie("AuthCookie", usuarioData.NombreUsuario)
                        {
                            Expires = DateTime.Now.AddHours(1)
                        };
                        FormsAuthentication.SetAuthCookie(usuarioData.NombreUsuario, false);
                        Response.Cookies.Add(cookie);
                        return RedirectToAction("", "");

                    case AuthResults.NotExists:
                        ModelState.AddModelError("", "El usuario no existe.");
                        break;

                    case AuthResults.PasswordNotMatch:
                        ModelState.AddModelError("", "Contraseña incorrecta.");
                        break;
                    default:
                        ModelState.AddModelError("", "Error desconocido intenta nuevamente");
                        break;
                }
            }

            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NombreUsuario,ContraseñaHash,HashKey,HashIV,FechaCreacion")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var usuarioExiste = db.Usuarios.Find(usuario.Id);
                if (usuarioExiste == null)
                {
                    return HttpNotFound();
                }

                usuarioExiste.NombreUsuario = usuario.NombreUsuario;
                usuarioExiste.ContraseñaHash = usuario.ContraseñaHash;
                usuarioExiste.HashKey = usuario.HashKey;
                usuarioExiste.HashIV = usuario.HashIV;
                usuarioExiste.FechaCreacion = usuario.FechaCreacion;

                db.Entry(usuarioExiste).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            
            return View(usuario);
        }

      
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
