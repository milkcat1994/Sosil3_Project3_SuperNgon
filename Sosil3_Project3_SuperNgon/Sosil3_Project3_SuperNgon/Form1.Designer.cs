namespace Sosil3_Project3_SuperNgon
{
    partial class Form_Main
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label_Title_1 = new System.Windows.Forms.Label();
            this.label_Title_2 = new System.Windows.Forms.Label();
            this.label_Notice_Start = new System.Windows.Forms.Label();
            this.label_Time = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.timer4 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label_Title_1
            // 
            this.label_Title_1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label_Title_1.Font = new System.Drawing.Font("굴림", 40F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(200)));
            this.label_Title_1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label_Title_1.Location = new System.Drawing.Point(93, 97);
            this.label_Title_1.Name = "label_Title_1";
            this.label_Title_1.Size = new System.Drawing.Size(220, 58);
            this.label_Title_1.TabIndex = 0;
            this.label_Title_1.Text = "SUPER";
            // 
            // label_Title_2
            // 
            this.label_Title_2.Font = new System.Drawing.Font("굴림", 40F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(200)));
            this.label_Title_2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label_Title_2.Location = new System.Drawing.Point(164, 155);
            this.label_Title_2.Name = "label_Title_2";
            this.label_Title_2.Size = new System.Drawing.Size(265, 61);
            this.label_Title_2.TabIndex = 1;
            this.label_Title_2.Text = "5-GON";
            // 
            // label_Notice_Start
            // 
            this.label_Notice_Start.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(130)));
            this.label_Notice_Start.Location = new System.Drawing.Point(142, 482);
            this.label_Notice_Start.Name = "label_Notice_Start";
            this.label_Notice_Start.Size = new System.Drawing.Size(287, 23);
            this.label_Notice_Start.TabIndex = 2;
            this.label_Notice_Start.Text = "PRESS SPACE TO START";
            // 
            // label_Time
            // 
            this.label_Time.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(130)));
            this.label_Time.Location = new System.Drawing.Point(445, 9);
            this.label_Time.Name = "label_Time";
            this.label_Time.Size = new System.Drawing.Size(136, 23);
            this.label_Time.TabIndex = 3;
            this.label_Time.Text = "TIME : 0";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 2000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Interval = 50;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // timer4
            // 
            this.timer4.Interval = 1000;
            this.timer4.Tick += new System.EventHandler(this.timer4_Tick);
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(593, 532);
            this.Controls.Add(this.label_Time);
            this.Controls.Add(this.label_Notice_Start);
            this.Controls.Add(this.label_Title_2);
            this.Controls.Add(this.label_Title_1);
            this.DoubleBuffered = true;
            this.Name = "Form_Main";
            this.Text = "Super NGon";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form_Main_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_Main_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_Title_1;
        private System.Windows.Forms.Label label_Title_2;
        private System.Windows.Forms.Label label_Notice_Start;
        private System.Windows.Forms.Label label_Time;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Timer timer4;
    }
}

