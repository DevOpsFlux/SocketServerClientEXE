namespace TCPIPSocketServer
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtServerPort = new System.Windows.Forms.TextBox();
            this.btnServerStart = new System.Windows.Forms.Button();
            this.btnServerEnd = new System.Windows.Forms.Button();
            this.btnServerSend = new System.Windows.Forms.Button();
            this.txtSendMsg = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lstMsg = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblCnt = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "포트번호 : ";
            // 
            // txtServerPort
            // 
            this.txtServerPort.Location = new System.Drawing.Point(82, 25);
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.Size = new System.Drawing.Size(100, 21);
            this.txtServerPort.TabIndex = 1;
            // 
            // btnServerStart
            // 
            this.btnServerStart.Location = new System.Drawing.Point(336, 25);
            this.btnServerStart.Name = "btnServerStart";
            this.btnServerStart.Size = new System.Drawing.Size(75, 23);
            this.btnServerStart.TabIndex = 2;
            this.btnServerStart.Text = "서버 시작";
            this.btnServerStart.UseVisualStyleBackColor = true;
            this.btnServerStart.Click += new System.EventHandler(this.btnServerStart_Click);
            // 
            // btnServerEnd
            // 
            this.btnServerEnd.Location = new System.Drawing.Point(336, 60);
            this.btnServerEnd.Name = "btnServerEnd";
            this.btnServerEnd.Size = new System.Drawing.Size(75, 23);
            this.btnServerEnd.TabIndex = 3;
            this.btnServerEnd.Text = "서버 종료";
            this.btnServerEnd.UseVisualStyleBackColor = true;
            this.btnServerEnd.Click += new System.EventHandler(this.btnServerEnd_Click);
            // 
            // btnServerSend
            // 
            this.btnServerSend.Location = new System.Drawing.Point(336, 111);
            this.btnServerSend.Name = "btnServerSend";
            this.btnServerSend.Size = new System.Drawing.Size(75, 23);
            this.btnServerSend.TabIndex = 4;
            this.btnServerSend.Text = "전 송";
            this.btnServerSend.UseVisualStyleBackColor = true;
            this.btnServerSend.Click += new System.EventHandler(this.btnServerSend_Click);
            // 
            // txtSendMsg
            // 
            this.txtSendMsg.Location = new System.Drawing.Point(96, 111);
            this.txtSendMsg.Name = "txtSendMsg";
            this.txtSendMsg.Size = new System.Drawing.Size(234, 21);
            this.txtSendMsg.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "전송메세지 : ";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(336, 140);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "리 셋";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lstMsg
            // 
            this.lstMsg.FormattingEnabled = true;
            this.lstMsg.ItemHeight = 12;
            this.lstMsg.Location = new System.Drawing.Point(15, 178);
            this.lstMsg.Name = "lstMsg";
            this.lstMsg.Size = new System.Drawing.Size(396, 256);
            this.lstMsg.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "클라이언트 연결 수 : ";
            // 
            // lblCnt
            // 
            this.lblCnt.AutoSize = true;
            this.lblCnt.Location = new System.Drawing.Point(139, 65);
            this.lblCnt.Name = "lblCnt";
            this.lblCnt.Size = new System.Drawing.Size(11, 12);
            this.lblCnt.TabIndex = 11;
            this.lblCnt.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 441);
            this.Controls.Add(this.lblCnt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstMsg);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtSendMsg);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnServerSend);
            this.Controls.Add(this.btnServerEnd);
            this.Controls.Add(this.btnServerStart);
            this.Controls.Add(this.txtServerPort);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Dev - Socket Server";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtServerPort;
        private System.Windows.Forms.Button btnServerStart;
        private System.Windows.Forms.Button btnServerEnd;
        private System.Windows.Forms.Button btnServerSend;
        private System.Windows.Forms.TextBox txtSendMsg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox lstMsg;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblCnt;
    }
}

