namespace WpfApp1
{
    partial class JVForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JVForm));
            this.JvLinkClass = new AxJVDTLabLib.AxJVLink();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.JvLinkClass)).BeginInit();
            this.SuspendLayout();
            // 
            // JvLinkClass
            // 
            this.JvLinkClass.Enabled = true;
            this.JvLinkClass.Location = new System.Drawing.Point(57, 8);
            this.JvLinkClass.Name = "JvLinkClass";
            this.JvLinkClass.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("JvLinkClass.OcxState")));
            this.JvLinkClass.Size = new System.Drawing.Size(87, 241);
            this.JvLinkClass.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "接続中";
            // 
            // JVForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 47);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.JvLinkClass);
            this.Name = "JVForm";
            this.Text = "JRA-VAN DataLab接続中";
            this.Load += new System.EventHandler(this.JVForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.JvLinkClass)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxJVDTLabLib.AxJVLink JvLinkClass;
        private System.Windows.Forms.Label label1;
    }
}