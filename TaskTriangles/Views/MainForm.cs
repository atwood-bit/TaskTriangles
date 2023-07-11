using System.Collections.Immutable;
using System.Collections.Specialized;
using TaskTriangles.Extensions;
using TaskTriangles.Models;
using TaskTriangles.ViewModels;

namespace TaskTriangles.Views
{
    public partial class MainForm : Form
    {
        private readonly RangeObservableCollection<Triangle> _triangles = new();
        private Panel? _drawPanel;
        private string _color;
        private const KnownColor _defaultColor = KnownColor.MediumAquamarine;
        //private readonly ImmutableDictionary<int, double> _transparencyByLevel = new ImmutableDictionary<int, double>();

        public MainForm(MainViewModel mainViewModel)
        {
            InitializeComponent();

            mainViewModel.AddItemsToViewCollection = _triangles.AddRange;
            mainViewModel.ShowErrorMessage = ShowError;
            //mainViewModel.AddTransparencies = _transparencyByLevel.AddRange;
            DataContext = mainViewModel;
            _color = string.Empty;

            CreateInputPanel();
            CreateDrawPanel();

            _triangles.CollectionChanged += PointsCollectionChanged;
        }

        private void DrawPanel_Paint(object? sender, PaintEventArgs e)
        {
            if (!_triangles.Any())
            {
                return;
            }

            var knownColor = Enum.GetValues<KnownColor>().Cast<KnownColor>().FirstOrDefault(x => x.ToString().Equals(_color), _defaultColor);
            var color = Color.FromKnownColor(knownColor);

            BackColor = color.ChangeColorBrightness(0.9);
            foreach (var triangle in _triangles)
            {
                //_transparencyByLevel.TryGetValue(triangle.DepthLevel.Value, out var transparency);
                e.Graphics.FillTriangle(triangle.Points, Color.FromKnownColor(knownColor), 0); // todo
            }
        }

        private void PointsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            _drawPanel?.Invalidate();
        }

        private void ComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            _color = comboBox?.SelectedItem?.ToString();
        }

        private void CreateInputPanel()
        {
            var inputPanel = new Panel
            {
                Width = 1000,
                Height = 220,
                Dock = DockStyle.Top,
                Location = new Point(10, 10),
                BackColor = Color.Bisque
            };

            inputPanel.Controls.Add(CreateFilePathTextBox());
            inputPanel.Controls.AddRange(CreateLabels());

            inputPanel.Controls.Add(CreateColorComboBox());
            inputPanel.Controls.Add(CreateDrawButton());

            Controls.Add(inputPanel);
        }

        private void CreateDrawPanel()
        {
            var drawPanel = new Panel
            {
                Padding = new Padding(10),
                Width = 1000,
                Height = 1000,
                Location = new Point(10, 220),
            };

            drawPanel.Paint += new PaintEventHandler(DrawPanel_Paint);
            _drawPanel = drawPanel;
            Controls.Add(drawPanel);
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

            var selectColorLabel = new Label
            {
                Location = new Point(10, 100),
                AutoSize = true,
                Text = "Select color"
            };

            return new Label[] { resultMessageLabel, warningMessageLabel, selectColorLabel };
        }

        private Button CreateDrawButton()
        {
            var drawTrianglesBtn = new Button
            {
                Text = "Draw Triangles",
                AutoSize = true,
                Location = new Point(10, 140)
            };

            drawTrianglesBtn.Click += (s, e) =>
            {
                _triangles.Clear();
                //_transparencyByLevel.Clear();
            };
            drawTrianglesBtn.DataBindings.Add(new Binding("Command", DataContext, nameof(MainViewModel.AddCommand), true));

            return drawTrianglesBtn;
        }

        private ComboBox CreateColorComboBox()
        {
            var colors = Enum.GetValues(typeof(KnownColor)).Cast<KnownColor>().Select(x => x.ToString()).ToArray();
            var comboBox = new ComboBox
            {
                Location = new Point(110, 100),
                Width = 200
            };
            comboBox.Items.AddRange(colors);
            comboBox.SelectedItem = _defaultColor.ToString();
            comboBox.SelectedValueChanged += ComboBox_SelectedIndexChanged;

            return comboBox;
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

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}