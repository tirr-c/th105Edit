namespace cvn_helper
{
    partial class frmCvnEditor
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.myMenu = new System.Windows.Forms.MenuStrip();
            this.MenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuImport = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuExtract = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSeparate2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuEncodings = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuJapanese = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuKorean = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuChangeEncoding = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSaveAsKorean = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSaveAsJapanese = new System.Windows.Forms.ToolStripMenuItem();
            this.cv0Data = new System.Windows.Forms.TextBox();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.cv1List = new System.Windows.Forms.ListView();
            this.cv2Image = new System.Windows.Forms.PictureBox();
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.myMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cv2Image)).BeginInit();
            this.SuspendLayout();
            // 
            // myMenu
            // 
            this.myMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFile,
            this.MenuEncodings});
            this.myMenu.Location = new System.Drawing.Point(0, 0);
            this.myMenu.Name = "myMenu";
            this.myMenu.Size = new System.Drawing.Size(393, 24);
            this.myMenu.TabIndex = 0;
            this.myMenu.Text = "menuStrip1";
            // 
            // MenuFile
            // 
            this.MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuImport,
            this.MenuExtract,
            this.MenuSeparate2,
            this.MenuExit});
            this.MenuFile.Name = "MenuFile";
            this.MenuFile.Size = new System.Drawing.Size(57, 20);
            this.MenuFile.Text = "파일(&F)";
            // 
            // MenuImport
            // 
            this.MenuImport.Enabled = false;
            this.MenuImport.Name = "MenuImport";
            this.MenuImport.Size = new System.Drawing.Size(183, 22);
            this.MenuImport.Text = "파일에서 불러오기...";
            // 
            // MenuExtract
            // 
            this.MenuExtract.Enabled = false;
            this.MenuExtract.Name = "MenuExtract";
            this.MenuExtract.Size = new System.Drawing.Size(183, 22);
            this.MenuExtract.Text = "추출...";
            this.MenuExtract.Click += new System.EventHandler(this.MenuExtract_Click);
            // 
            // MenuSeparate2
            // 
            this.MenuSeparate2.Name = "MenuSeparate2";
            this.MenuSeparate2.Size = new System.Drawing.Size(180, 6);
            // 
            // MenuExit
            // 
            this.MenuExit.Name = "MenuExit";
            this.MenuExit.Size = new System.Drawing.Size(183, 22);
            this.MenuExit.Text = "수정 마치기";
            this.MenuExit.Click += new System.EventHandler(this.MenuExit_Click);
            // 
            // MenuEncodings
            // 
            this.MenuEncodings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuJapanese,
            this.MenuKorean,
            this.MenuChangeEncoding});
            this.MenuEncodings.Enabled = false;
            this.MenuEncodings.Name = "MenuEncodings";
            this.MenuEncodings.Size = new System.Drawing.Size(55, 20);
            this.MenuEncodings.Text = "인코딩";
            this.MenuEncodings.Visible = false;
            // 
            // MenuJapanese
            // 
            this.MenuJapanese.CheckOnClick = true;
            this.MenuJapanese.Name = "MenuJapanese";
            this.MenuJapanese.Size = new System.Drawing.Size(162, 22);
            this.MenuJapanese.Text = "Shift-JIS(일본어)";
            this.MenuJapanese.CheckedChanged += new System.EventHandler(this.MenuJapanese_CheckedChanged);
            // 
            // MenuKorean
            // 
            this.MenuKorean.CheckOnClick = true;
            this.MenuKorean.Name = "MenuKorean";
            this.MenuKorean.Size = new System.Drawing.Size(162, 22);
            this.MenuKorean.Text = "EUC-KR(한국어)";
            this.MenuKorean.CheckedChanged += new System.EventHandler(this.MenuKorean_CheckedChanged);
            // 
            // MenuChangeEncoding
            // 
            this.MenuChangeEncoding.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuSaveAsKorean,
            this.MenuSaveAsJapanese});
            this.MenuChangeEncoding.Name = "MenuChangeEncoding";
            this.MenuChangeEncoding.Size = new System.Drawing.Size(162, 22);
            this.MenuChangeEncoding.Text = "인코딩 변환";
            // 
            // MenuSaveAsKorean
            // 
            this.MenuSaveAsKorean.Name = "MenuSaveAsKorean";
            this.MenuSaveAsKorean.Size = new System.Drawing.Size(162, 22);
            this.MenuSaveAsKorean.Text = "한국어(EUC-KR)";
            this.MenuSaveAsKorean.Click += new System.EventHandler(this.MenuSaveAsKorean_Click);
            // 
            // MenuSaveAsJapanese
            // 
            this.MenuSaveAsJapanese.Name = "MenuSaveAsJapanese";
            this.MenuSaveAsJapanese.Size = new System.Drawing.Size(162, 22);
            this.MenuSaveAsJapanese.Text = "일본어(Shift-JIS)";
            this.MenuSaveAsJapanese.Click += new System.EventHandler(this.MenuSaveAsJapanese_Click);
            // 
            // cv0Data
            // 
            this.cv0Data.AcceptsReturn = true;
            this.cv0Data.Enabled = false;
            this.cv0Data.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cv0Data.Location = new System.Drawing.Point(0, 24);
            this.cv0Data.Multiline = true;
            this.cv0Data.Name = "cv0Data";
            this.cv0Data.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.cv0Data.Size = new System.Drawing.Size(61, 54);
            this.cv0Data.TabIndex = 1;
            this.cv0Data.Visible = false;
            this.cv0Data.TextChanged += new System.EventHandler(this.cv0Data_TextChanged);
            // 
            // cv1List
            // 
            this.cv1List.Enabled = false;
            this.cv1List.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cv1List.Location = new System.Drawing.Point(67, 27);
            this.cv1List.Name = "cv1List";
            this.cv1List.Size = new System.Drawing.Size(63, 67);
            this.cv1List.TabIndex = 2;
            this.cv1List.UseCompatibleStateImageBehavior = false;
            this.cv1List.View = System.Windows.Forms.View.Details;
            this.cv1List.Visible = false;
            this.cv1List.DoubleClick += new System.EventHandler(this.cv1List_DoubleClick);
            // 
            // cv2Image
            // 
            this.cv2Image.BackColor = System.Drawing.Color.Transparent;
            this.cv2Image.Enabled = false;
            this.cv2Image.Location = new System.Drawing.Point(136, 28);
            this.cv2Image.Name = "cv2Image";
            this.cv2Image.Size = new System.Drawing.Size(57, 50);
            this.cv2Image.TabIndex = 3;
            this.cv2Image.TabStop = false;
            this.cv2Image.Visible = false;
            // 
            // frmCvnEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(393, 284);
            this.Controls.Add(this.cv2Image);
            this.Controls.Add(this.cv1List);
            this.Controls.Add(this.cv0Data);
            this.Controls.Add(this.myMenu);
            this.MainMenuStrip = this.myMenu;
            this.Name = "frmCvnEditor";
            this.Text = "cvn 편집기";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCvnEditor_FormClosing);
            this.myMenu.ResumeLayout(false);
            this.myMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cv2Image)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip myMenu;
        private System.Windows.Forms.ToolStripMenuItem MenuFile;
        private System.Windows.Forms.ToolStripMenuItem MenuExit;
        private System.Windows.Forms.TextBox cv0Data;
        private System.Windows.Forms.ToolStripMenuItem MenuEncodings;
        private System.Windows.Forms.ToolStripMenuItem MenuJapanese;
        private System.Windows.Forms.ToolStripMenuItem MenuKorean;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private System.Windows.Forms.ListView cv1List;
        private System.Windows.Forms.PictureBox cv2Image;
        private System.Windows.Forms.ToolStripMenuItem MenuImport;
        private System.Windows.Forms.ToolStripMenuItem MenuExtract;
        private System.Windows.Forms.ToolStripSeparator MenuSeparate2;
        private System.Windows.Forms.SaveFileDialog dlgSave;
        private System.Windows.Forms.ToolStripMenuItem MenuChangeEncoding;
        private System.Windows.Forms.ToolStripMenuItem MenuSaveAsKorean;
        private System.Windows.Forms.ToolStripMenuItem MenuSaveAsJapanese;
    }
}

