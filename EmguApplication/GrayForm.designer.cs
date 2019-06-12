namespace EmguApplication
{
    partial class GrayForm
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
            this.picBoxDst = new System.Windows.Forms.PictureBox();
            this.picBoxSrc = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxDst)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSrc)).BeginInit();
            this.SuspendLayout();
            // 
            // picBoxDst
            // 
            this.picBoxDst.Location = new System.Drawing.Point(407, 41);
            this.picBoxDst.Name = "picBoxDst";
            this.picBoxDst.Size = new System.Drawing.Size(350, 350);
            this.picBoxDst.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBoxDst.TabIndex = 3;
            this.picBoxDst.TabStop = false;
            // 
            // picBoxSrc
            // 
            this.picBoxSrc.Location = new System.Drawing.Point(27, 41);
            this.picBoxSrc.Name = "picBoxSrc";
            this.picBoxSrc.Size = new System.Drawing.Size(350, 350);
            this.picBoxSrc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBoxSrc.TabIndex = 2;
            this.picBoxSrc.TabStop = false;
            // 
            // GrayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 462);
            this.Controls.Add(this.picBoxDst);
            this.Controls.Add(this.picBoxSrc);
            this.Name = "GrayForm";
            this.Text = "GrayForm";
            ((System.ComponentModel.ISupportInitialize)(this.picBoxDst)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSrc)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picBoxDst;
        private System.Windows.Forms.PictureBox picBoxSrc;
    }
}