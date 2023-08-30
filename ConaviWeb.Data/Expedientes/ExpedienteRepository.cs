using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConaviWeb.Data.Expedientes;
using ConaviWeb.Model;
using ConaviWeb.Model.Expedientes;
using ConaviWeb.Model.Response;
using Dapper;
using MySql.Data.MySqlClient;

namespace ConaviWeb.Data.Expedientes
{
    public class ExpedienteRepository : IExpedienteRepository
    {
        private readonly MySQLConfiguration _connectionString;
        public ExpedienteRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection DbConnection()
        {
            return new MySqlConnection(_connectionString.ExpConnectionString);
        }
        public async Task<IEnumerable<Catalogo>> GetCodigosExp()
        {
            var db = DbConnection();
            var sql = @"
                        select id Id,codigo Clave from prod_control_exp.cat_serie_documental order by id;
                       ";
            return await db.QueryAsync<Catalogo>(sql, new { });
        }
        public async Task<IEnumerable<Catalogo>> GetAreas()
        {
            var db = DbConnection();
            var sql = @"
                        select id Id,descripcion Clave from prod_control_exp.cat_areas order by id;
                       ";
            return await db.QueryAsync<Catalogo>(sql, new { });
        }
        public async Task<IEnumerable<Catalogo>> GetTiposSoporte()
        {
            var db = DbConnection();
            var sql = @"
                        select id Id, concat(clave,' - ',descripcion) Clave from prod_control_exp.cat_tipo_soporte order by id;
                        ";
            return await db.QueryAsync<Catalogo>(sql, new { });
        }
        public async Task<IEnumerable<SerieDocumental>> GetSeries()
        {
            var db = DbConnection();
            var sql = @"
                        select id Id, codigo Codigo, descripcion Descripcion,
                        vig_doc_val_a VigDocA, vig_doc_val_l VigDocL, vig_doc_val_fc VigDocFC,
                        vig_doc_pla_con_at PlazoConAT, vig_doc_pla_con_ac PlazoConAC, (vig_doc_pla_con_at + vig_doc_pla_con_ac) PlazoConTot,
                        tec_sel_e TecSelE, tec_sel_c TecSelC, tec_sel_m TecSelM,
                        observaciones Observaciones, estatus
                        from prod_control_exp.cat_serie_documental
                        order by id;
                        ";
            return await db.QueryAsync<SerieDocumental>(sql, new { });
        }
        public async Task<SerieDocumental> GetSerieDocumental(int id)
        {
            var db = DbConnection();

            var sql = @"
                        select id Id, codigo Codigo, descripcion Descripcion,
                        vig_doc_val_a VigDocA, vig_doc_val_l VigDocL, vig_doc_val_fc VigDocFC,
                        vig_doc_pla_con_at PlazoConAT, vig_doc_pla_con_ac PlazoConAC, (vig_doc_pla_con_at + vig_doc_pla_con_ac) PlazoConTot,
                        tec_sel_e TecSelE, tec_sel_c TecSelC, tec_sel_m TecSelM,
                        observaciones Observaciones, estatus
                        from prod_control_exp.cat_serie_documental
                        where id = @Id";

            return await db.QueryFirstOrDefaultAsync<SerieDocumental>(sql, new { Id = id });
        }
        public async Task<bool> UpdateSerieDocCat(SerieDocumental serie)
        {
            var db = DbConnection();

            var sql = @"
                        UPDATE cat_serie_documental
                        SET
                        codigo = @Codigo,
                        descripcion = @Descripcion,
                        vig_doc_val_a = @VigDocValA,
                        vig_doc_val_l = @VigDocValL, 
                        vig_doc_val_fc = @VigDocValFC, 
                        vig_doc_pla_con_at = @VigDocPlaConAt, 
                        vig_doc_pla_con_ac = @VigDocPlaConAC, 
                        vig_doc_pla_con_tot = @VigDocPlaConAt + @VigDocPlaConAC,
                        tec_sel_e = @TecSelE, 
                        tec_sel_c = @TecSelC, 
                        tec_sel_m = @TecSelM, 
                        observaciones = @Observaciones
                        WHERE id = @IdSerieDoc;";

            var result = await db.ExecuteAsync(sql, new
            {
                Codigo = serie.Codigo,
                Descripcion = serie.Descripcion,
                VigDocValA = serie.VigDocA,
                VigDocValL = serie.VigDocL,
                VigDocValFC = serie.VigDocFC,
                VigDocPlaConAt = serie.PlazoConAT,
                VigDocPlaConAC = serie.PlazoConAC,
                TecSelE = serie.TecSelE,
                TecSelC = serie.TecSelC,
                TecSelM = serie.TecSelM,
                Observaciones = serie.Observaciones,
                IdSerieDoc = serie.Id
            });
            return result > 0;
        }
        public async Task<int> GetIdInventario(string area)
        {
            var db = DbConnection();

            var sql = @"
                        select itr.id
                        from prod_control_exp.inventario_transferencia itr
                        join prod_control_exp.cat_areas ca on itr.id_area = ca.id
                        where ca.descripcion = @Area";

            return await db.QueryFirstOrDefaultAsync<int>(sql, new { Area = area });
        }
        public async Task<bool> InsertInventarioTP(Inventario inventario)
        {
            var db = DbConnection();

            var sql = @"
                        INSERT INTO prod_control_exp.inventario_transferencia
                        (id_area, fecha_elaboracion, fecha_transferencia, nombre_responsable_archivo_tramite)
                        VALUES (@IdArea, @FechaElaboracion, @FechaTransferencia, @NombreResponsable);";

            var result = await db.ExecuteAsync(sql, new
            {
                IdArea = inventario.IdArea,
                FechaElaboracion = inventario.FechaElaboracion,
                FechaTransferencia = inventario.FechaTransferencia,
                NombreResponsable = inventario.NombreResponsableAT
            });
            return result > 0;
        }
        public async Task<bool> InsertExpedienteInventarioTP(Expediente expediente)
        {
            var db = DbConnection();

            var sql = @"
                        INSERT INTO prod_control_exp.expediente_transferencia
                        (id_expediente, nombre, periodo, anios_resguardo, numero_legajos, numero_fojas, observaciones, id_inventario, id_user)
                        VALUES (@IdExpediente, @Nombre, @Periodo, @AniosResguardo, @NumeroLegajos, @NumeroFojas, @Observaciones, @IdInventario, @IdUser);";

            var result = await db.ExecuteAsync(sql, new
            {
                IdExpediente = expediente.IdExpediente,
                Nombre = expediente.Nombre,
                Periodo = expediente.Periodo,
                AniosResguardo = expediente.AniosResguardo,
                NumeroLegajos = expediente.Legajos,
                NumeroFojas = expediente.Fojas,
                Observaciones = expediente.Observaciones,
                IdUser = expediente.IdUser,
                IdInventario = expediente.IdInventario
            });
            return result > 0;
        }
        public async Task<IEnumerable<Expediente>> GetExpedientesInventarioTP(int id, int id_inventario)
        {
            var db = DbConnection();
            
            var sql = @"
                        select ROW_NUMBER() over(order by et.id) NoProg, et.id Id, et.id_expediente IdExpediente, csd.codigo Codigo, nombre Nombre, periodo Periodo, anios_resguardo AniosResguardo, numero_legajos Legajos, numero_fojas Fojas, et.observaciones Observaciones, et.fecha_registro FechaRegistro, et.id_inventario IdInventario
                            ,if(et.id_user = @id, 'editable', 'noeditable') EsEditable, et.estatus Estatus
                        from prod_control_exp.expediente_transferencia et
                        join prod_control_exp.cat_serie_documental csd on et.id_expediente = csd.id
                        where et.id_inventario = @IdInv
                        order by et.id;";
            return await db.QueryAsync<Expediente>(sql, new { Id = id, IdInv = id_inventario });
        }
        public async Task<Expediente> GetExpedienteTP(int id)
        {
            var db = DbConnection();

            var sql = @"
                        select et.id Id, cs.codigo Codigo, id_expediente IdExpediente, nombre Nombre, periodo Periodo, anios_resguardo AniosResguardo, numero_legajos Legajos, numero_fojas Fojas, et.observaciones Observaciones, et.fecha_registro FechaRegistro, id_inventario IdInventario
                            ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
                            ,ca.descripcion Area, et.estatus Estatus
                        from prod_control_exp.expediente_transferencia et
                        join prod_control_exp.cat_serie_documental cs on et.id_expediente = cs.id
                        join prod_control_exp.inventario_transferencia itf on et.id_inventario = itf.id
                        join prod_control_exp.cat_areas ca on itf.id_area = ca.id
                        where et.id = @Id";

            return await db.QueryFirstOrDefaultAsync<Expediente>(sql, new { Id = id });
        }
        public async Task<bool> DropExpediente(int id)
        {
            var db = DbConnection();
            var sql = @"
                        delete from prod_control_exp.expediente_transferencia where id = @Id;";
            var result = await db.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
        public async Task<bool> SendValExpediente(int id)
        {
            var db = DbConnection();
            var sql = @"
                        update prod_control_exp.expediente_transferencia set estatus = 2 where id = @Id;";
            var result = await db.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
        public async Task<bool> VoBoExpediente(int id)
        {
            var db = DbConnection();
            var sql = @"
                        update prod_control_exp.expediente_transferencia set estatus = 3 where id = @Id;";
            var result = await db.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
        public async Task<bool> RevalidacionExpediente(int id)
        {
            var db = DbConnection();
            var sql = @"
                        update prod_control_exp.expediente_transferencia set estatus = 4 where id = @Id;";
            var result = await db.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
        public async Task<int> GetIdInventarioControl(string area)
        {
            var db = DbConnection();

            var sql = @"
                        select itr.id
                        from prod_control_exp.inventario_control itr
                        join prod_control_exp.cat_areas ca on itr.id_area = ca.id
                        where ca.descripcion = @Area";

            return await db.QueryFirstOrDefaultAsync<int>(sql, new { Area = area });
        }
        public async Task<bool> InsertInventarioControl(Inventario inventario)
        {
            var db = DbConnection();

            var sql = @"
                        INSERT INTO prod_control_exp.inventario_control
                        (id_area, responsable_archivo_tramite, fecha_elaboracion, fecha_entrega)
                        VALUES (@IdArea, @NombreResponsable, @FechaElaboracion, @FechaEntrega);";

            var result = await db.ExecuteAsync(sql, new
            {
                IdArea = inventario.IdArea,
                NombreResponsable = inventario.NombreResponsableAT,
                FechaElaboracion = inventario.FechaElaboracion,
                FechaEntrega = inventario.FechaEntrega
            });
            return result > 0;
        }
        public async Task<bool> InsertExpedienteInventarioControl(Expediente expediente)
        {
            var db = DbConnection();

            var sql = @"
                        INSERT INTO prod_control_exp.expediente_control
                        (id_expediente, nombre, numero_legajos, fecha_primero, fecha_ultimo, id_inventario_control, id_user)
                        VALUES (@IdExpediente, @Nombre, @NumeroLegajos, @FechaPrimero, @FechaUltimo, @IdInventario, @IdUser);";

            var result = await db.ExecuteAsync(sql, new
            {
                IdExpediente = expediente.IdExpediente,
                Nombre = expediente.Nombre,
                NumeroLegajos = expediente.Legajos,
                FechaPrimero = expediente.FechaPrimeroAntiguo,
                FechaUltimo = expediente.FechaUltimoReciente,
                IdInventario = expediente.IdInventario,
                IdUser = expediente.IdUser,
            });
            return result > 0;
        }
        public async Task<IEnumerable<Expediente>> GetExpedientesInventarioControl(int id, int id_inventario)
        {
            var db = DbConnection();
            var sql = @"
                        select ROW_NUMBER() over(order by ec.id) NoProg, ec.id Id, ec.id_expediente IdExpediente, csd.codigo Codigo, ec.nombre Nombre, ec.numero_legajos Legajos, ec.fecha_primero FechaPrimeroAntiguo, ec.fecha_ultimo FechaUltimoReciente, ec.id_inventario_control IdInventario
                            ,if(ec.id_user = @Id, 'editable', 'noeditable') EsEditable, ec.estatus Estatus
                        from prod_control_exp.expediente_control ec
                        join prod_control_exp.cat_serie_documental csd on ec.id_expediente = csd.id
                        where ec.id_inventario_control = @IdInv
                        order by ec.id;";
            return await db.QueryAsync<Expediente>(sql, new { Id = id, IdInv = id_inventario });
        }
        public async Task<Expediente> GetExpedienteControl(int id)
        {
            var db = DbConnection();

            var sql = @"
                        select ec.id Id, cs.codigo Codigo, id_expediente IdExpediente, nombre Nombre, numero_legajos Legajos, ec.fecha_primero FechaPrimeroAntiguo, ec.fecha_ultimo FechaUltimoReciente, id_inventario_control IdInventario
                            ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
                            ,ca.descripcion Area, ec.estatus Estatus
                        from prod_control_exp.expediente_control ec
                        join prod_control_exp.cat_serie_documental cs on ec.id_expediente = cs.id
                        join prod_control_exp.inventario_control itf on ec.id_inventario_control = itf.id
                        join prod_control_exp.cat_areas ca on itf.id_area = ca.id
                        where ec.id = @Id";

            return await db.QueryFirstOrDefaultAsync<Expediente>(sql, new { Id = id });
        }
        public async Task<bool> DropExpedienteControl(int id)
        {
            var db = DbConnection();
            var sql = @"
                        delete from prod_control_exp.expediente_control where id = @Id;";
            var result = await db.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
        public async Task<bool> SendValExpedienteControl(int id)
        {
            var db = DbConnection();
            var sql = @"
                        update prod_control_exp.expediente_control set estatus = 2 where id = @Id;";
            var result = await db.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
        public async Task<bool> VoBoExpedienteControl(int id)
        {
            var db = DbConnection();
            var sql = @"
                        update prod_control_exp.expediente_control set estatus = 3 where id = @Id;";
            var result = await db.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
        public async Task<bool> RevalidacionExpedienteControl(int id)
        {
            var db = DbConnection();
            var sql = @"
                        update prod_control_exp.expediente_control set estatus = 4 where id = @Id;";
            var result = await db.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
        public async Task<int> GetIdInventarioBibliohemerografico(string area)
        {
            var db = DbConnection();

            var sql = @"
                        select itr.id
                        from prod_control_exp.inventario_bibliohemerografico itr
                        join prod_control_exp.cat_areas ca on itr.id_area = ca.id
                        where ca.descripcion = @Area";

            return await db.QueryFirstOrDefaultAsync<int>(sql, new { Area = area });
        }
        public async Task<bool> InsertInventarioBibliohemerografico(Inventario inventario)
        {
            var db = DbConnection();

            var sql = @"
                        INSERT INTO prod_control_exp.inventario_bibliohemerografico
                        (id_area, nombre_responsable, fecha_transferencia, fecha_elaboracion)
                        VALUES (@IdArea, @NombreResponsable, @FechaTransferencia, @FechaElaboracion);";

            var result = await db.ExecuteAsync(sql, new
            {
                IdArea = inventario.IdArea,
                NombreResponsable = inventario.NombreResponsableAT,
                FechaTransferencia = inventario.FechaTransferencia,
                FechaElaboracion = inventario.FechaElaboracion
            });
            return result > 0;
        }
        public async Task<bool> InsertExpedienteBibliohemerografico(ExpedienteBibliohemerografico expediente)
        {

            var db = DbConnection();

            var sql = @"
                        INSERT INTO prod_control_exp.expediente_bibliohemerografico
                        (numero_ejemplar, id_tipo_soporte, titulo_del_libro, nombre_autor, tema, editorial, anio, isbn_issn, numero_paginas, numero_volumen, id_inventario_bibliohemerografico, id_user)
                        VALUES (@Ejemplar, @IdSoporte, @Titulo, @Autor, @Tema, @Editorial, @Anio, @Isbn, @Paginas, @Volumen, @IdInventario, @IdUser);";

            var result = await db.ExecuteAsync(sql, new
            {
                Ejemplar = expediente.Ejemplar,
                IdSoporte = expediente.IdTipoSoporte,
                Titulo = expediente.Titulo,
                Autor = expediente.Autor,
                Tema = expediente.Tema,
                Editorial = expediente.Editorial,
                Anio = expediente.Anio,
                Isbn = expediente.IsbnIssn,
                Paginas = expediente.Paginas,
                Volumen = expediente.Volumen,
                IdInventario = expediente.IdInventario,
                IdUser = expediente.IdUser
            });
            return result > 0;
        }
        public async Task<IEnumerable<ExpedienteBibliohemerografico>> GetExpedientesBibliohemerograficos(int id, int id_inventario)
        {
            var db = DbConnection();
            var sql = @"
                        select ROW_NUMBER() over(order by eb.id) NoProg, eb.id Id, eb.numero_ejemplar Ejemplar, eb.id_tipo_soporte IdTipoSoporte, cts.descripcion Soporte, eb.titulo_del_libro Titulo, eb.nombre_autor Autor, eb.tema Tema, eb.editorial Editorial, eb.anio Anio, eb.isbn_issn IsbnIssn, eb.numero_paginas Paginas, eb.numero_volumen Volumen, eb.fecha_registro FechaRegistro, eb.id_inventario_bibliohemerografico IdInventario
                            ,if(eb.id_user = @Id, 'editable', 'noeditable') EsEditable
                        from prod_control_exp.expediente_bibliohemerografico eb
                        join prod_control_exp.cat_tipo_soporte cts on eb.id_tipo_soporte = cts.id
                        where eb.id_inventario_bibliohemerografico = @IdInv
                        order by eb.id;";
            return await db.QueryAsync<ExpedienteBibliohemerografico>(sql, new { Id = id, IdInv = id_inventario });
        }
        public async Task<bool> DropExpedienteBibliohemerografico(int id)
        {
            var db = DbConnection();
            var sql = @"
                        delete from prod_control_exp.expediente_bibliohemerografico where id = @Id;";
            var result = await db.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
        public async Task<int> GetIdInventarioNoExpedientable(string area)
        {
            var db = DbConnection();

            var sql = @"
                        select itr.id
                        from prod_control_exp.inventario_noexpedientable itr
                        join prod_control_exp.cat_areas ca on itr.id_area = ca.id
                        where ca.descripcion = @Area";

            return await db.QueryFirstOrDefaultAsync<int>(sql, new { Area = area });
        }
        public async Task<bool> InsertInventarioNoExpedientable(Inventario inventario)
        {
            var db = DbConnection();

            var sql = @"
                        INSERT INTO prod_control_exp.inventario_noexpedientable
                        (id_area, nombre_responsable, fecha_elaboracion, fecha_transferencia)
                        VALUES (@IdArea, @NombreResponsable, @FechaElaboracion, @FechaTransferencia);";

            var result = await db.ExecuteAsync(sql, new
            {
                IdArea = inventario.IdArea,
                NombreResponsable = inventario.NombreResponsableAT,
                FechaElaboracion = inventario.FechaElaboracion,
                FechaTransferencia = inventario.FechaTransferencia,
            });
            return result > 0;
        }
        public async Task<bool> InsertExpedienteNoExpedientable(ExpedienteNoExpedientable expediente)
        {
            var db = DbConnection();

            var sql = @"
                        INSERT INTO prod_control_exp.expediente_no_expedientable
                        (id_tipo_soporte, clave_interna, numero_partes, fecha_elaboracion, titulo_expediente, observaciones, id_inventario_no_expedientable, id_user)
                        VALUES (@IdSoporte, @Clave, @Partes, @FechaElaboracion, @Titulo, @Observaciones, @IdInventario, @IdUser);";

            var result = await db.ExecuteAsync(sql, new
            {
                IdSoporte = expediente.IdTipoSoporte,
                Clave = expediente.ClaveInterna,
                Partes = expediente.Partes,
                FechaElaboracion = expediente.FechaElaboracion,
                Titulo = expediente.Titulo,
                Observaciones = expediente.Observaciones,
                IdInventario = expediente.IdInventario,
                IdUser = expediente.IdUser
            });
            return result > 0;
        }
        public async Task<IEnumerable<ExpedienteNoExpedientable>> GetExpedientesNoExpedientables(int id, int id_inventario)
        {
            var db = DbConnection();
            var sql = @"
                        select ROW_NUMBER() over(order by en.id) NoProg, en.id Id, cts.clave Clave, en.id_tipo_soporte IdTipoSoporte, cts.descripcion Soporte, en.clave_interna ClaveInterna, en.titulo_expediente Titulo, en.numero_partes Partes, en.observaciones Observaciones, en.fecha_elaboracion FechaElaboracion, en.fecha_registro FechaRegistro, en.id_inventario_no_expedientable IdInventario
                            ,if(en.id_user = @Id, 'editable', 'noeditable') EsEditable
                        from prod_control_exp.expediente_no_expedientable en
                        join prod_control_exp.cat_tipo_soporte cts on en.id_tipo_soporte = cts.id
                        where en.id_inventario_no_expedientable = @IdInv
                        order by en.id;";
            return await db.QueryAsync<ExpedienteNoExpedientable>(sql, new { Id = id, IdInv = id_inventario });
        }
        public async Task<bool> DropExpedienteNoExpedientable(int id)
        {
            var db = DbConnection();
            var sql = @"
                        delete from prod_control_exp.expediente_no_expedientable where id = @Id;";
            var result = await db.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
    }
}
