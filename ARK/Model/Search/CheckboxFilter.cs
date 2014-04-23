using System;
using System.Windows.Controls;

namespace ARK.Model.Search
{
    public class CheckboxFilter
    {
        // TODO fix EventArgs til at indeholde active
        private CheckBox _control;

        public CheckboxFilter(CheckBox checkbox, Action updateAction)
        {
            Control = checkbox;
            UpdateAction = updateAction;
        }

        public bool Active
        {
            get
            {
                return Control.IsChecked.GetValueOrDefault();
            }
        }

        public CheckBox Control
        {
            get
            {
                return _control;
            }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Control");
                }

                _control = value;
                _control.Checked += (sender, e) => UpdateAction();
                _control.Unchecked += (sender, e) => UpdateAction();
            }
        }

        public Action UpdateAction { get; private set; }
    }
}