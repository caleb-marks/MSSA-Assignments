namespace Caleb_Marks_Assignment_11._3_Client
{
    partial class MainForm
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
            gamesGrid = new DataGridView();
            lblSeason = new Label();
            txtSeason = new TextBox();
            lblOpponent = new Label();
            txtOpponent = new TextBox();
            lblOuScore = new Label();
            txtOuScore = new TextBox();
            lblOpponentScore = new Label();
            txtOpponentScore = new TextBox();
            lblLocation = new Label();
            txtLocation = new TextBox();
            btnLoad = new Button();
            btnAdd = new Button();
            btnSelect = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            btnClear = new Button();
            ((System.ComponentModel.ISupportInitialize)gamesGrid).BeginInit();
            SuspendLayout();
            // 
            // gamesGrid
            // 
            gamesGrid.AllowUserToAddRows = false;
            gamesGrid.AllowUserToDeleteRows = false;
            gamesGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gamesGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            gamesGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gamesGrid.Location = new Point(12, 12);
            gamesGrid.MultiSelect = false;
            gamesGrid.Name = "gamesGrid";
            gamesGrid.ReadOnly = true;
            gamesGrid.RowHeadersVisible = false;
            gamesGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gamesGrid.Size = new Size(556, 456);
            gamesGrid.TabIndex = 0;
            // 
            // lblSeason
            // 
            lblSeason.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSeason.AutoSize = true;
            lblSeason.Location = new Point(584, 12);
            lblSeason.Name = "lblSeason";
            lblSeason.Size = new Size(44, 15);
            lblSeason.TabIndex = 1;
            lblSeason.Text = "Season";
            // 
            // txtSeason
            // 
            txtSeason.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtSeason.Location = new Point(584, 30);
            txtSeason.Name = "txtSeason";
            txtSeason.Size = new Size(300, 23);
            txtSeason.TabIndex = 2;
            // 
            // lblOpponent
            // 
            lblOpponent.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblOpponent.AutoSize = true;
            lblOpponent.Location = new Point(584, 62);
            lblOpponent.Name = "lblOpponent";
            lblOpponent.Size = new Size(59, 15);
            lblOpponent.TabIndex = 3;
            lblOpponent.Text = "Opponent";
            // 
            // txtOpponent
            // 
            txtOpponent.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtOpponent.Location = new Point(584, 80);
            txtOpponent.Name = "txtOpponent";
            txtOpponent.Size = new Size(300, 23);
            txtOpponent.TabIndex = 4;
            // 
            // lblOuScore
            // 
            lblOuScore.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblOuScore.AutoSize = true;
            lblOuScore.Location = new Point(584, 112);
            lblOuScore.Name = "lblOuScore";
            lblOuScore.Size = new Size(54, 15);
            lblOuScore.TabIndex = 5;
            lblOuScore.Text = "OU Score";
            // 
            // txtOuScore
            // 
            txtOuScore.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtOuScore.Location = new Point(584, 130);
            txtOuScore.Name = "txtOuScore";
            txtOuScore.Size = new Size(300, 23);
            txtOuScore.TabIndex = 6;
            // 
            // lblOpponentScore
            // 
            lblOpponentScore.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblOpponentScore.AutoSize = true;
            lblOpponentScore.Location = new Point(584, 162);
            lblOpponentScore.Name = "lblOpponentScore";
            lblOpponentScore.Size = new Size(93, 15);
            lblOpponentScore.TabIndex = 7;
            lblOpponentScore.Text = "Opponent Score";
            // 
            // txtOpponentScore
            // 
            txtOpponentScore.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtOpponentScore.Location = new Point(584, 180);
            txtOpponentScore.Name = "txtOpponentScore";
            txtOpponentScore.Size = new Size(300, 23);
            txtOpponentScore.TabIndex = 8;
            // 
            // lblLocation
            // 
            lblLocation.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblLocation.AutoSize = true;
            lblLocation.Location = new Point(584, 212);
            lblLocation.Name = "lblLocation";
            lblLocation.Size = new Size(53, 15);
            lblLocation.TabIndex = 9;
            lblLocation.Text = "Location";
            // 
            // txtLocation
            // 
            txtLocation.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtLocation.Location = new Point(584, 230);
            txtLocation.Name = "txtLocation";
            txtLocation.Size = new Size(300, 23);
            txtLocation.TabIndex = 10;
            // 
            // btnLoad
            // 
            btnLoad.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLoad.Location = new Point(584, 274);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(96, 30);
            btnLoad.TabIndex = 11;
            btnLoad.Text = "Load Games";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAdd.Location = new Point(686, 274);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(96, 30);
            btnAdd.TabIndex = 12;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnSelect
            // 
            btnSelect.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSelect.Location = new Point(788, 274);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(96, 30);
            btnSelect.TabIndex = 13;
            btnSelect.Text = "Select";
            btnSelect.UseVisualStyleBackColor = true;
            btnSelect.Click += btnSelect_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnUpdate.Location = new Point(584, 310);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(96, 30);
            btnUpdate.TabIndex = 14;
            btnUpdate.Text = "Update";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnDelete
            // 
            btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDelete.Location = new Point(686, 310);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(96, 30);
            btnDelete.TabIndex = 15;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnClear
            // 
            btnClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClear.Location = new Point(788, 310);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(96, 30);
            btnClear.TabIndex = 16;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 480);
            Controls.Add(btnClear);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnSelect);
            Controls.Add(btnAdd);
            Controls.Add(btnLoad);
            Controls.Add(txtLocation);
            Controls.Add(lblLocation);
            Controls.Add(txtOpponentScore);
            Controls.Add(lblOpponentScore);
            Controls.Add(txtOuScore);
            Controls.Add(lblOuScore);
            Controls.Add(txtOpponent);
            Controls.Add(lblOpponent);
            Controls.Add(txtSeason);
            Controls.Add(lblSeason);
            Controls.Add(gamesGrid);
            MinimumSize = new Size(916, 519);
            Name = "MainForm";
            Text = "OU Games Client";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)gamesGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView gamesGrid;
        private Label lblSeason;
        private TextBox txtSeason;
        private Label lblOpponent;
        private TextBox txtOpponent;
        private Label lblOuScore;
        private TextBox txtOuScore;
        private Label lblOpponentScore;
        private TextBox txtOpponentScore;
        private Label lblLocation;
        private TextBox txtLocation;
        private Button btnLoad;
        private Button btnAdd;
        private Button btnSelect;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClear;
    }
}