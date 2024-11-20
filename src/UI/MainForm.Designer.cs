namespace GestionDeProcesosRecursos
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridViewProcesses;
        private System.Windows.Forms.Button btnCreateProcess;
        private System.Windows.Forms.Button btnTerminateProcess;
        private System.Windows.Forms.TextBox txtPriority;
        private System.Windows.Forms.Label lblPriority;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">Verdadero si los recursos administrados deben ser eliminados; de lo contrario, falso.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridViewProcesses = new System.Windows.Forms.DataGridView();
            this.btnCreateProcess = new System.Windows.Forms.Button();
            this.btnTerminateProcess = new System.Windows.Forms.Button();
            this.txtPriority = new System.Windows.Forms.TextBox();
            this.lblPriority = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProcesses)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewProcesses
            // 
            this.dataGridViewProcesses.AllowUserToAddRows = false;
            this.dataGridViewProcesses.AllowUserToDeleteRows = false;
            this.dataGridViewProcesses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProcesses.Location = new System.Drawing.Point(12, 12);
            this.dataGridViewProcesses.Name = "dataGridViewProcesses";
            this.dataGridViewProcesses.ReadOnly = true;
            this.dataGridViewProcesses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewProcesses.Size = new System.Drawing.Size(560, 300);
            this.dataGridViewProcesses.TabIndex = 0;
            // 
            // btnCreateProcess
            // 
            this.btnCreateProcess.Location = new System.Drawing.Point(12, 328);
            this.btnCreateProcess.Name = "btnCreateProcess";
            this.btnCreateProcess.Size = new System.Drawing.Size(120, 30);
            this.btnCreateProcess.TabIndex = 1;
            this.btnCreateProcess.Text = "Crear Proceso";
            this.btnCreateProcess.UseVisualStyleBackColor = true;
            this.btnCreateProcess.Click += new System.EventHandler(this.btnCreateProcess_Click);
            // 
            // btnTerminateProcess
            // 
            this.btnTerminateProcess.Location = new System.Drawing.Point(452, 328);
            this.btnTerminateProcess.Name = "btnTerminateProcess";
            this.btnTerminateProcess.Size = new System.Drawing.Size(120, 30);
            this.btnTerminateProcess.TabIndex = 2;
            this.btnTerminateProcess.Text = "Terminar Proceso";
            this.btnTerminateProcess.UseVisualStyleBackColor = true;
            this.btnTerminateProcess.Click += new System.EventHandler(this.btnTerminateProcess_Click);
            // 
            // txtPriority
            // 
            this.txtPriority.Location = new System.Drawing.Point(190, 334);
            this.txtPriority.Name = "txtPriority";
            this.txtPriority.Size = new System.Drawing.Size(50, 20);
            this.txtPriority.TabIndex = 3;
            this.txtPriority.Text = "1";
            // 
            // lblPriority
            // 
            this.lblPriority.AutoSize = true;
            this.lblPriority.Location = new System.Drawing.Point(138, 337);
            this.lblPriority.Name = "lblPriority";
            this.lblPriority.Size = new System.Drawing.Size(46, 13);
            this.lblPriority.TabIndex = 4;
            this.lblPriority.Text = "Prioridad";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(584, 370);
            this.Controls.Add(this.lblPriority);
            this.Controls.Add(this.txtPriority);
            this.Controls.Add(this.btnTerminateProcess);
            this.Controls.Add(this.btnCreateProcess);
            this.Controls.Add(this.dataGridViewProcesses);
            this.Name = "MainForm";
            this.Text = "Gestión de Procesos y Recursos";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProcesses)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
