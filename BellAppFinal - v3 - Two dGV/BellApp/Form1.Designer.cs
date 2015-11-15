namespace BellApp
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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.dGVOutputMC = new System.Windows.Forms.DataGridView();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnMissingChannels = new System.Windows.Forms.Button();
            this.btnRecordedMovies = new System.Windows.Forms.Button();
            this.btnViewFResultsList = new System.Windows.Forms.Button();
            this.tBPrevDay = new System.Windows.Forms.TextBox();
            this.btnUpdateList = new System.Windows.Forms.Button();
            this.Progress = new System.Windows.Forms.TextBox();
            this.dGVOutput = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSearch = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbSearchOther = new System.Windows.Forms.TextBox();
            this.tbSearchPhrase = new System.Windows.Forms.TextBox();
            this.tbSearchExact = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGVOutputMC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dGVOutput)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(633, 662);
            this.tabControl1.TabIndex = 18;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.dGVOutputMC);
            this.tabPage2.Controls.Add(this.btnMissingChannels);
            this.tabPage2.Controls.Add(this.btnRecordedMovies);
            this.tabPage2.Controls.Add(this.btnViewFResultsList);
            this.tabPage2.Controls.Add(this.tBPrevDay);
            this.tabPage2.Controls.Add(this.btnUpdateList);
            this.tabPage2.Controls.Add(this.Progress);
            this.tabPage2.Controls.Add(this.dGVOutput);
            this.tabPage2.Controls.Add(this.btnSearch);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(625, 636);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Results";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(222, 409);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(208, 20);
            this.label3.TabIndex = 27;
            this.label3.Text = "Movies on Missing Channels";
            // 
            // dGVOutputMC
            // 
            this.dGVOutputMC.AllowUserToAddRows = false;
            this.dGVOutputMC.AllowUserToDeleteRows = false;
            this.dGVOutputMC.AllowUserToResizeColumns = false;
            this.dGVOutputMC.AllowUserToResizeRows = false;
            this.dGVOutputMC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGVOutputMC.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10});
            this.dGVOutputMC.Location = new System.Drawing.Point(8, 432);
            this.dGVOutputMC.Name = "dGVOutputMC";
            this.dGVOutputMC.ReadOnly = true;
            this.dGVOutputMC.RowHeadersVisible = false;
            this.dGVOutputMC.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dGVOutputMC.Size = new System.Drawing.Size(608, 192);
            this.dGVOutputMC.TabIndex = 26;
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column6.HeaderText = "Name";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Category";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Date";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Time";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "Channel";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            // 
            // btnMissingChannels
            // 
            this.btnMissingChannels.Location = new System.Drawing.Point(149, 34);
            this.btnMissingChannels.Name = "btnMissingChannels";
            this.btnMissingChannels.Size = new System.Drawing.Size(146, 20);
            this.btnMissingChannels.TabIndex = 25;
            this.btnMissingChannels.Text = "View Missing Channels List";
            this.btnMissingChannels.UseVisualStyleBackColor = true;
            this.btnMissingChannels.Click += new System.EventHandler(this.btnMissingChannels_Click);
            // 
            // btnRecordedMovies
            // 
            this.btnRecordedMovies.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRecordedMovies.Location = new System.Drawing.Point(316, 7);
            this.btnRecordedMovies.Name = "btnRecordedMovies";
            this.btnRecordedMovies.Size = new System.Drawing.Size(114, 46);
            this.btnRecordedMovies.TabIndex = 24;
            this.btnRecordedMovies.Text = "View Recorded Movies";
            this.btnRecordedMovies.UseVisualStyleBackColor = true;
            this.btnRecordedMovies.Click += new System.EventHandler(this.btnRecordedMovies_Click);
            // 
            // btnViewFResultsList
            // 
            this.btnViewFResultsList.Location = new System.Drawing.Point(149, 8);
            this.btnViewFResultsList.Name = "btnViewFResultsList";
            this.btnViewFResultsList.Size = new System.Drawing.Size(146, 20);
            this.btnViewFResultsList.TabIndex = 23;
            this.btnViewFResultsList.Text = "View False Results List";
            this.btnViewFResultsList.UseVisualStyleBackColor = true;
            this.btnViewFResultsList.Click += new System.EventHandler(this.btnViewFResultsList_Click);
            // 
            // tBPrevDay
            // 
            this.tBPrevDay.Location = new System.Drawing.Point(8, 8);
            this.tBPrevDay.Name = "tBPrevDay";
            this.tBPrevDay.Size = new System.Drawing.Size(116, 20);
            this.tBPrevDay.TabIndex = 22;
            // 
            // btnUpdateList
            // 
            this.btnUpdateList.Location = new System.Drawing.Point(31, 32);
            this.btnUpdateList.Name = "btnUpdateList";
            this.btnUpdateList.Size = new System.Drawing.Size(69, 21);
            this.btnUpdateList.TabIndex = 21;
            this.btnUpdateList.Text = "Update List";
            this.btnUpdateList.UseVisualStyleBackColor = true;
            this.btnUpdateList.Click += new System.EventHandler(this.btnUpdateList_Click);
            // 
            // Progress
            // 
            this.Progress.Location = new System.Drawing.Point(543, 8);
            this.Progress.Multiline = true;
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(73, 23);
            this.Progress.TabIndex = 20;
            this.Progress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dGVOutput
            // 
            this.dGVOutput.AllowUserToAddRows = false;
            this.dGVOutput.AllowUserToDeleteRows = false;
            this.dGVOutput.AllowUserToResizeColumns = false;
            this.dGVOutput.AllowUserToResizeRows = false;
            this.dGVOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGVOutput.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            this.dGVOutput.Location = new System.Drawing.Point(8, 59);
            this.dGVOutput.Name = "dGVOutput";
            this.dGVOutput.ReadOnly = true;
            this.dGVOutput.RowHeadersVisible = false;
            this.dGVOutput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dGVOutput.Size = new System.Drawing.Size(608, 347);
            this.dGVOutput.TabIndex = 19;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.HeaderText = "Name";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Category";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Date";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Time";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Channel";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(462, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 18;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.tbSearchOther);
            this.tabPage1.Controls.Add(this.tbSearchPhrase);
            this.tabPage1.Controls.Add(this.tbSearchExact);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(625, 636);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Movie Lists";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(421, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Other";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(221, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Title Phrase Match";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Title Exact Match";
            // 
            // tbSearchOther
            // 
            this.tbSearchOther.Location = new System.Drawing.Point(355, 25);
            this.tbSearchOther.Multiline = true;
            this.tbSearchOther.Name = "tbSearchOther";
            this.tbSearchOther.Size = new System.Drawing.Size(170, 547);
            this.tbSearchOther.TabIndex = 13;
            // 
            // tbSearchPhrase
            // 
            this.tbSearchPhrase.Location = new System.Drawing.Point(179, 25);
            this.tbSearchPhrase.Multiline = true;
            this.tbSearchPhrase.Name = "tbSearchPhrase";
            this.tbSearchPhrase.Size = new System.Drawing.Size(170, 547);
            this.tbSearchPhrase.TabIndex = 12;
            // 
            // tbSearchExact
            // 
            this.tbSearchExact.Location = new System.Drawing.Point(3, 25);
            this.tbSearchExact.Multiline = true;
            this.tbSearchExact.Name = "tbSearchExact";
            this.tbSearchExact.Size = new System.Drawing.Size(170, 547);
            this.tbSearchExact.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 658);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Bell Sample App";
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGVOutputMC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dGVOutput)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSearchOther;
        private System.Windows.Forms.TextBox tbSearchPhrase;
        private System.Windows.Forms.TextBox tbSearchExact;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnRecordedMovies;
        private System.Windows.Forms.Button btnViewFResultsList;
        private System.Windows.Forms.TextBox tBPrevDay;
        private System.Windows.Forms.Button btnUpdateList;
        private System.Windows.Forms.TextBox Progress;
        private System.Windows.Forms.DataGridView dGVOutput;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnMissingChannels;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dGVOutputMC;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;

    }
}

