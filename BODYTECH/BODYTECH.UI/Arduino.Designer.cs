namespace BODYTECH.UI
{
    partial class Arduino
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.spArduino = new System.IO.Ports.SerialPort(this.components);
            this.SuspendLayout();
            // 
            // spArduino
            // 
            this.spArduino.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.spArduino_DataReceived);
            // 
            // Arduino
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "Arduino";
            this.ResumeLayout(false);

        }

        #endregion

        public System.IO.Ports.SerialPort spArduino;
    }
}
