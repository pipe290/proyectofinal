using proyectofinal1_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Services
{
    public interface IAuthenticationService
    {
        AuthResults Auth(string user, string password, out Usuario usuario);
    }
    public enum AuthResults
    {
        Success,
        PasswordNotMatch,
        NotExists,
        Error
    }
}