using System.Windows;
using System.Windows.Controls;

namespace OperatorClient.Pages
{
    public partial class ReportProblemPage : UserControl
    {
        public ReportProblemPage()
        {
            InitializeComponent();
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtProblem.Text))
            {
                MessageBox.Show("Введите описание проблемы");
                return;
            }
            LblResult.Text = "✅ Сообщение отправлено технологу";
            TxtProblem.Clear();
        }
    }
}