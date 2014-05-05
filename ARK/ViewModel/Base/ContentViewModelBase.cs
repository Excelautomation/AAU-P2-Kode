using System;
using System.Windows;
using System.Windows.Input;
using ARK.ViewModel.Base.Interfaces;

namespace ARK.ViewModel.Base
{
    public abstract class ContentViewModelBase : ViewModelBase, IContentViewModelBase
    {
        private IViewModelBase _parent;

        public IViewModelBase Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;

                if (ParentAttached != null && _parent != null) ParentAttached(this, new EventArgs());
            }
        }

        public event EventHandler ParentAttached;

        public virtual void ParentDetached()
        {
            _parent = null;
        }

        private KeyboardContainerViewModelBase GetKeyboard()
        {
            return GetKeyboard(this);
        }

        public static KeyboardContainerViewModelBase GetKeyboard(IContentViewModelBase content)
        {
            var keyboard = content.Parent as KeyboardContainerViewModelBase;
            if (keyboard != null)
                return keyboard;

            var parent = content.Parent as IContentViewModelBase;
            if (parent != null)
                return GetKeyboard(parent);

            throw new NotImplementedException();
        }

        public ICommand GotFocus
        {
            get { return GetCommand<FrameworkElement>(element => GetKeyboard().GotFocus.Execute(element)); }
        }
    }
}