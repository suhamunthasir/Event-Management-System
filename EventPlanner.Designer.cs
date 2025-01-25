namespace FinalProject
{
    partial class EventPlanner
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventPlanner));
            this.btnCustomer = new System.Windows.Forms.Button();
            this.btnEvent = new System.Windows.Forms.Button();
            this.btnCalender = new System.Windows.Forms.Button();
            this.btnFeedback = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnEventHistory = new System.Windows.Forms.Button();
            this.btnMeetings = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCustomer
            // 
            this.btnCustomer.BackColor = System.Drawing.Color.RosyBrown;
            this.btnCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomer.Location = new System.Drawing.Point(415, 209);
            this.btnCustomer.Name = "btnCustomer";
            this.btnCustomer.Size = new System.Drawing.Size(228, 70);
            this.btnCustomer.TabIndex = 0;
            this.btnCustomer.Text = "Customer Details";
            this.btnCustomer.UseVisualStyleBackColor = false;
            this.btnCustomer.Click += new System.EventHandler(this.btnCustomer_Click);
            // 
            // btnEvent
            // 
            this.btnEvent.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnEvent.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEvent.Location = new System.Drawing.Point(415, 300);
            this.btnEvent.Name = "btnEvent";
            this.btnEvent.Size = new System.Drawing.Size(228, 70);
            this.btnEvent.TabIndex = 1;
            this.btnEvent.Text = "Event Details";
            this.btnEvent.UseVisualStyleBackColor = false;
            this.btnEvent.Click += new System.EventHandler(this.btnEvent_Click);
            // 
            // btnCalender
            // 
            this.btnCalender.BackColor = System.Drawing.Color.RosyBrown;
            this.btnCalender.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalender.Location = new System.Drawing.Point(415, 393);
            this.btnCalender.Name = "btnCalender";
            this.btnCalender.Size = new System.Drawing.Size(228, 70);
            this.btnCalender.TabIndex = 2;
            this.btnCalender.Text = "Calender";
            this.btnCalender.UseVisualStyleBackColor = false;
            this.btnCalender.Click += new System.EventHandler(this.btnCalender_Click);
            // 
            // btnFeedback
            // 
            this.btnFeedback.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnFeedback.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFeedback.Location = new System.Drawing.Point(415, 491);
            this.btnFeedback.Name = "btnFeedback";
            this.btnFeedback.Size = new System.Drawing.Size(228, 70);
            this.btnFeedback.TabIndex = 3;
            this.btnFeedback.Text = "Feedback";
            this.btnFeedback.UseVisualStyleBackColor = false;
            this.btnFeedback.Click += new System.EventHandler(this.btnFeedback_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.Location = new System.Drawing.Point(839, 731);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(99, 46);
            this.btnLogout.TabIndex = 4;
            this.btnLogout.Text = "Log out";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnEventHistory
            // 
            this.btnEventHistory.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnEventHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEventHistory.Location = new System.Drawing.Point(415, 681);
            this.btnEventHistory.Name = "btnEventHistory";
            this.btnEventHistory.Size = new System.Drawing.Size(228, 70);
            this.btnEventHistory.TabIndex = 6;
            this.btnEventHistory.Text = "Event History";
            this.btnEventHistory.UseVisualStyleBackColor = false;
            this.btnEventHistory.Click += new System.EventHandler(this.btnEventHistory_Click);
            // 
            // btnMeetings
            // 
            this.btnMeetings.BackColor = System.Drawing.Color.RosyBrown;
            this.btnMeetings.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMeetings.Location = new System.Drawing.Point(415, 586);
            this.btnMeetings.Name = "btnMeetings";
            this.btnMeetings.Size = new System.Drawing.Size(228, 70);
            this.btnMeetings.TabIndex = 7;
            this.btnMeetings.Text = "Meetings";
            this.btnMeetings.UseVisualStyleBackColor = false;
            this.btnMeetings.Click += new System.EventHandler(this.btnMeetings_Click);
            // 
            // EventPlanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1006, 804);
            this.Controls.Add(this.btnMeetings);
            this.Controls.Add(this.btnEventHistory);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnFeedback);
            this.Controls.Add(this.btnCalender);
            this.Controls.Add(this.btnEvent);
            this.Controls.Add(this.btnCustomer);
            this.Name = "EventPlanner";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EventPlanner";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCustomer;
        private System.Windows.Forms.Button btnEvent;
        private System.Windows.Forms.Button btnCalender;
        private System.Windows.Forms.Button btnFeedback;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnEventHistory;
        private System.Windows.Forms.Button btnMeetings;
    }
}