
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
            this.PieceCount = new System.Windows.Forms.Label();
            this.chkShowClues = new System.Windows.Forms.CheckBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.chkPieceText = new System.Windows.Forms.CheckBox();
            this.OpenE2PFile = new System.Windows.Forms.OpenFileDialog();
            this.SaveE2PFile = new System.Windows.Forms.SaveFileDialog();
            this.chkHideUselessPieces = new System.Windows.Forms.CheckBox();
            this.PieceList = new E2Play.PieceSelectorListBox();
            this.board1 = new E2Play.Board();
            this.SuspendLayout();
            // 
            // PieceCount
            // 
            this.PieceCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PieceCount.Location = new System.Drawing.Point(796, 765);
            this.PieceCount.Name = "PieceCount";
            this.PieceCount.Size = new System.Drawing.Size(70, 21);
            this.PieceCount.TabIndex = 4;
            // 
            // chkShowClues
            // 
            this.chkShowClues.AutoSize = true;
            this.chkShowClues.Location = new System.Drawing.Point(12, 780);
            this.chkShowClues.Name = "chkShowClues";
            this.chkShowClues.Size = new System.Drawing.Size(82, 17);
            this.chkShowClues.TabIndex = 5;
            this.chkShowClues.Text = "Show Clues";
            this.chkShowClues.UseVisualStyleBackColor = true;
            this.chkShowClues.CheckedChanged += new System.EventHandler(this.chkShowClues_CheckedChanged);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(534, 776);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 6;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(615, 776);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(696, 775);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 8;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // chkPieceText
            // 
            this.chkPieceText.AutoSize = true;
            this.chkPieceText.Location = new System.Drawing.Point(100, 780);
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
            this.OpenE2PFile.Filter = "E2P|*.e2p";
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
            this.chkHideUselessPieces.AutoSize = true;
            this.chkHideUselessPieces.Location = new System.Drawing.Point(183, 779);
            this.chkHideUselessPieces.Name = "chkHideUselessPieces";
            this.chkHideUselessPieces.Size = new System.Drawing.Size(123, 17);
            this.chkHideUselessPieces.TabIndex = 10;
            this.chkHideUselessPieces.Text = "Hide Useless Pieces";
            this.chkHideUselessPieces.UseVisualStyleBackColor = true;
            this.chkHideUselessPieces.CheckedChanged += new System.EventHandler(this.chkHideUselessPieces_CheckedChanged);
            // 
            // PieceList
            // 
            this.PieceList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.PieceList.FormattingEnabled = true;
            this.PieceList.ItemHeight = 50;
            this.PieceList.Location = new System.Drawing.Point(777, 8);
            this.PieceList.Name = "PieceList";
            this.PieceList.Size = new System.Drawing.Size(113, 754);
            this.PieceList.TabIndex = 3;
            this.PieceList.SelectedIndexChanged += new System.EventHandler(this.PieceListSelected);
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
            this.board1.Size = new System.Drawing.Size(768, 768);
            this.board1.TabIndex = 0;
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(902, 810);
            this.Controls.Add(this.chkHideUselessPieces);
            this.Controls.Add(this.chkPieceText);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.chkShowClues);
            this.Controls.Add(this.PieceCount);
            this.Controls.Add(this.PieceList);
            this.Controls.Add(this.board1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "E2Play";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainViewClosing);
            this.Load += new System.EventHandler(this.MainLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Board board1;
        private PieceSelectorListBox PieceList;
        private System.Windows.Forms.Label PieceCount;
        private System.Windows.Forms.CheckBox chkShowClues;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.CheckBox chkPieceText;
        private System.Windows.Forms.OpenFileDialog OpenE2PFile;
        private System.Windows.Forms.SaveFileDialog SaveE2PFile;
        private System.Windows.Forms.CheckBox chkHideUselessPieces;
    }
}

