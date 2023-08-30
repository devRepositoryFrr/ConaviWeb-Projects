using ConaviWeb.Model.RH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Data.RH
{
    public interface INominaRepository
    {
        Task<bool> InsertAceptacion(Aceptacion aceptacion);
    }
}
