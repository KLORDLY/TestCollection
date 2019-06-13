namespace SingletonTest
{
    partial class EntityTest
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
            this.btnSetEntity1 = new System.Windows.Forms.Button();
            this.btnEntity2 = new System.Windows.Forms.Button();
            this.btnEntities = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSetEntity1
            // 
            this.btnSetEntity1.Location = new System.Drawing.Point(176, 50);
            this.btnSetEntity1.Name = "btnSetEntity1";
            this.btnSetEntity1.Size = new System.Drawing.Size(255, 78);
            this.btnSetEntity1.TabIndex = 0;
            this.btnSetEntity1.Text = "建立实例1";
            this.btnSetEntity1.UseVisualStyleBackColor = true;
            this.btnSetEntity1.Click += new System.EventHandler(this.btnSetEntity1_Click);
            // 
            // btnEntity2
            // 
            this.btnEntity2.Location = new System.Drawing.Point(176, 134);
            this.btnEntity2.Name = "btnEntity2";
            this.btnEntity2.Size = new System.Drawing.Size(255, 78);
            this.btnEntity2.TabIndex = 1;
            this.btnEntity2.Text = "建立实例2";
            this.btnEntity2.UseVisualStyleBackColor = true;
            this.btnEntity2.Click += new System.EventHandler(this.btnEntity2_Click);
            // 
            // btnEntities
            // 
            this.btnEntities.Location = new System.Drawing.Point(176, 218);
            this.btnEntities.Name = "btnEntities";
            this.btnEntities.Size = new System.Drawing.Size(255, 78);
            this.btnEntities.TabIndex = 2;
            this.btnEntities.Text = "建立两实例";
            this.btnEntities.UseVisualStyleBackColor = true;
            this.btnEntities.Click += new System.EventHandler(this.btnEntities_Click);
            // 
            // EntityTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 405);
            this.Controls.Add(this.btnEntities);
            this.Controls.Add(this.btnEntity2);
            this.Controls.Add(this.btnSetEntity1);
            this.Name = "EntityTest";
            this.Text = "EntityTest";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSetEntity1;
        private System.Windows.Forms.Button btnEntity2;
        private System.Windows.Forms.Button btnEntities;
    }
}