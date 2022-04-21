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
            this.label1 = new System.Windows.Forms.Label();
            this.JvLinkClass = new AxJVDTLabLib.AxJVLink();
            ((System.ComponentModel.ISupportInitialize)(this.JvLinkClass)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "接続中";
            // 
            // JvLinkClass
            // 
            this.JvLinkClass.Enabled = true;
            this.JvLinkClass.Location = new System.Drawing.Point(157, 13);
            this.JvLinkClass.Name = "JvLinkClass";
            this.JvLinkClass.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("JvLinkClass.OcxState")));
            this.JvLinkClass.Size = new System.Drawing.Size(764, 384);
            this.JvLinkClass.TabIndex = 2;
            // 
            // JVForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 94);
            this.Controls.Add(this.JvLinkClass);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "JVForm";
            this.Text = "JRA-VAN DataLab接続中";
            this.Load += new System.EventHandler(this.JVForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.JvLinkClass)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private AxJVDTLabLib.AxJVLink JvLinkClass;
    }
}