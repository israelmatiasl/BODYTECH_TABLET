using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BODYTECH.DL.DM;

namespace BODYTECH.BL.BC
{
    public class UsuarioBC
    {
        private static BodytechEntities context = new BodytechEntities();
        

        public LISTA_BLANCA getAfiliado(String documento)
        {
            return context.LISTA_BLANCA.FirstOrDefault(x => x.PERSONA_DOCUMENTO == documento);
        }

        public VISITAS getVisita(String documento)
        {
            return context.VISITAS.FirstOrDefault(x => x.PERSONA_DOCUMENTO == documento);
        }
    }
}
