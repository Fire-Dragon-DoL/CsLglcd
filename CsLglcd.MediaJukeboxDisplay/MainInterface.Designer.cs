namespace CsLglcd.MediaJukeboxDisplay
{
    partial class MainInterface
    {
        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Liberare le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione componenti

        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.raiseAppletPriorityCheckBox = new System.Windows.Forms.CheckBox();
            this.priorityTimeoutNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.priorityTimeoutNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // raiseAppletPriorityCheckBox
            // 
            this.raiseAppletPriorityCheckBox.AutoSize = true;
            this.raiseAppletPriorityCheckBox.Location = new System.Drawing.Point(3, 3);
            this.raiseAppletPriorityCheckBox.Name = "raiseAppletPriorityCheckBox";
            this.raiseAppletPriorityCheckBox.Size = new System.Drawing.Size(236, 17);
            this.raiseAppletPriorityCheckBox.TabIndex = 0;
            this.raiseAppletPriorityCheckBox.Text = "Raise applet priority when track changes for:";
            this.raiseAppletPriorityCheckBox.UseVisualStyleBackColor = true;
            this.raiseAppletPriorityCheckBox.CheckedChanged += new System.EventHandler(this.raiseAppletPriorityCheckBox_CheckedChanged);
            // 
            // priorityTimeoutNumericUpDown
            // 
            this.priorityTimeoutNumericUpDown.Location = new System.Drawing.Point(3, 26);
            this.priorityTimeoutNumericUpDown.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.priorityTimeoutNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.priorityTimeoutNumericUpDown.Name = "priorityTimeoutNumericUpDown";
            this.priorityTimeoutNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.priorityTimeoutNumericUpDown.TabIndex = 1;
            this.priorityTimeoutNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.priorityTimeoutNumericUpDown.ValueChanged += new System.EventHandler(this.priorityTimeoutNumericUpDown_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(129, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "seconds";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(0, 52);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(239, 23);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "Save Settings";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // MainInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.priorityTimeoutNumericUpDown);
            this.Controls.Add(this.raiseAppletPriorityCheckBox);
            this.Name = "MainInterface";
            this.Size = new System.Drawing.Size(599, 403);
            ((System.ComponentModel.ISupportInitialize)(this.priorityTimeoutNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox raiseAppletPriorityCheckBox;
        private System.Windows.Forms.NumericUpDown priorityTimeoutNumericUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button saveButton;
    }
}
