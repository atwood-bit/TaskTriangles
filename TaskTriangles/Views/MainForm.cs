using Microsoft.Extensions.Options;
using System.Collections.Specialized;
using TaskTriangles.Enums;
using TaskTriangles.Extensions;
using TaskTriangles.Models;
using TaskTriangles.ViewModels;
using TaskTriangles.ViewModels.Interfaces;

namespace TaskTriangles.Views
{
    public partial class MainForm : Form, IObserver
    {
        private readonly RangeObservableCollection<Triangle> _triangles = new();
        private readonly AppSettings _appSettings;

        private Panel? _drawPanel;
        private string _color;
        private double _transparencyStep;

        private const KnownColor DEFAULT_COLOR = KnownColor.MediumAquamarine;

        public MainForm(MainViewModel mainViewModel, IOptions<AppSettings> settings)
        {
            InitializeComponent();
            _appSettings = settings.Value;

            mainViewModel.RegisterObserver(this);
            mainViewModel.Color = DEFAULT_COLOR.ToString();
            DataContext = mainViewModel;
            _color = string.Empty;

            CreateInputPanel();
            CreateDrawPanel();

            FormClosed += (e, s) =>
            {
                mainViewModel.RemoveObserver(this);
            };
            _triangles.CollectionChanged += PointsCollectionChanged;
        }

        private void DrawPanel_Paint(object? sender, PaintEventArgs e)
        {
            if (!_triangles.Any())
            {
                return;
            }

            var knownColor = Enum.GetValues<KnownColor>().Cast<KnownColor>()
                .FirstOrDefault(x => x.ToString().Equals(_color), DEFAULT_COLOR);
            var color = Color.FromKnownColor(knownColor);

            (sender as Panel).BackColor = color.ChangeColorBrightness(_appSettings.MaxRangeOfTransparency);
            foreach (var triangle in _triangles)
            {
                var transparency = triangle.GetTransparency(_appSettings.MaxRangeOfTransparency, _appSettings.MinRangeOfTransparency, _transparencyStep);
                e.Graphics.FillTriangle(triangle.Points, Color.FromKnownColor(knownColor), transparency, true);
            }
        }

        private void PointsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            _drawPanel?.Invalidate();
        }

        private void CreateInputPanel()
        {
            var inputPanel = new Panel
            {
                Width = 1000,
                Height = 180,
                Dock = DockStyle.Top,
                Location = new Point(10, 10),
                BackColor = Color.Bisque
            };

            inputPanel.Controls.AddRange(CreateFilePathTextBox());
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
                BorderStyle = BorderStyle.FixedSingle,
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
                Text = "Select a color"
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
            comboBox.DataBindings.Add(new Binding(nameof(ComboBox.SelectedItem), DataContext, nameof(MainViewModel.Color), true, DataSourceUpdateMode.OnPropertyChanged));

            return comboBox;
        }

        private Control[] CreateFilePathTextBox()
        {
            var filePathLabel = new Label
            {
                Location = new Point(10, 10),
                Size = new Size(100, 30),
                Text = "Path to file"
            };

            var filePathTextBox = new TextBox
            {
                Location = new Point(90, 10),
                Size = new Size(600, 30),
            };
            filePathTextBox.DataBindings.Add(new Binding(nameof(TextBox.Text), DataContext, nameof(MainViewModel.FilePath), true, DataSourceUpdateMode.OnPropertyChanged));

            return new Control[] { filePathTextBox, filePathLabel };
        }

        private void ShowError(string? message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void Update(object model, NotifyAction action)
        {
            if (model is null) return;

            switch (action)
            {
                case NotifyAction.Success:
                    if (model is ResultModel result)
                    {
                        _transparencyStep = result.TransparencyStepByLevel;
                        _color = result.Color;
                        _triangles.AddRange(result.Items);
                    }
                    break;
                case NotifyAction.Error:
                    ShowError(model.ToString());
                    break;
            }
        }
    }
}