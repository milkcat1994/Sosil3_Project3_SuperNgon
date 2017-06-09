using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Sosil3_Project3_SuperNgon
{
    public partial class Form_Main : Form
    {
        //using draw N_Gon
        private int n_number = 5;
        private int width, height;
        private const float FI = (float)3.14;
        private const int little_radius_1 = 30;
        private const int little_radius_2 = 10;
        //to draw center Polygon, save Point List
        private List<Point> Polygon_Point_List = new List<Point>();
        //to draw background color
        private SolidBrush[] background_Brush = new SolidBrush[3];
        //to draw center Polygon
        private SolidBrush center_Brush = null;
        //to draw pen
        private Pen pen = null;
        //game_type
        private game_Type g_Type = 0;
        //game_time
        private int game_Time = 0;
        //player point, 기울기 to draw
        //player 중점
        private Point pc;
        //player 좌표
        private PointF[] p_Point;
        //방향키에 따른 player 각도
        private int p_Move_Degree;

        private int change_Angle = 2;
        private int start_Angle = 0;
        private Random rand_Num;
        private DateTime dt_Time;

        enum game_Type
        {
            PRE_GAME,
            IN_GAME,
            END_GAME
        };
        
        //Form생성시 실행
        public Form_Main()
        {
            InitializeComponent();
            p_Move_Degree = n_number-1;
            //default setting n_number
            width = ClientSize.Width;
            height = ClientSize.Height;

            dt_Time = DateTime.Now;
            rand_Num = new Random(dt_Time.Millisecond);

            save_information(0);
            save_pPoint(0, p_Move_Degree);

            background_Brush[0] = new SolidBrush(Color.GreenYellow);
            background_Brush[1] = new SolidBrush(Color.LawnGreen);
            background_Brush[2] = new SolidBrush(Color.LightGreen);
            g_Type = game_Type.PRE_GAME;

            //label배경색 투명하게 설정
            this.label_Title_1.BackColor = Color.Transparent;
            this.label_Title_1.Parent = this;
            this.label_Title_2.BackColor = Color.Transparent;
            this.label_Title_2.Parent = this;
            this.label_Notice_Start.BackColor = Color.Transparent;
            this.label_Notice_Start.Parent = this;
            this.label_Time.BackColor = Color.Transparent;
            this.label_Time.Parent = this;

            //timer stop
            this.timer1.Stop();
            this.timer2.Stop();
            this.timer3.Stop();
        }

        private void save_pPoint(int start_angle, int p_Move_Degree)
        {

            p_Point = new PointF[3];

            //save center x,y
            int cx = (int)(width / 2.0);
            int cy = (int)(height / 2.0);
            //float pc_degree = (float)360.0 - ((float)180.0 / (float)n_number) + start_angle + p_Move_Degree * ((float)360.0 / (float)n_number);
            float pc_degree = ((float)180.0 / (float)n_number);

            float pc_radian = ((float)Math.PI * pc_degree / (float)(180.0));

            /*
            rx = ((px - rotx) * cos(rad) - (py - roty) * sin(rad)) + rotx;
            ry = ((px - rotx) * sin(rad) + (py - roty) * cos(rad)) + roty;

            px , py => 원래 좌표
            rotx, roty => 회전 중심점
            */
            int ex = Polygon_Point_List[p_Move_Degree].X;
            int ey = Polygon_Point_List[p_Move_Degree].Y;
            pc = new Point((int)((ex-cx) * Math.Cos(pc_radian)) - (int)((ey-cy)* Math.Sin(pc_radian)) + cx,
                       (int)((ex - cx) * Math.Sin(pc_radian)) - (int)((ey - cy) * Math.Cos(pc_radian)) + cx);

            //Calculate Radian using degree(n_number)
            int p_degree =360 / 3;
            //p_point index
            int i = 0;
            //save new Point => need add Point arrary
            start_angle += (120 - (int)(180.0 / n_number));
            for (int angle = start_angle ; angle < start_angle + 360; angle += p_degree)
            {
                float radian = ((float)Math.PI * angle / (float)(180.0));

                //save Next Point x,y -> using ex Point x,y
                p_Point[i++] = new Point((int)(pc.X + little_radius_2 * Math.Cos(radian)),
                    (int)(pc.Y + little_radius_2 * (float)Math.Sin(radian)));
            }
        }

        //키 입력에 따른 다각형의 Point 저장
        private void save_information(float start_angle)
        {
            if (Polygon_Point_List != null)
                Polygon_Point_List.Clear();

            //save center x,y
            int cx = (int)(width / 2.0);
            int cy = (int)(height / 2.0);

            //save default Point
            Point ep;

            //Calculate Radian using degree(n_number)
            float degree = (float)360.0 / (float)n_number;

            //save new Point => need add Point arrary
            for (float angle = start_angle; angle < (float)start_angle+ 360.0; angle+=degree)
            {
                float radian = ((float)Math.PI * angle / (float)(180.0));

                //save Next Point x,y -> using ex Point x,y
                ep = new Point((int)(cx + little_radius_1 * Math.Cos(radian)),
                    (int)(cy + little_radius_1 * (float)Math.Sin(radian)));

                //save Point List
                Polygon_Point_List.Add(ep);
            }
        }
        
        //Paint 이벤트 발생시 발생하는 행위
        private void Form_Main_Paint(object sender, PaintEventArgs e)
        {
            float cx = width / (float)2.0;
            float cy = height / (float)2.0;

            //Calculate Radian using degree(n_number)
            float degree = (float)360.0 / (float)n_number;
            float radian = (degree * FI / (float)(180.0));

            //인게임과 인게임 이전에서의 배경화면 도식.
            if (g_Type == game_Type.PRE_GAME || g_Type == game_Type.IN_GAME)
            {
                //3.2_score : 2.5
                Graphics g = e.Graphics;
                //배경 화면 도식
                center_Brush = new SolidBrush(Color.White);

                for (int i = 0; i < n_number; i++)
                {
                    //구간이 3+1일 경우
                    if (n_number % 3 == 1)
                    {
                        if (i == n_number - 1)
                        {
                            g.FillPie(background_Brush[1], -200, -200, width + 400, height + 400, start_Angle + degree * i, degree);
                            break;
                        }
                    }
                    g.FillPie(background_Brush[i % 3], -200, -200, width + 400, height + 400, start_Angle + degree * i, degree);
                }
                //3.2_score : 2.5
                //Center에 있는 작은 다각형 그리는 함수
                g.FillPolygon(center_Brush, Polygon_Point_List.ToArray());

                //Player 삼각형 그리기
                pen = new Pen(Color.Black);
                g.DrawPolygon(pen, p_Point);
            }
        }

        //3.2_score : 2.5
        //4.2_score : 0.5
        //1초를 간격으로 time 라벨 값 변경
        private void timer1_Tick(object sender, EventArgs e)
        {
            game_Time += 1;
            label_Time.Text = "TIME : " + game_Time.ToString();
        }

        //4.1_score : 1
        //화면 회전 구간 설정
        private void timer2_Tick(object sender, EventArgs e)
        {
            //각 도형 방향 전환
            change_Angle *= -1;
            
            //timer2 시간 재설정
            timer2.Interval = rand_Num.Next(500,2000);
        }

        //4.1_score : 1
        //전체 화면 재도식 Paint 함수 호출
        private void timer3_Tick(object sender, EventArgs e)
        {
            start_Angle += change_Angle;
            save_information(start_Angle);
            save_pPoint(start_Angle, p_Move_Degree);
            this.Invalidate();
            this.Update();
            this.Refresh();
        }

        private void Form_Main_KeyDown(object sender, KeyEventArgs e)
        {
            //인게임 이전에서의 Keyboard stork 인식
            //3.2_score : 1
            if (g_Type == game_Type.PRE_GAME)
            {
                if ((e.KeyCode == Keys.Down) && (n_number > 3))
                {
                    n_number--;
                    p_Move_Degree = n_number-1;
                    label_Title_2.Text = n_number.ToString() + "-GON";
                    save_information(start_Angle);
                    save_pPoint(start_Angle, p_Move_Degree);
                    this.Invalidate();
                    this.Update();
                    this.Refresh();
                }
                else if ((e.KeyCode == Keys.Up) && (n_number <= 21))
                {
                    n_number++;
                    p_Move_Degree = n_number-1;
                    label_Title_2.Text = n_number.ToString() + "-GON";
                    save_information(start_Angle);
                    save_pPoint(start_Angle, p_Move_Degree);
                    this.Invalidate();
                    this.Update();
                    this.Refresh();
                }
                else if (e.KeyCode == Keys.Space)
                {
                    //게임 타입 변경
                    g_Type = game_Type.IN_GAME;
                    //start timer
                    timer1.Start();
                    timer2.Start();
                    timer3.Start();
                    label_Title_1.Visible = false;
                    label_Title_2.Visible = false;
                    label_Notice_Start.Visible = false;
                    this.Invalidate();
                    this.Update();
                    this.Refresh();
                }
            }
            //인게임 에서의 Keyboard stork 인식
            //4.3_score : 0.5
            else if (g_Type == game_Type.IN_GAME)
            {
                //Left -> 왼쪽 이동
                if (e.KeyCode == Keys.Left)
                {
                    p_Move_Degree -= 1;
                    if (p_Move_Degree < 0)
                        p_Move_Degree = n_number;
                    save_pPoint(start_Angle, p_Move_Degree);
                    //start timer
                    this.Invalidate();
                    this.Update();
                    this.Refresh();
                }
                //Right -> 오른쪽 이동
                else if (e.KeyCode == Keys.Right)
                {
                    p_Move_Degree += 1;
                    if (p_Move_Degree >= n_number)
                        p_Move_Degree = 0;
                    save_pPoint(start_Angle, p_Move_Degree);
                    //start timer
                    this.Invalidate();
                    this.Update();
                    this.Refresh();
                }
                //esc키 입력 후 게임 종료, 시작 화면으로 이동
                else if (e.KeyCode == Keys.Escape)
                {
                    //게임 타입 변경
                    g_Type = game_Type.PRE_GAME;
                    //start timer
                    timer1.Stop();
                    timer2.Stop();
                    timer3.Stop();
                    game_Time = 0;
                    label_Time.Text = "TIME : 0";
                    label_Title_1.Visible = true;
                    label_Title_2.Visible = true;
                    label_Notice_Start.Visible = true;
                    this.Invalidate();
                    this.Update();
                    this.Refresh();
                }
            }
        }
    }
}
