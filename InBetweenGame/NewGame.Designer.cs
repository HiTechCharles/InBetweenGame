 namespace InBetweenGame
{
    partial class NewGame
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.HumanLBL = new System.Windows.Forms.Label();
            this.ComputerLBL = new System.Windows.Forms.Label();
            this.HumanNameTB = new System.Windows.Forms.TextBox();
            this.HumanLB = new System.Windows.Forms.ListBox();
            this.ComputerLB = new System.Windows.Forms.ListBox();
            this.HumanDelBTN = new System.Windows.Forms.Button();
            this.HumanAddBTN = new System.Windows.Forms.Button();
            this.ComputerNameCB = new System.Windows.Forms.ComboBox();
            this.ComputerAddBTN = new System.Windows.Forms.Button();
            this.ComputerRemoveBTN = new System.Windows.Forms.Button();
            this.HelpLBL = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.AccessibleName = "Start game with added players";
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(150)))), ((int)(((byte)(50)))));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(674, 668);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(120, 50);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(25, 668);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 50);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // HumanLBL
            // 
            this.HumanLBL.AccessibleRole = System.Windows.Forms.AccessibleRole.OutlineButton;
            this.HumanLBL.AutoSize = true;
            this.HumanLBL.ForeColor = System.Drawing.Color.Gold;
            this.HumanLBL.Location = new System.Drawing.Point(63, 213);
            this.HumanLBL.Name = "HumanLBL";
            this.HumanLBL.Size = new System.Drawing.Size(201, 29);
            this.HumanLBL.TabIndex = 1;
            this.HumanLBL.Text = "&Human Players:";
            this.HumanLBL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ComputerLBL
            // 
            this.ComputerLBL.AccessibleRole = System.Windows.Forms.AccessibleRole.OutlineButton;
            this.ComputerLBL.AutoSize = true;
            this.ComputerLBL.ForeColor = System.Drawing.Color.Gold;
            this.ComputerLBL.Location = new System.Drawing.Point(485, 213);
            this.ComputerLBL.Name = "ComputerLBL";
            this.ComputerLBL.Size = new System.Drawing.Size(234, 29);
            this.ComputerLBL.TabIndex = 6;
            this.ComputerLBL.Text = "&Computer Players:";
            this.ComputerLBL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // HumanNameTB
            // 
            this.HumanNameTB.AccessibleDescription = "Type in a name, press the Human Add button.";
            this.HumanNameTB.AccessibleName = "Human Name.";
            this.HumanNameTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            this.HumanNameTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.HumanNameTB.Location = new System.Drawing.Point(58, 264);
            this.HumanNameTB.Name = "HumanNameTB";
            this.HumanNameTB.Size = new System.Drawing.Size(304, 36);
            this.HumanNameTB.TabIndex = 2;
            // 
            // HumanLB
            // 
            this.HumanLB.AccessibleName = "List of Human Players:";
            this.HumanLB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            this.HumanLB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.HumanLB.FormattingEnabled = true;
            this.HumanLB.ItemHeight = 29;
            this.HumanLB.Location = new System.Drawing.Point(58, 339);
            this.HumanLB.Name = "HumanLB";
            this.HumanLB.Size = new System.Drawing.Size(304, 176);
            this.HumanLB.Sorted = true;
            this.HumanLB.TabIndex = 3;
            // 
            // ComputerLB
            // 
            this.ComputerLB.AccessibleName = "List of Computer Players:";
            this.ComputerLB.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ComputerLB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ComputerLB.FormattingEnabled = true;
            this.ComputerLB.ItemHeight = 29;
            this.ComputerLB.Location = new System.Drawing.Point(490, 339);
            this.ComputerLB.Name = "ComputerLB";
            this.ComputerLB.Size = new System.Drawing.Size(304, 176);
            this.ComputerLB.Sorted = true;
            this.ComputerLB.TabIndex = 8;
            this.ComputerLB.SelectedIndexChanged += new System.EventHandler(this.ComputerLB_SelectedIndexChanged);
            // 
            // HumanDelBTN
            // 
            this.HumanDelBTN.AccessibleName = "Remove Human Player";
            this.HumanDelBTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(150)))));
            this.HumanDelBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HumanDelBTN.ForeColor = System.Drawing.Color.White;
            this.HumanDelBTN.Location = new System.Drawing.Point(242, 539);
            this.HumanDelBTN.Name = "HumanDelBTN";
            this.HumanDelBTN.Size = new System.Drawing.Size(120, 50);
            this.HumanDelBTN.TabIndex = 5;
            this.HumanDelBTN.Text = "&Remove";
            this.HumanDelBTN.UseVisualStyleBackColor = false;
            this.HumanDelBTN.Click += new System.EventHandler(this.HumanDelBTN_Click);
            // 
            // HumanAddBTN
            // 
            this.HumanAddBTN.AccessibleName = "Add Human Player";
            this.HumanAddBTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(150)))));
            this.HumanAddBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HumanAddBTN.ForeColor = System.Drawing.Color.White;
            this.HumanAddBTN.Location = new System.Drawing.Point(58, 539);
            this.HumanAddBTN.Name = "HumanAddBTN";
            this.HumanAddBTN.Size = new System.Drawing.Size(120, 50);
            this.HumanAddBTN.TabIndex = 4;
            this.HumanAddBTN.Text = "&Add";
            this.HumanAddBTN.UseVisualStyleBackColor = false;
            this.HumanAddBTN.Click += new System.EventHandler(this.HumanAddBTN_Click);
            // 
            // ComputerNameCB
            // 
            this.ComputerNameCB.AccessibleDescription = "Type, or select a name foran AI  player";
            this.ComputerNameCB.AccessibleName = "Computer Name ComboBox";
            this.ComputerNameCB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            this.ComputerNameCB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComputerNameCB.FormattingEnabled = true;
            this.ComputerNameCB.Items.AddRange(new object[] {
            "Alexa",
            "Apple II",
            "C-3PO",
            "Commodore 64",
            "Google Home",
            "Hal 9000",
            "IBM PC",
            "KITT",
            "R2-D2",
            "Siri",
            "SkyNet",
            "WOPR"});
            this.ComputerNameCB.Location = new System.Drawing.Point(490, 263);
            this.ComputerNameCB.Name = "ComputerNameCB";
            this.ComputerNameCB.Size = new System.Drawing.Size(304, 37);
            this.ComputerNameCB.TabIndex = 7;
            // 
            // ComputerAddBTN
            // 
            this.ComputerAddBTN.AccessibleName = "Add Computer Player";
            this.ComputerAddBTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(150)))));
            this.ComputerAddBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComputerAddBTN.ForeColor = System.Drawing.Color.White;
            this.ComputerAddBTN.Location = new System.Drawing.Point(490, 539);
            this.ComputerAddBTN.Name = "ComputerAddBTN";
            this.ComputerAddBTN.Size = new System.Drawing.Size(120, 50);
            this.ComputerAddBTN.TabIndex = 9;
            this.ComputerAddBTN.Text = "&Add";
            this.ComputerAddBTN.UseVisualStyleBackColor = false;
            this.ComputerAddBTN.Click += new System.EventHandler(this.ComputerAddBTN_Click);
            // 
            // ComputerRemoveBTN
            // 
            this.ComputerRemoveBTN.AccessibleName = "Remove Computer Player";
            this.ComputerRemoveBTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(150)))));
            this.ComputerRemoveBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComputerRemoveBTN.ForeColor = System.Drawing.Color.White;
            this.ComputerRemoveBTN.Location = new System.Drawing.Point(674, 539);
            this.ComputerRemoveBTN.Name = "ComputerRemoveBTN";
            this.ComputerRemoveBTN.Size = new System.Drawing.Size(120, 50);
            this.ComputerRemoveBTN.TabIndex = 10;
            this.ComputerRemoveBTN.Text = "&Remove";
            this.ComputerRemoveBTN.UseVisualStyleBackColor = false;
            this.ComputerRemoveBTN.Click += new System.EventHandler(this.ComputerRemoveBTN_Click);
            // 
            // HelpLBL
            // 
            this.HelpLBL.AccessibleRole = System.Windows.Forms.AccessibleRole.OutlineButton;
            this.HelpLBL.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.HelpLBL.ForeColor = System.Drawing.Color.Gold;
            this.HelpLBL.Location = new System.Drawing.Point(58, 26);
            this.HelpLBL.Name = "HelpLBL";
            this.HelpLBL.Size = new System.Drawing.Size(736, 140);
            this.HelpLBL.TabIndex = 0;
            this.HelpLBL.Text = "Configure New Game";
            this.HelpLBL.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // NewGame
            // 
            this.AcceptButton = this.btnOK;
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(80)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(859, 739);
            this.Controls.Add(this.HelpLBL);
            this.Controls.Add(this.ComputerAddBTN);
            this.Controls.Add(this.ComputerRemoveBTN);
            this.Controls.Add(this.ComputerNameCB);
            this.Controls.Add(this.HumanAddBTN);
            this.Controls.Add(this.HumanDelBTN);
            this.Controls.Add(this.ComputerLB);
            this.Controls.Add(this.HumanLB);
            this.Controls.Add(this.HumanNameTB);
            this.Controls.Add(this.ComputerLBL);
            this.Controls.Add(this.HumanLBL);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "NewGame";
            this.Text = "NewGame";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label HumanLBL;
        private System.Windows.Forms.Label ComputerLBL;
        private System.Windows.Forms.TextBox HumanNameTB;
        private System.Windows.Forms.ListBox HumanLB;
        private System.Windows.Forms.ListBox ComputerLB;
        private System.Windows.Forms.Button HumanDelBTN;
        private System.Windows.Forms.Button HumanAddBTN;
        private System.Windows.Forms.ComboBox ComputerNameCB;
        private System.Windows.Forms.Button ComputerAddBTN;
        private System.Windows.Forms.Button ComputerRemoveBTN;
        private System.Windows.Forms.Label HelpLBL;
    }
}