namespace FinalProject
{
    partial class GeneralManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeneralManager));
            this.btnEmployeeDetails = new System.Windows.Forms.Button();
            this.btnEventDetails = new System.Windows.Forms.Button();
            this.btnCustomerDetails = new System.Windows.Forms.Button();
            this.btnSupplierDetails = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEmployeeDetails
            // 
            this.btnEmployeeDetails.BackColor = System.Drawing.Color.RosyBrown;
            this.btnEmployeeDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmployeeDetails.Location = new System.Drawing.Point(422, 237);
            this.btnEmployeeDetails.Name = "btnEmployeeDetails";
            this.btnEmployeeDetails.Size = new System.Drawing.Size(228, 70);
            this.btnEmployeeDetails.TabIndex = 2;
            this.btnEmployeeDetails.Text = "Employee Details";
            this.btnEmployeeDetails.UseVisualStyleBackColor = false;
            this.btnEmployeeDetails.Click += new System.EventHandler(this.btnEmployeeDetails_Click);
            // 
            // btnEventDetails
            // 
            this.btnEventDetails.BackColor = System.Drawing.Color.RosyBrown;
            this.btnEventDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEventDetails.Location = new System.Drawing.Point(422, 421);
            this.btnEventDetails.Name = "btnEventDetails";
            this.btnEventDetails.Size = new System.Drawing.Size(228, 70);
            this.btnEventDetails.TabIndex = 4;
            this.btnEventDetails.Text = "Event Details";
            this.btnEventDetails.UseVisualStyleBackColor = false;
            this.btnEventDetails.Click += new System.EventHandler(this.btnEventDetails_Click);
            // 
            // btnCustomerDetails
            // 
            this.btnCustomerDetails.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnCustomerDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomerDetails.Location = new System.Drawing.Point(422, 510);
            this.btnCustomerDetails.Name = "btnCustomerDetails";
            this.btnCustomerDetails.Size = new System.Drawing.Size(228, 70);
            this.btnCustomerDetails.TabIndex = 5;
            this.btnCustomerDetails.Text = "Customer Details";
            this.btnCustomerDetails.UseVisualStyleBackColor = false;
            this.btnCustomerDetails.Click += new System.EventHandler(this.btnCustomerDetails_Click);
            // 
            // btnSupplierDetails
            // 
            this.btnSupplierDetails.BackColor = System.Drawing.Color.RosyBrown;
            this.btnSupplierDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSupplierDetails.Location = new System.Drawing.Point(422, 603);
            this.btnSupplierDetails.Name = "btnSupplierDetails";
            this.btnSupplierDetails.Size = new System.Drawing.Size(228, 70);
            this.btnSupplierDetails.TabIndex = 6;
            this.btnSupplierDetails.Text = "Supplier Details";
            this.btnSupplierDetails.UseVisualStyleBackColor = false;
            this.btnSupplierDetails.Click += new System.EventHandler(this.btnSupplierDetails_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.Location = new System.Drawing.Point(928, 777);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(99, 46);
            this.btnLogout.TabIndex = 7;
            this.btnLogout.Text = "Log out";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(422, 322);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(228, 70);
            this.button1.TabIndex = 8;
            this.button1.Text = "Warehouse Details";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // GeneralManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1082, 835);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnSupplierDetails);
            this.Controls.Add(this.btnCustomerDetails);
            this.Controls.Add(this.btnEventDetails);
            this.Controls.Add(this.btnEmployeeDetails);
            this.DoubleBuffered = true;
            this.Name = "GeneralManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GeneralManager";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnEmployeeDetails;
        private System.Windows.Forms.Button btnEventDetails;
        private System.Windows.Forms.Button btnCustomerDetails;
        private System.Windows.Forms.Button btnSupplierDetails;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button button1;
    }
}