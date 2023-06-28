using CrudModels;
using Newtonsoft.Json;
using softaware.Authentication.Hmac.Client;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WPFClientApplication.ViewModel
{
    public class InfoViewModel : ViewModelBase
    {
        public InfoViewModel()
        {
            Console.WriteLine("Info view Started");
            Task.Run(async () => await CustomerEditDetails());
            EditCommand = new ViewModelCommand(ExecuteEditCommand, CanExecuteEditCommand);
            SaveCommand = new ViewModelCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
        }

        private int _username;
        private string _firstname;
        private string _address;
        private bool _isReadOnly = true;
        private int _addressId;
        private string _errorMessage;


        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public int Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                OnPropertyChanged("Username");
            }
        }
        public string Firstname
        {
            get
            {
                return _firstname;
            }
            set
            {
                _firstname = value;
                OnPropertyChanged("Firstname");
            }

        }
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }



        public bool IsReadOnly
        {
            get
            {
                return _isReadOnly;
            }
            set
            {
                _isReadOnly = value;
                OnPropertyChanged(nameof(IsReadOnly));
            }
        }

        public int AddressId
        {
            get
            {
                return _addressId;
            }
            set
            {
                _addressId = value;

            }
        }


        // Commands
        public ICommand SaveCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand RecoverPasswordCommand { get; }

        public ICommand ShowPasswordCommand { get; }

        public ICommand RememberPasswordCommand { get; }



        public void ExecuteEditCommand(object obj)
        {
            IsReadOnly = false;

        }



        public async Task RunAsync()
        {

            var custModel = new CustomerEditDetails
            {
                Id = Username,
                FirstName = Firstname,
                AddressId = AddressId,
                Address = new AddressDetails
                {
                    AddressId = AddressId,
                    Address = Address
                }

            };

            try
            {
                Console.WriteLine("Calling the backend API");
                string apiBaseAddress = $"https://localhost:7172/customer/EditCustomerById/{Username}";
                string appId = "4d53bce03ec34c0a911182d4c228ee6c";
                string apiKey = "A93reRTUJHsCuQSHR+L3GxqOJyDmQpCgps102ciuabc=";
                var customDelegateHandler = new ApiKeyDelegatingHandler(appId, apiKey)
                {
                    InnerHandler = new HttpClientHandler()
                };

                HttpClient _client = new HttpClient(customDelegateHandler);
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(custModel);
                var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PutAsync(apiBaseAddress, stringContent);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseString);
                    Console.WriteLine("Updated Successfully");
                    MessageBox.Show("Updated Successfully Please click on Ok button to go to Home Page", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);

                    MainWindow.ViewIndex = 0;
                    //  MessageBox.Show("Updated Successfully Please click on Ok button to go to Home Page", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                    //MainWindow.ViewIndex = 0;
                }
                else
                {
                    Console.WriteLine("Failed to update the Customer details");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }



        public void UpdateCustomer(int Username)
        {

            Task.Run(async () => await RunAsync());
            Console.ReadLine();

        }



        public void ExecuteSaveCommand(object obj)
        {
            UpdateCustomer(Username);
        }



        private void GetDetails(CustomerEditDetails customer)
        {
            Username = customer.Id;
            Firstname = customer.FirstName;
            Address = customer.Address?.Address;
            AddressId = customer.AddressId;
        }


        public async Task CustomerEditDetails()
        {
            var url = $"https://localhost:7172/customer/GetFullCustomerDetailById/{MainWindow.UserId}";
            string appId = "4d53bce03ec34c0a911182d4c228ee6c";
            string apiKey = "A93reRTUJHsCuQSHR+L3GxqOJyDmQpCgps102ciuabc=";
            var customDelegateHandler = new ApiKeyDelegatingHandler(appId, apiKey)
            {
                InnerHandler = new HttpClientHandler()
            };

            HttpClient _client = new HttpClient(customDelegateHandler);

            HttpResponseMessage response = await _client.GetAsync(url);
            Console.WriteLine("received response");

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                var contentResponse = JsonConvert.DeserializeObject<CustomerEditDetails>(responseString);
                GetDetails(contentResponse);
                Console.WriteLine(responseString);
            }
            else
            {
                Console.WriteLine("Failed to call the api end point");
            }

        }


        private bool CanExecuteEditCommand(object obj)
        {
            bool canExecute;
            if ((Username.GetType() != typeof(int)) || (Firstname == "" || Address == ""))
            {
                canExecute = false;
            }
            else
            {
                canExecute = true;
            }
            return canExecute;
        }



        private bool CanExecuteSaveCommand(object obj)
        {
            bool canExecute;
            if (Firstname == "" || Address == "")
            {
                canExecute = false;
            }
            else
            {
                canExecute = true;
            }
            return canExecute;
        }
    }
}
