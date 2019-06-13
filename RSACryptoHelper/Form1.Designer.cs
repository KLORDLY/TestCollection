namespace RSACryptoHelperTest
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
            this.txtBoxResult = new System.Windows.Forms.TextBox();
            this.btnMode1 = new System.Windows.Forms.Button();
            this.btnMode2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtBoxResult
            // 
            this.txtBoxResult.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBoxResult.Location = new System.Drawing.Point(132, 44);
            this.txtBoxResult.Multiline = true;
            this.txtBoxResult.Name = "txtBoxResult";
            this.txtBoxResult.Size = new System.Drawing.Size(596, 327);
            this.txtBoxResult.TabIndex = 0;
            // 
            // btnMode1
            // 
            this.btnMode1.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMode1.Location = new System.Drawing.Point(160, 418);
            this.btnMode1.Name = "btnMode1";
            this.btnMode1.Size = new System.Drawing.Size(180, 60);
            this.btnMode1.TabIndex = 1;
            this.btnMode1.Text = "加密模式";
            this.btnMode1.UseVisualStyleBackColor = true;
            this.btnMode1.Click += new System.EventHandler(this.btnMode1_Click);
            // 
            // btnMode2
            // 
            this.btnMode2.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMode2.Location = new System.Drawing.Point(503, 418);
            this.btnMode2.Name = "btnMode2";
            this.btnMode2.Size = new System.Drawing.Size(180, 60);
            this.btnMode2.TabIndex = 2;
            this.btnMode2.Text = "数字签名";
            this.btnMode2.UseVisualStyleBackColor = true;
            this.btnMode2.Click += new System.EventHandler(this.btnMode2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 521);
            this.Controls.Add(this.btnMode2);
            this.Controls.Add(this.btnMode1);
            this.Controls.Add(this.txtBoxResult);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBoxResult;
        private System.Windows.Forms.Button btnMode1;
        private System.Windows.Forms.Button btnMode2;
    }
}

