using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Security
{
    public interface IPasswordEncripter
    {
        string Encript(string value, List<byte[]> hashes);
    }
}