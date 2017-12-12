using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BODYTECH.DL.DM;

namespace BODYTECH.BL.BC
{
    public class HistorialActividadBC
    {
        private static BodytechEntities context = new BodytechEntities();
        
        public HISTORIAL_ACTIVIDAD getHistorialCliente(Int32 clienteid)
        {
            HISTORIAL_ACTIVIDAD historial = new HISTORIAL_ACTIVIDAD();
            return context.HISTORIAL_ACTIVIDAD.Where(x => x.ID_CLIENTE == clienteid).OrderByDescending(x => x.ID_ACTIVIDAD).FirstOrDefault();
        }


        public void nuevoHistorial(Int32 clienteid, Int32 sedeid)
        {
            HISTORIAL_ACTIVIDAD historial = new HISTORIAL_ACTIVIDAD();

            historial.ID_CLIENTE = clienteid;
            historial.ID_SEDE = sedeid;
            historial.ACTIVIDAD_REALIZADA = "E";

            context.HISTORIAL_ACTIVIDAD.Add(historial);
            context.SaveChanges();
        }


        public void completarHistorial(Int32 historialid, Int32 tabletaid)
        {
            HISTORIAL_ACTIVIDAD historial = context.HISTORIAL_ACTIVIDAD.FirstOrDefault(x => x.ID_ACTIVIDAD == historialid);

            if(historial.HORA_ENTRADA == null && historial.HORA_SALIDA == null)
            {
                historial.HORA_ENTRADA = DateTime.Now;
                historial.ID_TABLETA_ENTRADA = tabletaid;
                context.SaveChanges();
            }
            else
            {
                historial.HORA_SALIDA = DateTime.Now;
                historial.ID_TABLETA_SALIDA = tabletaid;
                context.SaveChanges();
            }
        }

        
        public void entraTrabajar(Int32 historialid)
        {
            HISTORIAL_ACTIVIDAD historial = context.HISTORIAL_ACTIVIDAD.FirstOrDefault(x => x.ID_ACTIVIDAD == historialid);

            if(historial!=null)
            {
                historial.ACTIVIDAD_REALIZADA = "T";
                context.SaveChanges();
            }
        }
        
    }
}
