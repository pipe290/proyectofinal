using ProyectoFinal.Data;
using ProyectoFinal.Security;
using ProyectoFinal.Utils;
using proyectofinal1_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Services
{
    public class AuthenticationService:IAuthenticationService
    {
        private readonly ProyectoFinalContext db = new ProyectoFinalContext();
        private readonly IPasswordEncripter _passwordEncripter = new PasswordEncripter();

        public AuthResults Auth(string nombreUsuario, string contraseña, out Usuario usuario)
        {
            usuario = db.Usuarios.Where(x => x.NombreUsuario.Equals(nombreUsuario)).FirstOrDefault();

            if (usuario == null)
                return AuthResults.NotExists;

            contraseña = _passwordEncripter.Encript(contraseña,new List<byte[]>()
                .AddHash(usuario.HashKey)
                .AddHash(usuario.HashIV)
                );
            if (contraseña != usuario.ContraseñaHash)
                return AuthResults.PasswordNotMatch;

            return AuthResults.Success;
        }
    }
}