using System;
using System.Linq;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;
using ARK.View.Protokolsystem.Pages;

namespace ARK.ViewModel.Protokolsystem.Confirmations
{
    public class DamageFormConfirmViewModel : ConfirmationViewModelBase
    {
        // Fields
        private readonly DbArkContext db = DbArkContext.GetDbContext();
        private DamageForm _damageForm;
        private string _comment;

        public DamageForm DamageForm
        { 
            get { return _damageForm; }
            set { _damageForm = value; Notify(); }
        }

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; Notify(); }
        }

        public ICommand Save
        {
            get
            {
                return GetCommand(() =>
                {
                    //DamageForm.Description = string.Concat(DamageForm.Description, "\n\nKommentar\n", Comment);

                    DamageForm.Description = Comment;
                    DamageForm.Closed = true;

                    db.SaveChanges();
                    Hide();
                    ProtocolSystem.StatisticsDistance.Execute(null);
                });
            }
        }

        public ICommand Cancel
        {
            get
            {
                return GetCommand(e =>
                {
                    Hide();
                    ProtocolSystem.StatisticsDistance.Execute(null);
                });
            }
        }

        public ICommand ChangeForm
        {
            get
            {
                return GetCommand(e =>
                {
                    Hide();
                });
            }
        }
    }
}