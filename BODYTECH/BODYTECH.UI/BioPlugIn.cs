using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BODYTECH.DL.DM;
using BODYTECH.BL.BC;
using System.IO;

namespace BODYTECH.UI
{
    public partial class BioPlugIn : UserControl
    {
        UsuarioBC afiliadoBC = new UsuarioBC();
        HistorialActividadBC historialBC = new HistorialActividadBC();
        HelpersBC helperBC = new HelpersBC();

        public LISTA_BLANCA afiliado { get; set; }
        public String documento { get; set; }
        public Int32 tabletaid { get; set; }
        public Boolean tabletaEntrada { get; set; } /// 1-ENTRADA / 0-SALIDA
        public Int32 sedeid { get; set; }
        public Int32 historialid { get; set; }

        public enum MensajeSalida { NoIdentificado, AfiliacionVencida, CumpleañosTrabajador, EntradaTrabajador, CumpleañosAfiliado, EntradaAfiliado, SalidaCorrecta, ActividadInusual }
        public MensajeSalida mensajeSalida { get; set; }
        public String tituloPantalla { get; set; }


        private string path_FileDebug = "Debug.txt";
        public bool Flag = true;

        public BioPlugIn()
        {
            InitializeComponent();
        }

        public void DoActionRegister(Action action)
        {
            action();
        }

        public void DoActionRegister(string action)
        {
            StreamWriter FileDebug = new StreamWriter(path_FileDebug, true);
            FileDebug.WriteLine(action);
            FileDebug.Close();
        }

        private void axBioPlugInActX_OnIdentify(object sender, EventArgs e)
        {
            documento = axBioPlugInActX.result;
            afiliado = afiliadoBC.getAfiliado(documento);

            if(afiliado == null)
            {
                mensajeSalida = MensajeSalida.NoIdentificado;
                return;
            }
            else
            {
                var diasRestantes = DateTime.Today - afiliado.PRODUCTO_FECHA_VENCE;
                
                if(diasRestantes.Days > 0)
                {
                    mensajeSalida = MensajeSalida.AfiliacionVencida;
                    return;
                }

                Int32 clienteid = afiliado.TABLA_CLIENTE.Select(x => x.ID_CLIENTE).FirstOrDefault();
                HISTORIAL_ACTIVIDAD historial = historialBC.getHistorialCliente(clienteid);

                Boolean cumpleaños = helperBC.esCumpleanios(documento);
                Boolean trabaja = helperBC.esTrabajador(documento);

                if (historial == null || (historial.HORA_ENTRADA != null && historial.HORA_SALIDA!=null))
                {
                    historialBC.nuevoHistorial(clienteid, 1);
                    historial = historialBC.getHistorialCliente(clienteid);
                }

                historialid = historial.ID_ACTIVIDAD;
                
                if(historial.HORA_ENTRADA == null && historial.HORA_SALIDA == null) // ESTOY FUERA Y QUIERO ENTRAR
                {
                    if(tabletaEntrada) //TABLETA ENTRADA // PUEDE INGRESAR
                    {
                        if(trabaja) //TRABAJA
                        {
                            if (cumpleaños) mensajeSalida = MensajeSalida.CumpleañosTrabajador;
                            else mensajeSalida = MensajeSalida.EntradaTrabajador;
                        }
                        else // SOLO AFILIADO
                        {
                            if (cumpleaños) mensajeSalida = MensajeSalida.CumpleañosAfiliado;
                            else mensajeSalida = MensajeSalida.EntradaAfiliado;
                        }
                        tituloPantalla = "BIENVENIDO";
                    }
                    else  //TABLETA SALIDA // NO PUEDE INGRESAR
                    {
                        mensajeSalida = MensajeSalida.ActividadInusual;
                    }
                }
                else // ESTA DENTRO Y QUIERE SALIR
                {
                    
                    if (!tabletaEntrada) //TABLETA SALIDA // PUEDE SALIR
                    {
                        mensajeSalida = MensajeSalida.SalidaCorrecta;
                        tituloPantalla = "HASTA PRONTO";
                    }
                    else //TABLETA ENTRADA // NO PUEDE SALIR
                    {
                        mensajeSalida = MensajeSalida.ActividadInusual;
                    }
                }
                
            }
        }

        public void StartScan(object sender, EventArgs e)
        {
            DoActionRegister(() => axBioPlugInActX.StartActiveIdentification("5"));
        }

        public void StopScan()
        {
            DoActionRegister(() => axBioPlugInActX.StopActiveIdentification());
        }
    }
}
