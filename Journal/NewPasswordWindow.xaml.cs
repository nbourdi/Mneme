using System.Windows;

namespace Journal
{
    public partial class NewPasswordWindow : Window
    {
        public string NewPassword { get; private set; }

        public NewPasswordWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = NewPasswordBox.Password.Trim();
            string confirmPassword = ConfirmPasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Password cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NewPassword = newPassword;
            this.DialogResult = true;
            this.Close();
        }
    }
}
