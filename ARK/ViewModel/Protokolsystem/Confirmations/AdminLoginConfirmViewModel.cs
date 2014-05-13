using ARK.Model;
using ARK.Model.DB;
using ARK.View.Administrationssystem;
using ARK.View.Protokolsystem.Confirmations;
using ARK.ViewModel.Administrationssystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace ARK.ViewModel.Protokolsystem.Confirmations
{
    class AdminLoginConfirmViewModel : ConfirmationViewModelBase
    {
        private string _errorLabel;

        public string Username { get; set; }
        public string Password { get; set; }

        public string ErrorLabel
        {
            get { return _errorLabel; }
            set { _errorLabel = value; Notify(); }
        }

        public ICommand Login
        {
            get
            {
                return GetCommand<AdminLoginConfirm>(e =>
                {
                    DbArkContext db = DbArkContext.GetDbContext();

                    Admin admin = db.Admin.Find(Username);

                    if (admin != null && admin.Username == Username && admin.Password == e.PasswordBox.Password )
                    {
                        var window = new AdminSystem();
                        ((AdminSystemViewModel)window.DataContext).CurrentLoggedInUser = admin;
                        window.Show();

                        Hide();
                    }
                    else
                    {
                        ErrorLabel = "Brugernavn eller adgangskode ugyldig!";
                    }
                });
            }
        }
    }
}
