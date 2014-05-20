using System;
using System.Windows.Input;

using ARK.HelperFunctions;
using ARK.Model;
using ARK.Model.DB;
using ARK.View.Administrationssystem;
using ARK.View.Protokolsystem.Confirmations;
using ARK.ViewModel.Administrationssystem;

namespace ARK.ViewModel.Protokolsystem.Confirmations
{
    internal class AdminLoginConfirmViewModel : ConfirmationViewModelBase
    {
        private string _errorLabel;

        public AdminLoginConfirmViewModel()
        {
            ParentAttached += AdminLoginConfirmViewModel_ParentAttached;
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
                                    ((AdminLoginConfirm)e).PasswordBox.Password))
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

        public string Password { get; set; }

        public string Username { get; set; }

        public override void Hide()
        {
            ProtocolSystem.KeyboardClear();
            ProtocolSystem.EnableSearch = false;
            base.Hide();
        }

        private void AdminLoginConfirmViewModel_ParentAttached(object sender, EventArgs e)
        {
            ProtocolSystem.EnableSearch = true;

            // Unbind event
            ParentAttached -= AdminLoginConfirmViewModel_ParentAttached;
        }
    }
}