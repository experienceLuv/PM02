using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace OperatorClient.Pages
{
    public partial class ExtruderPage : UserControl
    {
        private readonly Random _random = new Random();
        private readonly DispatcherTimer _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };

        public ExtruderPage()
        {
            InitializeComponent();
            Loaded += (s, e) => _timer.Start();
            Unloaded += (s, e) => _timer.Stop();
            _timer.Tick += UpdateTelemetry;
        }

        private void UpdateTelemetry(object sender, EventArgs e)
        {
            // Имитация колебаний температуры
            double temp1 = 78.5 + (_random.NextDouble() * 4 - 2);
            double temp2 = 62.1 + (_random.NextDouble() * 3 - 1.5);
            double pressure = 2.9 + (_random.NextDouble() * 0.8 - 0.4);
            int speed = 120 + _random.Next(-5, 5);

            TxtTemp1.Text = $"{temp1:F1}°C";
            TxtTemp2.Text = $"{temp2:F1}°C";
            TxtPressure.Text = $"{pressure:F1} бар";
            TxtSpeed.Text = $"{speed} об/мин";

            // Простая проверка нормы
            bool ok = temp1 > 75 && temp1 < 85 && temp2 > 55 && temp2 < 65 && pressure > 2.0 && pressure < 3.5 && speed > 100 && speed < 150;
            TxtStatus.Text = ok ? "🟢 Все параметры в норме" : "🔴 Обнаружено отклонение!";
            TxtStatus.Foreground = ok ? System.Windows.Media.Brushes.Green : System.Windows.Media.Brushes.Red;
        }
    }
}