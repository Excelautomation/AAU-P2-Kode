using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Search;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ARK.ViewModel.Base;
using System.Windows.Input;
using ARK.Administrationssystem.Pages;

namespace ARK.ViewModel
{
    public class FormsDamageViewModel : Base.ViewModel
    {
        public DamageForm DamageForm { get; set; }

        public ICommand DeaktiverBåd
        {
            get
            {
                return GetCommand<DamageForm>(e => 
                { 
                    DamageForm = e; 
                    DamageForm.Boat.Active = false; 
                    DamageForm.Boat.Usable = false;
                });
            }
        }

        public ICommand AktiverBåd
        {
            get
            {
                return GetCommand<DamageForm>(e =>
                {
                    DamageForm = e;
                    DamageForm.Boat.Active = true;
                    DamageForm.Boat.Usable = true;
                });
            }
        }
    }
}
