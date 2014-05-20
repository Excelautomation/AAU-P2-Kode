using System;
using System.Windows;
using System.Windows.Input;

using ARK.ViewModel.Base.Interfaces;

namespace ARK.ViewModel.Base
{
    public abstract class ContentViewModelBase : ViewModelBase, IContentViewModelBase
    {
        private IViewModelBase _parent;

        public event EventHandler ParentAttached;

        public event EventHandler ParentDetached;

        public ICommand GotFocus
        {
            get
            {
                return GetCommand(element => GetKeyboard().GotFocus.Execute((FrameworkElement)element));
            }
        }

        public IViewModelBase Parent
        {
            get
            {
                return _parent;
            }

            set
            {
                if (value != null)
                {
                    // Attach
                    _parent = value;

                    if (ParentAttached != null)
                    {
                        ParentAttached(this, new EventArgs());
                    }
                }
                else
                {
                    // Detach
                    if (ParentDetached != null)
                    {
                        ParentDetached(this, new EventArgs());
                    }

                    _parent = null;
                }
            }
        }

        public static IKeyboardContainerViewModelBase GetKeyboard(IContentViewModelBase content)
        {
            var keyboard = content.Parent as IKeyboardContainerViewModelBase;
            if (keyboard != null)
            {
                return keyboard;
            }

            var parent = content.Parent as IContentViewModelBase;
            if (parent != null)
            {
                return GetKeyboard(parent);
            }

            throw new NotImplementedException();
        }

        private IKeyboardContainerViewModelBase GetKeyboard()
        {
            return GetKeyboard(this);
        }
    }
}