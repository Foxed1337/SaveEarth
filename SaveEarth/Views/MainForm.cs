using SaveEarth.MainClasses;
using System.Drawing;
using System.Windows.Forms;

namespace SaveEarth.Views
{
    public partial class MainForm : Form
    {
        private MainMenuControl MainMenuControl;
        private LevelSelectionControl LevelSelection;
        private BattleControl BattleControl;

        public MainForm()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Black;
            Size = SystemInformation.PrimaryMonitorSize;


            Level zeroLevel = new Level(0, 0, 1, 1, new Size(1,1));
            MainMenuControl = new MainMenuControl(this);
            LevelSelection = new LevelSelectionControl(this);
            BattleControl = new BattleControl(zeroLevel, this);
            Controls.Add(MainMenuControl);
            Controls.Add(LevelSelection);
            Controls.Add(BattleControl);
            ShowMainMenuControl();
        }

        private void ChangeBattleControlLevel(Level GameLevel)
        {
            BattleControl.ChangeCurrentLevel(GameLevel);
        }

        public void ShowLevelSelectionComntrol()
        {
            LevelSelection.Show();
            BattleControl.Hide();
            MainMenuControl.Hide();
        }

        public void ShowMainMenuControl()
        {
            MainMenuControl.Show();
            LevelSelection.Hide();
            BattleControl.Hide();
        }

        public void ShowBattleControl(Level GameLevel)
        {
            ChangeBattleControlLevel(GameLevel);
            BattleControl.Show();
            LevelSelection.Hide();
            MainMenuControl.Hide();
        }

    }
}
