using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ARK.Model.DB;
using ARK.Model;
using ARK.Administrationssystem;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Administrationssystem
{
    class AdminLoginViewModel : ViewModelBase
    {
        private string _errorLabel;

        public string Username { get; set; }
        public string Password { get; set; }
        public AdminLogin AdminLogin { get; set; }
        public string ErrorLabel { get { return _errorLabel; } set { _errorLabel = value; Notify(); } }

        public ICommand Login
        {
            get
            {
                return GetCommand<Window>(e =>
                {
                    DbArkContext db = DbArkContext.GetDbContext();
                    
                    Admin admin = db.Admin.Find(Username);

                    if (admin != null && admin.Username == Username && admin.Password == Password)
                    {
                        AdminSystem window = new AdminSystem();
                        window.Show();
                        ((AdminSystemViewModel)window.DataContext).CurrentLoggedInUser = admin;

                        e.Close();
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
