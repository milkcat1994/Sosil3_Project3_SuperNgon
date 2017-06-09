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
        private const int wall_radius = 500;
        private const int wall_radius2 = 400;
        //wall velocity
        private const float wall_velocity = (float)20.0;
        //to draw center Polygon, save Point List
        private List<PointF> Polygon_Point_List = new List<PointF>();
        //to draw center Polygon, save Point List
        private List<PointF> Hide_Player_Point = new List<PointF>();
        //test///////////////////////////////////
        private List<PointF> testPL = new List<PointF>();
        //save wall List at wall
        private List<wall> wall_Point_List = new List<wall>();
        //save wall Point List
        private List<PointF> all_Wall_Point_List_1 = new List<PointF>();
        private List<PointF> all_Wall_Point_List_2 = new List<PointF>();

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
            IN_GAME
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
            this.timer4.Stop();
        }
        
        private float makeDistance(float wx, float wy, float px, float py)
        {
            double distance = Math.Sqrt( Math.Pow((double)(wx - px), 2) + Math.Pow((double)(wy - py), 2));
            return (float)distance;
        }

        //4.4_score : 1
        //플레이어 좌표 결정 함수
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
        //플레이어 좌표 변경 함수
        private void change_pPoint(Direction_Type key)
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

        //5.1_1_score : 1.5
        //벽 좌표 저장 함수
        private void save_wall()
        {
            //select sector 0~ n_number-1
            //5.1_2_score : 0.5
            //랜덤하게 벽 생성 갯수를 결정하며 한 자리는 남도록 설계
            int wall_count = rand_Num.Next(1, n_number-1);
            int[] wall_Location = new int[wall_count];
            for (int i =0; i<wall_count; i++)
                wall_Location[i] = rand_Num.Next(0, n_number);


            float dx, dy, dx2, dy2, dx3, dy3, dx4, dy4;
            for(int i =0; i<wall_count; i++)
            {
                wall wallp = new wall();
                wallp.pf = new PointF[4];
                wallp.vx = new float[4];
                wallp.vy = new float[4];

                //왼쪽 상단
                wallp.pf[0] = all_Wall_Point_List_1[wall_Location[i]];
                //왼쪽 하단
                wallp.pf[1] = all_Wall_Point_List_2[wall_Location[i]];
                //오른쪽 하단
                wallp.pf[2] = all_Wall_Point_List_2[wall_Location[i] + 1];
                //오른쪽 상단
                wallp.pf[3] = all_Wall_Point_List_1[wall_Location[i] + 1];

                //wallp 객체에 대해 저장하는 함수로써 어떤 벽을 선택할지 고르게 된다.
                //추가적으로 각 4개의 꼭지점에 대해 속도 = 거리/시간 을 저장하게 된다.
                dx = cx - wallp.pf[0].X;
                dy = cy - wallp.pf[0].Y;

                dx2 = cx - wallp.pf[1].X;
                dy2 = cy - wallp.pf[1].Y;

                dx3 = cx - wallp.pf[2].X;
                dy3 = cy - wallp.pf[2].Y;

                dx4 = cx - wallp.pf[3].X;
                dy4 = cy - wallp.pf[3].Y;


                if (dx != 0)
                    wallp.vx[0] = dx / wall_velocity;
                else
                    wallp.vx[0] = (float)0.0;

                if (dy != 0)
                    wallp.vy[0] = dy / wall_velocity;
                else
                    wallp.vy[0] = (float)0.0;

                if (dx2 != 0)
                    wallp.vx[1] = dx2 / wall_velocity;
                else
                    wallp.vx[1] = (float)0.0;

                if (dy2 != 0)
                    wallp.vy[1] = dy2 / wall_velocity;
                else
                    wallp.vy[1] = (float)0.0;

                if (dx3 != 0)
                    wallp.vx[2] = dx3 / wall_velocity;
                else
                    wallp.vx[2] = (float)0.0;

                if (dy3 != 0)
                    wallp.vy[2] = dy3 / wall_velocity;
                else
                    wallp.vy[2] = (float)0.0;

                if (dx4 != 0)
                    wallp.vx[3] = dx4 / wall_velocity;
                else
                    wallp.vx[3] = (float)0.0;

                if (dy4 != 0)
                    wallp.vy[3] = dy4 / wall_velocity;
                else
                    wallp.vy[3] = (float)0.0;

                //내부 변수 초기화
                wallp.Location = 0;
                wall_Point_List.Add(wallp);
            }
        }

        //5.1_1_score : 1.5
        //벽 좌표 변경 함수
        private void change_wall(Direction_Type key)
        {
            float back_Radian = (change_Angle * (float)PI / (float)(180.0));
            PointF temppf;
            float dx, dy, dx2, dy2, dx3, dy3, dx4, dy4;
            for (int i =0; i < wall_Point_List.Count; i++)
            {
                dx = cx - wall_Point_List[i].pf[0].X;
                dy = cy - wall_Point_List[i].pf[0].Y;
                dx2 = cx - wall_Point_List[i].pf[1].X;
                dy2 = cy - wall_Point_List[i].pf[1].Y;

                dx3 = cx - wall_Point_List[i].pf[2].X;
                dy3 = cy - wall_Point_List[i].pf[2].Y;
                dx4 = cx - wall_Point_List[i].pf[3].X;
                dy4 = cy - wall_Point_List[i].pf[3].Y;

                if (dx != 0)
                    wall_Point_List[i].vx[0] = dx / wall_velocity;
                else
                    wall_Point_List[i].vx[0] = 0;

                if (dy != 0)
                    wall_Point_List[i].vy[0] = dy / wall_velocity;
                else
                    wall_Point_List[i].vy[0] = 0;

                if (dx2 != 0)
                    wall_Point_List[i].vx[1] = dx2 / wall_velocity;
                else
                    wall_Point_List[i].vx[1] = 0;
                
                if (dy2 != 0)
                    wall_Point_List[i].vy[1] = dy2 / wall_velocity;
                else
                    wall_Point_List[i].vy[1] = 0;

                if (dx3 != 0)
                    wall_Point_List[i].vx[2] = dx3 / wall_velocity;
                else
                    wall_Point_List[i].vx[2] = 0;

                if (dy3 != 0)
                    wall_Point_List[i].vy[2] = dy3 / wall_velocity;
                else
                    wall_Point_List[i].vy[2] = 0;

                if (dx4 != 0)
                    wall_Point_List[i].vx[3] = dx4 / wall_velocity;
                else
                    wall_Point_List[i].vx[3] = 0;

                if (dy4 != 0)
                    wall_Point_List[i].vy[3] = dy4 / wall_velocity;
                else
                    wall_Point_List[i].vy[3] = 0;
                //move Position
                wall_Point_List[i].pf[0].X += wall_Point_List[i].vx[0];
                wall_Point_List[i].pf[0].Y += wall_Point_List[i].vy[0];
                wall_Point_List[i].pf[1].X += wall_Point_List[i].vx[1];
                wall_Point_List[i].pf[1].Y += wall_Point_List[i].vy[1];

                wall_Point_List[i].pf[2].X += wall_Point_List[i].vx[2];
                wall_Point_List[i].pf[2].Y += wall_Point_List[i].vy[2];
                wall_Point_List[i].pf[3].X += wall_Point_List[i].vx[3];
                wall_Point_List[i].pf[3].Y += wall_Point_List[i].vy[3];
                if (key == Direction_Type.BACK_LEFT)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        temppf = wall_Point_List[i].pf[j];
                        //save Next Point x,y -> using ex Point x,y
                        wall_Point_List[i].pf[j] = new PointF((float)(((temppf.X - cx) * Math.Cos(back_Radian))) - (float)(((temppf.Y - cy) * Math.Sin(back_Radian))) + cx,
                                (float)(((temppf.X - cx) * Math.Sin(back_Radian))) + (float)(((temppf.Y - cy) * Math.Cos(back_Radian))) + cy);
                    }
                }
                else if(key == Direction_Type.BACK_RIGHT)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        temppf = wall_Point_List[i].pf[j];
                        //save Next Point x,y -> using ex Point x,y
                        wall_Point_List[i].pf[j] = new PointF((float)(((temppf.X - cx) * Math.Cos(back_Radian))) - (float)(((temppf.Y - cy) * Math.Sin(back_Radian))) + cx,
                                (float)(((temppf.X - cx) * Math.Sin(back_Radian))) + (float)(((temppf.Y - cy) * Math.Cos(back_Radian))) + cy);
                    }
                }
            }
        }

        //키 입력에 따른 다각형의 Point 저장
        private void save_information(float start_angle)
        {
            if (Polygon_Point_List != null) {
                Polygon_Point_List.Clear();
                Hide_Player_Point.Clear();
                all_Wall_Point_List_1.Clear();
                all_Wall_Point_List_2.Clear();
                testPL.Clear();
            }

            //save default Point
            PointF ep, hidep;
            PointF wallparr_1 = new PointF();
            PointF wallparr_2 = new PointF();

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
                
                wallparr_1 = new PointF((float)(cx + wall_radius * Math.Cos(radian)),
                     (float)(cy + wall_radius * (float)Math.Sin(radian)));
                wallparr_2 = new PointF((float)(cx + wall_radius2 * Math.Cos(radian)),
                     (float)(cy + wall_radius2 * (float)Math.Sin(radian)));

                //한쌍의 벽 좌표를 담고있다.
                all_Wall_Point_List_1.Add(wallparr_1);
                all_Wall_Point_List_2.Add(wallparr_2);
                testPL.Add(wallparr_1);
                //save Point List
                Polygon_Point_List.Add(ep);
                Hide_Player_Point.Add(hidep);
            }

            //for circular Form
            all_Wall_Point_List_1.Add(all_Wall_Point_List_1[0]);
            all_Wall_Point_List_2.Add(all_Wall_Point_List_2[0]);
        }
        
        //5.2_1_score : 0.5
        //막대와의 충돌 구성
        private bool check_Collision()
        {
            //밑의 구간으로 삼각형의 중점과의 거리를 통해 사이에 있다면 true를 반환하여 충돌임을 알려준다.
            float wCenterX, wCenterY;
            float pCenterX, pCenterY;
            float distance;
            for(int i=0; i<wall_Point_List.Count; i++)
            {
                wCenterX= (wall_Point_List[i].pf[1].X + wall_Point_List[i].pf[2].X) / (float)2.0;
                wCenterY = (wall_Point_List[i].pf[1].Y + wall_Point_List[i].pf[2].Y) / (float)2.0;
                pCenterX = 0;   pCenterY = 0;
                for(int j =0; j < 3; j++)
                {
                    pCenterX += p_Point[j].X;
                    pCenterY += p_Point[j].Y;
                }
                pCenterX /= (float)3.0;
                pCenterY /= (float)3.0;

                distance = makeDistance(wCenterX, wCenterY, pCenterX, pCenterY);

                if(distance <= little_radius_2)
                {
                    return true;
                }
            }
            return false;
        }

        //Paint 이벤트 발생시 발생하는 함수
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
                //
                for (int j = 0; j < wall_Point_List.Count; j++)
                    g.FillPolygon(new SolidBrush(Color.LightSeaGreen), wall_Point_List[j].pf);

                //3.2_score : 2.5
                //Center에 있는 작은 다각형 그리는 함수
                g.FillPolygon(center_Brush, Polygon_Point_List.ToArray());
                
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
            change_pPoint(back_Direction);
            change_wall(back_Direction);

            //5.2_1_score : 0.5
            //충돌 Check
            if (check_Collision())
            {
                //충돌 발생
                timer1.Stop();
                timer2.Stop();
                timer3.Stop();
                timer4.Stop();

                MessageBox.Show("기록 : " + game_Time);
                //5.2_1_score : 0.5
                g_Type = game_Type.PRE_GAME;
                start_Angle = 0;
                save_information(start_Angle);
                save_pPoint(start_Angle);
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
            //충돌 미발생
            else
            {
                this.Invalidate();
                this.Update();
                this.Refresh();
            }
        }
        
        //장애물 생성주기 timer
        private void timer4_Tick(object sender, EventArgs e)
        {
            save_wall();
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
                    //repaint
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
                    //repaint
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
                    timer4.Start();

                    label_Title_1.Visible = false;
                    label_Title_2.Visible = false;
                    label_Notice_Start.Visible = false;
                    //repaint
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

                    change_pPoint(Direction_Type.LEFT);
                    //repaint
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

                    change_pPoint(Direction_Type.RIGHT);
                    //repaint
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

                    change_pPoint(Direction_Type.LEFT);
                    //repaint
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

                    change_pPoint(Direction_Type.RIGHT);
                    //repaint
                    this.Invalidate();
                    this.Update();
                    this.Refresh();
                }
                //esc키 입력 후 게임 종료, 시작 화면으로 이동
                else if (e.KeyCode == Keys.Escape)
                {
                    //게임 타입 변경
                    g_Type = game_Type.PRE_GAME;
                    //stop timer
                    timer1.Stop();
                    timer2.Stop();
                    timer3.Stop();
                    timer4.Stop();

                    game_Time = 0;
                    //모두 초기화
                    start_Angle = 0;
                    save_information(start_Angle);
                    save_pPoint(start_Angle);
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
