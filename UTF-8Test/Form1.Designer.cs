namespace UTF_8Test
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnUTF8FromString = new System.Windows.Forms.Button();
            this.btnUTF8ToString = new System.Windows.Forms.Button();
            this.btnUTF16ToString = new System.Windows.Forms.Button();
            this.btnUTF16FromString = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(23, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(751, 360);
            this.textBox1.TabIndex = 0;
            // 
            // btnUTF8FromString
            // 
            this.btnUTF8FromString.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUTF8FromString.Location = new System.Drawing.Point(23, 406);
            this.btnUTF8FromString.Name = "btnUTF8FromString";
            this.btnUTF8FromString.Size = new System.Drawing.Size(156, 67);
            this.btnUTF8FromString.TabIndex = 1;
            this.btnUTF8FromString.Text = "UTF-8编码";
            this.btnUTF8FromString.UseVisualStyleBackColor = true;
            this.btnUTF8FromString.Click += new System.EventHandler(this.btnUTF8FromString_Click);
            // 
            // btnUTF8ToString
            // 
            this.btnUTF8ToString.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUTF8ToString.Location = new System.Drawing.Point(221, 406);
            this.btnUTF8ToString.Name = "btnUTF8ToString";
            this.btnUTF8ToString.Size = new System.Drawing.Size(156, 67);
            this.btnUTF8ToString.TabIndex = 2;
            this.btnUTF8ToString.Text = "UTF-8解码";
            this.btnUTF8ToString.UseVisualStyleBackColor = true;
            this.btnUTF8ToString.Click += new System.EventHandler(this.btnUTF8ToString_Click);
            // 
            // btnUTF16ToString
            // 
            this.btnUTF16ToString.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUTF16ToString.Location = new System.Drawing.Point(617, 406);
            this.btnUTF16ToString.Name = "btnUTF16ToString";
            this.btnUTF16ToString.Size = new System.Drawing.Size(156, 67);
            this.btnUTF16ToString.TabIndex = 4;
            this.btnUTF16ToString.Text = "UTF-16解码";
            this.btnUTF16ToString.UseVisualStyleBackColor = true;
            this.btnUTF16ToString.Click += new System.EventHandler(this.btnUTF16ToString_Click);
            // 
            // btnUTF16FromString
            // 
            this.btnUTF16FromString.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUTF16FromString.Location = new System.Drawing.Point(419, 406);
            this.btnUTF16FromString.Name = "btnUTF16FromString";
            this.btnUTF16FromString.Size = new System.Drawing.Size(156, 67);
            this.btnUTF16FromString.TabIndex = 3;
            this.btnUTF16FromString.Text = "UTF-16编码";
            this.btnUTF16FromString.UseVisualStyleBackColor = true;
            this.btnUTF16FromString.Click += new System.EventHandler(this.btnUTF16FromString_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 540);
            this.Controls.Add(this.btnUTF16ToString);
            this.Controls.Add(this.btnUTF16FromString);
            this.Controls.Add(this.btnUTF8ToString);
            this.Controls.Add(this.btnUTF8FromString);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnUTF8FromString;
        private System.Windows.Forms.Button btnUTF8ToString;
        private System.Windows.Forms.Button btnUTF16ToString;
        private System.Windows.Forms.Button btnUTF16FromString;
    }
}

