
namespace E2Play
{
    partial class MainView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainView));
            this.chkShowClues = new System.Windows.Forms.CheckBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.chkPieceText = new System.Windows.Forms.CheckBox();
            this.OpenE2PFile = new System.Windows.Forms.OpenFileDialog();
            this.SaveE2PFile = new System.Windows.Forms.SaveFileDialog();
            this.chkHideUselessPieces = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkSize = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPiecesPlaced = new System.Windows.Forms.Label();
            this.PieceList = new System.Windows.Forms.ListView();
            this.colHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.board1 = new E2Play.Board();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkShowClues
            // 
            this.chkShowClues.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkShowClues.AutoSize = true;
            this.chkShowClues.Location = new System.Drawing.Point(12, 17);
            this.chkShowClues.Name = "chkShowClues";
            this.chkShowClues.Size = new System.Drawing.Size(82, 17);
            this.chkShowClues.TabIndex = 5;
            this.chkShowClues.Text = "Show Clues";
            this.chkShowClues.UseVisualStyleBackColor = true;
            this.chkShowClues.CheckedChanged += new System.EventHandler(this.chkShowClues_CheckedChanged);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(701, 457);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 6;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(701, 486);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoad.Location = new System.Drawing.Point(701, 515);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 8;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // chkPieceText
            // 
            this.chkPieceText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkPieceText.AutoSize = true;
            this.chkPieceText.Location = new System.Drawing.Point(12, 41);
            this.chkPieceText.Name = "chkPieceText";
            this.chkPieceText.Size = new System.Drawing.Size(77, 17);
            this.chkPieceText.TabIndex = 9;
            this.chkPieceText.Text = "Piece Text";
            this.chkPieceText.UseVisualStyleBackColor = true;
            this.chkPieceText.CheckedChanged += new System.EventHandler(this.chkPieceText_CheckedChanged);
            // 
            // OpenE2PFile
            // 
            this.OpenE2PFile.FileName = "E2PlayProgress";
            this.OpenE2PFile.Filter = "E2Play Saves|*.e2p|Backtracker Saves|*.E2O";
            this.OpenE2PFile.Title = "Select E2Play file";
            // 
            // SaveE2PFile
            // 
            this.SaveE2PFile.DefaultExt = "e2p";
            this.SaveE2PFile.Filter = "E2Play|*.e2p";
            this.SaveE2PFile.Title = "Save E2Play File";
            // 
            // chkHideUselessPieces
            // 
            this.chkHideUselessPieces.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkHideUselessPieces.AutoSize = true;
            this.chkHideUselessPieces.Location = new System.Drawing.Point(12, 64);
            this.chkHideUselessPieces.Name = "chkHideUselessPieces";
            this.chkHideUselessPieces.Size = new System.Drawing.Size(123, 17);
            this.chkHideUselessPieces.TabIndex = 10;
            this.chkHideUselessPieces.Text = "Hide Useless Pieces";
            this.chkHideUselessPieces.UseVisualStyleBackColor = true;
            this.chkHideUselessPieces.CheckedChanged += new System.EventHandler(this.chkHideUselessPieces_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkSize);
            this.groupBox1.Controls.Add(this.chkHideUselessPieces);
            this.groupBox1.Controls.Add(this.chkShowClues);
            this.groupBox1.Controls.Add(this.chkPieceText);
            this.groupBox1.Location = new System.Drawing.Point(682, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(141, 167);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // chkSize
            // 
            this.chkSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSize.AutoSize = true;
            this.chkSize.Location = new System.Drawing.Point(12, 133);
            this.chkSize.Name = "chkSize";
            this.chkSize.Size = new System.Drawing.Size(73, 17);
            this.chkSize.TabIndex = 11;
            this.chkSize.Text = "Big 48x48";
            this.chkSize.UseVisualStyleBackColor = true;
            this.chkSize.CheckedChanged += new System.EventHandler(this.chkSize_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(679, 200);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pieces Placed";
            // 
            // lblPiecesPlaced
            // 
            this.lblPiecesPlaced.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPiecesPlaced.AutoSize = true;
            this.lblPiecesPlaced.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPiecesPlaced.Location = new System.Drawing.Point(682, 217);
            this.lblPiecesPlaced.Name = "lblPiecesPlaced";
            this.lblPiecesPlaced.Size = new System.Drawing.Size(0, 13);
            this.lblPiecesPlaced.TabIndex = 13;
            // 
            // PieceList
            // 
            this.PieceList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PieceList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHeader});
            this.PieceList.FullRowSelect = true;
            this.PieceList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.PieceList.LabelWrap = false;
            this.PieceList.Location = new System.Drawing.Point(521, 2);
            this.PieceList.MultiSelect = false;
            this.PieceList.Name = "PieceList";
            this.PieceList.ShowGroups = false;
            this.PieceList.Size = new System.Drawing.Size(152, 512);
            this.PieceList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.PieceList.TabIndex = 14;
            this.PieceList.UseCompatibleStateImageBehavior = false;
            this.PieceList.View = System.Windows.Forms.View.Details;
            this.PieceList.ItemMouseHover += new System.Windows.Forms.ListViewItemMouseHoverEventHandler(this.PieceList_ItemMouseHover);
            this.PieceList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.PieceList_ItemSelectionChanged);
            // 
            // colHeader
            // 
            this.colHeader.Text = "Count:";
            this.colHeader.Width = 128;
            // 
            // board1
            // 
            this.board1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.board1.HidePiecesWithZeroSurround = false;
            this.board1.Location = new System.Drawing.Point(3, 2);
            this.board1.Name = "board1";
            this.board1.PossiblePieces = null;
            this.board1.ShowClues = false;
            this.board1.ShowPieceText = false;
            this.board1.Size = new System.Drawing.Size(512, 512);
            this.board1.TabIndex = 0;
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 541);
            this.Controls.Add(this.PieceList);
            this.Controls.Add(this.lblPiecesPlaced);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.board1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "E2Play";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainViewClosing);
            this.Load += new System.EventHandler(this.MainLoad);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Board board1;
        private System.Windows.Forms.CheckBox chkShowClues;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.CheckBox chkPieceText;
        private System.Windows.Forms.OpenFileDialog OpenE2PFile;
        private System.Windows.Forms.SaveFileDialog SaveE2PFile;
        private System.Windows.Forms.CheckBox chkHideUselessPieces;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPiecesPlaced;
        private System.Windows.Forms.ListView PieceList;
        private System.Windows.Forms.ColumnHeader colHeader;
    }
}

