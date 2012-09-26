namespace cvn_helper
{
    partial class frmMain
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.MenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSeperate1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuEncodings = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuJapanese = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuKorean = new System.Windows.Forms.ToolStripMenuItem();
            this.cv0Data = new System.Windows.Forms.TextBox();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.cv1List = new System.Windows.Forms.ListView();
            this.cv2Image = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cv2Image)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFile,
            this.MenuEncodings});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(393, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // MenuFile
            // 
            this.MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuOpen,
            this.MenuSave,
            this.MenuSaveAs,
            this.MenuSeperate1,
            this.MenuExit});
            this.MenuFile.Name = "MenuFile";
            this.MenuFile.Size = new System.Drawing.Size(57, 20);
            this.MenuFile.Text = "파일(&F)";
            // 
            // MenuOpen
            // 
            this.MenuOpen.Name = "MenuOpen";
            this.MenuOpen.Size = new System.Drawing.Size(203, 22);
            this.MenuOpen.Text = "열기...(&O)";
            this.MenuOpen.Click += new System.EventHandler(this.MenuOpen_Click);
            // 
            // MenuSave
            // 
            this.MenuSave.Enabled = false;
            this.MenuSave.Name = "MenuSave";
            this.MenuSave.Size = new System.Drawing.Size(203, 22);
            this.MenuSave.Text = "저장(&S)";
            this.MenuSave.Click += new System.EventHandler(this.MenuSave_Click);
            // 
            // MenuSaveAs
            // 
            this.MenuSaveAs.Enabled = false;
            this.MenuSaveAs.Name = "MenuSaveAs";
            this.MenuSaveAs.Size = new System.Drawing.Size(203, 22);
            this.MenuSaveAs.Text = "다른 이름으로 저장...(&A)";
            // 
            // MenuSeperate1
            // 
            this.MenuSeperate1.Name = "MenuSeperate1";
            this.MenuSeperate1.Size = new System.Drawing.Size(200, 6);
            // 
            // MenuExit
            // 
            this.MenuExit.Name = "MenuExit";
            this.MenuExit.Size = new System.Drawing.Size(203, 22);
            this.MenuExit.Text = "종료(&X)";
            this.MenuExit.Click += new System.EventHandler(this.MenuExit_Click);
            // 
            // MenuEncodings
            // 
            this.MenuEncodings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuJapanese,
            this.MenuKorean});
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
            // 
            // dlgOpen
            // 
            this.dlgOpen.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgOpen_FileOk);
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
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(393, 284);
            this.Controls.Add(this.cv2Image);
            this.Controls.Add(this.cv1List);
            this.Controls.Add(this.cv0Data);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "cvn 편집기";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cv2Image)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuFile;
        private System.Windows.Forms.ToolStripMenuItem MenuOpen;
        private System.Windows.Forms.ToolStripMenuItem MenuSave;
        private System.Windows.Forms.ToolStripMenuItem MenuSaveAs;
        private System.Windows.Forms.ToolStripSeparator MenuSeperate1;
        private System.Windows.Forms.ToolStripMenuItem MenuExit;
        private System.Windows.Forms.TextBox cv0Data;
        private System.Windows.Forms.ToolStripMenuItem MenuEncodings;
        private System.Windows.Forms.ToolStripMenuItem MenuJapanese;
        private System.Windows.Forms.ToolStripMenuItem MenuKorean;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private System.Windows.Forms.ListView cv1List;
        private System.Windows.Forms.PictureBox cv2Image;
    }
}

