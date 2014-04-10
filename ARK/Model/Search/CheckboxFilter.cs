using System;
using System.Windows;
using System.Windows.Controls;

namespace ARK.Model.Search
{
    public class CheckboxFilter<T> : IFilter<T, CheckBox>
    {
        // TODO fix EventArgs til at indeholde active
        private CheckBox _control;

        public CheckboxFilter(CheckBox checkbox, Action filter)
        {
            this.Control = checkbox;
            this.Filter = filter;

            WrapperEvent += (sender, e) => ActiveChanged(this, new EventArgs());
        }

        public event EventHandler ActiveChanged;
        private event RoutedEventHandler WrapperEvent;

        public bool Active
        {
            get
            {
                return Control.IsChecked.GetValueOrDefault();
            }
            set { Control.IsChecked = value; ActiveChanged(this, new EventArgs()); }
        }

        public CheckBox Control
        {
            get { return _control; }
            private set
            {
                if (value == null) throw new ArgumentNullException("Control");

                _control = value;
                _control.Checked += WrapperEvent;
            }
        }
        public Action Filter { get; private set; }
    }
}