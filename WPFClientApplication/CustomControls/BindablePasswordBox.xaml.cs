using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace WPFClientApplication.CustomControls
{
    /// <summary>
    /// Interaction logic for BindablePasswordBox.xaml
    /// </summary>
    public partial class BindablePasswordBox : UserControl
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(SecureString), typeof(BindablePasswordBox));


        public SecureString Password
        {
            get { return (SecureString)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public BindablePasswordBox()
        {
            InitializeComponent();
            this.txtPassword.PasswordChanged += TxtPassword_PasswordChanged;
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var unsecPass = (sender as PasswordBox).Password;

        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = txtPassword.SecurePassword;
        }
    }
}
