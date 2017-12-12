using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BODYTECH.UI
{
    public partial class Arduino : UserControl
    {
        public string Rx { get; set; }
        public Arduino(String port)
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false; //Soluciona Error
            spArduino.PortName = port; //verificar puerto COM
            spArduino.BaudRate = 9600;
            spArduino.Open(); //Abre Comunicacion
        }

        public void spArduino_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string dataRx = spArduino.ReadLine(); //Leer valor

            if (dataRx.Trim() == "Client passed")
            {
                Rx = "OK";
            }
            else if (dataRx.Trim() == "Leinad")
            {
                Rx = "YA";
            }
            else
            {
                Rx = "En Espera";
            }
        }
    }
}
