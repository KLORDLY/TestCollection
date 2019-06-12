namespace EmguApplication
{
    partial class MainForm
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
            this.btnGray = new System.Windows.Forms.Button();
            this.btnRecognize = new System.Windows.Forms.Button();
            this.btnEyeDetect = new System.Windows.Forms.Button();
            this.btnLicense = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGray
            // 
            this.btnGray.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGray.Location = new System.Drawing.Point(47, 52);
            this.btnGray.Name = "btnGray";
            this.btnGray.Size = new System.Drawing.Size(162, 96);
            this.btnGray.TabIndex = 0;
            this.btnGray.Text = "灰度图";
            this.btnGray.UseVisualStyleBackColor = true;
            this.btnGray.Click += new System.EventHandler(this.btnGray_Click);
            // 
            // btnRecognize
            // 
            this.btnRecognize.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRecognize.Location = new System.Drawing.Point(239, 52);
            this.btnRecognize.Name = "btnRecognize";
            this.btnRecognize.Size = new System.Drawing.Size(162, 96);
            this.btnRecognize.TabIndex = 1;
            this.btnRecognize.Text = "人物辨认";
            this.btnRecognize.UseVisualStyleBackColor = true;
            this.btnRecognize.Click += new System.EventHandler(this.btnRecognize_Click);
            // 
            // btnEyeDetect
            // 
            this.btnEyeDetect.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEyeDetect.Location = new System.Drawing.Point(47, 202);
            this.btnEyeDetect.Name = "btnEyeDetect";
            this.btnEyeDetect.Size = new System.Drawing.Size(162, 96);
            this.btnEyeDetect.TabIndex = 2;
            this.btnEyeDetect.Text = "人眼识别";
            this.btnEyeDetect.UseVisualStyleBackColor = true;
            this.btnEyeDetect.Click += new System.EventHandler(this.btnEyeDetect_Click);
            // 
            // btnLicense
            // 
            this.btnLicense.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLicense.Location = new System.Drawing.Point(239, 202);
            this.btnLicense.Name = "btnLicense";
            this.btnLicense.Size = new System.Drawing.Size(162, 96);
            this.btnLicense.TabIndex = 3;
            this.btnLicense.Text = "车牌号识别";
            this.btnLicense.UseVisualStyleBackColor = true;
            this.btnLicense.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 341);
            this.Controls.Add(this.btnLicense);
            this.Controls.Add(this.btnEyeDetect);
            this.Controls.Add(this.btnRecognize);
            this.Controls.Add(this.btnGray);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGray;
        private System.Windows.Forms.Button btnRecognize;
        private System.Windows.Forms.Button btnEyeDetect;
        private System.Windows.Forms.Button btnLicense;
    }
}

