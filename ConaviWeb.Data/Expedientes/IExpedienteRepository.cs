﻿using System;
using ConaviWeb.Model;
using ConaviWeb.Model.Expedientes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Data.Expedientes
{
    public interface IExpedienteRepository
    {
        Task<int> GetIdInventario(string area);
        Task<int> GetIdInventarioControl(string area);
        Task<int> GetIdInventarioBibliohemerografico(string area);
        Task<int> GetIdInventarioNoExpedientable(string area);
        Task<IEnumerable<Catalogo>> GetCodigosExp();
        Task<IEnumerable<Catalogo>> GetTiposSoporte();
        Task<IEnumerable<SerieDocumental>> GetSeries();
        Task<SerieDocumental> GetSerieDocumental(int id);
        Task<bool> UpdateSerieDocCat(SerieDocumental serie);
        Task<IEnumerable<Catalogo>> GetAreas();
        Task<bool> InsertInventarioTP(Inventario inventario);
        Task<bool> InsertExpedienteInventarioTP(Expediente expediente);
        Task<IEnumerable<Expediente>> GetExpedientesInventarioTP(int id, int id_inventario);
        Task<Expediente> GetExpedienteTP(int id);
        Task<bool> DropExpediente(int id);
        Task<bool> SendValExpediente(int id);
        Task<bool> VoBoExpediente(int id);
        Task<bool> RevalidacionExpediente(int id);
        Task<bool> InsertInventarioControl(Inventario inventario);
        Task<bool> InsertExpedienteInventarioControl(Expediente expediente);
        Task<IEnumerable<Expediente>> GetExpedientesInventarioControl(int id, int id_inventario);
        Task<Expediente> GetExpedienteControl(int id);
        Task<bool> DropExpedienteControl(int id);
        Task<bool> SendValExpedienteControl(int id);
        Task<bool> VoBoExpedienteControl(int id);
        Task<bool> RevalidacionExpedienteControl(int id);
        Task<bool> InsertInventarioBibliohemerografico(Inventario inventario);
        Task<bool> InsertExpedienteBibliohemerografico(ExpedienteBibliohemerografico expediente);
        Task<IEnumerable<ExpedienteBibliohemerografico>> GetExpedientesBibliohemerograficos(int id, int id_inventario);
        Task<bool> DropExpedienteBibliohemerografico(int id);
        Task<bool> InsertInventarioNoExpedientable(Inventario inventario);
        Task<bool> InsertExpedienteNoExpedientable(ExpedienteNoExpedientable expediente);
        Task<IEnumerable<ExpedienteNoExpedientable>> GetExpedientesNoExpedientables(int id, int id_inventario);
        Task<bool> DropExpedienteNoExpedientable(int id);
    }
}