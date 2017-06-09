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
        //save Form's width, height
        private int width, height;
        //save cetner Point
        private float cx, cy;

        //define Math.Pi
        private const double PI = Math.PI;
        //1 -> center White Polygon
        //2 -> Player White Polygon
        private const int little_radius_1 = 30;
        private const int little_radius_2 = 10;
        private const int wall_radius = 700;
        private const int wall_radius2 = 600;
        //wall velocity
        private const float wall_velocity = (float)10.0;
        //to draw center Polygon, save Point List
        private List<PointF> Polygon_Point_List = new List<PointF>();
        //to draw center Polygon, save Point List
        private List<PointF> Hide_Player_Point = new List<PointF>();
        //save wall List at wall
        private List<wall> wall_Point_List = new List<wall>();
        //save wall Point List
        private List<PointF[]> all_Wall_Point_List = new List<PointF[]>();

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

        //player 중점
        private PointF pc;
        //player 좌표
        private PointF[] p_Point;
        //방향키에 따른 player 각도
        private int p_Move_Degree;

        //for Change View angle
        private int change_Angle = 2;
        private int start_Angle = 0;
        //for Random
        private Random rand_Num;
        private DateTime dt_Time;

        //backGround 회전 방향 설정
        private Direction_Type back_Direction = Direction_Type.BACK_LEFT;

        private struct wall {
            public int Location;
            //each 4 point
            public PointF[] pf;
            //상시적으로 계속하여 바뀌는 값
            public float[] vx, vy;
        }

        enum game_Type
        {
            PRE_GAME,
            IN_GAME,
            END_GAME
        };
        
        enum Direction_Type
        {
            RIGHT,
            LEFT,
            BACK_LEFT,
            BACK_RIGHT
        };

        //Form생성시 실행
        public Form_Main()
        {
            InitializeComponent();
            p_Move_Degree = n_number-1;
            //default setting n_number
            width = ClientSize.Width;
            height = ClientSize.Height;
            cx = (int)(width / 2.0);
            cy = (int)(height / 2.0);

            dt_Time = DateTime.Now;
            rand_Num = new Random(dt_Time.Millisecond);

            save_information(0);
            save_pPoint(0);

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

        //4.4_score : 1
        private void save_pPoint(int start_angle)
        {
            //player Point 좌표 저장하는 배열 할당
            p_Point = new PointF[3];
            
            //
            float pc_Degree = ((float)180.0 / (float)n_number);
            float pc_Radian = ((float)PI * pc_Degree / (float)(180.0));

            /*
            rx = ((px - rotx) * cos(rad) - (py - roty) * sin(rad)) + rotx;
            ry = ((px - rotx) * sin(rad) + (py - roty) * cos(rad)) + roty;
            px , py => 원래 좌표
            rotx, roty => 회전 중심점
            */

            //ex Point 좌표
            float ex = Hide_Player_Point[n_number - 1].X;
            float ey = Hide_Player_Point[n_number - 1].Y;
            //Plyaer center point
            pc = new PointF((float)((ex-cx) * Math.Cos(pc_Radian)) - (float)((ey-cy)* Math.Sin(pc_Radian)) + cx,
                       (float)((ex - cx) * Math.Sin(pc_Radian)) + (float)((ey - cy) * Math.Cos(pc_Radian)) + cy);
            
            //Calculate Radian using degree(n_number)
            int p_degree =360 / 3;
            //p_point index
            int i = 0;

            //save new Point => need add Point arrary
            start_angle += (120 - (int)(180.0 / n_number));
            for (int angle = start_angle ; angle < start_angle + 360; angle += p_degree)
            {
                float radian = ((float)PI * angle / (float)(180.0));

                //save Next Point x,y -> using ex Point x,y
                p_Point[i++] = new Point((int)(pc.X + little_radius_2 * Math.Cos(radian)),
                    (int)(pc.Y + little_radius_2 * (float)Math.Sin(radian)));
            }
        }

        //4.4_score : 1
        private void change_pPoint(int start_angle, Direction_Type key)
        {
            float pc_Degree = ((float)360.0 / (float)n_number);
            float pc_Radian = ((float)PI * pc_Degree / (float)(180.0));
           
            float back_Radian = (change_Angle * (float)PI / (float)(180.0));

            //변화된 Player 좌표 기억
            //Calculate Radian using degree(n_number)
            //p_point index
            int i;

            //왼쪽, 오른쪽 따라 플레이어 좌표 갱신 하여 저장
            if(key == Direction_Type.LEFT)
            {
                for (i = 0; i < 3; i++)
                {
                       //save Next Point x,y -> using ex Point x,y
                       p_Point[i] = new PointF((float)(((p_Point[i].X - cx) * Math.Cos(-pc_Radian))) - (float)(((p_Point[i].Y - cy) * Math.Sin(-pc_Radian))) + cx,
                               (float)(((p_Point[i].X - cx) * Math.Sin(-pc_Radian))) + (float)(((p_Point[i].Y - cy) * Math.Cos(-pc_Radian))) + cy);
                }
            }
            else if(key == Direction_Type.RIGHT)
            {
                for( i= 0; i< 3; i++)
                {
                    //save Next Point x,y -> using ex Point x,y
                    p_Point[i] = new PointF((float)(((p_Point[i].X - cx) * Math.Cos(pc_Radian))) - (float)(((p_Point[i].Y - cy) * Math.Sin(pc_Radian))) + cx,
                            (float)(((p_Point[i].X - cx) * Math.Sin(pc_Radian))) + (float)(((p_Point[i].Y - cy) * Math.Cos(pc_Radian))) + cy);
                }
            }
            else if (key == Direction_Type.BACK_LEFT)
            {
                for (i = 0; i < 3; i++)
                {
                    //save Next Point x,y -> using ex Point x,y
                    p_Point[i] = new PointF((float)(((p_Point[i].X - cx) * Math.Cos(back_Radian))) - (float)(((p_Point[i].Y - cy) * Math.Sin(back_Radian))) + cx,
                            (float)(((p_Point[i].X - cx) * Math.Sin(back_Radian))) + (float)(((p_Point[i].Y - cy) * Math.Cos(back_Radian))) + cy);
                }
            }
            else if (key == Direction_Type.BACK_RIGHT)
            {
                for (i = 0; i < 3; i++)
                {
                    //save Next Point x,y -> using ex Point x,y
                    p_Point[i] = new PointF((float)(((p_Point[i].X - cx) * Math.Cos(back_Radian))) - (float)(((p_Point[i].Y - cy) * Math.Sin(back_Radian))) + cx,
                            (float)(((p_Point[i].X - cx) * Math.Sin(back_Radian))) + (float)(((p_Point[i].Y - cy) * Math.Cos(back_Radian))) + cy);
                }
            }

        }
        
        private void save_wall()
        {
            //select sector 0~ n_number-1
            int wall_Location = rand_Num.Next(0, n_number - 1);

            wall wallp = new wall();
            wallp.pf = new PointF[4];
            wallp.vx = new float[4];
            wallp.vy = new float[4];

            float dx, dy, dx2, dy2;

            wallp.pf[0] = all_Wall_Point_List[wall_Location][0];
            wallp.pf[1] = all_Wall_Point_List[wall_Location][1];
            wallp.pf[2] = all_Wall_Point_List[wall_Location+1][1];
            wallp.pf[3] = all_Wall_Point_List[wall_Location+1][0];

            //4개 저장
            for (int i =0; i<4; i+=2)
            {
                //wallp 객체에 대해 저장하는 함수로써 어떤 벽을 선택할지 고르게 된다.
                //추가적으로 각 4개의 꼭지점에 대해 속도 = 거리/시간 을 저장하게 된다.
                dx = cx - all_Wall_Point_List[wall_Location + i/2][0].X;
                dy = cy - all_Wall_Point_List[wall_Location + i/2][0].Y;

                dx2 = cx - all_Wall_Point_List[wall_Location + i/2][1].X;
                dy2 = cy - all_Wall_Point_List[wall_Location + i/2][1].Y;

                if (dx != 0)
                    wallp.vx[i] = dx / (dx / wall_velocity);
                else
                    wallp.vx[i] = (float)0.0;

                if (dy != 0)
                    wallp.vy[i] = dy / (dy / wall_velocity);
                else
                    wallp.vy[i] = (float)0.0;

                if (dx != 0)
                    wallp.vx[i+1] = dx2 / (dx2 / wall_velocity);
                else
                    wallp.vx[i+1] = (float)0.0;

                if (dy != 0)
                    wallp.vy[i+1] = dy2 / (dy2 / wall_velocity);
                else
                    wallp.vy[i+1] = (float)0.0;
            }
            //내부 변수 초기화
            wallp.Location = 0;
            wall_Point_List.Add(wallp);
        }

        private void change_wall()
        {
            for (int i =0; i < wall_Point_List.Count; i++)
            {
                    float dx = cx - wall_Point_List[i].pf[0].X;
                    float dy = cy - wall_Point_List[i].pf[0].Y;

                    float dx2 = cx - wall_Point_List[i].pf[1].X;
                    float dy2 = cy - wall_Point_List[i].pf[1].Y;
                
                    if (dx != 0)
                        wall_Point_List[i].vx[0] = dx / (dx / wall_velocity);
                    else
                        wall_Point_List[i].vx[0] = 0;

                    if (dx2 != 0)
                        wall_Point_List[i].vx[1] = dx2 / (dx2 / wall_velocity);
                    else
                        wall_Point_List[i].vx[1] = 0;
                    
                    if (dy != 0)
                        wall_Point_List[i].vy[0] = dy / (dy / wall_velocity);
                    else
                        wall_Point_List[i].vx[0] = 0;

                    if (dy2 != 0)
                        wall_Point_List[i].vx[1] = dy2 / (dy2 / wall_velocity);
                    else
                        wall_Point_List[i].vx[1] = 0;

                    //move Position
                    wall_Point_List[i].pf[0].X += wall_Point_List[i].vx[0];
                    wall_Point_List[i].pf[1].X += wall_Point_List[i].vx[1];
                    wall_Point_List[i].pf[0].Y += wall_Point_List[i].vy[0];
                    wall_Point_List[i].pf[1].Y += wall_Point_List[i].vy[1];
            }

            /*
            float width = player_obj->x - bullet->x;
            float height = player_obj->y - bullet->y;

            if (bullet->direction == EAST)
            {
                if (width > 0)
                    width *= -1;
                bullet->vx = -BULLET_SPEED;
                bullet->vy = height / ((width / bullet->vx));
            }
            */
        }

        //키 입력에 따른 다각형의 Point 저장
        private void save_information(float start_angle)
        {
            if (Polygon_Point_List != null) {
                Polygon_Point_List.Clear();
                Hide_Player_Point.Clear();
                all_Wall_Point_List.Clear();
            }

            //save default Point
            PointF ep, hidep;
            PointF[] wallparr = new PointF[2];

            //Calculate Radian using degree(n_number)
            float degree = (float)360.0 / (float)n_number;

            //save new Point => need add Point arrary
            for (float angle = start_angle; angle < (float)start_angle+ 360.0; angle+=degree)
            {
                float radian = ((float)PI * angle / (float)(180.0));

                //save Next Point x,y -> using ex Point x,y
                ep = new PointF((float)(cx + little_radius_1 * Math.Cos(radian)),
                    (float)(cy + little_radius_1 * (float)Math.Sin(radian)));
                hidep = new PointF((float)(cx + (little_radius_1+10) * Math.Cos(radian)),
                     (float)(cy + (little_radius_1 + 10) * (float)Math.Sin(radian)));
                
                wallparr[0] = new PointF((float)(cx + wall_radius * Math.Cos(radian)),
                     (float)(cy + wall_radius * (float)Math.Sin(radian)));
                wallparr[1] = new PointF((float)(cx + wall_radius2 * Math.Cos(radian)),
                     (float)(cy + wall_radius2 * (float)Math.Sin(radian)));

                //한쌍의 벽 좌표를 담고있다.
                all_Wall_Point_List.Add(wallparr);

                //save Point List
                Polygon_Point_List.Add(ep);
                Hide_Player_Point.Add(hidep);
            }
            //for circular Form
            all_Wall_Point_List.Add(all_Wall_Point_List[0]);
        }
        
        //Paint 이벤트 발생시 발생하는 행위
        private void Form_Main_Paint(object sender, PaintEventArgs e)
        {
            //Calculate Radian using degree(n_number)
            float degree = (float)360.0 / (float)n_number;
            float radian = (degree * (float)PI / (float)(180.0));

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
                //Player 삼각형 그리기
                pen = new Pen(Color.Black);
                //3.2_score : 2.5
                //Center에 있는 작은 다각형 그리는 함수
                g.FillPolygon(center_Brush, Polygon_Point_List.ToArray());
                for(int j = 0; j< wall_Point_List.Count; j++)
                    g.DrawPolygon(pen, wall_Point_List[j].pf);

                //g.DrawPolygon(pen, Hide_Player_Point.ToArray());
                g.FillPolygon(center_Brush, p_Point);
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
            if(back_Direction == Direction_Type.BACK_LEFT)
            {
                back_Direction = Direction_Type.BACK_RIGHT;
            }
            else if (back_Direction == Direction_Type.BACK_RIGHT)
            {
                back_Direction = Direction_Type.BACK_LEFT;
            }
            //change_Angle *= -1;
            
            //timer2 시간 재설정
            timer2.Interval = rand_Num.Next(500,2000);
        }

        //4.1_score : 1
        //전체 화면 재도식 Paint 함수 호출
        private void timer3_Tick(object sender, EventArgs e)
        {
            //start_Angle += change_Angle;
            save_information(start_Angle);
            save_wall();
            change_pPoint(start_Angle, back_Direction);
            change_wall();
            //save_pPoint(start_Angle);
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
                    save_pPoint(start_Angle);
                    save_wall();
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
                    save_pPoint(start_Angle);
                    save_wall();
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
                //Left -> 왼쪽 이동
                else if (e.KeyCode == Keys.Left)
                {
                    if (p_Move_Degree == 0)
                        p_Move_Degree = n_number - 1;
                    else
                        p_Move_Degree -= 1;

                    change_pPoint(start_Angle, Direction_Type.LEFT);
                    //start timer
                    this.Invalidate();
                    this.Update();
                    this.Refresh();
                }
                //Right -> 오른쪽 이동
                else if (e.KeyCode == Keys.Right)
                {
                    if (p_Move_Degree == n_number - 1)
                        p_Move_Degree = 0;
                    else
                        p_Move_Degree += 1;

                    change_pPoint(start_Angle, Direction_Type.RIGHT);
                    //start timer
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
                    if (p_Move_Degree == 0)
                        p_Move_Degree = n_number - 1;
                    else
                        p_Move_Degree -= 1;
                    MessageBox.Show(" ");

                    change_pPoint(start_Angle, Direction_Type.LEFT);
                    //start timer
                    this.Invalidate();
                    this.Update();
                    this.Refresh();
                }
                //Right -> 오른쪽 이동
                else if (e.KeyCode == Keys.Right)
                {
                    if (p_Move_Degree == n_number - 1)
                        p_Move_Degree = 0;
                    else
                        p_Move_Degree += 1;

                    change_pPoint(start_Angle, Direction_Type.RIGHT);
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
                    wall_Point_List.Clear();
                    this.Invalidate();
                    this.Update();
                    this.Refresh();
                }
            }
        }
    }
}
