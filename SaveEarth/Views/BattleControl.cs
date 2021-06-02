using SaveEarth.MainClasses;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SaveEarth.Views
{
    public partial class BattleControl : UserControl
    {
        private MainForm Form;

        private Level CurrentLevel;
        private readonly Image drawImage;

        private Timer mainTimer;
        private Timer timerForAnimation;
        private Timer timerForShooting;

        private bool isAirPlaneTurnLeft;
        private bool isAirPlaneTurnRight;
        private bool isShooting = true;
        private bool isPaused = false;
        private bool menuButtonPress = false;
        private bool restartButtonPress = false;
        private bool pauseMenuOn = false;
        private bool resumeButtonPress = false;
        private bool menuButtonOnLoseDeskPress = false;
        private bool tutorialIsEnded = false;

        private Image backgroundImg = Image.FromFile("../../image/Menu/Background/backgroundSpace.png");
        private Image infoBar = Image.FromFile("../../image/Menu/InfoBar/infoBar.png");
        private Image dimming = Image.FromFile("../../image/Menu/Desks/LoseDesk/DimmingFrame.png");
        private Image loseDesk = Image.FromFile("../../image/Menu/Desks/LoseDesk/LoseDesk.png");
        private Image menu = Image.FromFile("../../image/Menu/Desks/LoseDesk/Menu.png");
        private Image menuPress = Image.FromFile("../../image/Menu/Desks/LoseDesk/MenuPress.png");
        private Image restart = Image.FromFile("../../image/Menu/Desks/LoseDesk/ResrtartLvl.png");
        private Image restartPress = Image.FromFile("../../image/Menu/Desks/LoseDesk/ResrtartLvlPress.png");
        private Image pause = Image.FromFile("../../image/Menu/Desks/PauseDesk/Pause.png");
        private Image resume = Image.FromFile("../../image/Menu/Desks/PauseDesk/Resume.png");
        private Image resumePress = Image.FromFile("../../image/Menu/Desks/PauseDesk/ResumePress.png");

        private int tutorialFrame = 1;


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
        }

        public BattleControl(Level currentLevel, MainForm form)
        {
            InitializeComponent();
            Form = form;
            ClientSize = new Size(Form.Width, Form.Height);

            CurrentLevel = currentLevel;

            drawImage = new Bitmap(ClientSize.Width, ClientSize.Height, PixelFormat.Format32bppArgb);

            mainTimer = new Timer { Interval = 20 };
            mainTimer.Tick += MainTimerTick;
            mainTimer.Start();

            timerForAnimation = new Timer { Interval = 350 };
            timerForAnimation.Tick += TimerForAnimationTick;
            timerForAnimation.Start();

            timerForShooting = new Timer { Interval = 300 };
            timerForShooting.Tick += TimerForShootingTick;
        }


        private void TimerForAnimationTick(object sender, EventArgs e)
        {
            if (CurrentLevel == null) return;
            CurrentLevel.CurrentPlanet.AnimateEarth();
            CurrentLevel.AirPlane.AnimateAirPlane();
            foreach (var alien in CurrentLevel.GetAliens())
            {
                alien.AnimateAlien();
            }
            System.GC.Collect();
        }

        private void MainTimerTick(object sender, EventArgs e)
        {
            if (CurrentLevel == null) return;
            MoveAirPlane();
            Invalidate();
            Update();
        }

        //Рисование
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Black, ClientRectangle);
            var g = Graphics.FromImage(drawImage);
            Draw(g);
            e.Graphics.DrawImage(drawImage, ((ClientSize.Width - drawImage.Width) / 2), ((ClientSize.Height - drawImage.Height) / 2));
        }

        private void Draw(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (CurrentLevel == null) return;

            if (CurrentLevel.IsLost) isPaused = true;

            var earthImage = CurrentLevel.CurrentPlanet.GetFrameForAnimation();

            g.TranslateTransform(ClientSize.Width / 2, ClientSize.Height / 2);
            g.DrawImage(backgroundImg, -backgroundImg.Width / 2 - 200, -backgroundImg.Height / 2 - 100);
            var tempMatrix = g.Transform;

            //Рисуем самолет
            DrawAirPlane(g);
            if (!isPaused && !CurrentLevel.CurrentPlanet.isDead)
            {
                CurrentLevel.CheckForHits();//Проверяем попадание снарядов
                CurrentLevel.DeleteDeadAlienFromBattle();//Удалем побежденных инопланетян
                CurrentLevel.CheckForBoost();//"Подбираем бусты"
                CurrentLevel.DeleteTakenBoosts();//Удаляем подобраные бусты
                CurrentLevel.DeleteBulletFromBattle(ClientSize);//удаляем все снаряды, вылетевшие за игровое поле или попавшие в протиника
            }
            //рисуем снаряды
            DrawBullets(g, tempMatrix);
            //Рисуем инопланетян
            DrawAliens(g, tempMatrix);
            //Рисуем бусты
            DrawBoosts(g, tempMatrix);


            g.Transform = tempMatrix;
            //Рисуем Землю
            g.DrawImage(earthImage, -earthImage.Width / 2 - 30, -earthImage.Height / 2 - 25);

            DrawInfoBar(g);

            if (isPaused && CurrentLevel.IsLost)
                DrawLoseDesk(g);

            if (pauseMenuOn)
                DrawPauseMenuDesk(g);

            if (!tutorialIsEnded)
                DrawTutorial(g);
        }

        private void DrawTutorial(Graphics g)
        {
            if (tutorialFrame > 7)
            {
                tutorialIsEnded = true;
                return;
            }
            if (isPaused) return;
            var imgTutorial = Image.FromFile("../../image/Tutorial/Tutorial_" + tutorialFrame.ToString() + ".png");
            var imgEnter = Image.FromFile("../../image/Tutorial/Tutorial_Enter.png");
            var offsetY = +ClientSize.Height / 2 - 100;
            g.DrawImage(imgTutorial, -imgTutorial.Width / 2, -imgTutorial.Height / 2 + offsetY);
            g.DrawImage(imgEnter, -imgEnter.Width / 2 - 600, -imgEnter.Height / 2 + offsetY);

        }

        private void DrawPauseMenuDesk(Graphics g)
        {
            CheckMousePressingPauseDesk();
            g.DrawImage(dimming, -dimming.Width / 2, -dimming.Height / 2);
            g.DrawImage(pause, -pause.Width / 2, -pause.Height / 2 - 200);

            if (menuButtonPress)
                g.DrawImage(menuPress, -menuPress.Width / 2, -menuPress.Height / 2);
            else g.DrawImage(menu, -menu.Width / 2, -menu.Height / 2);

            if (resumeButtonPress)
                g.DrawImage(resumePress, -resumePress.Width / 2, -resumePress.Height / 2 - 75);
            else
                g.DrawImage(resume, -resume.Width / 2, -resume.Height / 2 - 75);

        }

        private void DrawInfoBar(Graphics g)
        {
            var infoBarRockets = Image.FromFile("../../image/Menu/InfoBar/infoBarRockets_" + CurrentLevel.AirPlane.NumberOfAvailableRocket.ToString() + ".png");
            g.DrawImage(infoBar, -ClientSize.Width / 2, -ClientSize.Height / 2);
            g.DrawImage(infoBarRockets, -ClientSize.Width / 2 + infoBar.Width + 10, -ClientSize.Height / 2);
            var dx = CurrentLevel.CurrentPlanet.HealthPoint * 100 / CurrentLevel.CurrentPlanet.MaxHealthPoint;
            g.DrawLine(new Pen(Color.Red, 20), new Point(-ClientSize.Width / 2 + 120, -ClientSize.Height / 2 + 52), new Point(dx - ClientSize.Width / 2 + 120, -ClientSize.Height / 2 + 52));
        }

        private void DrawLoseDesk(Graphics g)
        {
            CheckMousePressingLoseDesk();
            g.DrawImage(dimming, -dimming.Width / 2, -dimming.Height / 2);
            g.DrawImage(loseDesk, -loseDesk.Width / 2, -loseDesk.Height / 2);
            g.DrawString(CurrentLevel.Score.ToString(), new Font("PlayMeGames", 40, FontStyle.Bold),
                new SolidBrush(Color.White), 50, -25);

            if (menuButtonOnLoseDeskPress)
                g.DrawImage(menuPress, -menuPress.Width / 2 - 80, -menuPress.Height / 2 + 100);
            else
                g.DrawImage(menu, -menu.Width / 2 - 80, -menu.Height / 2 + 100);

            if (restartButtonPress)
                g.DrawImage(restartPress, -restartPress.Width / 2 + 100, -restartPress.Height / 2 + 100);
            else
                g.DrawImage(restart, -restart.Width / 2 + 100, -restart.Height / 2 + 100);


        }

        private void DrawAirPlane(Graphics g)
        {
            Image airPlaneImage = CurrentLevel.AirPlane.GetFrameForAnimation(isAirPlaneTurnLeft, isAirPlaneTurnRight);
            g.TranslateTransform((float)CurrentLevel.AirPlane.LocationX, (float)CurrentLevel.AirPlane.LocationY);
            g.RotateTransform((float)(CurrentLevel.AirPlane.Direction * 180 / Math.PI));
            g.DrawImage(airPlaneImage, -airPlaneImage.Width / 2, -airPlaneImage.Height / 2);
        }

        private void DrawBoosts(Graphics g, Matrix tempMatrix)
        {
            foreach (var boost in CurrentLevel.GetBosts())
            {
                g.Transform = tempMatrix;
                if (!isPaused)
                    boost.Move();
                g.TranslateTransform((float)boost.LocationX, (float)boost.LocationY);
                g.DrawImage(boost.GetFrameForAnimation(), -boost.GetFrameForAnimation().Width / 2, -boost.GetFrameForAnimation().Height / 2);
            }
        }

        private void DrawBullets(Graphics g, Matrix tempMatrix)
        {
            foreach (var bullet in CurrentLevel.GetBullets())
            {
                g.Transform = tempMatrix;
                if (!isPaused)
                    bullet.Move();
                g.TranslateTransform((float)bullet.LocationX, (float)bullet.LocationY);
                g.RotateTransform((float)(bullet.Direction * 180 / Math.PI));
                g.DrawImage(bullet.GetFrameForAnimation(), -bullet.GetFrameForAnimation().Width / 2, -bullet.GetFrameForAnimation().Height / 2);
            }
        }

        private void DrawAliens(Graphics g, Matrix tempMatrix)
        {
            foreach (var alien in CurrentLevel.GetAliens())
            {
                g.Transform = tempMatrix;
                if (!isPaused)
                {
                    alien.Move(0.03);
                    if (alien.CanDoAttack())
                        CurrentLevel.AlienAttack(alien);
                }
                g.TranslateTransform((float)alien.LocationX, (float)alien.LocationY);
                g.DrawImage(alien.GetFrameForAnimation(), -alien.GetFrameForAnimation().Width / 2, -alien.GetFrameForAnimation().Height / 2);
            }
        }

        //Управление
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (!isPaused && !CurrentLevel.IsLost)
            {
                HandleKey(e.KeyCode, true);
                switch (e.KeyCode)
                {
                    case Keys.Space:
                        DoShooting();
                        break;
                    case Keys.Escape:
                        {
                            isPaused = true;
                            pauseMenuOn = true;
                        }
                        break;
                    case Keys.W:
                        FireRocket();
                        break;
                    case Keys.Enter:
                        {
                            if (!tutorialIsEnded)
                                tutorialFrame++;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            HandleKey(e.KeyCode, false);
        }

        //стрельба самолета
        private void TimerForShootingTick(object sender, EventArgs e)
        {
            if (CurrentLevel == null) return;
            isShooting = true;
            timerForShooting.Stop();
        }

        private void DoShooting()
        {
            if (isShooting)
            {
                CurrentLevel.AddAirPlaneBulletToBattle();
                timerForShooting.Start();
                isShooting = false;
            }
        }

        private void FireRocket()
        {
            CurrentLevel.AddRocketToBattleForAirPlane();
        }

        //Движение Самолета
        private void MoveAirPlane()
        {
            CurrentLevel.AirPlane.Move(isAirPlaneTurnLeft ? Turn.Left : (isAirPlaneTurnRight ? Turn.Right : Turn.None), 0.3);
        }

        private void HandleKey(Keys e, bool down)
        {
            if (e == Keys.A) isAirPlaneTurnLeft = down;
            if (e == Keys.D) isAirPlaneTurnRight = down;
        }


        private void CheckMousePressingLoseDesk()
        {
            if (CursorOnTheButton(menu, -80, 100))
            {
                OffAllButtonsPress();
                menuButtonOnLoseDeskPress = true;
                return;
            }
            else
                OffAllButtonsPress();

            if (CursorOnTheButton(restart, 100, 100))
            {
                OffAllButtonsPress();
                restartButtonPress = true;
                return;
            }
            else OffAllButtonsPress();
        }

        private void CheckMousePressingPauseDesk()
        {
            if (CursorOnTheButton(resume, 0, -75))
            {
                OffAllButtonsPress();
                resumeButtonPress = true;
                return;
            }
            else OffAllButtonsPress();

            if (CursorOnTheButton(menu, 0, 0))
            {
                OffAllButtonsPress();
                menuButtonPress = true;
                return;
            }
            else OffAllButtonsPress();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (menuButtonOnLoseDeskPress)
            {
                Form.ShowMainMenuControl();
                mainTimer.Stop();
            }
            if (restartButtonPress)
            {
                if (!tutorialIsEnded)
                    tutorialFrame = 1;
                CurrentLevel = new Level(CurrentLevel.maxAliensInBattle, CurrentLevel.AirPlane.MaxNumberOfAvailableRocket,
                    CurrentLevel.ChanceBoostDrop, CurrentLevel.CurrentPlanet.MaxHealthPoint, new Size(ClientSize.Width, ClientSize.Height));
                isPaused = false;
            }
            if (menuButtonPress)
            {

                Form.ShowMainMenuControl();
                if (!tutorialIsEnded)
                    tutorialFrame = 1;
                isPaused = false;
                pauseMenuOn = false;
                mainTimer.Stop();
            }
            if (resumeButtonPress)
            {
                isPaused = false;
                pauseMenuOn = false;
            }
        }


        private bool CursorOnTheButton(Image buttonIamage, int offsetX, int offsetY)
        {
            var coursorX = Cursor.Position.X;
            var coursorY = Cursor.Position.Y;

            return Math.Abs(ClientSize.Width / 2 + offsetX - coursorX) < buttonIamage.Width / 2 &&
              (Math.Abs(ClientSize.Height / 2 + offsetY - coursorY) < buttonIamage.Height / 2);
        }

        private void OffAllButtonsPress()
        {
            menuButtonPress = false;
            restartButtonPress = false;
            resumeButtonPress = false;
            menuButtonOnLoseDeskPress = false;
        }

        public void ChangeCurrentLevel(Level newLevel)
        {
            CurrentLevel = newLevel;
            mainTimer.Start();
            isPaused = false;
        }
    }
}
