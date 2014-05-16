using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ARK.HelperFunctions;
using ARK.Model.DB;
using ARK.Model;
using ARK.View.Administrationssystem;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Administrationssystem
{
    internal class AdminLoginViewModel : ViewModelBase
    {
        #region Fields

        private string _errorLabel;

        #endregion

        #region Public Properties

        public AdminLogin AdminLogin { get; set; }

        public ICommand CloseWindow
        {
            get
            {
                return this.GetCommand(
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

                    if (admin != null && admin.Username == Username && PasswordHashing.VerifyHashedPassword(admin.Password, ((AdminLogin)e).PasswordBox.Password))
                    {
                        var adminSystem = new AdminSystem();
                        ((AdminSystemViewModel)adminSystem.DataContext).CurrentLoggedInUser = admin;
                        adminSystem.Show();

                        ((Window)e).Close();
                    }
                    else
                    {
                                this.ErrorLabel = "Brugernavn eller adgangskode ugyldig";
                    }
                });
            }
        }

        public string Username { get; set; }

        #endregion
    }
}
