using System.Runtime.CompilerServices;

namespace Q8E6EZ_labirintus
{
    public partial class Form1 : Form
    {
        private PictureBox player = new PictureBox();
        private Label stepsLabel = new Label();
        private Label timerLabel = new Label();
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private List<PictureBox> walls = new List<PictureBox>();
        private int steps = 0;
        private int timeSpent = 0;
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.Load += Form1_Load; 

            stepsLabel.Location = new Point(50, 0);
            stepsLabel.AutoSize = true;
            stepsLabel.BackColor = Color.Black; 
            stepsLabel.ForeColor=Color.White;
            stepsLabel.Text = "Steps: 0";
            this.Controls.Add(stepsLabel);

            timerLabel.Location = new Point(120, 0);
            timerLabel.AutoSize = true;
            timerLabel.BackColor = Color.Black;
            timerLabel.ForeColor = Color.White;
            timerLabel.Text = "Time: 0";
            this.Controls.Add(timerLabel);
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Select maze file",
                Filter = "Text Files|*.txt",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            if (ofd.ShowDialog()==DialogResult.OK)
            {
                try
                {
                    using (StreamReader streamReader = new StreamReader(ofd.FileName))
                    {
                        int sor = 0;
                        while (!streamReader.EndOfStream)
                        {
                            string row = streamReader.ReadLine();
                            for (int oszlop=0; oszlop<row.Length; oszlop++)
                            {
                                if (row[oszlop] == '#')
                                {
                                    PictureBox pictureBox = new PictureBox();
                                    pictureBox.Size = new Size(20, 20);
                                    pictureBox.Location = new Point(oszlop*20, sor*20);
                                    pictureBox.BackColor = Color.DarkSalmon;
                                    this.Controls.Add(pictureBox);
                                    walls.Add(pictureBox);
                                }
                            }
                            sor++;
                        }
                        int FormWidth = (walls.Max(w => w.Location.X ) / 20 + 1) * 20;
                        int FormHeight = (walls.Max(w => w.Location.Y) / 20 + 1) * 20;
                        this.ClientSize = new Size(FormWidth, FormHeight);
                    }
                    player.Location = new Point(0, 0);
                    player.Size = new Size(20, 20);
                    player.BackColor = Color.CornflowerBlue;
                    this.Controls.Add(player);
                    KeyDown += Form1_KeyDown;
                    timer.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba történt a fájl beolvasával: " +ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Nem jelöltél ki fájlt. " , "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            timeSpent++;
            timerLabel.Text = $"Time: {timeSpent} s";
        }
        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            int x = player.Location.X;
            int y = player.Location.Y;
            if(e.KeyCode == Keys.Left) 
            {
                x -= 20;
            }
            if (e.KeyCode == Keys.Right)
            {
                x += 20;
            }
            if (e.KeyCode == Keys.Up)
            {
                y -= 20;
            }
            if (e.KeyCode == Keys.Down)
            {
                y += 20;
            }
            if (x<0 || y<0 || x >= this.ClientSize.Width || y >= this.ClientSize.Height)
            {
                return;
            }
            var wall = walls.FirstOrDefault(w => w.Location.X == x && w.Location.Y == y);
            if (wall == null) 
            { 
                player.Location = new Point(x, y);
                steps++;
                stepsLabel.Text = $"Steps: {steps}";
            }
            int FormWidth = (walls.Max(w => w.Location.X) / 20) * 20;
            int FormHeight = (walls.Max(w => w.Location.Y) / 20) * 20;
            if (x == FormWidth || y == FormHeight)
            {
                MessageBox.Show($"Gratula, kijutottál! Eltelt idõ: {timeSpent}, lépések száma: {steps}.");
                this.Close();
            }
        }
    }
}