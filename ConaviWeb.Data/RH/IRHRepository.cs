﻿using ConaviWeb.Model.RH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Data.RH
{
    public interface IRHRepository
    {
        Task<bool> InsertViaticos(Viaticos viaticos);
        Task<IEnumerable<Viaticos>> GetSolicitudes();
        Task<bool> UpdateViaticos(Viaticos viaticos);
        Task<Viaticos> GetSolicitud(int id);

    }
}
