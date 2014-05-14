using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ARK.Model.DB;
using ARK.Model;
using ARK.View.Administrationssystem;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Administrationssystem
{
    class AdminLoginViewModel : ViewModelBase
    {
        private string _errorLabel;

        public string Username { get; set; }
        public AdminLogin AdminLogin { get; set; }
        public string ErrorLabel
        {
            get { return _errorLabel; }
            set { _errorLabel = value; Notify(); }
        }

        public ICommand CloseWindow
        {
            get
            {
                return GetCommand(e =>
                {
                    var window = e as Window;
                    if (window != null) window.Close();
                });
            }
        }

        public ICommand Login
        {
            get
            {
                return GetCommand(e =>
                {
                    DbArkContext db = DbArkContext.GetDbContext();
                    
                    Admin admin = db.Admin.Find(Username);

                    if (admin != null && admin.Username == Username && admin.Password == ((AdminLogin)e).PasswordBox.Password)
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
    }
}
