using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConaviWeb.Controllers.Reporteador
{
    partial class Citas
    {
        public string Fecha { get; set; }
        public string HoraIn { get; set; }
        public string HoraFin { get; set; }
    }
    public class AsignaCitasController : Controller
    {
        public IActionResult Index()
        {
            CitasBB();
            return Ok();
        }

        public void CitasBB()
        {
            var cantCitas = 180f;
            var cantSucursal = 50f;
            var cantDays = Math.Ceiling(cantCitas / cantSucursal);
            string fechaIn = "29/07/2022";
            string[] horarios = { "09:00", "09:15", "09:30", "09:45", "10:00", "10:15", "10:30", "10:45", "11:00", "11:15", "11:30", "11:45", "12:00", "12:15", "12:30", "12:45", "13:00", "13:15", "13:30", "13:45", "14:00", "14:15", "14:30", "14:45", "15:00", "15:15", "15:30", "15:45", "16:00" };
            var fechas = new List<string>();
            var workDay = fechaIn.Split("/");
            var date = new DateTime(Convert.ToInt32(workDay[2]), Convert.ToInt32(workDay[1]), Convert.ToInt32(workDay[0]));
            var j = 0;
            for (int i = 0; i < cantDays; i++)
            {
                DateTime newDate = date.AddDays(j);
                if (newDate.DayOfWeek != DayOfWeek.Saturday && newDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    fechas.Add(newDate.ToString());
                    j ++;
                }
                else
                {
                    i -= 1;
                    j++;
                }
                
            }
            var asignacion = new List<Citas>();
            for (int i = 0; i < cantDays; i++)
            {
                
                    for (int l = 0; l < cantSucursal; l++)
                    {
                        var citas = new Citas();
                        citas.Fecha = fechas[i];
                        citas.HoraIn = horarios[l];
                        if (l < horarios.Length -1)
                        {
                            citas.HoraFin = horarios[l + 1];
                        }
                        else
                        {
                            citas.HoraFin = horarios[l];
                        }
                        asignacion.Add(citas);
                    }
                    
            }
            
        }
    }
}
