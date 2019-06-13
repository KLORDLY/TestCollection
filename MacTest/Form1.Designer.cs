namespace MacTest
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGetMac = new System.Windows.Forms.Button();
            this.btnGetMac2 = new System.Windows.Forms.Button();
            this.btnGetMac3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGetMac
            // 
            this.btnGetMac.Location = new System.Drawing.Point(55, 24);
            this.btnGetMac.Name = "btnGetMac";
            this.btnGetMac.Size = new System.Drawing.Size(185, 57);
            this.btnGetMac.TabIndex = 0;
            this.btnGetMac.Text = "GetMac";
            this.btnGetMac.UseVisualStyleBackColor = true;
            this.btnGetMac.Click += new System.EventHandler(this.btnGetMac_Click);
            // 
            // btnGetMac2
            // 
            this.btnGetMac2.Location = new System.Drawing.Point(55, 97);
            this.btnGetMac2.Name = "btnGetMac2";
            this.btnGetMac2.Size = new System.Drawing.Size(185, 57);
            this.btnGetMac2.TabIndex = 1;
            this.btnGetMac2.Text = "GetMac2";
            this.btnGetMac2.UseVisualStyleBackColor = true;
            this.btnGetMac2.Click += new System.EventHandler(this.btnGetMac2_Click);
            // 
            // btnGetMac3
            // 
            this.btnGetMac3.Location = new System.Drawing.Point(55, 173);
            this.btnGetMac3.Name = "btnGetMac3";
            this.btnGetMac3.Size = new System.Drawing.Size(185, 57);
            this.btnGetMac3.TabIndex = 2;
            this.btnGetMac3.Text = "GetMac3";
            this.btnGetMac3.UseVisualStyleBackColor = true;
            this.btnGetMac3.Click += new System.EventHandler(this.btnGetMac3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.btnGetMac3);
            this.Controls.Add(this.btnGetMac2);
            this.Controls.Add(this.btnGetMac);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGetMac;
        private System.Windows.Forms.Button btnGetMac2;
        private System.Windows.Forms.Button btnGetMac3;
    }
}

