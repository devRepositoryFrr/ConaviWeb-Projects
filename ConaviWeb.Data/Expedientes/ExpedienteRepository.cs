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
                        select id Id,concat(codigo,'-',descripcion) Clave from prod_control_exp.cat_serie_documental order by id;
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
        public async Task<int> GetIdUserArea(string area)
        {
            var db = DbConnection();
            var sql = @"
                        select id from prod_control_exp.cat_areas where descripcion = @Area order by id;
                       ";
            return await db.QueryFirstOrDefaultAsync<int>(sql, new { Area = area });
        }

        public async Task<IEnumerable<Catalogo>> GetTiposSoporte()
        {
            var db = DbConnection();
            var sql = @"
                        SELECT id Id, concat(clave,' - ',descripcion) Clave FROM prod_control_exp.cat_tipo_soporte WHERE activo = 1 ORDER BY id;
                        ";
            return await db.QueryAsync<Catalogo>(sql, new { });
        }
        public async Task<IEnumerable<Catalogo>> GetTiposDocumentales()
        {
            var db = DbConnection();
            var sql = @"
                        SELECT id Id, CONCAT(id,'. ',descripcion) Clave FROM prod_control_exp.cat_tipo_documentales WHERE activo = 1 ORDER BY id;
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
        public async Task<Inventario> GetInventarioTP(string area)
        {
            var db = DbConnection();

            var sql = @"
                        select itr.id Id, itr.fecha_elaboracion FechaElaboracion, itr.fecha_transferencia FechaTransferencia, itr.nombre_responsable_archivo_tramite NombreResponsableAT
                        from prod_control_exp.inventario_transferencia itr
                        join prod_control_exp.cat_areas ca on itr.id_area = ca.id
                        where ca.descripcion = @Area";

            return await db.QueryFirstOrDefaultAsync<Inventario>(sql, new { Area = area });
        }
        public async Task<bool> InsertInventarioTP(Inventario inventario)
        {
            var db = DbConnection();

            //var sql = @"
            //            INSERT INTO prod_control_exp.inventario_transferencia
            //            (id_area, fecha_elaboracion, fecha_transferencia, nombre_responsable_archivo_tramite)
            //            VALUES (@IdArea, @FechaElaboracion, @FechaTransferencia, @NombreResponsable)
            //            ON DUPLICATE KEY UPDATE id_area = @IdArea, fecha_elaboracion = @FechaElaboracion, fecha_transferencia = @FechaTransferencia, nombre_responsable_archivo_tramite = @NombreResponsable;";
            var sql = @"
                        UPDATE prod_control_exp.inventario_control SET fecha_transferencia = @FechaTransferencia WHERE id_area = @IdArea;";

            var result = await db.ExecuteAsync(sql, new
            {
                IdArea = inventario.IdArea,
                //FechaElaboracion = inventario.FechaElaboracion,
                FechaTransferencia = inventario.FechaTransferencia,
                //NombreResponsable = inventario.NombreResponsableAT
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
        public async Task<bool> InsertCaratulaExpedienteTP(Caratula caratula)
        {
            var db = DbConnection();

            var sql = @"
                        INSERT INTO prod_control_exp.caratula
                        (`cant_doc_ori`,`cant_doc_copias`,`cant_cds`,`tec_sel_doc`,`publica`,`confidencial`,`reservada_sol_info`,`descripcion_asunto_expediente`,`fecha_clasificacion`,`periodo_reserva`,`fundamento_legal`,`ampliacion_periodo_reserva`,`fecha_desclasificacion`,`nombre_desclasifica`,`cargo_desclasifica`,`partes_reservando`,`id_user_captura`,`datos_topograficos`,`id_expediente_tp`)
                        VALUES (@DocOriginales, @DocCopias, @Cds, @TecnicasSeleccion, @Publica, @Confidencial, @Reservada, @DescripcionAsunto, @FechaClasificacion, @PeriodoReserva, @FundamentoLegal, @AmpliacionPeriodo, @FechaDesclasificacion, @NombreDesclasifica, @CargoDesclasifica, @PartesReservando, @IdUser, @DatosTopograficos, @IdExpediente)
                        ON DUPLICATE KEY UPDATE `cant_doc_ori` = @DocOriginales,`cant_doc_copias` = @DocCopias,`cant_cds` = @Cds,`tec_sel_doc` = @TecnicasSeleccion,`publica` = @Publica,`confidencial` = @Confidencial,`reservada_sol_info` = @Reservada,`descripcion_asunto_expediente` = @DescripcionAsunto,`fecha_clasificacion` = @FechaClasificacion,`periodo_reserva` = @PeriodoReserva,`fundamento_legal` = @FundamentoLegal,`ampliacion_periodo_reserva` = @AmpliacionPeriodo,`fecha_desclasificacion` = @FechaDesclasificacion,`nombre_desclasifica` = @NombreDesclasifica,`cargo_desclasifica` = @CargoDesclasifica,`partes_reservando` = @PartesReservando,`id_user_captura` = @IdUser,`datos_topograficos` = @DatosTopograficos;";

            var result = await db.ExecuteAsync(sql, new
            {
                IdExpediente = caratula.IdExpediente,
                //Nombre = caratula.Nombre,
                //NumeroLegajos = caratula.Legajos,
                //Fojas = caratula.Fojas,
                DocOriginales = caratula.DocOriginales,
                DocCopias = caratula.DocCopias,
                Cds = caratula.Cds,
                TecnicasSeleccion = caratula.TecnicasSeleccion,
                Publica = caratula.Publica,
                Confidencial = caratula.Confidencial,
                Reservada = caratula.Reservada,
                DescripcionAsunto = caratula.DescripcionAsunto,
                FechaClasificacion = caratula.FechaClasificacion,
                PeriodoReserva = caratula.PeriodoReserva,
                FundamentoLegal = caratula.FundamentoLegal,
                AmpliacionPeriodo = caratula.AmpliacionPeriodo,
                FechaDesclasificacion = caratula.FechaDesclasificacion,
                NombreDesclasifica = caratula.NombreDesclasifica,
                CargoDesclasifica = caratula.CargoDesclasifica,
                PartesReservando = caratula.PartesReservando,
                DatosTopograficos = caratula.DatosTopograficos,
                IdUser = caratula.IdUser,
            });
            return result > 0;
        }
        public async Task<IEnumerable<Expediente>> GetExpedientesInventarioTP(int id, int id_inventario)
        {
            var db = DbConnection();

            //var sql = @"
            //            select ROW_NUMBER() over(order by et.id) NoProg, et.id Id, et.id_expediente IdExpediente, csd.codigo Codigo, nombre Nombre, periodo Periodo, anios_resguardo AniosResguardo, numero_legajos Legajos, numero_fojas Fojas, et.observaciones Observaciones, et.fecha_registro FechaRegistro, et.id_inventario IdInventario
            //                ,if(et.id_user = @id, 'editable', 'noeditable') EsEditable, et.estatus Estatus
            //            from prod_control_exp.expediente_transferencia et
            //            join prod_control_exp.cat_serie_documental csd on et.id_expediente = csd.id
            //            where et.id_inventario = @IdInv
            //            order by et.id;";
            var sql = @"
                        select ROW_NUMBER() over(order by ec.fecha_ultimo, ec.id) NoProg, ec.id Id, ec.id_expediente IdExpediente, csd.codigo Codigo, ec.nombre Nombre, if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo, ec.anios_resguardo AniosResguardo, ec.numero_legajos Legajos, ec.numero_fojas Fojas, ec.observaciones Observaciones, ec.fecha_registro FechaRegistro, ec.id_inventario_control IdInventario
                            ,if(ec.id_user = @id, 'editable', 'noeditable') EsEditable, ec.estatus Estatus
                        from prod_control_exp.expediente_control ec
                        join prod_control_exp.cat_serie_documental csd on ec.id_expediente = csd.id
                        where ec.id_inventario_control = @IdInv and ec.migrado_tp = 1
                        order by ec.fecha_ultimo, ec.id;";
            return await db.QueryAsync<Expediente>(sql, new { Id = id, IdInv = id_inventario });
        }
        public async Task<IEnumerable<ExpedienteBibliohemerografico>> GetExpedientesValidacionBiblio(int idArea)
        {
            var db = DbConnection();

            var sql = @"
                        
                        select cons.NoProg NoProg, eb.id Id, eb.numero_ejemplar Ejemplar, eb.id_tipo_soporte IdTipoSoporte, ct.clave ClaveSoporte, eb.titulo_del_libro Titulo, eb.nombre_autor Autor, eb.tema Tema, eb.editorial Editorial, eb.anio Anio, eb.isbn_issn IsbnIssn, eb.numero_paginas Paginas, eb.numero_volumen Volumen, eb.id_inventario_bibliohemerografico IdInventario
                        from prod_control_exp.expediente_bibliohemerografico eb
                        join prod_control_exp.inventario_bibliohemerografico ib on eb.id_inventario_bibliohemerografico = ib.id
                        join prod_control_exp.cat_tipo_soporte ct on eb.id_tipo_soporte = ct.id
                        join (select ROW_NUMBER() over(order by ets.anio,ets.id) NoProg, ets.id from prod_control_exp.expediente_bibliohemerografico ets join prod_control_exp.inventario_bibliohemerografico ib on ets.id_inventario_bibliohemerografico = ib.id where ib.id_area = @IdArea) cons on eb.id = cons.id
                        where ib.id_area = @IdArea and eb.estatus = 2
                        order by eb.anio,eb.id;";
            return await db.QueryAsync<ExpedienteBibliohemerografico>(sql, new { IdArea = idArea });
        }
        public async Task<Expediente> GetExpedienteTP(int id)
        {
            var db = DbConnection();

            var sql = @"
                        select cons.NoProg NoProg, et.id Id, cs.codigo Codigo, id_expediente IdExpediente, nombre Nombre, periodo Periodo, anios_resguardo AniosResguardo, numero_legajos Legajos, numero_fojas Fojas, et.observaciones Observaciones, et.fecha_registro FechaRegistro, id_inventario IdInventario
                            ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
                            ,ca.descripcion Area, et.estatus Estatus
                        from prod_control_exp.expediente_transferencia et
                        join prod_control_exp.cat_serie_documental cs on et.id_expediente = cs.id
                        join prod_control_exp.inventario_transferencia itf on et.id_inventario = itf.id
                        join prod_control_exp.cat_areas ca on itf.id_area = ca.id
                        join (select ROW_NUMBER() over(order by ets.id) NoProg, ets.id from prod_control_exp.expediente_transferencia ets where ets.id_inventario = (select id_inventario from expediente_transferencia where id = @Id)) cons on et.id = cons.id
                        where et.id = @Id";

            return await db.QueryFirstOrDefaultAsync<Expediente>(sql, new { Id = id });
        }
        public async Task<Caratula> GetCaratulaExpedienteTP(int id)
        {
            var db = DbConnection();

            //var sql = @"
            //            select cons.NoProg NoProg, et.id Id, cs.codigo Codigo, id_expediente IdExpediente, nombre Nombre, periodo Periodo, anios_resguardo AniosResguardo, numero_legajos Legajos, numero_fojas Fojas, et.observaciones Observaciones, et.fecha_registro FechaRegistro, id_inventario IdInventario
            //                ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
            //                ,ca.descripcion Area, et.estatus Estatus
            //                ,crt.fojas Fojas, crt.cant_doc_ori DocOriginales, crt.cant_doc_copias DocCopias, crt.cant_cds Cds, crt.tec_sel_doc TecnicasSeleccion, crt.publica Publica, crt.confidencial Confidencial, crt.reservada_sol_info Reservada, crt.descripcion_asunto_expediente DescripcionAsunto, crt.fecha_clasificacion FechaClasificacion, crt.periodo_reserva PeriodoReserva, crt.fundamento_legal FundamentoLegal, crt.ampliacion_periodo_reserva AmpliacionPeriodo, crt.fecha_desclasificacion FechaDesclasificacion, crt.nombre_desclasifica NombreDesclasifica, crt.cargo_desclasifica CargoDesclasifica, crt.partes_reservando PartesReservando, crt.datos_topograficos DatosTopograficos, crt.id_expediente_tp
            //            from prod_control_exp.expediente_transferencia et
            //            join prod_control_exp.cat_serie_documental cs on et.id_expediente = cs.id
            //            join prod_control_exp.inventario_transferencia itf on et.id_inventario = itf.id
            //            join prod_control_exp.cat_areas ca on itf.id_area = ca.id
            //            join (select ROW_NUMBER() over(order by ets.id) NoProg, ets.id from prod_control_exp.expediente_transferencia ets where ets.id_inventario = (select id_inventario from expediente_transferencia where id = @Id)) cons on et.id = cons.id
            //            left join prod_control_exp.caratula crt on et.id = crt.id_expediente_tp
            //            where et.id = @Id";
            var sql = @"
                        select cons.NoProg NoProg, et.id Id, cs.codigo Codigo, id_expediente IdExpediente, nombre Nombre, if(year(et.fecha_primero)=year(et.fecha_ultimo),year(et.fecha_primero),concat(year(et.fecha_primero),'-',year(et.fecha_ultimo))) Periodo, anios_resguardo AniosResguardo, numero_legajos Legajos, numero_fojas Fojas, et.observaciones Observaciones, et.fecha_registro FechaRegistro, et.fecha_primero FechaPrimeroAntiguo, et.fecha_ultimo FechaUltimoReciente, et.id_inventario_control IdInventario
                            ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
                            ,ca.descripcion Area, et.estatus Estatus
                            ,crt.cant_doc_ori DocOriginales, crt.cant_doc_copias DocCopias, crt.cant_cds Cds, crt.tec_sel_doc TecnicasSeleccion, crt.publica Publica, crt.confidencial Confidencial, crt.reservada_sol_info Reservada, crt.descripcion_asunto_expediente DescripcionAsunto, crt.fecha_clasificacion FechaClasificacion, crt.periodo_reserva PeriodoReserva, crt.fundamento_legal FundamentoLegal, crt.ampliacion_periodo_reserva AmpliacionPeriodo, crt.fecha_desclasificacion FechaDesclasificacion, crt.nombre_desclasifica NombreDesclasifica, crt.cargo_desclasifica CargoDesclasifica, crt.partes_reservando PartesReservando, crt.datos_topograficos DatosTopograficos, crt.id_expediente_tp
                        from prod_control_exp.expediente_control et
                        join prod_control_exp.cat_serie_documental cs on et.id_expediente = cs.id
                        join prod_control_exp.inventario_control itf on et.id_inventario_control = itf.id
                        join prod_control_exp.cat_areas ca on itf.id_area = ca.id
                        join (select ROW_NUMBER() over(order by ets.fecha_ultimo, ets.id) NoProg, ets.id from prod_control_exp.expediente_control ets where ets.id_inventario_control = (select id_inventario_control from prod_control_exp.expediente_control where id = @Id) AND ets.migrado_tp = 1) cons on et.id = cons.id
                        left join prod_control_exp.caratula crt on et.id = crt.id_expediente_control
                        where et.id = @Id";

            return await db.QueryFirstOrDefaultAsync<Caratula>(sql, new { Id = id });
        }
        public async Task<bool> DropExpediente(int id)
        {
            var db = DbConnection();
            var sql = @"
                        delete from prod_control_exp.expediente_transferencia where id = @Id;";
            var result = await db.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
        public async Task<bool> MigrarExpedienteInvTP(int id)
        {
            var db = DbConnection();
            var sql = @"
                        call migrarExpedienteInvTP(@Id);";
            var result = await db.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
        public async Task<bool> MigrarExpedienteInvNE(int id)
        {
            var db = DbConnection();
            var sql = @"
                        call migrarExpedienteInvNE(@Id);";
            var result = await db.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
        public async Task<Inventario> GetInventarioControl(string area)
        {
            var db = DbConnection();

            var sql = @"
                        select itr.id Id, itr.id_area IdArea, itr.responsable_archivo_tramite NombreResponsableAT, itr.fecha_elaboracion FechaElaboracion, itr.fecha_entrega FechaEntrega, itr.fecha_transferencia FechaTransferencia
                        from prod_control_exp.inventario_control itr
                        join prod_control_exp.cat_areas ca on itr.id_area = ca.id
                        where ca.descripcion = @Area";

            return await db.QueryFirstOrDefaultAsync<Inventario>(sql, new { Area = area });
        }
        public async Task<bool> InsertInventarioControl(Inventario inventario)
        {
            var db = DbConnection();

            var sql = @"
                        INSERT INTO prod_control_exp.inventario_control
                        (id_area, responsable_archivo_tramite, fecha_elaboracion, fecha_entrega)
                        VALUES (@IdArea, @NombreResponsable, @FechaElaboracion, @FechaEntrega)
                        ON DUPLICATE KEY UPDATE id_area = @IdArea, responsable_archivo_tramite = @NombreResponsable, fecha_elaboracion = @FechaElaboracion, fecha_entrega = @FechaEntrega;";

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
            //(id_expediente, nombre, numero_legajos, fecha_primero, fecha_ultimo, id_inventario_control, id_user)
            var sql = @"
                        INSERT INTO prod_control_exp.expediente_control
                        (nombre, numero_legajos, numero_fojas, numero_partes, fecha_primero, fecha_ultimo, fecha_elaboracion, observaciones, anios_resguardo, id_user, id_expediente, id_inventario_control, id_tipo_documental, id_tipo_soporte)
                        VALUES (@Nombre, @NumeroLegajos, @NumeroFojas, @NumeroPartes, @FechaPrimero, @FechaUltimo, @FechaElaboracion, @Observaciones, @AniosResguardo, @IdUser, @IdExpediente, @IdInventario, @IdTipoDocumental, @IdTipoSoporte);";

            var result = await db.ExecuteAsync(sql, new
            {
                Nombre = expediente.Nombre,
                NumeroLegajos = expediente.Legajos,
                NumeroFojas = expediente.Fojas,
                NumeroPartes = expediente.NoPartes,
                FechaPrimero = expediente.FechaPrimeroAntiguo,
                FechaUltimo = expediente.FechaUltimoReciente,
                FechaElaboracion = expediente.FechaElaboracion,
                Observaciones = expediente.Observaciones,
                AniosResguardo = expediente.AniosResguardo,
                IdUser = expediente.IdUser,
                IdExpediente = expediente.IdExpediente,
                IdInventario = expediente.IdInventario,
                IdTipoDocumental = expediente.IdTipoDocumental,
                IdTipoSoporte = expediente.IdTipoSoporte
            });
            return result > 0;
        }
        public async Task<bool> UpdateExpedienteInventarioControl(Expediente expediente)
        {
            var db = DbConnection();
            var sql = @"
                        UPDATE prod_control_exp.expediente_control set nombre = @Nombre, numero_legajos = @NumeroLegajos, numero_fojas = @NumeroFojas, numero_partes = @NumeroPartes, fecha_primero = @FechaPrimero, fecha_ultimo = @FechaUltimo, fecha_elaboracion = @FechaElaboracion, observaciones = @Observaciones, anios_resguardo = @AniosResguardo, id_user = @IdUser, id_expediente = @IdExpediente, id_inventario_control = @IdInventario, id_tipo_documental = @IdTipoDocumental, id_tipo_soporte = @IdTipoSoporte
                        WHERE id = @Id;";

            var result = await db.ExecuteAsync(sql, new
            {
                Id = expediente.Id,
                Nombre = expediente.Nombre,
                NumeroLegajos = expediente.Legajos,
                NumeroFojas = expediente.Fojas,
                NumeroPartes = expediente.NoPartes,
                FechaPrimero = expediente.FechaPrimeroAntiguo,
                FechaUltimo = expediente.FechaUltimoReciente,
                FechaElaboracion = expediente.FechaElaboracion,
                Observaciones = expediente.Observaciones,
                AniosResguardo = expediente.AniosResguardo,
                IdUser = expediente.IdUser,
                IdExpediente = expediente.IdExpediente,
                IdInventario = expediente.IdInventario,
                IdTipoDocumental = expediente.IdTipoDocumental,
                IdTipoSoporte = expediente.IdTipoSoporte
            });
            return result > 0;
        }
        public async Task<bool> InsertCaratulaExpedienteIC(Caratula caratula)
        {
            var db = DbConnection();

            var sql = @"
                        INSERT INTO prod_control_exp.caratula
                        (`cant_doc_ori`,`cant_doc_copias`,`cant_cds`,`tec_sel_doc`,`publica`,`confidencial`,`reservada_sol_info`,`descripcion_asunto_expediente`,`fecha_clasificacion`,`periodo_reserva`,`fundamento_legal`,`ampliacion_periodo_reserva`,`fecha_desclasificacion`,`nombre_desclasifica`,`cargo_desclasifica`,`partes_reservando`,`id_user_captura`,`datos_topograficos`,`id_expediente_control`)
                        VALUES (@DocOriginales, @DocCopias, @Cds, @TecnicasSeleccion, @Publica, @Confidencial, @Reservada, @DescripcionAsunto, @FechaClasificacion, @PeriodoReserva, @FundamentoLegal, @AmpliacionPeriodo, @FechaDesclasificacion, @NombreDesclasifica, @CargoDesclasifica, @PartesReservando, @IdUser, @DatosTopograficos, @IdExpediente)
                        ON DUPLICATE KEY UPDATE `cant_doc_ori` = @DocOriginales,`cant_doc_copias` = @DocCopias,`cant_cds` = @Cds,`tec_sel_doc` = @TecnicasSeleccion,`publica` = @Publica,`confidencial` = @Confidencial,`reservada_sol_info` = @Reservada,`descripcion_asunto_expediente` = @DescripcionAsunto,`fecha_clasificacion` = @FechaClasificacion,`periodo_reserva` = @PeriodoReserva,`fundamento_legal` = @FundamentoLegal,`ampliacion_periodo_reserva` = @AmpliacionPeriodo,`fecha_desclasificacion` = @FechaDesclasificacion,`nombre_desclasifica` = @NombreDesclasifica,`cargo_desclasifica` = @CargoDesclasifica,`partes_reservando` = @PartesReservando,`id_user_captura` = @IdUser,`datos_topograficos` = @DatosTopograficos;";

            var result = await db.ExecuteAsync(sql, new
            {
                IdExpediente = caratula.IdExpediente,
                DocOriginales = caratula.DocOriginales,
                DocCopias = caratula.DocCopias,
                Cds = caratula.Cds,
                TecnicasSeleccion = caratula.TecnicasSeleccion,
                Publica = caratula.Publica,
                Confidencial = caratula.Confidencial,
                Reservada = caratula.Reservada,
                DescripcionAsunto = caratula.DescripcionAsunto,
                FechaClasificacion = caratula.FechaClasificacion,
                PeriodoReserva = caratula.PeriodoReserva,
                FundamentoLegal = caratula.FundamentoLegal,
                AmpliacionPeriodo = caratula.AmpliacionPeriodo,
                FechaDesclasificacion = caratula.FechaDesclasificacion,
                NombreDesclasifica = caratula.NombreDesclasifica,
                CargoDesclasifica = caratula.CargoDesclasifica,
                PartesReservando = caratula.PartesReservando,
                DatosTopograficos = caratula.DatosTopograficos,
                IdUser = caratula.IdUser,
            });
            return result > 0;
        }
        public async Task<IEnumerable<Expediente>> GetExpedientesInventarioControl(int id, int id_inventario)
        {
            var db = DbConnection();
            var sql = @"
                        select ROW_NUMBER() over(order by ec.fecha_ultimo) NoProg, ec.id Id, ec.id_expediente IdExpediente, csd.codigo Codigo, if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo, ec.nombre Nombre, ec.numero_legajos Legajos, ec.fecha_primero FechaPrimeroAntiguo, ec.fecha_ultimo FechaUltimoReciente, ec.id_inventario_control IdInventario
                            ,if(ec.id_user = @Id, 'editable', 'noeditable') EsEditable, ec.estatus Estatus
                            ,if(ec.migrado_tp = 1, ec.migrado_tp, if(ec.anios_resguardo is not null and ec.numero_fojas is not null, 0, 1)) MigradoTP
                            ,if(ec.migrado_ne = 1, ec.migrado_ne, if(ec.numero_partes is not null and ec.fecha_elaboracion is not null and ec.id_tipo_documental is not null and ec.id_tipo_soporte is not null, 0, 1)) MigradoNE
                        from prod_control_exp.expediente_control ec
                        join prod_control_exp.cat_serie_documental csd on ec.id_expediente = csd.id
                        where ec.id_inventario_control = @IdInv AND ec.migrado_tp = 0 AND ec.migrado_ne = 0
                        order by ec.fecha_ultimo;";
            return await db.QueryAsync<Expediente>(sql, new { Id = id, IdInv = id_inventario });
        }
        public async Task<IEnumerable<Expediente>> GetExpedientesValidacionInventarioControl(int idArea)
        {
            var db = DbConnection();
            var sql = @"
                        select ROW_NUMBER() over(order by ec.fecha_ultimo) NoProg, ec.id Id, ec.id_expediente IdExpediente, csd.codigo Codigo, if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo, ec.nombre Nombre, ec.numero_legajos Legajos, ec.fecha_primero FechaPrimeroAntiguo, ec.fecha_ultimo FechaUltimoReciente, ec.id_inventario_control IdInventario, ec.estatus Estatus
                        from prod_control_exp.expediente_control ec
                        join prod_control_exp.inventario_control ic on ec.id_inventario_control = ic.id
                        join prod_control_exp.cat_serie_documental csd on ec.id_expediente = csd.id
                        where ic.id_area = @IdArea and ec.estatus = 2 AND ec.migrado_tp = 0 and ec.migrado_ne = 0
                        order by ec.fecha_ultimo;";
            return await db.QueryAsync<Expediente>(sql, new { IdArea = idArea });
        }
        public async Task<Expediente> GetExpedienteControl(int id)
        {
            var db = DbConnection();

            var sql = @"
                        select cons.NoProg NoProg, ec.id Id, cs.codigo Codigo, if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo, ec.id_expediente IdExpediente, nombre Nombre, ec.observaciones Observaciones, ec.numero_fojas Fojas, ec.numero_legajos Legajos, ec.fecha_primero FechaPrimeroAntiguo, ec.fecha_ultimo FechaUltimoReciente, ec.anios_resguardo AniosResguardo, ec.id_tipo_documental IdTipoDocumental, ec.id_tipo_soporte IdTipoSoporte, ec.numero_partes NoPartes, ec.fecha_elaboracion FechaElaboracion, id_inventario_control IdInventario
                            ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
                            ,ca.descripcion Area, ec.estatus Estatus
                        from prod_control_exp.expediente_control ec
                        join prod_control_exp.cat_serie_documental cs on ec.id_expediente = cs.id
                        join prod_control_exp.inventario_control itf on ec.id_inventario_control = itf.id
                        join prod_control_exp.cat_areas ca on itf.id_area = ca.id
                        join (select ROW_NUMBER() over(order by ets.fecha_ultimo) NoProg, ets.id from prod_control_exp.expediente_control ets WHERE ets.id_inventario_control = (select id_inventario_control from prod_control_exp.expediente_control where id = @Id) AND ets.migrado_tp = 0 AND ets.migrado_ne = 0) cons on ec.id = cons.id
                        where ec.id = @Id";

            return await db.QueryFirstOrDefaultAsync<Expediente>(sql, new { Id = id });
        }
        public async Task<Caratula> GetCaratulaExpedienteControl(int id)
        {
            var db = DbConnection();

            var sql = @"
                        select cons.NoProg NoProg, ec.id Id, cs.codigo Codigo, if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo, ec.id_expediente IdExpediente, ec.nombre Nombre, ec.numero_fojas Fojas, ec.numero_legajos Legajos, ec.fecha_primero FechaPrimeroAntiguo, ec.fecha_ultimo FechaUltimoReciente, ec.id_inventario_control IdInventario, ec.estatus Estatus
                            ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
                            ,ca.descripcion Area
                            ,crt.cant_doc_ori DocOriginales, crt.cant_doc_copias DocCopias, crt.cant_cds Cds, crt.tec_sel_doc TecnicasSeleccion, crt.publica Publica, crt.confidencial Confidencial, crt.reservada_sol_info Reservada, crt.descripcion_asunto_expediente DescripcionAsunto, crt.fecha_clasificacion FechaClasificacion, crt.periodo_reserva PeriodoReserva, crt.fundamento_legal FundamentoLegal, crt.ampliacion_periodo_reserva AmpliacionPeriodo, crt.fecha_desclasificacion FechaDesclasificacion, crt.nombre_desclasifica NombreDesclasifica, crt.cargo_desclasifica CargoDesclasifica, crt.partes_reservando PartesReservando, crt.datos_topograficos DatosTopograficos, crt.id_expediente_control
                        from prod_control_exp.expediente_control ec
                        join prod_control_exp.cat_serie_documental cs on ec.id_expediente = cs.id
                        join prod_control_exp.inventario_control itf on ec.id_inventario_control = itf.id
                        join prod_control_exp.cat_areas ca on itf.id_area = ca.id
                        join (select ROW_NUMBER() over(order by ets.fecha_ultimo) NoProg, ets.id from prod_control_exp.expediente_control ets where ets.id_inventario_control = (select id_inventario_control from prod_control_exp.expediente_control where id = @Id) AND ets.migrado_tp = 0 AND ets.migrado_ne = 0) cons on ec.id = cons.id
                        left join prod_control_exp.caratula crt on ec.id = crt.id_expediente_control
                        where ec.id = @Id";

            return await db.QueryFirstOrDefaultAsync<Caratula>(sql, new { Id = id });
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
        public async Task<Inventario> GetInventarioBibliohemerografico(string area)
        {
            var db = DbConnection();

            var sql = @"
                        select itr.id Id, itr.id_area IdArea, itr.nombre_responsable NombreResponsableAT, itr.fecha_transferencia FechaTransferencia, itr.fecha_elaboracion FechaElaboracion
                        from prod_control_exp.inventario_bibliohemerografico itr
                        join prod_control_exp.cat_areas ca on itr.id_area = ca.id
                        where ca.descripcion = @Area";

            return await db.QueryFirstOrDefaultAsync<Inventario>(sql, new { Area = area });
        }
        public async Task<bool> InsertInventarioBibliohemerografico(Inventario inventario)
        {
            var db = DbConnection();

            var sql = @"
                        INSERT INTO prod_control_exp.inventario_bibliohemerografico
                        (id_area, nombre_responsable, fecha_transferencia, fecha_elaboracion)
                        VALUES (@IdArea, @NombreResponsable, @FechaTransferencia, @FechaElaboracion)
                        ON DUPLICATE KEY UPDATE nombre_responsable = @NombreResponsable, fecha_transferencia = @FechaTransferencia, fecha_elaboracion = @FechaElaboracion;";

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
        public async Task<bool> UpdateExpedienteBibliohemerografico(ExpedienteBibliohemerografico expediente)
        {

            var db = DbConnection();

            var sql = @"
                        UPDATE prod_control_exp.expediente_bibliohemerografico SET numero_ejemplar = @Ejemplar, id_tipo_soporte = @IdSoporte, titulo_del_libro = @Titulo, nombre_autor = @Autor, tema = @Tema, editorial = @Editorial, anio = @Anio, isbn_issn = @Isbn, numero_paginas = @Paginas, numero_volumen = @Volumen, id_user = @IdUser
                        WHERE id = @Id;";

            var result = await db.ExecuteAsync(sql, new
            {
                Id = expediente.Id,
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
                //IdInventario = expediente.IdInventario,
                IdUser = expediente.IdUser
            });
            return result > 0;
        }
        public async Task<IEnumerable<ExpedienteBibliohemerografico>> GetExpedientesBibliohemerograficos(int id, int id_inventario)
        {
            var db = DbConnection();
            var sql = @"
                        select ROW_NUMBER() over(order by eb.anio,eb.id) NoProg, eb.id Id, eb.numero_ejemplar Ejemplar, eb.id_tipo_soporte IdTipoSoporte
                            ,cts.clave ClaveSoporte, cts.descripcion Soporte,eb.titulo_del_libro Titulo, eb.nombre_autor Autor, eb.tema Tema
                            ,eb.editorial Editorial, eb.anio Anio,eb.isbn_issn IsbnIssn,eb.numero_paginas Paginas, eb.numero_volumen Volumen
                            ,eb.fecha_registro FechaRegistro,eb.id_inventario_bibliohemerografico IdInventario, if(eb.id_user = @Id, 'editable', 'noeditable') EsEditable
                            ,eb.estatus Estatus
                        from prod_control_exp.expediente_bibliohemerografico eb
                        join prod_control_exp.cat_tipo_soporte cts on eb.id_tipo_soporte = cts.id
                        where eb.id_inventario_bibliohemerografico = @IdInv
                        order by eb.anio,eb.id;";
            return await db.QueryAsync<ExpedienteBibliohemerografico>(sql, new { Id = id, IdInv = id_inventario });
        }
        public async Task<ExpedienteBibliohemerografico> GetExpedienteBibliohemerografico(int id)
        {
            var db = DbConnection();
            var sql = @"
                        select cons.NoProg NoProg, eb.id Id, eb.numero_ejemplar Ejemplar, eb.id_tipo_soporte IdTipoSoporte, eb.titulo_del_libro Titulo, eb.nombre_autor Autor
                            ,eb.tema Tema, eb.editorial Editorial, eb.anio Anio,eb.isbn_issn IsbnIssn,eb.numero_paginas Paginas, eb.numero_volumen Volumen
                            ,eb.fecha_registro FechaRegistro,eb.id_inventario_bibliohemerografico IdInventario, if(eb.id_user = @Id, 'editable', 'noeditable') EsEditable
                            ,eb.estatus Estatus, cts.clave ClaveSoporte, cts.descripcion Soporte
                        from prod_control_exp.expediente_bibliohemerografico eb
                        join prod_control_exp.cat_tipo_soporte cts on eb.id_tipo_soporte = cts.id
                        join (select ROW_NUMBER() over(order by eb.anio, eb.id) NoProg, eb.id from prod_control_exp.expediente_bibliohemerografico eb where eb.id_inventario_bibliohemerografico = (select id_inventario_bibliohemerografico from prod_control_exp.expediente_bibliohemerografico where id = @Id)) cons on eb.id = cons.id
                        where eb.id = @Id;";
            return await db.QueryFirstOrDefaultAsync<ExpedienteBibliohemerografico>(sql, new { Id = id });
        }
        //public async Task<bool> SendValExpedienteBiblio(int id)
        //{
        //    var db = DbConnection();
        //    var sql = @"
        //                update prod_control_exp.expediente_bibliohemerografico set estatus = 2 where id = @Id;";
        //    var result = await db.ExecuteAsync(sql, new { Id = id });
        //    return result > 0;
        //}

        public async Task<bool> DropExpedienteBibliohemerografico(int id)
        {
            var db = DbConnection();
            var sql = @"
                        delete from prod_control_exp.expediente_bibliohemerografico where id = @Id;";
            var result = await db.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
        public async Task<bool> VoBoExpedienteBiblio(int id)
        {
            var db = DbConnection();
            var sql = @"
                        update prod_control_exp.expediente_bibliohemerografico set estatus = 3 where id = @Id;";
            var result = await db.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
        public async Task<bool> RevalidacionExpedienteBiblio(int id)
        {
            var db = DbConnection();
            var sql = @"
                        update prod_control_exp.expediente_bibliohemerografico set estatus = 4 where id = @Id;";
            var result = await db.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
        public async Task<Inventario> GetInventarioNoExpedientable(string area)
        {
            var db = DbConnection();

            var sql = @"
                        select itr.id Id, itr.id_area IdArea, itr.nombre_responsable NombreResponsableAT, itr.fecha_elaboracion FechaElaboracion, itr.fecha_transferencia FechaTransferencia
                        from prod_control_exp.inventario_noexpedientable itr
                        join prod_control_exp.cat_areas ca on itr.id_area = ca.id
                        where ca.descripcion = @Area";

            return await db.QueryFirstOrDefaultAsync<Inventario>(sql, new { Area = area });
        }
        public async Task<bool> InsertInventarioNoExpedientable(Inventario inventario)
        {
            var db = DbConnection();

            //var sql = @"
            //            INSERT INTO prod_control_exp.inventario_noexpedientable
            //            (id_area, nombre_responsable, fecha_elaboracion, fecha_transferencia)
            //            VALUES (@IdArea, @NombreResponsable, @FechaElaboracion, @FechaTransferencia)
            //            ON DUPLICATE KEY UPDATE nombre_responsable = @NombreResponsable, fecha_elaboracion = @FechaElaboracion, fecha_transferencia = @FechaTransferencia;";
            var sql = @"
                        UPDATE prod_control_exp.inventario_control SET fecha_transferencia = @FechaTransferencia WHERE id_area = @IdArea;";

            var result = await db.ExecuteAsync(sql, new
            {
                IdArea = inventario.IdArea,
                //NombreResponsable = inventario.NombreResponsableAT,
                //FechaElaboracion = inventario.FechaElaboracion,
                FechaTransferencia = inventario.FechaTransferencia,
            });
            return result > 0;
        }
        public async Task<bool> InsertExpedienteNoExpedientable(ExpedienteNoExpedientable expediente)
        {
            var db = DbConnection();

            var sql = @"
                        INSERT INTO prod_control_exp.expediente_no_expedientable
                        (id_tipo_documental, id_tipo_soporte, id_clave_interna, numero_partes, fecha_elaboracion, titulo_expediente, observaciones, id_inventario_no_expedientable, id_user)
                        VALUES (@IdTipoDocumental, @IdSoporte, @IdClaveInterna, @Partes, @FechaElaboracion, @Titulo, @Observaciones, @IdInventario, @IdUser);";

            var result = await db.ExecuteAsync(sql, new
            {
                IdTipoDocumental = expediente.IdTipoDocumental,
                IdSoporte = expediente.IdTipoSoporte,
                IdClaveInterna = expediente.IdClaveInterna,
                Partes = expediente.Partes,
                FechaElaboracion = expediente.FechaElaboracion,
                Titulo = expediente.Titulo,
                Observaciones = expediente.Observaciones,
                IdInventario = expediente.IdInventario,
                IdUser = expediente.IdUser
            });
            return result > 0;
        }
        public async Task<IEnumerable<Expediente>> GetExpedientesNoExpedientables(int id, int id_inventario)
        {
            var db = DbConnection();
            //var sql = @"
            //            select ROW_NUMBER() over(order by en.id) NoProg, en.id Id, cts.clave Clave, en.id_tipo_soporte IdTipoSoporte, en.id_tipo_documental IdTipoDocumental
            //                ,cts.descripcion Soporte,en.id_clave_interna IdClaveInterna, csd.codigo ClaveInterna, en.titulo_expediente Titulo, en.numero_partes Partes
            //                ,en.observaciones Observaciones, en.fecha_elaboracion FechaElaboracion, en.fecha_registro FechaRegistro
            //                ,en.id_inventario_no_expedientable IdInventario,if(en.id_user = @Id, 'editable', 'noeditable') EsEditable
            //            from prod_control_exp.expediente_no_expedientable en
            //            join prod_control_exp.cat_serie_documental csd on en.id_clave_interna = csd.id
            //            left join prod_control_exp.cat_tipo_soporte cts on en.id_tipo_soporte = cts.id
            //            where en.id_inventario_no_expedientable = @IdInv
            //            order by en.id;";
            var sql = @"
                        select ROW_NUMBER() over(order by ec.fecha_ultimo, ec.id) NoProg, ec.id Id, cts.clave Codigo, ec.id_tipo_soporte IdTipoSoporte, ec.id_tipo_documental IdTipoDocumental
	                        ,cts.descripcion Soporte, ec.id_expediente IdClaveInterna, csd.codigo Clave, ec.nombre Nombre, ec.numero_partes NoPartes
	                        ,ec.observaciones Observaciones, ec.fecha_elaboracion FechaElaboracion, ec.fecha_registro FechaRegistro
	                        ,ec.id_inventario_control IdInventario, if(ec.id_user = @Id, 'editable', 'noeditable') EsEditable
                            ,ec.estatus Estatus
                            ,if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo
                        from prod_control_exp.expediente_control ec
                        join prod_control_exp.cat_serie_documental csd on ec.id_expediente = csd.id
                        left join prod_control_exp.cat_tipo_soporte cts on ec.id_tipo_soporte = cts.id
                        where ec.id_inventario_control = @IdInv and ec.migrado_ne = 1
                        order by ec.fecha_ultimo, ec.id;";
            return await db.QueryAsync<Expediente>(sql, new { Id = id, IdInv = id_inventario });
        }
        public async Task<Expediente> GetNoExpedientable(int id)
        {
            var db = DbConnection();

            var sql = @"
                        select cons.NoProg NoProg, ec.id Id, cs.codigo Codigo, year(ec.fecha_elaboracion) Periodo, ec.id IdExpediente, ec.titulo_expediente Nombre, ec.fecha_elaboracion FechaElaboracion, ec.id_inventario_no_expedientable IdInventario
	                        ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
	                        ,ca.descripcion Area
                        from prod_control_exp.expediente_no_expedientable ec
                        join prod_control_exp.cat_serie_documental cs on ec.id = cs.id
                        join prod_control_exp.inventario_noexpedientable itf on ec.id_inventario_no_expedientable = itf.id
                        join prod_control_exp.cat_areas ca on itf.id_area = ca.id
                        join (select ROW_NUMBER() over(order by ets.id) NoProg, ets.id from prod_control_exp.expediente_no_expedientable ets where ets.id_inventario_no_expedientable = (select id_inventario_no_expedientable from expediente_no_expedientable where id = @Id)) cons on ec.id = cons.id
                        where ec.id = @Id";

            return await db.QueryFirstOrDefaultAsync<Expediente>(sql, new { Id = id });
        }
        public async Task<bool> InsertCaratulaNoExpedientable(Caratula caratula)
        {
            var db = DbConnection();

            var sql = @"
                        INSERT INTO prod_control_exp.caratula
                        (`fojas`,`cant_doc_ori`,`cant_doc_copias`,`cant_cds`,`tec_sel_doc`,`publica`,`confidencial`,`reservada_sol_info`,`descripcion_asunto_expediente`,`fecha_clasificacion`,`periodo_reserva`,`fundamento_legal`,`ampliacion_periodo_reserva`,`fecha_desclasificacion`,`nombre_desclasifica`,`cargo_desclasifica`,`partes_reservando`,`id_user_captura`,`datos_topograficos`,`id_expediente_noexp`)
                        VALUES (@Fojas, @DocOriginales, @DocCopias, @Cds, @TecnicasSeleccion, @Publica, @Confidencial, @Reservada, @DescripcionAsunto, @FechaClasificacion, @PeriodoReserva, @FundamentoLegal, @AmpliacionPeriodo, @FechaDesclasificacion, @NombreDesclasifica, @CargoDesclasifica, @PartesReservando, @IdUser, @DatosTopograficos, @IdExpediente)
                        ON DUPLICATE KEY UPDATE `fojas` = @Fojas, `cant_doc_ori` = @DocOriginales,`cant_doc_copias` = @DocCopias,`cant_cds` = @Cds,`tec_sel_doc` = @TecnicasSeleccion,`publica` = @Publica,`confidencial` = @Confidencial,`reservada_sol_info` = @Reservada,`descripcion_asunto_expediente` = @DescripcionAsunto,`fecha_clasificacion` = @FechaClasificacion,`periodo_reserva` = @PeriodoReserva,`fundamento_legal` = @FundamentoLegal,`ampliacion_periodo_reserva` = @AmpliacionPeriodo,`fecha_desclasificacion` = @FechaDesclasificacion,`nombre_desclasifica` = @NombreDesclasifica,`cargo_desclasifica` = @CargoDesclasifica,`partes_reservando` = @PartesReservando,`id_user_captura` = @IdUser,`datos_topograficos` = @DatosTopograficos;";

            var result = await db.ExecuteAsync(sql, new
            {
                IdExpediente = caratula.IdExpediente,
                //Nombre = caratula.Nombre,
                //NumeroLegajos = caratula.Legajos,
                Fojas = caratula.Fojas,
                DocOriginales = caratula.DocOriginales,
                DocCopias = caratula.DocCopias,
                Cds = caratula.Cds,
                TecnicasSeleccion = caratula.TecnicasSeleccion,
                Publica = caratula.Publica,
                Confidencial = caratula.Confidencial,
                Reservada = caratula.Reservada,
                DescripcionAsunto = caratula.DescripcionAsunto,
                FechaClasificacion = caratula.FechaClasificacion,
                PeriodoReserva = caratula.PeriodoReserva,
                FundamentoLegal = caratula.FundamentoLegal,
                AmpliacionPeriodo = caratula.AmpliacionPeriodo,
                FechaDesclasificacion = caratula.FechaDesclasificacion,
                NombreDesclasifica = caratula.NombreDesclasifica,
                CargoDesclasifica = caratula.CargoDesclasifica,
                PartesReservando = caratula.PartesReservando,
                DatosTopograficos = caratula.DatosTopograficos,
                IdUser = caratula.IdUser,
            });
            return result > 0;
        }
        public async Task<Caratula> GetCaratulaNoExpedientable(int id)
        {
            var db = DbConnection();

            //var sql = @"
            //            select cons.NoProg NoProg, ec.id Id, cs.codigo Codigo, year(ec.fecha_elaboracion) Periodo, ec.id IdExpediente, ec.titulo_expediente Nombre, ec.fecha_elaboracion FechaElaboracion, ec.id_inventario_no_expedientable IdInventario
	           //             ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
	           //             ,ca.descripcion Area
            //                ,crt.fojas Fojas, crt.cant_doc_ori DocOriginales, crt.cant_doc_copias DocCopias, crt.cant_cds Cds, crt.tec_sel_doc TecnicasSeleccion, crt.publica Publica, crt.confidencial Confidencial, crt.reservada_sol_info Reservada, crt.descripcion_asunto_expediente DescripcionAsunto, crt.fecha_clasificacion FechaClasificacion, crt.periodo_reserva PeriodoReserva, crt.fundamento_legal FundamentoLegal, crt.ampliacion_periodo_reserva AmpliacionPeriodo, crt.fecha_desclasificacion FechaDesclasificacion, crt.nombre_desclasifica NombreDesclasifica, crt.cargo_desclasifica CargoDesclasifica, crt.partes_reservando PartesReservando, crt.datos_topograficos DatosTopograficos, crt.id_expediente_noexp
            //            from prod_control_exp.expediente_no_expedientable ec
            //            join prod_control_exp.cat_serie_documental cs on ec.id = cs.id
            //            join prod_control_exp.inventario_noexpedientable itf on ec.id_inventario_no_expedientable = itf.id
            //            join prod_control_exp.cat_areas ca on itf.id_area = ca.id
            //            join (select ROW_NUMBER() over(order by ets.id) NoProg, ets.id from prod_control_exp.expediente_no_expedientable ets where ets.id_inventario_no_expedientable = (select id_inventario_no_expedientable from expediente_no_expedientable where id = @Id)) cons on ec.id = cons.id
            //            left join prod_control_exp.caratula crt on ec.id = crt.id_expediente_noexp
            //            where ec.id = @Id";
            var sql = @"
                        select cons.NoProg NoProg, ec.id Id, cs.codigo Codigo, ec.id_expediente IdExpediente, ec.nombre Nombre, if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo, ec.anios_resguardo AniosResguardo, ec.numero_legajos Legajos, ec.numero_fojas Fojas, ec.observaciones Observaciones, ec.fecha_registro FechaRegistro, ec.fecha_primero FechaPrimeroAntiguo, ec.fecha_ultimo FechaUltimoReciente, ec.id_inventario_control IdInventario
	                        ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
	                        ,ca.descripcion Area, ec.estatus Estatus
                            ,crt.cant_doc_ori DocOriginales, crt.cant_doc_copias DocCopias, crt.cant_cds Cds, crt.tec_sel_doc TecnicasSeleccion, crt.publica Publica, crt.confidencial Confidencial, crt.reservada_sol_info Reservada, crt.descripcion_asunto_expediente DescripcionAsunto, crt.fecha_clasificacion FechaClasificacion, crt.periodo_reserva PeriodoReserva, crt.fundamento_legal FundamentoLegal, crt.ampliacion_periodo_reserva AmpliacionPeriodo, crt.fecha_desclasificacion FechaDesclasificacion, crt.nombre_desclasifica NombreDesclasifica, crt.cargo_desclasifica CargoDesclasifica, crt.partes_reservando PartesReservando, crt.datos_topograficos DatosTopograficos, crt.id_expediente_noexp
                        from prod_control_exp.expediente_control ec
                        join prod_control_exp.cat_serie_documental cs on ec.id = cs.id
                        join prod_control_exp.inventario_control itf on ec.id_inventario_control = itf.id
                        join prod_control_exp.cat_areas ca on itf.id_area = ca.id
                        join (select ROW_NUMBER() over(order by ets.fecha_ultimo, ets.id) NoProg, ets.id from prod_control_exp.expediente_control ets where ets.id_inventario_control = (select id_inventario_control from prod_control_exp.expediente_control where id = @Id) AND ets.migrado_ne = 1) cons on ec.id = cons.id
                        left join prod_control_exp.caratula crt on ec.id = crt.id_expediente_control
                        where ec.id = @Id";

            return await db.QueryFirstOrDefaultAsync<Caratula>(sql, new { Id = id });
        }
        public async Task<bool> DropExpedienteNoExpedientable(int id)
        {
            var db = DbConnection();
            var sql = @"
                        delete from prod_control_exp.expediente_no_expedientable where id = @Id;";
            var result = await db.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
        public async Task<IEnumerable<Area>> GetAreasLista()
        {
            var db = DbConnection();
            var sql = @"
                        select id Id, descripcion Descripcion, estatus
                        from prod_control_exp.cat_areas
                        order by id;
                        ";
            return await db.QueryAsync<Area>(sql, new { });
        }
        public async Task<bool> UpdateArea(Area area)
        {
            var db = DbConnection();

            var sql = @"
                        UPDATE cat_areas
                        SET
                        descripcion = @Descripcion
                        WHERE id = @IdArea;";

            var result = await db.ExecuteAsync(sql, new
            {
                Descripcion = area.Descripcion,
                IdArea = area.Id
            });
            return result > 0;
        }
        public async Task<Area> GetArea(int id)
        {
            var db = DbConnection();

            var sql = @"
                        select id Id, descripcion Descripcion, estatus
                        from prod_control_exp.cat_areas
                        where id = @Id";

            return await db.QueryFirstOrDefaultAsync<Area>(sql, new { Id = id });
        }
    }
}
