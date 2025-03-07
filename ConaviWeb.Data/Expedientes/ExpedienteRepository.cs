﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
                        select id Id,concat(codigo,'-',descripcion) Clave from prod_control_exp.cat_serie_documental where estatus = 1 order by id;
                       ";
            return await db.QueryAsync<Catalogo>(sql, new { });
        }
        public async Task<IEnumerable<Catalogo>> GetAreas()
        {
            var db = DbConnection();
            var sql = @"
                        select id Id,descripcion Clave from prod_control_exp.cat_areas where estatus = 1 order by id;
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
            var sql = @"";
            if(serie.Id == 0)
            {
                sql = @"
                        INSERT INTO prod_control_exp.cat_serie_documental(codigo,descripcion,vig_doc_val_a,vig_doc_val_l,vig_doc_val_fc, vig_doc_pla_con_at, vig_doc_pla_con_ac, vig_doc_pla_con_tot,tec_sel_e, tec_sel_c, tec_sel_m, observaciones,id_seccion)
                        VALUES(@Codigo,@Descripcion,@VigDocValA,@VigDocValL,@VigDocValFC,@VigDocPlaConAt,@VigDocPlaConAC,@VigDocPlaConAt + @VigDocPlaConAC,@TecSelE,@TecSelC,@TecSelM,@Observaciones,1);";
            }
            else
            {
                sql = @"
                        UPDATE prod_control_exp.cat_serie_documental
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
            }
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
        public async Task<bool> ActivarSerieDocCat(int id)
        {
            var db = DbConnection();

            var sql = @"
                        UPDATE prod_control_exp.cat_serie_documental
                        SET
                        estatus = 1
                        WHERE id = @IdSerieDoc;";

            var result = await db.ExecuteAsync(sql, new
            {
                IdSerieDoc = id
            });
            return result > 0;
        }
        public async Task<bool> DesactivarSerieDocCat(int id)
        {
            var db = DbConnection();

            var sql = @"
                        UPDATE prod_control_exp.cat_serie_documental
                        SET
                        estatus = 2
                        WHERE id = @IdSerieDoc;";

            var result = await db.ExecuteAsync(sql, new
            {
                IdSerieDoc = id
            });
            return result > 0;
        }

        public async Task<Inventario> GetInventarioTP(string puesto)
        {
            var db = DbConnection();

            var sql = @"
                        select itr.id Id, itr.fecha_elaboracion FechaElaboracion, itr.fecha_transferencia FechaTransferencia, itr.nombre_responsable_archivo_tramite NombreResponsableAT
                        from prod_control_exp.inventario_transferencia itr
                        join prod_control_exp.cat_puestos cp on itr.id_puesto = cp.id
                        where cp.descripcion = @Puesto";

            return await db.QueryFirstOrDefaultAsync<Inventario>(sql, new { Puesto = puesto });
        }
        public async Task<bool> InsertInventarioTP(Inventario inventario)
        {
            var db = DbConnection();
            var sql = @"
                        UPDATE prod_control_exp.inventario_control SET fecha_transferencia = @FechaTransferencia WHERE id_puesto = @IdPuesto;";

            var result = await db.ExecuteAsync(sql, new
            {
                IdPuesto = inventario.IdPuesto,
                FechaTransferencia = inventario.FechaTransferencia,
            });
            return result > 0;
        }
        public async Task<IEnumerable<Expediente>> GetExpedientesInventarioTP(int id, int id_inventario)
        {
            var db = DbConnection();
            var sql = @"
                        select ROW_NUMBER() over(order by ec.fecha_ultimo, ec.id) NoProg, ROW_NUMBER() over(partition by year(ec.fecha_ultimo) order by ec.fecha_ultimo, ec.id) Consecutivo, ec.id Id, ec.id_expediente IdExpediente, csd.codigo Codigo, ec.nombre Nombre, if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo, ec.anios_resguardo AniosResguardo, ec.numero_legajos Legajos, ec.numero_fojas Fojas, ec.observaciones Observaciones, ec.fecha_registro FechaRegistro, ec.id_inventario_control IdInventario
                            ,if(ec.id_user = @id, 'editable', 'noeditable') EsEditable, ec.estatus Estatus
                        from prod_control_exp.expediente_control ec
                        join prod_control_exp.cat_serie_documental csd on ec.id_expediente = csd.id
                        where ec.id_inventario_control = @IdInv and ec.migrado_tp = 1
                        order by ec.fecha_ultimo, ec.id;";
            return await db.QueryAsync<Expediente>(sql, new { Id = id, IdInv = id_inventario });
        }
        public async Task<IEnumerable<Expediente>> GetExpedientesTPByIdInv(int id)
        {
            var db = DbConnection();

            var sql = @"
                        select ROW_NUMBER() over(order by ec.fecha_ultimo, ec.id) NoProg, ROW_NUMBER() over(partition by year(ec.fecha_ultimo) order by ec.fecha_ultimo, ec.id) Consecutivo, ec.id Id, ec.id_expediente IdExpediente, csd.codigo Codigo, ec.nombre Nombre, if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo, ec.anios_resguardo AniosResguardo, ec.numero_legajos Legajos, ec.numero_fojas Fojas, ec.observaciones Observaciones
                            ,ec.fecha_registro FechaRegistro, ec.id_inventario_control IdInventario, ec.estatus Estatus
                        from prod_control_exp.expediente_control ec
                        join prod_control_exp.cat_serie_documental csd on ec.id_expediente = csd.id
                        where ec.id_inventario_control = @Id and ec.migrado_tp = 1
                        order by ec.fecha_ultimo, ec.id;";
            return await db.QueryAsync<Expediente>(sql, new { Id = id });
        }
        public async Task<Caratula> GetCaratulaExpedienteTP(int id, int legajo)
        {
            var db = DbConnection();
            var sql = @"
                        select cons.NoProg, cons.Consecutivo, et.id Id, cs.codigo Codigo, id_expediente IdExpediente, nombre Nombre, if(year(et.fecha_primero)=year(et.fecha_ultimo),year(et.fecha_primero),concat(year(et.fecha_primero),'-',year(et.fecha_ultimo))) Periodo, anios_resguardo AniosResguardo, et.numero_legajos Legajos, if(et.numero_legajos>1,crt.fojas,ifnull(crt.fojas,et.numero_fojas)) Fojas, et.observaciones Observaciones, et.fecha_registro FechaRegistro, if(et.numero_legajos>1,date_format(crt.fecha_primero,'%Y/%m/%d'),ifnull(date_format(crt.fecha_primero,'%Y/%m/%d'),date_format(et.fecha_primero,'%Y/%m/%d'))) FechaPrimeroAntiguo, if(et.numero_legajos>1,date_format(crt.fecha_ultimo,'%Y/%m/%d'),ifnull(date_format(crt.fecha_ultimo,'%Y/%m/%d'),date_format(et.fecha_ultimo,'%Y/%m/%d'))) FechaUltimoReciente, et.id_inventario_control IdInventario, et.ubicacion DatosTopograficos, et.descripcion DescripcionAsunto, et.tipo_soporte_documental TipoSoporteDocumental
                            ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
                            ,cp.descripcion Puesto, et.estatus Estatus, ca.descripcion Area
                            ,crt.cant_doc_ori DocOriginales, crt.cant_doc_copias DocCopias, crt.cant_cds Cds, crt.tec_sel_doc TecnicasSeleccion, crt.publica Publica, crt.confidencial Confidencial, crt.reservada_sol_info Reservada, date_format(crt.fecha_clasificacion,'%Y/%m/%d') FechaClasificacion, crt.periodo_reserva PeriodoReserva, crt.fundamento_legal FundamentoLegal, crt.ampliacion_periodo_reserva AmpliacionPeriodo, date_format(crt.fecha_desclasificacion,'%Y/%m/%d') FechaDesclasificacion, crt.nombre_desclasifica NombreDesclasifica, crt.cargo_desclasifica CargoDesclasifica, crt.partes_reservando PartesReservando
                        from prod_control_exp.expediente_control et
                        join prod_control_exp.cat_serie_documental cs on et.id_expediente = cs.id
                        join prod_control_exp.inventario_control itf on et.id_inventario_control = itf.id
                        join prod_control_exp.cat_puestos cp on itf.id_puesto = cp.id
                        join prod_control_exp.cat_areas ca on cp.id_area = ca.id
                        join (select ROW_NUMBER() over(order by ets.fecha_ultimo, ets.id) NoProg, ROW_NUMBER() over(partition by year(ets.fecha_ultimo) order by ets.fecha_ultimo, ets.id) Consecutivo, ets.id from prod_control_exp.expediente_control ets where ets.id_inventario_control = (select id_inventario_control from prod_control_exp.expediente_control where id = @Id) AND ets.migrado_tp = 1 ORDER BY ets.fecha_ultimo, ets.id) cons on et.id = cons.id
                        left join prod_control_exp.caratula crt on et.id = crt.id_expediente_control and crt.legajo = @Legajo
                        where et.id = @Id";
            return await db.QueryFirstOrDefaultAsync<Caratula>(sql, new { Id = id, Legajo = legajo });
        }
        public async Task<int> sePuedeMigrarExpediente(int id, int tipo)
        {
            var db = DbConnection();
            var campos = "";
            if (tipo == 1) //para inventario transferencia primaria al archivo de concentración
                campos = " and ec.anios_resguardo is not null and ec.numero_fojas is not null";
            else //para inventario de documentación no expedientable
                campos = " and ec.numero_partes is not null and ec.fecha_elaboracion is not null and ec.id_tipo_documental is not null and ec.id_tipo_soporte is not null";
            var sql = @"
                        select id from prod_control_exp.expediente_control ec where id = @Id " + campos + ";";
            var result = await db.QueryFirstOrDefaultAsync<int>(sql, new { Id = id });
            if(result == 0)
            {
                sql = @"
                        UPDATE prod_control_exp.expediente_control SET estatus = 5 WHERE id = @Id;";
                _ = db.ExecuteAsync(sql, new { Id = id });
            }
            return result;
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
        public async Task<Inventario> GetInventarioControl(string puesto)
        {
            var db = DbConnection();

            var sql = @"
                        select itr.id Id, itr.id_puesto IdPuesto, itr.responsable_archivo_tramite NombreResponsableAT, date_format(itr.fecha_elaboracion, '%Y/%m/%d') FechaElaboracion, date_format(itr.fecha_entrega, '%Y/%m/%d') FechaEntrega, date_format(itr.fecha_transferencia,'%Y/%m/%d') FechaTransferencia, itr.ubicacion Ubicacion, itr.peso_electronico PesoElectronico, itr.almacenamiento Almacenamiento
                        from prod_control_exp.inventario_control itr
                        join prod_control_exp.cat_puestos cp on itr.id_puesto = cp.id
                        where cp.descripcion = @Puesto";

            return await db.QueryFirstOrDefaultAsync<Inventario>(sql, new { Puesto = puesto });
        }
        public async Task<Inventario> GetInventarioControlById(int id)
        {
            var db = DbConnection();

            var sql = @"
                        select itr.id Id, itr.id_puesto IdPuesto, ca.descripcion NombreUnidadAdministrativa, cp.descripcion NombrePuesto, itr.responsable_archivo_tramite NombreResponsableAT, date_format(itr.fecha_elaboracion,'%Y/%m/%d') FechaElaboracion, date_format(itr.fecha_entrega,'%Y/%m/%d') FechaEntrega, date_format(itr.fecha_transferencia,'%Y/%m/%d') FechaTransferencia, itr.ubicacion Ubicacion, itr.peso_electronico PesoElectronico, itr.almacenamiento Almacenamiento
                        from prod_control_exp.inventario_control itr
                        join prod_control_exp.cat_puestos cp on itr.id_puesto = cp.id
                        join prod_control_exp.cat_areas ca on cp.id_area = ca.id
                        where itr.id = @Id";

            return await db.QueryFirstOrDefaultAsync<Inventario>(sql, new { Id = id });
        }
        public async Task<bool> InsertInventarioControl(Inventario inventario)
        {
            var db = DbConnection();

            var sql = @"
                        INSERT INTO prod_control_exp.inventario_control
                        (id_puesto, responsable_archivo_tramite, fecha_elaboracion, fecha_entrega, ubicacion, peso_electronico, almacenamiento)
                        VALUES (@IdPuesto, @NombreResponsable, @FechaElaboracion, @FechaEntrega, @Ubicacion, @Peso, @Almacenamiento)
                        ON DUPLICATE KEY UPDATE id_puesto = @IdPuesto, responsable_archivo_tramite = @NombreResponsable, fecha_elaboracion = @FechaElaboracion, fecha_entrega = @FechaEntrega, ubicacion = @Ubicacion, peso_electronico = @Peso, almacenamiento = @Almacenamiento;";

            var result = await db.ExecuteAsync(sql, new
            {
                IdPuesto = inventario.IdPuesto,
                NombreResponsable = inventario.NombreResponsableAT,
                FechaElaboracion = inventario.FechaElaboracion,
                FechaEntrega = inventario.FechaEntrega,
                Ubicacion = inventario.Ubicacion,
                Peso = inventario.PesoElectronico,
                Almacenamiento = inventario.Almacenamiento
            });
            return result > 0;
        }
        public async Task<bool> InsertExpedienteInventarioControl(Expediente expediente)
        {
            var db = DbConnection();
            var sql = @"
                        INSERT INTO prod_control_exp.expediente_control
                        (nombre, numero_legajos, numero_fojas, numero_partes, fecha_primero, fecha_ultimo, fecha_elaboracion, ubicacion, descripcion, observaciones, anios_resguardo, id_user, id_expediente, id_inventario_control, id_tipo_documental, id_tipo_soporte, tipo_soporte_documental)
                        VALUES (@Nombre, @NumeroLegajos, @NumeroFojas, @NumeroPartes, @FechaPrimero, @FechaUltimo, @FechaElaboracion, @Ubicacion, @Descripcion, @Observaciones, @AniosResguardo, @IdUser, @IdExpediente, @IdInventario, @IdTipoDocumental, @IdTipoSoporte, @TipoSoporteDocumental);";

            var result = await db.ExecuteAsync(sql, new
            {
                Nombre = expediente.Nombre,
                NumeroLegajos = expediente.Legajos,
                NumeroFojas = expediente.Fojas,
                NumeroPartes = expediente.NoPartes,
                FechaPrimero = expediente.FechaPrimeroAntiguo,
                FechaUltimo = expediente.FechaUltimoReciente,
                FechaElaboracion = expediente.FechaElaboracion,
                Ubicacion = expediente.Ubicacion,
                Descripcion = expediente.Descripcion,
                Observaciones = expediente.Observaciones,
                AniosResguardo = expediente.AniosResguardo,
                IdUser = expediente.IdUser,
                IdExpediente = expediente.IdExpediente,
                IdInventario = expediente.IdInventario,
                IdTipoDocumental = expediente.IdTipoDocumental,
                IdTipoSoporte = expediente.IdTipoSoporte,
                TipoSoporteDocumental = expediente.IdTipoSoporteDocumental
            });
            return result > 0;
        }
        public async Task<bool> UpdateExpedienteInventarioControl(Expediente expediente)
        {
            var db = DbConnection();
            var sql = @"
                        UPDATE prod_control_exp.expediente_control set nombre = @Nombre, numero_legajos = @NumeroLegajos, numero_fojas = @NumeroFojas, numero_partes = @NumeroPartes, fecha_primero = @FechaPrimero, fecha_ultimo = @FechaUltimo, fecha_elaboracion = @FechaElaboracion, ubicacion = @Ubicacion, descripcion = @Descripcion, observaciones = @Observaciones, anios_resguardo = @AniosResguardo, id_user = @IdUser, id_expediente = @IdExpediente, id_inventario_control = @IdInventario, id_tipo_documental = @IdTipoDocumental, id_tipo_soporte = @IdTipoSoporte, tipo_soporte_documental = @TipoSoporteDocumental
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
                Ubicacion = expediente.Ubicacion,
                Descripcion = expediente.Descripcion,
                Observaciones = expediente.Observaciones,
                AniosResguardo = expediente.AniosResguardo,
                IdUser = expediente.IdUser,
                IdExpediente = expediente.IdExpediente,
                IdInventario = expediente.IdInventario,
                IdTipoDocumental = expediente.IdTipoDocumental,
                IdTipoSoporte = expediente.IdTipoSoporte,
                TipoSoporteDocumental = expediente.IdTipoSoporteDocumental
            });
            return result > 0;
        }
        public async Task<bool> InsertCaratulaExpedienteIC(Caratula caratula)
        {
            var db = DbConnection();
            var sql = @"
                        INSERT INTO prod_control_exp.caratula
                        (`cant_doc_ori`,`cant_doc_copias`,`cant_cds`,`tec_sel_doc`,`publica`,`confidencial`,`reservada_sol_info`,`fecha_clasificacion`,`periodo_reserva`,`fundamento_legal`,`ampliacion_periodo_reserva`,`fecha_desclasificacion`,`nombre_desclasifica`,`cargo_desclasifica`,`partes_reservando`,`id_user_captura`,`id_expediente_control`,`fecha_primero`,`fecha_ultimo`,`fojas`,`legajo`)
                        VALUES (@DocOriginales, @DocCopias, @Cds, @TecnicasSeleccion, @Publica, @Confidencial, @Reservada, @FechaClasificacion, @PeriodoReserva, @FundamentoLegal, @AmpliacionPeriodo, @FechaDesclasificacion, @NombreDesclasifica, @CargoDesclasifica, @PartesReservando, @IdUser, @IdExpediente, @FechaPrimero, @FechaUltimo, @Fojas, @Legajo)
                        ON DUPLICATE KEY UPDATE `cant_doc_ori` = @DocOriginales,`cant_doc_copias` = @DocCopias,`cant_cds` = @Cds,`tec_sel_doc` = @TecnicasSeleccion,`publica` = @Publica,`confidencial` = @Confidencial,`reservada_sol_info` = @Reservada,`fecha_clasificacion` = @FechaClasificacion,`periodo_reserva` = @PeriodoReserva,`fundamento_legal` = @FundamentoLegal,`ampliacion_periodo_reserva` = @AmpliacionPeriodo,`fecha_desclasificacion` = @FechaDesclasificacion,`nombre_desclasifica` = @NombreDesclasifica,`cargo_desclasifica` = @CargoDesclasifica,`partes_reservando` = @PartesReservando,`id_user_captura` = @IdUser, `fecha_primero` = @FechaPrimero, `fecha_ultimo` = @FechaUltimo, `fojas` = @Fojas, `legajo` = @Legajo;";
            
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
                FechaClasificacion = caratula.FechaClasificacion,
                PeriodoReserva = caratula.PeriodoReserva,
                FundamentoLegal = caratula.FundamentoLegal,
                AmpliacionPeriodo = caratula.AmpliacionPeriodo,
                FechaDesclasificacion = caratula.FechaDesclasificacion,
                NombreDesclasifica = caratula.NombreDesclasifica,
                CargoDesclasifica = caratula.CargoDesclasifica,
                PartesReservando = caratula.PartesReservando,
                FechaPrimero = caratula.FechaPrimeroAntiguo,
                FechaUltimo = caratula.FechaUltimoReciente,
                Fojas = caratula.Fojas,
                Legajo = caratula.Legajos,
                IdUser = caratula.IdUser,
            });
            return result > 0;
        }
        public async Task<IEnumerable<Expediente>> GetExpedientesInventarioControl(int id, int id_inventario)
        {
            var db = DbConnection();
            var sql = @"
                        select ROW_NUMBER() over(order by ec.fecha_ultimo, ec.id) NoProg, ROW_NUMBER() over(partition by year(ec.fecha_ultimo) order by ec.fecha_ultimo, ec.id) Consecutivo, ec.id Id, ec.id_expediente IdExpediente, if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo, ec.nombre Nombre, ec.numero_legajos Legajos, ec.numero_fojas Fojas, ec.observaciones Observaciones, ec.fecha_primero FechaPrimeroAntiguo, ec.fecha_ultimo FechaUltimoReciente, ec.id_inventario_control IdInventario, ec.obs_revalidacion ObservacionesRevalidacion, ec.ubicacion Ubicacion, ec.descripcion Descripcion, ec.tipo_soporte_documental TipoSoporteDocumental
                            ,if(ec.id_user = @Id, 'editable', 'noeditable') EsEditable, ec.estatus Estatus
                            -- ,if(ec.migrado_tp = 1, ec.migrado_tp, if(ec.anios_resguardo is not null and ec.numero_fojas is not null, 0, 1)) MigradoTP
                            ,ec.migrado_tp MigradoTP, ec.migrado_ne MigradoNE
                            -- ,if(ec.migrado_ne = 1, ec.migrado_ne, if(ec.numero_partes is not null and ec.fecha_elaboracion is not null and ec.id_tipo_documental is not null and ec.id_tipo_soporte is not null, 0, 1)) MigradoNE
                            ,csd.codigo Codigo, csd.vig_doc_val_a VigDocValA, csd.vig_doc_val_l VigDocValL, csd.vig_doc_val_fc VigDocValFC, csd.vig_doc_pla_con_at VigDocPlaConAT, csd.vig_doc_pla_con_ac VigDocPlaConAC, csd.vig_doc_pla_con_tot VigDocPlaConTot
                            ,cds.no_cds CDs
                        from prod_control_exp.expediente_control ec
                        join prod_control_exp.cat_serie_documental csd on ec.id_expediente = csd.id
                        left join (select coalesce(sum(cant_cds),0) no_cds, id_expediente_control from prod_control_exp.caratula group by id_expediente_control) cds on ec.id = cds.id_expediente_control
                        where ec.id_inventario_control = @IdInv AND ec.migrado_tp = 0 AND ec.migrado_ne = 0
                        order by ec.fecha_ultimo, ec.id;";
            return await db.QueryAsync<Expediente>(sql, new { Id = id, IdInv = id_inventario });
        }
        public async Task<IEnumerable<Expediente>> GetExpedientesInventarioControlByIdInv(int id)
        {
            var db = DbConnection();
            var sql = @"
                        select ROW_NUMBER() over(order by ec.fecha_ultimo, ec.id) NoProg, ROW_NUMBER() over(partition by year(ec.fecha_ultimo) order by ec.fecha_ultimo, ec.id) Consecutivo, ec.id Id, ec.id_expediente IdExpediente, csd.codigo Codigo, if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo, ec.nombre Nombre, ec.numero_legajos Legajos, ec.fecha_primero FechaPrimeroAntiguo
                            ,ec.fecha_ultimo FechaUltimoReciente, ec.id_inventario_control IdInventario, ec.obs_revalidacion ObservacionesRevalidacion, ec.estatus Estatus
                            ,if(ec.migrado_tp = 1, ec.migrado_tp, if(ec.anios_resguardo is not null and ec.numero_fojas is not null, 0, 1)) MigradoTP
                            ,if(ec.migrado_ne = 1, ec.migrado_ne, if(ec.numero_partes is not null and ec.fecha_elaboracion is not null and ec.id_tipo_documental is not null and ec.id_tipo_soporte is not null, 0, 1)) MigradoNE
                        from prod_control_exp.expediente_control ec
                        join prod_control_exp.cat_serie_documental csd on ec.id_expediente = csd.id
                        where ec.id_inventario_control = @IdInv AND ec.migrado_tp = 0 AND ec.migrado_ne = 0
                        order by ec.fecha_ultimo, ec.id;";
            return await db.QueryAsync<Expediente>(sql, new { IdInv = id });
        }
        public async Task<IEnumerable<Expediente>> GetExpedientesValidacionInventarioControl(int idPuesto)
        {
            var db = DbConnection();
            var sql = @"
                        select ROW_NUMBER() over(order by ec.fecha_ultimo, ec.id) NoProg, cons.Consecutivo, ec.id Id, ec.id_expediente IdExpediente, csd.codigo Codigo, if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo, ec.nombre Nombre, ec.numero_legajos Legajos, ec.fecha_primero FechaPrimeroAntiguo, ec.fecha_ultimo FechaUltimoReciente, ec.id_inventario_control IdInventario, ec.estatus Estatus
                        from prod_control_exp.expediente_control ec
                        join prod_control_exp.inventario_control ic on ec.id_inventario_control = ic.id
                        join prod_control_exp.cat_serie_documental csd on ec.id_expediente = csd.id
                        join (select ROW_NUMBER() over(partition by year(ets.fecha_ultimo) order by ets.fecha_ultimo, ets.id) Consecutivo, ets.id from prod_control_exp.expediente_control ets join prod_control_exp.inventario_control ic2 on ets.id_inventario_control = ic2.id and ic2.id_puesto = @IdPuesto WHERE ets.migrado_tp = 0 AND ets.migrado_ne = 0 ORDER BY ets.fecha_ultimo, ets.id) cons on ec.id = cons.id
                        where ic.id_puesto = @IdPuesto and ec.estatus = 2 AND ec.migrado_tp = 0 and ec.migrado_ne = 0
                        order by ec.fecha_ultimo, ec.id;";
            return await db.QueryAsync<Expediente>(sql, new { IdPuesto = idPuesto });
        }
        public async Task<Expediente> GetExpedienteControl(int id)
        {
            var db = DbConnection();

            var sql = @"
                        select cons.NoProg, cons.Consecutivo, ec.id Id, cs.codigo Codigo, if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo, ec.id_expediente IdExpediente, nombre Nombre, ec.descripcion Descripcion, ec.observaciones Observaciones, ec.numero_fojas Fojas, ec.numero_legajos Legajos, date_format(ec.fecha_primero,'%Y/%m/%d') FechaPrimeroAntiguo, date_format(ec.fecha_ultimo,'%Y/%m/%d') FechaUltimoReciente, ec.anios_resguardo AniosResguardo, ec.id_tipo_documental IdTipoDocumental, ec.id_tipo_soporte IdTipoSoporte, ec.numero_partes NoPartes, date_format(ec.fecha_elaboracion,'%Y/%m/%d') FechaElaboracion, id_inventario_control IdInventario, ec.tipo_soporte_documental TipoSoporteDocumental, ec.ubicacion Ubicacion
                            ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
                            ,cp.descripcion Area, ec.estatus Estatus
                        from prod_control_exp.expediente_control ec
                        join prod_control_exp.cat_serie_documental cs on ec.id_expediente = cs.id
                        join prod_control_exp.inventario_control itf on ec.id_inventario_control = itf.id
                        join prod_control_exp.cat_puestos cp on itf.id_puesto = cp.id
                        join (select ROW_NUMBER() over(order by ets.fecha_ultimo, ets.id) NoProg, ROW_NUMBER() over(partition by year(ets.fecha_ultimo) order by ets.fecha_ultimo, ets.id) Consecutivo, ets.id from prod_control_exp.expediente_control ets WHERE ets.id_inventario_control = (select id_inventario_control from prod_control_exp.expediente_control where id = @Id) AND ets.migrado_tp = 0 AND ets.migrado_ne = 0 ORDER BY ets.fecha_ultimo, ets.id) cons on ec.id = cons.id
                        where ec.id = @Id";

            return await db.QueryFirstOrDefaultAsync<Expediente>(sql, new { Id = id });
        }
        public async Task<Caratula> GetCaratulaExpedienteControl(int id, int legajo, int idUser)
        {
            var db = DbConnection();
            var sql = @"
                        select cons.NoProg, cons.Consecutivo, ec.id Id, cs.codigo Codigo, if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo, ec.id_expediente IdExpediente, ec.nombre Nombre, if(ec.numero_legajos>1,crt.fojas,ifnull(crt.fojas,ec.numero_fojas)) Fojas, ec.numero_legajos Legajos, if(ec.numero_legajos>1,date_format(crt.fecha_primero,'%Y/%m/%d'),ifnull(date_format(crt.fecha_primero,'%Y/%m/%d'),date_format(ec.fecha_primero,'%Y/%m/%d'))) FechaPrimeroAntiguo, if(ec.numero_legajos>1,date_format(crt.fecha_ultimo,'%Y/%m/%d'),ifnull(date_format(crt.fecha_ultimo,'%Y/%m/%d'),date_format(ec.fecha_ultimo,'%Y/%m/%d'))) FechaUltimoReciente, ec.id_inventario_control IdInventario, ec.estatus Estatus, ec.descripcion DescripcionAsunto, ec.observaciones Observaciones, ec.tipo_soporte_documental TipoSoporteDocumental, ec.ubicacion DatosTopograficos
                            ,if(ec.id_user = @IdUser, 'editable', 'noeditable') EsEditable
                            ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
                            ,cp.descripcion Puesto
                            ,ca.descripcion Area
                            ,crt.cant_doc_ori DocOriginales, crt.cant_doc_copias DocCopias, crt.cant_cds Cds, crt.tec_sel_doc TecnicasSeleccion, crt.publica Publica, crt.confidencial Confidencial, crt.reservada_sol_info Reservada, date_format(crt.fecha_clasificacion,'%Y/%m/%d') FechaClasificacion, crt.periodo_reserva PeriodoReserva, crt.fundamento_legal FundamentoLegal, crt.ampliacion_periodo_reserva AmpliacionPeriodo, date_format(crt.fecha_desclasificacion,'%Y/%m/%d') FechaDesclasificacion, crt.nombre_desclasifica NombreDesclasifica, crt.cargo_desclasifica CargoDesclasifica, crt.partes_reservando PartesReservando, crt.id_expediente_control
                            ,concat(u.nombre,' ',u.primer_apellido,' ',u.segundo_apellido) UserName
                        from prod_control_exp.expediente_control ec
                        join prod_control_exp.cat_serie_documental cs on ec.id_expediente = cs.id
                        join prod_control_exp.inventario_control itf on ec.id_inventario_control = itf.id
                        join prod_control_exp.cat_puestos cp on itf.id_puesto = cp.id
                        join prod_control_exp.cat_areas ca on cp.id_area = ca.id
                        join (select ROW_NUMBER() over(order by ets.fecha_ultimo, ets.id) NoProg, ROW_NUMBER() over(partition by year(ets.fecha_ultimo) order by ets.fecha_ultimo, ets.id) Consecutivo, ets.id from prod_control_exp.expediente_control ets where ets.id_inventario_control = (select id_inventario_control from prod_control_exp.expediente_control where id = @Id) AND ets.migrado_tp = 0 AND ets.migrado_ne = 0 ORDER BY ets.fecha_ultimo, ets.id) cons on ec.id = cons.id
                        left join prod_control_exp.caratula crt on ec.id = crt.id_expediente_control and legajo = @Legajo
                        left join qa_adms_conavi.usuario u on crt.id_user_captura = u.id
                        where ec.id = @Id";

            return await db.QueryFirstOrDefaultAsync<Caratula>(sql, new { Id = id, Legajo = legajo, IdUser = idUser });
        }
        public async Task<bool> DropExpedienteControl(int id)
        {
            var db = DbConnection();
            var sql = @"
                        delete ec,cc from prod_control_exp.expediente_control ec left join prod_control_exp.caratula cc on ec.id = cc.id_expediente_control where ec.id = @Id;";
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
        public async Task<bool> RevalidacionExpedienteControl(int id, string obs)
        {
            var db = DbConnection();
            var sql = @"
                        update prod_control_exp.expediente_control set estatus = 4, obs_revalidacion = @Obs where id = @Id;";
            var result = await db.ExecuteAsync(sql, new { Id = id, Obs = obs });
            return result > 0;
        }
        public async Task<Inventario> GetInventarioBibliohemerografico(string puesto)
        {
            var db = DbConnection();

            var sql = @"
                        select itr.id Id, itr.id_puesto IdPuesto, itr.nombre_responsable NombreResponsableAT, date_format(itr.fecha_transferencia,'%Y/%m/%d') FechaTransferencia, date_format(itr.fecha_elaboracion,'%Y/%m/%d') FechaElaboracion
                        from prod_control_exp.inventario_bibliohemerografico itr
                        join prod_control_exp.cat_puestos cp on itr.id_puesto = cp.id
                        where cp.descripcion = @Puesto";

            return await db.QueryFirstOrDefaultAsync<Inventario>(sql, new { Puesto = puesto });
        }
        public async Task<Inventario> GetInventarioBiblioById(int id)
        {
            var db = DbConnection();

            var sql = @"
                        select itr.id Id, itr.id_puesto IdPuesto, cp.descripcion NombreUnidadAdministrativa, itr.nombre_responsable NombreResponsableAT, date_format(itr.fecha_transferencia,'%Y/%m/%d') FechaTransferencia, date_format(itr.fecha_elaboracion,'%Y/%m/%d') FechaElaboracion
                        from prod_control_exp.inventario_bibliohemerografico itr
                        join prod_control_exp.cat_puestos cp on itr.id_puesto = cp.id
                        where itr.id = @Id";

            return await db.QueryFirstOrDefaultAsync<Inventario>(sql, new { Id = id });
        }
        public async Task<bool> InsertInventarioBibliohemerografico(Inventario inventario)
        {
            var db = DbConnection();

            var sql = @"
                        INSERT INTO prod_control_exp.inventario_bibliohemerografico
                        (id_puesto, nombre_responsable, fecha_transferencia, fecha_elaboracion)
                        VALUES (@IdPuesto, @NombreResponsable, @FechaTransferencia, @FechaElaboracion)
                        ON DUPLICATE KEY UPDATE nombre_responsable = @NombreResponsable, fecha_transferencia = @FechaTransferencia, fecha_elaboracion = @FechaElaboracion;";

            var result = await db.ExecuteAsync(sql, new
            {
                IdPuesto = inventario.IdPuesto,
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
                IdUser = expediente.IdUser
            });
            return result > 0;
        }
        public async Task<IEnumerable<ExpedienteBibliohemerografico>> GetExpedientesBibliohemerograficos(int id, int id_inventario)
        {
            var db = DbConnection();
            var sql = @"
                        select ROW_NUMBER() over(order by eb.anio,eb.id) NoProg, ROW_NUMBER() over(partition by eb.anio order by eb.anio,eb.id) Consecutivo, eb.id Id, eb.numero_ejemplar Ejemplar, eb.id_tipo_soporte IdTipoSoporte
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
        public async Task<IEnumerable<ExpedienteBibliohemerografico>> GetExpedientesBibliohemerograficosByIdInv(int id)
        {
            var db = DbConnection();
            var sql = @"
                        select ROW_NUMBER() over(order by eb.anio,eb.id) NoProg, ROW_NUMBER() over(partition by eb.anio order by eb.anio,eb.id) Consecutivo, eb.id Id, eb.numero_ejemplar Ejemplar, eb.id_tipo_soporte IdTipoSoporte
                            ,cts.clave ClaveSoporte, cts.descripcion Soporte,eb.titulo_del_libro Titulo, eb.nombre_autor Autor, eb.tema Tema
                            ,eb.editorial Editorial, eb.anio Anio,eb.isbn_issn IsbnIssn,eb.numero_paginas Paginas, eb.numero_volumen Volumen
                            ,eb.fecha_registro FechaRegistro,eb.id_inventario_bibliohemerografico IdInventario
                            ,eb.estatus Estatus
                        from prod_control_exp.expediente_bibliohemerografico eb
                        join prod_control_exp.cat_tipo_soporte cts on eb.id_tipo_soporte = cts.id
                        where eb.id_inventario_bibliohemerografico = @IdInv
                        order by eb.anio,eb.id;";
            return await db.QueryAsync<ExpedienteBibliohemerografico>(sql, new { IdInv = id });
        }
        public async Task<ExpedienteBibliohemerografico> GetExpedienteBibliohemerografico(int id)
        {
            var db = DbConnection();
            var sql = @"
                        select cons.NoProg, cons.Consecutivo, eb.id Id, eb.numero_ejemplar Ejemplar, eb.id_tipo_soporte IdTipoSoporte, eb.titulo_del_libro Titulo, eb.nombre_autor Autor
                            ,eb.tema Tema, eb.editorial Editorial, eb.anio Anio,eb.isbn_issn IsbnIssn,eb.numero_paginas Paginas, eb.numero_volumen Volumen
                            ,eb.fecha_registro FechaRegistro,eb.id_inventario_bibliohemerografico IdInventario, if(eb.id_user = @Id, 'editable', 'noeditable') EsEditable
                            ,eb.estatus Estatus, cts.clave ClaveSoporte, cts.descripcion Soporte
                        from prod_control_exp.expediente_bibliohemerografico eb
                        join prod_control_exp.cat_tipo_soporte cts on eb.id_tipo_soporte = cts.id
                        join (select ROW_NUMBER() over(order by eb.anio, eb.id) NoProg, ROW_NUMBER() over(partition by eb.anio order by eb.anio, eb.id) Consecutivo, eb.id from prod_control_exp.expediente_bibliohemerografico eb where eb.id_inventario_bibliohemerografico = (select id_inventario_bibliohemerografico from prod_control_exp.expediente_bibliohemerografico where id = @Id) ORDER BY eb.anio, eb.id) cons on eb.id = cons.id
                        where eb.id = @Id;";
            return await db.QueryFirstOrDefaultAsync<ExpedienteBibliohemerografico>(sql, new { Id = id });
        }
        public async Task<bool> DropExpedienteBibliohemerografico(int id)
        {
            var db = DbConnection();
            var sql = @"
                        delete from prod_control_exp.expediente_bibliohemerografico where id = @Id;";
            var result = await db.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
        public async Task<bool> InsertInventarioNoExpedientable(Inventario inventario)
        {
            var db = DbConnection();

            var sql = @"
                        UPDATE prod_control_exp.inventario_control SET fecha_transferencia = @FechaTransferencia WHERE id_puesto = @IdPuesto;";

            var result = await db.ExecuteAsync(sql, new
            {
                IdPuesto = inventario.IdPuesto,
                FechaTransferencia = inventario.FechaTransferencia,
            });
            return result > 0;
        }
        public async Task<IEnumerable<Expediente>> GetExpedientesNoExpedientables(int id, int id_inventario)
        {
            var db = DbConnection();
            var sql = @"
                        select ROW_NUMBER() over(order by ec.fecha_ultimo, ec.id) NoProg, ROW_NUMBER() over(partition by year(ec.fecha_ultimo) order by ec.fecha_ultimo, ec.id) Consecutivo, ec.id Id, cts.clave Codigo, ec.id_tipo_soporte IdTipoSoporte, ec.id_tipo_documental IdTipoDocumental
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
        public async Task<IEnumerable<Expediente>> GetExpedientesNoExpedientablesByIdInv(int id)
        {
            var db = DbConnection();
            var sql = @"
                        select ROW_NUMBER() over(order by ec.fecha_ultimo, ec.id) NoProg, ROW_NUMBER() over(partition by year(ec.fecha_ultimo) order by ec.fecha_ultimo, ec.id) Consecutivo, ec.id Id, cts.clave Codigo, ec.id_tipo_soporte IdTipoSoporte, ec.id_tipo_documental IdTipoDocumental
	                        ,cts.descripcion Soporte, ec.id_expediente IdClaveInterna, csd.codigo Clave, ec.nombre Nombre, ec.numero_partes NoPartes
	                        ,ec.observaciones Observaciones, ec.fecha_elaboracion FechaElaboracion, ec.fecha_registro FechaRegistro, ec.id_inventario_control IdInventario
                            ,ec.estatus Estatus
                            ,if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo
                        from prod_control_exp.expediente_control ec
                        join prod_control_exp.cat_serie_documental csd on ec.id_expediente = csd.id
                        left join prod_control_exp.cat_tipo_soporte cts on ec.id_tipo_soporte = cts.id
                        where ec.id_inventario_control = @IdInv and ec.migrado_ne = 1
                        order by ec.fecha_ultimo, ec.id;";
            return await db.QueryAsync<Expediente>(sql, new { IdInv = id });
        }
        public async Task<Expediente> GetNoExpedientable(int id)
        {
            var db = DbConnection();

            var sql = @"
                        select cons.NoProg, cons.Consecutivo, ec.id Id, cs.codigo Codigo, year(ec.fecha_elaboracion) Periodo, ec.id IdExpediente, ec.titulo_expediente Nombre, date_format(ec.fecha_elaboracion,'%Y/%m/%d') FechaElaboracion, ec.id_inventario_no_expedientable IdInventario
	                        ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
	                        ,cp.descripcion Area
                        from prod_control_exp.expediente_no_expedientable ec
                        join prod_control_exp.cat_serie_documental cs on ec.id = cs.id
                        join prod_control_exp.inventario_noexpedientable itf on ec.id_inventario_no_expedientable = itf.id
                        join prod_control_exp.cat_puestos cp on itf.id_puesto = cp.id
                        join (select ROW_NUMBER() over(order by ets.fecha_ultimo, ets.id) NoProg, ROW_NUMBER() over(partition by year(ets.fecha_ultimo) order by ets.fecha_ultimo, ets.id) Consecutivo, ets.id from prod_control_exp.expediente_no_expedientable ets where ets.id_inventario_no_expedientable = (select id_inventario_no_expedientable from expediente_no_expedientable where id = @Id) ORDER BY ets.fecha_ultimo, ets.id) cons on ec.id = cons.id
                        where ec.id = @Id";

            return await db.QueryFirstOrDefaultAsync<Expediente>(sql, new { Id = id });
        }
        public async Task<Caratula> GetCaratulaNoExpedientable(int id, int legajo)
        {
            var db = DbConnection();

            var sql = @"
                        select cons.NoProg, cons.Consecutivo, ec.id Id, cs.codigo Codigo, ec.id_expediente IdExpediente, ec.nombre Nombre, if(year(ec.fecha_primero)=year(ec.fecha_ultimo),year(ec.fecha_primero),concat(year(ec.fecha_primero),'-',year(ec.fecha_ultimo))) Periodo, ec.anios_resguardo AniosResguardo, ec.numero_legajos Legajos, if(ec.numero_legajos>1,crt.fojas,ifnull(crt.fojas,ec.numero_fojas)) Fojas, ec.observaciones Observaciones, ec.fecha_registro FechaRegistro, if(ec.numero_legajos>1,date_format(crt.fecha_primero,'%Y/%m/%d'),ifnull(date_format(crt.fecha_primero,'%Y/%m/%d'),date_format(ec.fecha_primero,'%Y/%m/%d'))) FechaPrimeroAntiguo, if(ec.numero_legajos>1,date_format(crt.fecha_ultimo,'%Y/%m/%d'),ifnull(date_format(crt.fecha_ultimo,'%Y/%m/%d'),date_format(ec.fecha_ultimo,'%Y/%m/%d'))) FechaUltimoReciente, ec.id_inventario_control IdInventario, ec.ubicacion DatosTopograficos, ec.descripcion DescripcionAsunto, ec.tipo_soporte_documental TipoSoporteDocumental
	                        ,cs.vig_doc_val_a VigDocValA, cs.vig_doc_val_l VigDocValL, cs.vig_doc_val_fc VigDocValFC, cs.vig_doc_pla_con_at VigDocPlaConAT, cs.vig_doc_pla_con_ac VigDocPlaConAC, cs.vig_doc_pla_con_tot VigDocPlaConTot, cs.tec_sel_e TecSelE, cs.tec_sel_c TecSelC, cs.tec_sel_m TecSelM
	                        ,cp.descripcion Puesto, ec.estatus Estatus, ca.descripcion Area
                            ,crt.cant_doc_ori DocOriginales, crt.cant_doc_copias DocCopias, crt.cant_cds Cds, crt.tec_sel_doc TecnicasSeleccion, crt.publica Publica, crt.confidencial Confidencial, crt.reservada_sol_info Reservada, date_format(crt.fecha_clasificacion,'%Y/%m/%d') FechaClasificacion, crt.periodo_reserva PeriodoReserva, crt.fundamento_legal FundamentoLegal, crt.ampliacion_periodo_reserva AmpliacionPeriodo, date_format(crt.fecha_desclasificacion,'%Y/%m/%d') FechaDesclasificacion, crt.nombre_desclasifica NombreDesclasifica, crt.cargo_desclasifica CargoDesclasifica, crt.partes_reservando PartesReservando
                        from prod_control_exp.expediente_control ec
                        join prod_control_exp.cat_serie_documental cs on ec.id_expediente = cs.id
                        join prod_control_exp.inventario_control itf on ec.id_inventario_control = itf.id
                        join prod_control_exp.cat_puestos cp on itf.id_puesto = cp.id
                        join prod_control_exp.cat_areas ca on cp.id_area = ca.id
                        join (select ROW_NUMBER() over(order by ets.fecha_ultimo, ets.id) NoProg, ROW_NUMBER() over(partition by year(ets.fecha_ultimo) order by ets.fecha_ultimo, ets.id) Consecutivo, ets.id from prod_control_exp.expediente_control ets where ets.id_inventario_control = (select id_inventario_control from prod_control_exp.expediente_control where id = @Id) AND ets.migrado_ne = 1 ORDER BY ets.fecha_ultimo, ets.id) cons on ec.id = cons.id
                        left join prod_control_exp.caratula crt on ec.id = crt.id_expediente_control and crt.legajo = @Legajo
                        where ec.id = @Id";

            return await db.QueryFirstOrDefaultAsync<Caratula>(sql, new { Id = id, Legajo = legajo });
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
            var sql = @"";
            if(area.Id == 0)
            {
                sql = @"
                        INSERT INTO prod_control_exp.cat_areas (descripcion) VALUES(@Descripcion);";
            }
            else
            {
                sql = @"
                        UPDATE prod_control_exp.cat_areas SET descripcion = @Descripcion WHERE id = @IdArea;";
            }

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
        public async Task<IEnumerable<Area>> GetAreaUser(int id)
        {
            var db = DbConnection();

            var sql = @"
                        select id Id, descripcion Descripcion, estatus
                        from prod_control_exp.cat_areas
                        where id = @Id";

            return await db.QueryAsync<Area>(sql, new { Id = id });
        }
        public async Task<bool> ActivarArea(int id)
        {
            var db = DbConnection();

            var sql = @"
                        UPDATE prod_control_exp.cat_areas
                        SET
                        estatus = 1
                        WHERE id = @IdArea;";

            var result = await db.ExecuteAsync(sql, new
            {
                IdArea = id
            });
            return result > 0;
        }
        public async Task<bool> DesactivarArea(int id)
        {
            var db = DbConnection();

            var sql = @"
                        UPDATE prod_control_exp.cat_areas
                        SET
                        estatus = 2
                        WHERE id = @IdArea;";

            var result = await db.ExecuteAsync(sql, new
            {
                IdArea = id
            });
            return result > 0;
        }
        public async Task<IEnumerable<Area>> GetPuestosLista()
        {
            var db = DbConnection();
            var sql = @"
                        select cp.id IdPuesto, cp.descripcion Puesto, cp.estatus, cp.id_area Id, ca.descripcion Descripcion
                        from prod_control_exp.cat_puestos cp
                        join prod_control_exp.cat_areas ca on cp.id_area = ca.id
                        order by cp.id;";
            return await db.QueryAsync<Area>(sql, new { });
        }
        public async Task<IEnumerable<Area>> GetPuestosListaValidacion()
        {
            var db = DbConnection();
            var sql = @"
                        select distinct cp.id IdPuesto, cp.descripcion Puesto, cp.estatus
                        from prod_control_exp.inventario_control ic
                        join prod_control_exp.expediente_control ec on ic.id = ec.id_inventario_control and ec.estatus = 2
                        join prod_control_exp.cat_puestos cp on ic.id_puesto = cp.id
                        order by cp.id;";
            return await db.QueryAsync<Area>(sql, new { });
        }
        public async Task<bool> UpdatePuesto(Area area)
        {
            var db = DbConnection();
            var sql = @"";
            if (area.Id == 0)
            {
                sql = @"
                        INSERT INTO prod_control_exp.cat_puestos (descripcion, id_area) VALUES(@Puesto, @IdArea);";
            }
            else
            {
                sql = @"
                        UPDATE prod_control_exp.cat_puestos SET descripcion = @Puesto, id_area = @IdArea WHERE id = @IdPuesto;";
            }

            var result = await db.ExecuteAsync(sql, new
            {
                Puesto = area.Puesto,
                IdPuesto = area.IdPuesto,
                IdArea = area.Id
            });
            return result > 0;
        }
        public async Task<int> GetIdUserPuesto(string puesto)
        {
            var db = DbConnection();
            var sql = @"
                        select id from prod_control_exp.cat_puestos where descripcion = @Puesto order by id;
                       ";
            return await db.QueryFirstOrDefaultAsync<int>(sql, new { Puesto = puesto });
        }
        public async Task<IEnumerable<Area>> GetPuestoUser(int id)
        {
            var db = DbConnection();

            var sql = @"
                        select id IdPuesto, descripcion Puesto, estatus
                        from prod_control_exp.cat_puestos
                        where id = @Id";

            return await db.QueryAsync<Area>(sql, new { Id = id });
        }
        public async Task<Area> GetPuesto(int id)
        {
            var db = DbConnection();

            var sql = @"
                        select id IdPuesto, descripcion Puesto, estatus, id_area Id
                        from prod_control_exp.cat_puestos
                        where id = @Id";

            return await db.QueryFirstOrDefaultAsync<Area>(sql, new { Id = id });
        }
        public async Task<bool> ActivarPuesto(int id)
        {
            var db = DbConnection();

            var sql = @"
                        UPDATE prod_control_exp.cat_puestos
                        SET
                        estatus = 1
                        WHERE id = @IdPuesto;";

            var result = await db.ExecuteAsync(sql, new
            {
                IdPuesto = id
            });
            return result > 0;
        }
        public async Task<bool> DesactivarPuesto(int id)
        {
            var db = DbConnection();

            var sql = @"
                        UPDATE prod_control_exp.cat_puestos
                        SET
                        estatus = 2
                        WHERE id = @IdPuesto;";

            var result = await db.ExecuteAsync(sql, new
            {
                IdPuesto = id
            });
            return result > 0;
        }
        public async Task<IEnumerable<User>> GetUsuariosLista()
        {
            var db = DbConnection();
            var sql = @"
                        select u.id Id, concat(u.nombre, ' ', u.primer_apellido, ' ', u.segundo_apellido) Name, u.usuario SUser, ca.descripcion Signer, u.cargo Position, u.numero_empleado EmployeeNumber, u.rfc RFC, u.activo Active
                        from qa_adms_conavi.usuario u
                        join qa_adms_conavi.c_area ca on u.id_area = ca.id
                        where u.id_rol in (15,16) and u.id <> 212
                        order by u.id;";
            return await db.QueryAsync<User>(sql, new { });
        }
        public async Task<bool> UpdateUsuario(User usuario)
        {
            var db = DbConnection();
            var sql = @"";
            if (usuario.Id == 0)
            {
                sql = @"
                        INSERT INTO qa_adms_conavi.usuario (nombre, primer_apellido, segundo_apellido, usuario, password, id_rol, cargo, numero_empleado, rfc, grado_academico, id_area, email, update_pass) VALUES(upper(@Nombre), upper(@PApellido), upper(@SApellido), @UserName, sha2(@Password,256), if(@Rol = 1, 15, 16), @Cargo, @NumEmpleado, upper(@RFC), upper(@GradoAcademico), @IdArea, lower(@Email), b'0');";
            }
            else
            {
                sql = @"
                        UPDATE qa_adms_conavi.usuario SET nombre = upper(@Nombre), primer_apellido = upper(@PApellido), segundo_apellido = upper(@SApellido), id_area = @IdArea, usuario = @UserName, cargo = @Cargo, numero_empleado = @NumEmpleado, rfc = upper(@RFC), grado_academico = upper(@GradoAcademico), email = lower(@Email), id_rol = if(@Rol = 1, 15, 16)";
                if (!String.IsNullOrEmpty(usuario.Password))
                {
                    sql += @", password = sha2(@Password,256)";
                }
                sql += @"
                        WHERE id = @IdUsuario;";
            }

            var result = await db.ExecuteAsync(sql, new
            {
                Nombre = usuario.Name,
                PApellido = usuario.LName,
                SApellido = usuario.SLName,
                IdArea = usuario.IdSystem,
                UserName = usuario.SUser,
                Password = usuario.Password,
                Cargo = usuario.Position,
                NumEmpleado = usuario.EmployeeNumber,
                RFC = usuario.RFC,
                GradoAcademico = usuario.Degree,
                Email = usuario.Email,
                Rol = usuario.Rol,
                IdUsuario = usuario.Id
            });
            return result > 0;
        }
        public async Task<User> GetUsuario(int id)
        {
            var db = DbConnection();

            var sql = @"
                        select id Id, nombre Name, primer_apellido LName, segundo_apellido SLName, usuario SUser, if(id_rol=15,1,2) Rol, cargo Position, id_area IdSystem, numero_empleado EmployeeNumber, rfc RFC, grado_academico Degree, email Email
                        from qa_adms_conavi.usuario
                        where id = @Id";

            return await db.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }
        public async Task<bool> ActivarUsuario(int id)
        {
            var db = DbConnection();

            var sql = @"
                        UPDATE qa_adms_conavi.usuario
                        SET
                        activo = 1
                        WHERE id = @IdUsuario;";

            var result = await db.ExecuteAsync(sql, new
            {
                IdUsuario = id
            });
            return result > 0;
        }
        public async Task<bool> DesactivarUsuario(int id)
        {
            var db = DbConnection();

            var sql = @"
                        UPDATE qa_adms_conavi.usuario
                        SET
                        activo = 2
                        WHERE id = @IdUsuario;";

            var result = await db.ExecuteAsync(sql, new
            {
                IdUsuario = id
            });
            return result > 0;
        }
    }
}
