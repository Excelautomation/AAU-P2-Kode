using System.Windows.Input;

using ARK.Model;
using ARK.Model.DB;

namespace ARK.ViewModel.Protokolsystem.Confirmations
{
    public class DamageFormConfirmViewModel : ConfirmationViewModelBase
    {
        // Fields
        private readonly DbArkContext db = DbArkContext.GetDbContext();

        private string _comment;

        private DamageForm _damageForm;

        public ICommand Cancel
        {
            get
            {
                return GetCommand(
                    e =>
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
                return GetCommand(e => { Hide(); });
            }
        }

        public string Comment
        {
            get
            {
                return _comment;
            }

            set
            {
                _comment = value;
                Notify();
            }
        }

        public DamageForm DamageForm
        {
            get
            {
                return _damageForm;
            }

            set
            {
                _damageForm = value;
                Notify();
            }
        }

        public ICommand Save
        {
            get
            {
                return GetCommand(
                    () =>
                        {
                            DamageForm.Description = Comment;
                            DamageForm.Closed = true;

                            db.SaveChanges();
                            Hide();
                            ProtocolSystem.StatisticsDistance.Execute(null);
                        });
            }
        }
    }
}