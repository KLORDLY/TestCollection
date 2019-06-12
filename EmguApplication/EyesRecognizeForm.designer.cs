namespace EmguApplication
{
    partial class EyesRecognizeForm
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
            this.picBoxSrc = new System.Windows.Forms.PictureBox();
            this.picBoxDst = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSrc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxDst)).BeginInit();
            this.SuspendLayout();
            // 
            // picBoxSrc
            // 
            this.picBoxSrc.Location = new System.Drawing.Point(31, 12);
            this.picBoxSrc.Name = "picBoxSrc";
            this.picBoxSrc.Size = new System.Drawing.Size(350, 350);
            this.picBoxSrc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBoxSrc.TabIndex = 0;
            this.picBoxSrc.TabStop = false;
            // 
            // picBoxDst
            // 
            this.picBoxDst.Location = new System.Drawing.Point(411, 12);
            this.picBoxDst.Name = "picBoxDst";
            this.picBoxDst.Size = new System.Drawing.Size(350, 350);
            this.picBoxDst.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBoxDst.TabIndex = 1;
            this.picBoxDst.TabStop = false;
            // 
            // EyesRecognizeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 462);
            this.Controls.Add(this.picBoxDst);
            this.Controls.Add(this.picBoxSrc);
            this.Name = "EyesRecognizeForm";
            this.Text = "EyesRecognizeForm";
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSrc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxDst)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picBoxSrc;
        private System.Windows.Forms.PictureBox picBoxDst;
    }
}