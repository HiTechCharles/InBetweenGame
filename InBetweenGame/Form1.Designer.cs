namespace InBetweenGame
{
    partial class Form1
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
            if (disposing)
            {
                _cardTalk?.Dispose();
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
            this.MainMenuMST = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speakToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cardsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chipsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RightCardPB = new System.Windows.Forms.PictureBox();
            this.CenterCardPB = new System.Windows.Forms.PictureBox();
            this.LeftCardPB = new System.Windows.Forms.PictureBox();
            this.PlayersDGV = new System.Windows.Forms.DataGridView();
            this.StatusTB = new System.Windows.Forms.Label();
            this.MainMenuMST.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RightCardPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CenterCardPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LeftCardPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayersDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // MainMenuMST
            // 
            this.MainMenuMST.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.MainMenuMST.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.speakToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.MainMenuMST.Location = new System.Drawing.Point(0, 0);
            this.MainMenuMST.Name = "MainMenuMST";
            this.MainMenuMST.Padding = new System.Windows.Forms.Padding(15, 4, 0, 4);
            this.MainMenuMST.Size = new System.Drawing.Size(1217, 41);
            this.MainMenuMST.TabIndex = 0;
            this.MainMenuMST.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGameToolStripMenuItem,
            this.saveExitToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(117, 33);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // newGameToolStripMenuItem
            // 
            this.newGameToolStripMenuItem.Name = "newGameToolStripMenuItem";
            this.newGameToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newGameToolStripMenuItem.Size = new System.Drawing.Size(335, 34);
            this.newGameToolStripMenuItem.Text = "&New Game...";
            this.newGameToolStripMenuItem.Click += new System.EventHandler(this.newGameToolStripMenuItem_Click);
            // 
            // saveExitToolStripMenuItem
            // 
            this.saveExitToolStripMenuItem.Name = "saveExitToolStripMenuItem";
            this.saveExitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveExitToolStripMenuItem.Size = new System.Drawing.Size(335, 34);
            this.saveExitToolStripMenuItem.Text = "&Save && Exit";
            this.saveExitToolStripMenuItem.Click += new System.EventHandler(this.saveExitToolStripMenuItem_Click);
            // 
            // speakToolStripMenuItem
            // 
            this.speakToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cardsToolStripMenuItem,
            this.chipsToolStripMenuItem,
            this.statusBarToolStripMenuItem});
            this.speakToolStripMenuItem.Name = "speakToolStripMenuItem";
            this.speakToolStripMenuItem.Size = new System.Drawing.Size(97, 33);
            this.speakToolStripMenuItem.Text = "&Speak";
            // 
            // cardsToolStripMenuItem
            // 
            this.cardsToolStripMenuItem.Name = "cardsToolStripMenuItem";
            this.cardsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.cardsToolStripMenuItem.Size = new System.Drawing.Size(303, 34);
            this.cardsToolStripMenuItem.Text = "Car&ds";
            this.cardsToolStripMenuItem.Click += new System.EventHandler(this.cardsToolStripMenuItem_Click);
            // 
            // chipsToolStripMenuItem
            // 
            this.chipsToolStripMenuItem.Name = "chipsToolStripMenuItem";
            this.chipsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.chipsToolStripMenuItem.Size = new System.Drawing.Size(303, 34);
            this.chipsToolStripMenuItem.Text = "&Chips";
            this.chipsToolStripMenuItem.Click += new System.EventHandler(this.chipsToolStripMenuItem_Click);
            // 
            // statusBarToolStripMenuItem
            // 
            this.statusBarToolStripMenuItem.Name = "statusBarToolStripMenuItem";
            this.statusBarToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.statusBarToolStripMenuItem.Size = new System.Drawing.Size(303, 34);
            this.statusBarToolStripMenuItem.Text = "Stat&us Bar";
            this.statusBarToolStripMenuItem.Click += new System.EventHandler(this.statusBarToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(79, 33);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(73, 22);
            // 
            // RightCardPB
            // 
            this.RightCardPB.AccessibleName = "Right Card.";
            this.RightCardPB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.RightCardPB.Location = new System.Drawing.Point(609, 297);
            this.RightCardPB.Name = "RightCardPB";
            this.RightCardPB.Size = new System.Drawing.Size(250, 395);
            this.RightCardPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.RightCardPB.TabIndex = 1;
            this.RightCardPB.TabStop = false;
            // 
            // CenterCardPB
            // 
            this.CenterCardPB.AccessibleName = "Center Card";
            this.CenterCardPB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CenterCardPB.Location = new System.Drawing.Point(306, 297);
            this.CenterCardPB.Name = "CenterCardPB";
            this.CenterCardPB.Size = new System.Drawing.Size(250, 395);
            this.CenterCardPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.CenterCardPB.TabIndex = 2;
            this.CenterCardPB.TabStop = false;
            // 
            // LeftCardPB
            // 
            this.LeftCardPB.AccessibleName = "Left Card.";
            this.LeftCardPB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LeftCardPB.Location = new System.Drawing.Point(12, 297);
            this.LeftCardPB.Name = "LeftCardPB";
            this.LeftCardPB.Size = new System.Drawing.Size(250, 406);
            this.LeftCardPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.LeftCardPB.TabIndex = 3;
            this.LeftCardPB.TabStop = false;
            // 
            // PlayersDGV
            // 
            this.PlayersDGV.AllowUserToAddRows = false;
            this.PlayersDGV.AllowUserToDeleteRows = false;
            this.PlayersDGV.AllowUserToResizeRows = false;
            this.PlayersDGV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PlayersDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.PlayersDGV.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))));
            this.PlayersDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PlayersDGV.Location = new System.Drawing.Point(35, 69);
            this.PlayersDGV.MultiSelect = false;
            this.PlayersDGV.Name = "PlayersDGV";
            this.PlayersDGV.ReadOnly = true;
            this.PlayersDGV.RowHeadersVisible = false;
            this.PlayersDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.PlayersDGV.Size = new System.Drawing.Size(847, 222);
            this.PlayersDGV.TabIndex = 1;
            // 
            // StatusTB
            // 
            this.StatusTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))));
            this.StatusTB.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.StatusTB.ForeColor = System.Drawing.Color.Gold;
            this.StatusTB.Location = new System.Drawing.Point(12, 713);
            this.StatusTB.Name = "StatusTB";
            this.StatusTB.Size = new System.Drawing.Size(847, 36);
            this.StatusTB.TabIndex = 2;
            this.StatusTB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(1217, 761);
            this.Controls.Add(this.StatusTB);
            this.Controls.Add(this.PlayersDGV);
            this.Controls.Add(this.LeftCardPB);
            this.Controls.Add(this.CenterCardPB);
            this.Controls.Add(this.RightCardPB);
            this.Controls.Add(this.MainMenuMST);
            this.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.MainMenuMST;
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "In Between Game";
            this.MainMenuMST.ResumeLayout(false);
            this.MainMenuMST.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RightCardPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CenterCardPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LeftCardPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayersDGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenuMST;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.PictureBox RightCardPB;
        private System.Windows.Forms.PictureBox CenterCardPB;
        private System.Windows.Forms.PictureBox LeftCardPB;
        private System.Windows.Forms.ToolStripMenuItem newGameToolStripMenuItem;
        private System.Windows.Forms.DataGridView PlayersDGV;
        private System.Windows.Forms.Label StatusTB;
        private System.Windows.Forms.ToolStripMenuItem speakToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cardsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chipsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statusBarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}