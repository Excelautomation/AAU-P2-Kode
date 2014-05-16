using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

using ARK.Model;
using ARK.Model.DB;
using ARK.View.Administrationssystem;
using ARK.View.Protokolsystem.Confirmations;
using ARK.ViewModel.Administrationssystem;

namespace ARK.ViewModel.Protokolsystem.Confirmations
{
    internal class AdminLoginConfirmViewModel : ConfirmationViewModelBase
    {
        #region Fields

        private string _errorLabel;

        #endregion

        #region Constructors and Destructors

        public AdminLoginConfirmViewModel()
        {
            this.ParentAttached += this.AdminLoginConfirmViewModel_ParentAttached;
        }

        #endregion

        #region Public Properties

        public string ErrorLabel
        {
            get
            {
                return this._errorLabel;
            }

            set
            {
                this._errorLabel = value;
                this.Notify();
            }
        }

        public ICommand Login
        {
            get
            {
                return this.GetCommand(
                    e =>
                        {
                            DbArkContext db = DbArkContext.GetDbContext();

                            Admin admin = db.Admin.Find(this.Username);

                            if (admin != null && admin.Username == this.Username
                                && admin.Password == ((AdminLoginConfirm)e).PasswordBox.Password)
                            {
                                var window = new AdminSystem();
                                ((AdminSystemViewModel)window.DataContext).CurrentLoggedInUser = admin;
                                window.Show();

                                this.Hide();
                            }
                            else
                            {
                                this.ErrorLabel = "Brugernavn eller adgangskode ugyldig!";
                            }
                        });
            }
        }

        public string Password { get; set; }

        public string Username { get; set; }

        #endregion

        #region Public Methods and Operators

        public override void Hide()
        {
            this.ProtocolSystem.KeyboardClear();
            this.ProtocolSystem.EnableSearch = false;
            base.Hide();
        }

        #endregion

        #region Methods

        private void AdminLoginConfirmViewModel_ParentAttached(object sender, EventArgs e)
        {
            this.ProtocolSystem.EnableSearch = true;

            // Unbind event
            this.ParentAttached -= this.AdminLoginConfirmViewModel_ParentAttached;
        }

        #endregion
    }
}