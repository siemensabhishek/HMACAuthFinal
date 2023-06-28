using softaware.Authentication.Hmac.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFClientApplication.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        // fields
        private int _username;
        private int _password;
        private string _errorMessage;
        private bool _isViewVisible = true;


        public int Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged("Username"); }
        }
        public int Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged("Password"); }
        }
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }


        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }


        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }



        // Commands
        public ICommand LoginCommand { get; set; }
        public ICommand RecoverPasswordCommand { get; }

        public ICommand ShowPasswordCommand { get; }

        public ICommand RememberPasswordCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RecoverPasswordCommand = new ViewModelCommand(p => ExecuteRecoverPassword("", ""));

        }

        private void ExecuteRecoverPassword(string username, string email)
        {
            throw new NotImplementedException();
        }





        public async void ExecuteLoginCommand(object obj)
        {
            var temp = await IsValidCustomer(Username, Password);
            if (!temp)
            {
                ErrorMessage = "InValid UserName or Password";
            }
            else
            {
                MainWindow.UserId = Username;
                MainWindow.ViewIndex = 1;
            }
        }



        public async Task<bool> IsValidCustomer(int CustId, int password)
        {
            try
            {
                Console.WriteLine("Calling the backend API");
                string apiBaseAddress = $"https://localhost:7172/customer/validCustomer/{CustId}/{password}";
                string appId = "4d53bce03ec34c0a911182d4c228ee6c";
                string apiKey = "A93reRTUJHsCuQSHR+L3GxqOJyDmQpCgps102ciuabc=";
                var customDelegateHandler = new ApiKeyDelegatingHandler(appId, apiKey)
                {
                    InnerHandler = new HttpClientHandler()
                };
                HttpClient _client = new HttpClient(customDelegateHandler);
                HttpResponseMessage response = await Task.Run(() => _client.GetAsync(apiBaseAddress));
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseString);
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to call the api end point");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }




        private bool CanExecuteLoginCommand(object obj)
        {
            bool canExecute;

            if (Username != 0 && Password != 0)
            {
                canExecute = true;
            }

            else
            {
                canExecute = false;
            }
            return canExecute;
        }
    }
}
