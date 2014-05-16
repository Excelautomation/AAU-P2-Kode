using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using ARK.Model;
using ARK.Model.DB;
using ARK.View.Protokolsystem.Pages;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;

namespace ARK.ViewModel.Protokolsystem.Confirmations
{
    public class DamageFormConfirmViewModel : ConfirmationViewModelBase
    {
        // Fields
        #region Fields

        private readonly DbArkContext db = DbArkContext.GetDbContext();

        private string _comment;

        private DamageForm _damageForm;

        #endregion

        #region Public Properties

        public ICommand Cancel
        {
            get
            {
                return this.GetCommand(
                    e =>
                        {
                            this.Hide();
                            this.ProtocolSystem.StatisticsDistance.Execute(null);
                        });
            }
        }

        public ICommand ChangeForm
        {
            get
            {
                return this.GetCommand(e => { this.Hide(); });
            }
        }

        public string Comment
        {
            get
            {
                return this._comment;
            }

            set
            {
                this._comment = value;
                this.Notify();
            }
        }

        public DamageForm DamageForm
        {
            get
            {
                return this._damageForm;
            }

            set
            {
                this._damageForm = value;
                this.Notify();
            }
        }

        public ICommand Save
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            this.DamageForm.Description = this.Comment;
                            this.DamageForm.Closed = true;

                            this.db.SaveChanges();
                            this.Hide();
                            this.ProtocolSystem.StatisticsDistance.Execute(null);
                        });
            }
        }

        #endregion
    }
}