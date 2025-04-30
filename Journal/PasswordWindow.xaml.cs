using System.Windows;

namespace Journal
{
    public partial class PasswordWindow : Window
    {
        public bool IsUnlocked { get; private set; } = false;
        private bool isSettingNewPassword = false;

        public PasswordWindow()
        {
            InitializeComponent();
            CheckIfPasswordExists();
        }

        private void CheckIfPasswordExists()
        {
            if (!PasswordManager.PasswordExists())
            {
                isSettingNewPassword = true;
                InfoTextBlock.Text = "Set a new password:";
                SubmitButton.Content = "Set Password";
                ChangePasswordButton.Visibility = Visibility.Collapsed;
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string enteredPassword = PasswordBox.Password;

            if (string.IsNullOrEmpty(enteredPassword))
            {
                MessageBox.Show("Password cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (isSettingNewPassword)
            {
                // Save the new password
                PasswordManager.SavePassword(enteredPassword);
                MessageBox.Show("Password set successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                IsUnlocked = true;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                string savedPassword = PasswordManager.LoadPassword();
                if (enteredPassword == savedPassword)
                {
                    IsUnlocked = true;
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Incorrect password. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    PasswordBox.Clear();
                    PasswordBox.Focus();
                }
            }
        }
        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                SubmitButton_Click(this, new RoutedEventArgs());
            }
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string enteredPassword = PasswordBox.Password;
            string savedPassword = PasswordManager.LoadPassword();

            if (enteredPassword == savedPassword)
            {
                // Ask for the new password
                var inputWindow = new NewPasswordWindow();
                if (inputWindow.ShowDialog() == true)
                {
                    string newPassword = inputWindow.NewPassword;
                    PasswordManager.SavePassword(newPassword);
                    MessageBox.Show("Password changed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    PasswordBox.Clear();
                    PasswordBox.Focus();
                }
            }
            else
            {
                MessageBox.Show("Incorrect current password. Cannot change password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                PasswordBox.Clear();
                PasswordBox.Focus();
            }
        }
    }
}
