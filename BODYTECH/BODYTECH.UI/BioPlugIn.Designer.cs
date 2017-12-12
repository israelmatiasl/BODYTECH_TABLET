namespace BODYTECH.UI
{
    partial class BioPlugIn
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BioPlugIn));
            this.axBioPlugInActX = new AxBIOPLUGINACTXLib.AxBioPlugInActX();
            ((System.ComponentModel.ISupportInitialize)(this.axBioPlugInActX)).BeginInit();
            this.SuspendLayout();
            // 
            // axBioPlugInActX
            // 
            this.axBioPlugInActX.Enabled = true;
            this.axBioPlugInActX.Location = new System.Drawing.Point(3, 3);
            this.axBioPlugInActX.Name = "axBioPlugInActX";
            this.axBioPlugInActX.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axBioPlugInActX.OcxState")));
            this.axBioPlugInActX.Size = new System.Drawing.Size(144, 144);
            this.axBioPlugInActX.TabIndex = 0;
            this.axBioPlugInActX.OnIdentify += new System.EventHandler(this.axBioPlugInActX_OnIdentify);
            // 
            // BioPlugIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axBioPlugInActX);
            this.Name = "BioPlugIn";
            ((System.ComponentModel.ISupportInitialize)(this.axBioPlugInActX)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public AxBIOPLUGINACTXLib.AxBioPlugInActX axBioPlugInActX;
    }
}
