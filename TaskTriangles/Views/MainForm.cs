using TaskTriangles.Extensions;
using TaskTriangles.ViewModels;

namespace TaskTriangles.Views
{
    public partial class MainForm : Form
    {
        private List<Point[]> _points = new();

        public MainForm(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel;
            CreateControls();
            // test
            _points = GetPoints();

        }

        // todo: test
        private List<Point[]> GetPoints()
        {
            var points = new Point[3]
            {
                new Point { X = 90, Y = 30 },
                new Point { X = 90, Y = 40 },
                new Point { X = 80, Y = 40 },
            };
            var points2 = new Point[3]
            {
                new Point { X = 150, Y = 130 },
                new Point { X = 0, Y = 90 },
                new Point { X = 100, Y = 0 },
            };
            var points3 = new Point[3]
            {
                new Point { X = 20, Y = 80 },
                new Point { X = 80, Y = 70 },
                new Point { X = 50, Y = 100 },
            };
            var points4 = new Point[3]
            {
                new Point { X = 60, Y = 100 },
                new Point { X = 120, Y = 80 },
                new Point { X = 140, Y = 120 },
            };
            var points5 = new Point[3]
            {
                new Point { X = 100, Y = 100 },
                new Point { X = 120, Y = 100 },
                new Point { X = 120, Y = 90 },
            };
            var points6 = new Point[3]
            {
                new Point { X = 30, Y = 70 },
                new Point { X = 100, Y = 10 },
                new Point { X = 110, Y = 60 },
            };
            var points7 = new Point[3]
            {
                new Point { X = 60, Y = 50 },
                new Point { X = 60, Y = 60 },
                new Point { X = 90, Y = 60 },
            };
            var points8 = new Point[3]
            {
                new Point { X = 100, Y = 20 },
                new Point { X = 100, Y = 50 },
                new Point { X = 70, Y = 40 },
            };

            return new List<Point[]>
            {
                points, points2, points3, points4, points5, points6, points7, points8
            };
        }

        private void MainWindow_Paint(object sender, PaintEventArgs e)
        {
            var paintPanel = new Panel
            {
                Padding = new Padding(100),
                Width = 1000,
                Height = 1000,
                Dock = DockStyle.Fill,
                Location = new Point(0, 200),
            };


            Controls.Add(paintPanel);

            Graphics g = paintPanel.CreateGraphics();

            g.FillTriangle(_points[0], Color.Green, 5 / 100.0);
            g.FillTriangle(_points[1], Color.GreenYellow, 5 / 100.0);
            g.FillTriangle(_points[2], Color.DarkGreen, 5 / 100.0);
            g.FillTriangle(_points[3], Color.Red, 5 / 100.0);
            g.FillTriangle(_points[4], Color.Purple, 5 / 100.0);
            g.FillTriangle(_points[5], Color.SeaGreen, 5 / 100.0);
            g.FillTriangle(_points[6], Color.Brown, 5 / 100.0);
            g.FillTriangle(_points[7], Color.Pink, 5 / 100.0);
        }

        private void CreateControls()
        {
            var inputPanel = new Panel
            {
                Padding = new Padding(10),
                Width = 600,
                Height = 200,
                Dock = DockStyle.Left,
                Location = new Point(500, 500)
            };

            inputPanel.Controls.Add(CreateFilePathTextBox());
            inputPanel.Controls.AddRange(CreateLabels());
            inputPanel.Controls.Add(CreateDrawButton());

            //var paintPanel = new Panel
            //{
            //    Padding = new Padding(20),
            //    Width = 1000,
            //    Height = 1000,
            //    Dock = DockStyle.Fill,
            //    Location = new Point(0, 200),
            //};

            Controls.Add(inputPanel);
            //Controls.Add(paintPanel);
        }

        private Label[] CreateLabels()
        {
            var resultMessageLabel = new Label
            {
                Location = new Point(10, 40),
                AutoSize = true,
            };
            resultMessageLabel.DataBindings.Add(new Binding(nameof(Label.Text), DataContext, nameof(MainViewModel.ResultMessage), true, DataSourceUpdateMode.OnPropertyChanged));

            var warningMessageLabel = new Label
            {
                Location = new Point(10, 80),
                AutoSize = true,
            };
            warningMessageLabel.DataBindings.Add(new Binding(nameof(Label.Text), DataContext, nameof(MainViewModel.WarningMessage), true, DataSourceUpdateMode.OnPropertyChanged));

            return new Label[] { resultMessageLabel, warningMessageLabel };
        }

        private Button CreateDrawButton()
        {
            var drawTrianglesBtn = new Button
            {
                Text = "Draw Triangles",
                AutoSize = true,
                Location = new Point(10, 110)
            };

            drawTrianglesBtn.DataBindings.Add(new Binding("Command", DataContext, nameof(MainViewModel.AddCommand), true));

            //button.DataBindings.Add(new Binding(nameof(Button.CommandParameter), DataContext, nameof(MainViewModel.FilePath)));

            return drawTrianglesBtn;
        }

        private TextBox CreateFilePathTextBox()
        {
            var filePathTextBox = new TextBox
            {
                Location = new Point(10, 10),
                Size = new Size(400, 30),
            };
            filePathTextBox.DataBindings.Add(new Binding(nameof(TextBox.Text), DataContext, nameof(MainViewModel.FilePath), true, DataSourceUpdateMode.OnPropertyChanged));

            return filePathTextBox;
        }
    }
}