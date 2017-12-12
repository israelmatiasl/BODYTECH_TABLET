using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BODYTECH.DL.DM;

namespace BODYTECH.BL.BC
{
    public class HelpersBC
    {
        private static BodytechEntities context = new BodytechEntities();

        public Boolean esCumpleanios(String documento)
        {
            Boolean cumpleanios = false;
            LISTA_BLANCA afiliado = context.LISTA_BLANCA.FirstOrDefault(x => x.PERSONA_DOCUMENTO == documento);
            
            if(afiliado!=null)
            {
                if (afiliado.PERSONA_CUMPLE.Month == DateTime.Today.Month && afiliado.PERSONA_CUMPLE.Day == DateTime.Today.Day) cumpleanios = true;
                else cumpleanios = false;
            }
            
            return cumpleanios;
        }

        public Boolean esTrabajador(String documento)
        {
            Boolean trabaja = false;
            LISTA_BLANCA afiliado = context.LISTA_BLANCA.FirstOrDefault(x => x.PERSONA_DOCUMENTO == documento);

            if (afiliado != null)
            {
                if (afiliado.PRODUCTO_TIPO == "EMPLEADO") trabaja = true;
                else trabaja = false;
            }

            return trabaja;
        }

        public Int32 getHistorialVisitaId(String documento)
        {
            Int32 historialid;
            VISITAS visita = context.VISITAS.FirstOrDefault(x => x.PERSONA_DOCUMENTO == documento);
            TABLA_CLIENTE cliente = context.TABLA_CLIENTE.FirstOrDefault(x => x.ID_VISITA == visita.ID_VISITA);
            historialid = context.HISTORIAL_ACTIVIDAD.OrderByDescending(x => x.ID_ACTIVIDAD).FirstOrDefault(x => x.ID_CLIENTE == cliente.ID_CLIENTE).ID_ACTIVIDAD;

            return historialid;
        }

    }
}
