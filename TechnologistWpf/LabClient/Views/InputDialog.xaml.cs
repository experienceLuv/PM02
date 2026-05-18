using System.Windows;

namespace LabClient.Views
{
    public partial class InputDialog : Window
    {
        public string Result { get; private set; }

        public InputDialog(string prompt)
        {
            InitializeComponent();
            TxtPrompt.Text = prompt;
            TxtInput.Focus();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Result = TxtInput.Text;
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}