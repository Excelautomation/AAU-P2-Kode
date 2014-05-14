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

        public AdminLoginConfirmViewModel()
        {
            ParentAttached += AdminLoginConfirmViewModel_ParentAttached;
        }

        void AdminLoginConfirmViewModel_ParentAttached(object sender, EventArgs e)
        {
            ProtocolSystem.EnableSearch = true;

            // Unbind event
            ParentAttached -= AdminLoginConfirmViewModel_ParentAttached;
        }

        public string ErrorLabel
        {
            get { return _errorLabel; }
            set { _errorLabel = value; Notify(); }
        }

        public override void Hide()
        {
            ProtocolSystem.KeyboardClear();
            ProtocolSystem.EnableSearch = false;
            base.Hide();
        }

        public ICommand Login
        {
            get
            {
                return GetCommand(e =>
                {
                    DbArkContext db = DbArkContext.GetDbContext();

                    Admin admin = db.Admin.Find(Username);

                    if (admin != null && admin.Username == Username && admin.Password == ((AdminLoginConfirm)e).PasswordBox.Password )
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
