using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.RH
{
    public class Viaticos
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Puesto { get; set; }
        public string Area_adscripcion { get; set; }
        public string Descripcion_comision { get; set; }
        public string Objetivo { get; set; }
        public string Observaciones { get; set; }
        public string Lugares_asignados_comision { get; set; }
        public string Clave_estado { get; set; }
        public string Municipio { get; set; }
        public string Medio_transporte { get; set; }
        public string Periodo_comision_i { get; set; }
        public string Periodo_comision_f { get; set; }
        public string Dias_duracion { get; set; }
        public string Cuota_diaria { get; set; }
        public string Importe_viaticos { get; set; }
        public string Num_casetas { get; set; }
        //public string Dotacion_combustible { get; set; }
        public string Importe_gastos { get; set; }
        public string Total_peajes { get; set; }
        public string Destino_1 { get; set; }
        public string Fecha_1 { get; set; }
        public string Horario_1 { get; set; }
        public string Destino_2 { get; set; }
        public string Fecha_2 { get; set; }
        public string Horario_2 { get; set; }
        public string Destino_3 { get; set; }
        public string Fecha_3 { get; set; }
        public string Horario_3 { get; set; }
        public string Destino_4 { get; set; }
        public string Fecha_4 { get; set; }
        public string Horario_4 { get; set; }
        public string Linea_aerea { get; set; }
        public string Ruta_i { get; set; }
        public string Vuelo_i { get; set; }
        public string Sale_i { get; set; }
        public string Llega_i { get; set; }
        public string Ruta_f { get; set; }
        public string Vuelo_f { get; set; }
        public string Sale_f { get; set; }
        public string Llega_f { get; set; }
        public string Ruta_j { get; set; }
        public string Vuelo_j { get; set; }
        public string Sale_j { get; set; }
        public string Llega_j { get; set; }
        public string Ruta_k { get; set; }
        public string Vuelo_k { get; set; }
        public string Sale_k { get; set; }
        public string Llega_k { get; set; }
        public string FechaSol { get; set; }
        public string Folio { get; set; }
        public string Estatus { get; set; }
        public string Archivo_firma { get; set; }
        public string Archivo_pago { get; set; }
        public string Obs_aereo { get; set; }
        public string RFC { get; set; }
        public string CvNivel { get; set; }
        public string TotalViaticos { get; set; }
        public string Traza_ruta { get; set; }
        public string ObsCan { get; set; }
    }
}
