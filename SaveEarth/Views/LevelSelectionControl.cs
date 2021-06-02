using SaveEarth.MainClasses;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SaveEarth.Views
{
    public partial class LevelSelectionControl : UserControl
    {
        private MainForm Form;
        private Timer MainTimer = new Timer { Interval = 20 };
        private Image drawImage;
        private Image background = Image.FromFile("../../image/Menu/Background/backgroundSpace.png");
        private Image easy = Image.FromFile("../../image/Menu/Buttons/Easy.png");
        private Image easyPress = Image.FromFile("../../image/Menu/Buttons/EasyPress.png");
        private Image normal = Image.FromFile("../../image/Menu/Buttons/Normal.png");
        private Image normalPress = Image.FromFile("../../image/Menu/Buttons/NormalPress.png");
        private Image hard = Image.FromFile("../../image/Menu/Buttons/Hard.png");
        private Image hardPress = Image.FromFile("../../image/Menu/Buttons/HardPress.png");
        private Image back = Image.FromFile("../../image/Menu/Buttons/Back.png");
        private Image backPress = Image.FromFile("../../image/Menu/Buttons/BackPress.png");
        private Image select = Image.FromFile("../../image/Menu/Buttons/Select.png");
        private Image easyHelpDesk = Image.FromFile("../../image/Menu/Buttons/EasyHelpDesk.png");
        private Image normalHelpDesk = Image.FromFile("../../image/Menu/Buttons/NormalHelpDesk.png");
        private Image hardHelpDesk = Image.FromFile("../../image/Menu/Buttons/HardHelpDesk.png");

        private bool EasyButtonPress = false;
        private bool NormalButtonPress = false;
        private bool HardButtonPress = false;
        private bool BackButtonPress = false;


        protected override void OnLoad(EventArgs e)
        {
            DoubleBuffered = true;
        }

        public LevelSelectionControl(MainForm form)
        {

            InitializeComponent();
            this.BackColor = Color.Black;
            Form = form;
            ClientSize = new Size(Form.Width, Form.Height);

            drawImage = new Bitmap(ClientSize.Width, ClientSize.Height);
            MainTimer.Tick += MainTimerTick;
            MainTimer.Start();
        }

        private void MainTimerTick(object sender, EventArgs e)
        {
            Invalidate();
            Update();
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
            g.DrawImage(select, (ClientSize.Width - select.Width) / 2, (ClientSize.Height - select.Height) / 2 - 300);

            if (EasyButtonPress)
            {
                g.DrawImage(easyPress, (ClientSize.Width - easyPress.Width) / 2, (ClientSize.Height - easyPress.Height) / 2 - 100);
                g.DrawImage(easyHelpDesk, (ClientSize.Width - easyHelpDesk.Width) / 2 + 400, (ClientSize.Height - easyHelpDesk.Height) / 2);
            }
            else
                g.DrawImage(easy, (ClientSize.Width - easy.Width) / 2, (ClientSize.Height - easy.Height) / 2 - 100);

            if (NormalButtonPress)
            {
                g.DrawImage(normalPress, (ClientSize.Width - normalPress.Width) / 2, (ClientSize.Height - normalPress.Height) / 2);
                g.DrawImage(normalHelpDesk, (ClientSize.Width - normalHelpDesk.Width) / 2 + 400, (ClientSize.Height - normalHelpDesk.Height) / 2);
            }
            else
                g.DrawImage(normal, (ClientSize.Width - normal.Width) / 2, (ClientSize.Height - normal.Height) / 2);

            if (HardButtonPress)
            {
                g.DrawImage(hardPress, (ClientSize.Width - hardPress.Width) / 2, (ClientSize.Height - hardPress.Height) / 2 + 100);
                g.DrawImage(hardHelpDesk, (ClientSize.Width - hardHelpDesk.Width) / 2 + 400, (ClientSize.Height - hardHelpDesk.Height) / 2);
            }
            else
                g.DrawImage(hard, (ClientSize.Width - hard.Width) / 2, (ClientSize.Height - hard.Height) / 2 + 100);

            if (BackButtonPress)
                g.DrawImage(backPress, (ClientSize.Width - backPress.Width) / 2, (ClientSize.Height - backPress.Height) / 2 + 300);
            else
                g.DrawImage(back, (ClientSize.Width - back.Width) / 2, (ClientSize.Height - back.Height) / 2 + 300);

            System.GC.Collect();
        }


        private void CheckMousePressing()
        {
            if (CursorOnTheButton(easy, -100))
            {
                OffAllButtonsPress();
                EasyButtonPress = true;
                return;
            }
            else
                OffAllButtonsPress();

            if (CursorOnTheButton(normal, 0))
            {
                OffAllButtonsPress();
                NormalButtonPress = true;
                return;
            }
            else
                OffAllButtonsPress();
            if (CursorOnTheButton(hard, 100))
            {
                OffAllButtonsPress();
                HardButtonPress = true;
                return;
            }
            else
                OffAllButtonsPress();

            if (CursorOnTheButton(back, 300))
            {
                OffAllButtonsPress();
                BackButtonPress = true;
                return;
            }
            else
                OffAllButtonsPress();
        }

        private bool CursorOnTheButton(Image buttonIamage, int offset)
        {
            var coursorX = Cursor.Position.X;
            var coursorY = Cursor.Position.Y;

            return Math.Abs(ClientSize.Width / 2 - coursorX) < buttonIamage.Width / 2 &&
              (Math.Abs(ClientSize.Height / 2 + offset - coursorY) < buttonIamage.Height / 2);
        }

        private void OffAllButtonsPress()
        {
            EasyButtonPress = false;
            NormalButtonPress = false;
            HardButtonPress = false;
            BackButtonPress = false;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (EasyButtonPress)
            {
                var level1 = new Level(4, 2, 7, 500, Form.Size);
                Form.ShowBattleControl(level1);
                return;
            }
            if (NormalButtonPress)
            {
                var level2 = new Level(7, 3, 10, 700, Form.Size);
                Form.ShowBattleControl(level2);
                return;

            }
            if (HardButtonPress)
            {
                var level3 = new Level(13, 4, 5, 1000, Form.Size);
                Form.ShowBattleControl(level3);
                return;
            }
            if (BackButtonPress)
            {
                Form.ShowMainMenuControl();
                return;
            }
        }
    }
}

