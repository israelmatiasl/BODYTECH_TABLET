using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using BODYTECH.BL.BC;
using BODYTECH.DL.DM;
using System.Windows.Threading;

using MahApps.Metro.Controls;

namespace BODYTECH.UI
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        BioPlugIn bioplugin = new BioPlugIn();
        Arduino arduino = new Arduino("COM14");

        UsuarioBC usuarioBC = new UsuarioBC();
        HistorialActividadBC historialBC = new HistorialActividadBC();
        HelpersBC helperBC = new HelpersBC();

        public Int32 _historialid { get; set; }

        public enum TypeModo { ingresoMiembro, ingresoVisita }
        public TypeModo opcionEntrar { get; set; }

        public enum MensajeAlerta { NoIdentificadoAfiliado, AfiliacionVencida, ActividadInusual, FaltaData, NoIdentificadoVisita, InvitacionVencida }
        public MensajeAlerta mensajeSalida { get; set; }

        DispatcherTimer _actividadInvitado = new DispatcherTimer();
        DispatcherTimer _actividadAfiliado = new DispatcherTimer();

        DispatcherTimer _hidemensajeAlerta = new DispatcherTimer();
        DispatcherTimer _regresarInicio = new DispatcherTimer();
        DispatcherTimer _pasarOpcionesEmpleado = new DispatcherTimer();

        public Boolean tabletaEntrada { get; set; } = true; // CAMBIAR ESTO PARA 
        public Int32 tabletaid { get; set; } = 1;


        public MainWindow()
        {
            InitializeComponent();
            TitlebarHeight = 0;
        }

        private void Principal_Loaded(object sender, RoutedEventArgs e)
        {
            String ipServidor = "192.168.1.3";

            bioplugin.tabletaEntrada = tabletaEntrada;
            bioplugin.axBioPlugInActX.SetServerInfo(ipServidor, 1200);

            _actividadInvitado.Interval = new TimeSpan(0, 0, 7);
            _actividadInvitado.Tick += (s, a) =>
            {
                tabGENERAL.SelectedIndex = 0;
                txtInvitadosDNI.Clear();
                _actividadInvitado.Stop();
            };
            

            _actividadAfiliado.Interval = new TimeSpan(0, 0, 7);
            _actividadAfiliado.Tick += (s, a) =>
            {
                tabGENERAL.SelectedIndex = 0;
                _actividadAfiliado.Stop();
            };

            _regresarInicio.Interval = new TimeSpan(0, 0, 5);
            _regresarInicio.Tick += (s, a) =>
            {
                tabGENERAL.SelectedIndex = 0;
                _regresarInicio.Stop();
            };

            _hidemensajeAlerta.Interval = new TimeSpan(0, 0, 2);
            _hidemensajeAlerta.Tick += (s, a) =>
            {
                ocultarAlerta(opcionEntrar, mensajeSalida);
                _hidemensajeAlerta.Stop();
            };

            _pasarOpcionesEmpleado.Interval = new TimeSpan(0, 0, 3);
            _pasarOpcionesEmpleado.Tick += (s, a) =>
            {
                pantallOpciones();
                _pasarOpcionesEmpleado.Stop();
            };

            arduino.spArduino.DataReceived += delegate
            {
                if(arduino.Rx == "OK")
                {
                    Dispatcher.Invoke(() =>
                    {
                        //CODIGO JUNTO CON EL ARDUINO CUANDO EL MOLINETE GIRA

                        completarActividad(_historialid);
                    });
                }
                else if(arduino.Rx == "YA")
                {
                    Dispatcher.Invoke(() =>
                    {
                        //CODIGO JUNTO CON EL ARDUINO CUANDO EL MOLINETE NO HA GIRADO


                    });
                }
            };

            bioplugin.axBioPlugInActX.OnIdentify += delegate
            {
                opcionEntrar = TypeModo.ingresoMiembro;
                switch (bioplugin.mensajeSalida)
                {
                    case BioPlugIn.MensajeSalida.NoIdentificado:
                    {
                            mensajeSalida = MensajeAlerta.NoIdentificadoAfiliado;
                            mostrarAlerta(opcionEntrar, mensajeSalida);
                    }break;
                    case BioPlugIn.MensajeSalida.AfiliacionVencida:
                    {
                            mensajeSalida = MensajeAlerta.AfiliacionVencida;
                            mostrarAlerta(opcionEntrar, mensajeSalida);
                    }break;
                    case BioPlugIn.MensajeSalida.CumpleañosTrabajador:
                    {
                            pantallaCumpleañosTrabajador();
                    }break;
                    case BioPlugIn.MensajeSalida.EntradaTrabajador:
                    {
                            pantallOpciones();
                    }break;
                    case BioPlugIn.MensajeSalida.CumpleañosAfiliado:
                    {
                            pantallaCumpleañosAfiliado(bioplugin.afiliado);
                    }break;
                    case BioPlugIn.MensajeSalida.EntradaAfiliado:
                    {
                            pantallaBienvenido(opcionEntrar, bioplugin.afiliado);
                    }
                    break;
                    case BioPlugIn.MensajeSalida.SalidaCorrecta:
                    {
                            pantallaBienvenido(opcionEntrar, bioplugin.afiliado);
                    }break;
                    case BioPlugIn.MensajeSalida.ActividadInusual:
                    {
                            mensajeSalida = MensajeAlerta.ActividadInusual;
                            mostrarAlerta(opcionEntrar, mensajeSalida);
                    }break;
                }
                bioplugin.documento = null;
            };
        }


        #region PANTALLA_PRINCIPAL

        private void btnAFILIADO_Click(object sender, RoutedEventArgs e)
        {
            tabGENERAL.SelectedIndex = 1; // EMPIEZA A ESCANEAR
            bioplugin.StartScan(sender, e);
            _actividadAfiliado.Start();
        }

        private void btnVISITA_Click(object sender, RoutedEventArgs e)
        {
            tabGENERAL.SelectedIndex = 2; // LISTO PARA EL INGRESO DEL DNI
            _actividadInvitado.Start();
        }


        #endregion

        #region HISTORIAL_ACTIVIDAD

        public void completarActividad(Int32 historialid)
        {
            historialBC.completarHistorial(historialid, tabletaid);
        }
        #endregion

        #region PANTALLAS

        public void pantallaPrincipal()
        {
            tabGENERAL.SelectedIndex = 0;
            ConnecctionServer.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gray);
        }

        public void pantallaCumpleañosTrabajador()
        {
            Dispatcher.Invoke(() => { _actividadAfiliado.Stop(); });
            _pasarOpcionesEmpleado.Start();
            tabGENERAL.SelectedIndex = 3;
        }

        public void pantallaCumpleañosAfiliado(LISTA_BLANCA afiliado)
        {
            Dispatcher.Invoke(() => { _actividadAfiliado.Stop(); });
            _historialid = bioplugin.historialid;

            String nombres = afiliado.PERSONA_NOMBRE;
            txtnombreCumpleaniero.Text = nombres.Split(' ')[0];
            tabGENERAL.SelectedIndex = 7;
            Dispatcher.Invoke(() => { _regresarInicio.Start(); });

            //COMPLETA HISTORIAL // MOMENTANEO
            completarActividad(_historialid);

            arduino.spArduino.Write("1");
        }

        public void pantallOpciones()
        {
            Dispatcher.Invoke(() => { _actividadAfiliado.Stop(); });
            tabGENERAL.SelectedIndex = 4;
        }

        public void pantallaBienvenido(TypeModo typeModo, Object usuario, String tituloPantalla=null)
        {
            if(typeModo == TypeModo.ingresoMiembro)
            {
                LISTA_BLANCA afiliado = usuario as LISTA_BLANCA;

                _historialid = bioplugin.historialid;

                BienvenidoTxt.Text = bioplugin.tituloPantalla;
                nombreComple.Text = afiliado.PERSONA_NOMBRE;
                PeriodoFecha.Text = afiliado.PRODUCTO_FECHA_INICIA.ToShortDateString() + " - " + afiliado.PRODUCTO_FECHA_VENCE.ToShortDateString();
                tipoMembresia.Text = afiliado.PRODUCTO_TIPO;
                tipoMembresia.Visibility = Visibility.Visible;
                txtMembresia.Visibility = Visibility.Visible;
                Dispatcher.Invoke(() => { _actividadAfiliado.Stop(); });
            }
            else
            {
                VISITAS visita = usuario as VISITAS;

                _historialid = helperBC.getHistorialVisitaId(visita.PERSONA_DOCUMENTO);

                BienvenidoTxt.Text = tituloPantalla;
                nombreComple.Text = visita.PERSONA_NOMBRE;
                PeriodoFecha.Text = visita.PRODUCTO_FECHA_INICIO.ToShortDateString() + " - " + visita.PRODUCTO_FECHA_VENCIMIENTO.ToShortDateString();
                tipoMembresia.Visibility = Visibility.Hidden;
                txtMembresia.Visibility = Visibility.Hidden;
                txtInvitadosDNI.Clear();
                Dispatcher.Invoke(() => { _actividadInvitado.Stop(); });
            }

            tabGENERAL.SelectedIndex = 5;
            // TIEMPO PARA REGRESAR AL INICIO
            Dispatcher.Invoke(() => { _regresarInicio.Start(); });



            //COMPLETA HISTORIAL // MOMENTANEO
            completarActividad(_historialid);

            arduino.spArduino.Write("1");
        }


        public void mostrarAlerta(TypeModo usuario, MensajeAlerta mensaje)
        {
            if (usuario == TypeModo.ingresoMiembro) Dispatcher.Invoke(() => { _actividadAfiliado.Stop(); });
            else if (usuario == TypeModo.ingresoVisita) Dispatcher.Invoke(() => { _actividadInvitado.Stop(); });

            switch (mensaje)
            {
                case MensajeAlerta.NoIdentificadoAfiliado: ConnecctionServer.Fill = new SolidColorBrush(Colors.Red); break;
                case MensajeAlerta.AfiliacionVencida: pnlAfiliacionVencida.Visibility = Visibility.Visible; panelOpaco.Visibility = Visibility.Visible; break;
                case MensajeAlerta.FaltaData: pnlfaltaData.Visibility = Visibility.Visible; panelOpaco.Visibility = Visibility.Visible; break;
                case MensajeAlerta.NoIdentificadoVisita: pnlnoIdentificado.Visibility = Visibility.Visible; panelOpaco.Visibility = Visibility.Visible; break;
                case MensajeAlerta.ActividadInusual: pnlactividadInusual.Visibility = Visibility.Visible; panelOpaco.Visibility = Visibility.Visible; break;
                case MensajeAlerta.InvitacionVencida: pnlinvitacionVencida.Visibility = Visibility.Visible; panelOpaco.Visibility = Visibility.Visible; break;
            }
            _hidemensajeAlerta.Start();
        }

        public void ocultarAlerta(TypeModo usuario, MensajeAlerta mensaje)
        {
            panelOpaco.Visibility = Visibility.Hidden;
            switch (mensaje)
            {
                case MensajeAlerta.NoIdentificadoAfiliado: ConnecctionServer.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gray); break;
                case MensajeAlerta.AfiliacionVencida: pnlAfiliacionVencida.Visibility = Visibility.Hidden; break;
                case MensajeAlerta.FaltaData: pnlfaltaData.Visibility = Visibility.Hidden; break;
                case MensajeAlerta.NoIdentificadoVisita: pnlnoIdentificado.Visibility = Visibility.Hidden; break;
                case MensajeAlerta.ActividadInusual: pnlactividadInusual.Visibility = Visibility.Hidden; break;
                case MensajeAlerta.InvitacionVencida: pnlinvitacionVencida.Visibility = Visibility.Hidden; break;
            }
            if (usuario == TypeModo.ingresoMiembro) Dispatcher.Invoke(() => { _actividadAfiliado.Start(); });
            else if (usuario == TypeModo.ingresoVisita) Dispatcher.Invoke(() => { _actividadInvitado.Start(); });
        }

        #endregion

        #region BOTONES_FUNCIONALES

        private void regresarPrincipalAfiliado_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.Invoke(() => { _actividadAfiliado.Stop(); });
            tabGENERAL.SelectedIndex = 0;
        }

        private void regresarPrincipalVisita_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.Invoke(() => { _actividadInvitado.Stop(); });
            txtInvitadosDNI.Clear();
            tabGENERAL.SelectedIndex = 0;
        }

        #endregion

        #region TECLADO_NUMERICO


        private void btnnum1I_Click(object sender, RoutedEventArgs e)
        {
            txtInvitadosDNI.Text += "1";
            Dispatcher.Invoke(() => { _actividadInvitado.Stop(); });
            Dispatcher.Invoke(() => { _actividadInvitado.Start(); });
        }

        private void btnnum2I_Click(object sender, RoutedEventArgs e)
        {
            txtInvitadosDNI.Text += "2";
            Dispatcher.Invoke(() => { _actividadInvitado.Stop(); });
            Dispatcher.Invoke(() => { _actividadInvitado.Start(); });
        }

        private void btnnum3I_Click(object sender, RoutedEventArgs e)
        {
            txtInvitadosDNI.Text += "3";
            Dispatcher.Invoke(() => { _actividadInvitado.Stop(); });
            Dispatcher.Invoke(() => { _actividadInvitado.Start(); });
        }

        private void btnnum4I_Click(object sender, RoutedEventArgs e)
        {
            txtInvitadosDNI.Text += "4";
            Dispatcher.Invoke(() => { _actividadInvitado.Stop(); });
            Dispatcher.Invoke(() => { _actividadInvitado.Start(); });
        }

        private void btnnum5I_Click(object sender, RoutedEventArgs e)
        {
            txtInvitadosDNI.Text += "5";
            Dispatcher.Invoke(() => { _actividadInvitado.Stop(); });
            Dispatcher.Invoke(() => { _actividadInvitado.Start(); });
        }

        private void btnnum6I_Click(object sender, RoutedEventArgs e)
        {
            txtInvitadosDNI.Text += "6";
            Dispatcher.Invoke(() => { _actividadInvitado.Stop(); });
            Dispatcher.Invoke(() => { _actividadInvitado.Start(); });
        }

        private void btnnum7I_Click(object sender, RoutedEventArgs e)
        {
            txtInvitadosDNI.Text += "7";
            Dispatcher.Invoke(() => { _actividadInvitado.Stop(); });
            Dispatcher.Invoke(() => { _actividadInvitado.Start(); });
        }

        private void btnnum8I_Click(object sender, RoutedEventArgs e)
        {
            txtInvitadosDNI.Text += "8";
            Dispatcher.Invoke(() => { _actividadInvitado.Stop(); });
            Dispatcher.Invoke(() => { _actividadInvitado.Start(); });
        }

        private void btnnum9I_Click(object sender, RoutedEventArgs e)
        {
            txtInvitadosDNI.Text += "9";
            Dispatcher.Invoke(() => { _actividadInvitado.Stop(); });
            Dispatcher.Invoke(() => { _actividadInvitado.Start(); });
        }

        private void btnnum0In_Click(object sender, RoutedEventArgs e)
        {
            txtInvitadosDNI.Text += "0";
            Dispatcher.Invoke(() => { _actividadInvitado.Stop(); });
            Dispatcher.Invoke(() => { _actividadInvitado.Start(); });
        }


        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (txtInvitadosDNI.Text.Length > 0)
            {
                txtInvitadosDNI.Text = txtInvitadosDNI.Text.Remove(txtInvitadosDNI.Text.Length - 1);
            }
            Dispatcher.Invoke(() => { _actividadInvitado.Stop(); });
            Dispatcher.Invoke(() => { _actividadInvitado.Start(); });
        }

        private void btnCancelar_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.Invoke(() => { _actividadInvitado.Stop(); });
            Dispatcher.Invoke(() => { _actividadInvitado.Start(); });
            txtInvitadosDNI.Clear();
        }

        #endregion

        #region INGRESO_VISITA
        
        private void btnIngresarVisita_Click(object sender, RoutedEventArgs e)
        {
            opcionEntrar = TypeModo.ingresoVisita;
            if (txtInvitadosDNI.Text == "")
            {
                mensajeSalida = MensajeAlerta.FaltaData;
                mostrarAlerta(opcionEntrar, mensajeSalida);
            }
            else
            {
                VISITAS visita = usuarioBC.getVisita(txtInvitadosDNI.Text);

                if(visita==null)
                {
                    mensajeSalida = MensajeAlerta.NoIdentificadoVisita;
                    mostrarAlerta(opcionEntrar, mensajeSalida);
                }
                else
                {
                    var diasRestantes = DateTime.Today - visita.PRODUCTO_FECHA_VENCIMIENTO;
                    if (diasRestantes.Days > 0)
                    {
                        mensajeSalida = MensajeAlerta.InvitacionVencida;
                        mostrarAlerta(opcionEntrar, mensajeSalida);
                    }
                    else
                    {
                        Int32 clienteid = visita.TABLA_CLIENTE.Select(x => x.ID_CLIENTE).FirstOrDefault();
                        HISTORIAL_ACTIVIDAD historial = historialBC.getHistorialCliente(clienteid);
                        if (historial == null || (historial.HORA_ENTRADA != null && historial.HORA_SALIDA != null))
                        {
                            historialBC.nuevoHistorial(clienteid, 1);
                            historial = historialBC.getHistorialCliente(clienteid);
                        }

                        if(historial.HORA_ENTRADA == null && historial.HORA_SALIDA == null) //ESTOY AFUERA Y QUIERO ENTRAR
                        {
                            if(tabletaEntrada) //TABLETA ENTRADA // ENTRA
                            {
                                String titulo = "BIENVENIDO";
                                pantallaBienvenido(opcionEntrar, visita, titulo);
                            }
                            else //TABLETA SALIDA // NO PUEDE ENTRAR
                            {
                                mensajeSalida = MensajeAlerta.ActividadInusual;
                                mostrarAlerta(opcionEntrar, mensajeSalida);
                            }
                        }
                        else //ESTOY DENTRO Y QUIERO ENTRAR
                        {
                            if (!tabletaEntrada) //TABLETA SALIDA // PUEDE SALIR
                            {
                                String titulo = "HASTA PRONTO";
                                pantallaBienvenido(opcionEntrar, visita, titulo);
                            }
                            else //TABLETA ENTRADA // NO PUEDE SALIR
                            {
                                mensajeSalida = MensajeAlerta.ActividadInusual;
                                mostrarAlerta(opcionEntrar, mensajeSalida);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region OPCION_TRABAJADOR

        private void btnIngresarEntrenar_Click(object sender, RoutedEventArgs e)
        {
            pantallaBienvenido(TypeModo.ingresoMiembro, bioplugin.afiliado);
        }

        private void btnIngresarTrabajar_Click(object sender, RoutedEventArgs e)
        {
            _historialid = bioplugin.historialid;
            historialBC.entraTrabajar(_historialid);

            pantallaBienvenido(TypeModo.ingresoMiembro, bioplugin.afiliado);
        }

        #endregion

        #region OPCION_ADMINISTRADOR



        #endregion

        private void Principal_Closed(object sender, EventArgs e)
        {
            bioplugin.StopScan();
            bioplugin.Dispose();
            arduino.Dispose();
            arduino.spArduino.Close();
        }
        
    }
}
