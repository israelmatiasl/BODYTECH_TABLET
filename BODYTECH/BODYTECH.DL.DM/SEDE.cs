//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BODYTECH.DL.DM
{
    using System;
    using System.Collections.Generic;
    
    public partial class SEDE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SEDE()
        {
            this.HISTORIAL_ACTIVIDAD = new HashSet<HISTORIAL_ACTIVIDAD>();
        }
    
        public int ID_SEDE { get; set; }
        public string SEDE_NOMBRE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HISTORIAL_ACTIVIDAD> HISTORIAL_ACTIVIDAD { get; set; }
    }
}
