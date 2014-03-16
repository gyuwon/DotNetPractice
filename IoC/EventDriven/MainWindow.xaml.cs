using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace EventDriven
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnInputTextChanged(object sender, TextChangedEventArgs e)
        {
            string s = this.Input.Text;
            if (string.IsNullOrWhiteSpace(s))
            {
                this.Square.Text = string.Empty;
            }
            else
            {
                double number;
                if (double.TryParse(s, out number))
                {
                    double square = number * number;
                    this.Square.Text = square.ToString();
                }
                else
                {
                    this.Square.Text = "Not a number";
                }
            }
        }
    }
}
