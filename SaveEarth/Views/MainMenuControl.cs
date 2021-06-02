using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace SaveEarth.Views
{
    public partial class MainMenuControl : UserControl
    {

        private MainForm Form;
        private Image drawImage;
        private int CurrentAnomationSprite = 1;
        private Timer MainTimer = new Timer { Interval = 20 };
        private Timer timerForAnimation = new Timer { Interval = 350 };


        private Image background = Image.FromFile("../../image/Menu/Background/backgroundSpace.png");
        private Image start = Image.FromFile("../../image/Menu/Buttons/Start.png");
        private Image exit = Image.FromFile("../../image/Menu/Buttons/Exit.png");
        private Image startPress = Image.FromFile("../../image/Menu/Buttons/StartPress.png");
        private Image exitPress = Image.FromFile("../../image/Menu/Buttons/ExitPress.png");

        private bool StartButtonPress = false;
        private bool ExitButtonPress = false;



        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
        }

        public MainMenuControl(MainForm form)
        {
            InitializeComponent();
            Form = form;

            this.BackColor = Color.Black;
            ClientSize = new Size(Form.Width, Form.Height);

            timerForAnimation = new Timer { Interval = 350 };
            timerForAnimation.Tick += TimerForAnimationTick;
            timerForAnimation.Start();

            MainTimer.Tick += MainTimerTick;
            MainTimer.Start();

            drawImage = new Bitmap(ClientSize.Width, ClientSize.Height);
        }


        private void TimerForAnimationTick(object sender, EventArgs e)
        {
            AnimateLogo();
        }

        private void AnimateLogo()
        {
            if (CurrentAnomationSprite > 5)
                CurrentAnomationSprite = 1;
            CurrentAnomationSprite++;
        }

        private void MainTimerTick(object sender, EventArgs e)
        {
            Invalidate();
            Update();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (StartButtonPress)
            {
                Form.ShowLevelSelectionComntrol();
                return;
            }
            if (ExitButtonPress)
            {
                Application.Exit();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = Graphics.FromImage(drawImage);
            Draw(g);
            e.Graphics.DrawImage(drawImage, 0, 0);
        }

        private void Draw(Graphics g)
        {

            CheckMousePressing();

            g.DrawImage(background, (ClientSize.Width - background.Width) / 2 - 200, (ClientSize.Height - background.Height) / 2 - 100);

            Image logo = Image.FromFile("../../image/Menu/Logo/LOGO3_" + CurrentAnomationSprite.ToString() + ".png");
            g.DrawImage(logo, (ClientSize.Width - logo.Width) / 2, (ClientSize.Height - logo.Height) / 2 - 200);

            if (StartButtonPress)
                g.DrawImage(startPress, (ClientSize.Width - start.Width) / 2, (ClientSize.Height - start.Height) / 2 + 20);
            else
                g.DrawImage(start, (ClientSize.Width - start.Width) / 2, (ClientSize.Height - start.Height) / 2 + 20);
            if (ExitButtonPress)
                g.DrawImage(exitPress, (ClientSize.Width - exit.Width) / 2, (ClientSize.Height - exit.Height) / 2 + 120);
            else
                g.DrawImage(exit, (ClientSize.Width - exit.Width) / 2, (ClientSize.Height - exit.Height) / 2 + 120);
            System.GC.Collect();
        }

        private void CheckMousePressing()
        {
            if (CursorOnTheButton(start, 20))
            {
                StartButtonPress = true;
                ExitButtonPress = false;
                return;
            }
            else
            {
                StartButtonPress = false;
                ExitButtonPress = false;
            }

            if (CursorOnTheButton(exit, 120))
            {
                ExitButtonPress = true;
                StartButtonPress = false;
                return;
            }
            else
            {
                StartButtonPress = false;
                ExitButtonPress = false;
            }
        }
        private bool CursorOnTheButton(Image buttonIamage, int offset)
        {
            var coursorX = Cursor.Position.X;
            var coursorY = Cursor.Position.Y;

            return Math.Abs(ClientSize.Width / 2 - coursorX) < buttonIamage.Width / 2 &&
              (Math.Abs(ClientSize.Height / 2 + offset - coursorY) < buttonIamage.Height / 2);
        }
    }
}

