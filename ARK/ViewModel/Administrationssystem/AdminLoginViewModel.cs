using System.Windows;
using System.Windows.Input;

using ARK.HelperFunctions;
using ARK.Model;
using ARK.Model.DB;
using ARK.View.Administrationssystem;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Administrationssystem
{
    internal class AdminLoginViewModel : ViewModelBase
    {
        private string _errorLabel;

        public AdminLogin AdminLogin { get; set; }

        public ICommand CloseWindow
        {
            get
            {
                return GetCommand(
                    e =>
                        {
                            var window = e as Window;
                            if (window != null)
                            {
                                window.Close();
                            }
                        });
            }
        }

        public string ErrorLabel
        {
            get
            {
                return _errorLabel;
            }

            set
            {
                _errorLabel = value;
                Notify();
            }
        }

        public ICommand Login
        {
            get
            {
                return GetCommand(
                    e =>
                        {
                            DbArkContext db = DbArkContext.GetDbContext();

                            Admin admin = db.Admin.Find(Username);

                            if (admin != null && admin.Username == Username
                                && PasswordHashing.VerifyHashedPassword(
                                    admin.Password, 
                                    ((AdminLogin)e).PasswordBox.Password))
                            {
                                var adminSystem = new AdminSystem();
                                ((AdminSystemViewModel)adminSystem.DataContext).CurrentLoggedInUser = admin;
                                adminSystem.Show();

                                ((Window)e).Close();
                            }
                            else
                            {
                                ErrorLabel = "Brugernavn eller adgangskode ugyldig";
                            }
                        });
            }
        }

        public string Username { get; set; }
    }
}