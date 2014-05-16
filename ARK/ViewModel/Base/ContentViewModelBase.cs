using System;
using System.Windows;
using System.Windows.Input;

using ARK.ViewModel.Base.Interfaces;

namespace ARK.ViewModel.Base
{
    public abstract class ContentViewModelBase : ViewModelBase, IContentViewModelBase
    {
        #region Fields

        private IViewModelBase _parent;

        #endregion

        #region Public Events

        public event EventHandler ParentAttached;

        public event EventHandler ParentDetached;

        #endregion

        #region Public Properties

        public ICommand GotFocus
        {
            get
            {
                return this.GetCommand(element => this.GetKeyboard().GotFocus.Execute((FrameworkElement)element));
            }
        }

        public IViewModelBase Parent
        {
            get
            {
                return this._parent;
            }

            set
            {
                if (value != null)
                {
                    // Attach
                    this._parent = value;

                    if (this.ParentAttached != null)
                    {
                        this.ParentAttached(this, new EventArgs());
                    }
                }
                else
                {
                    // Detach
                    if (this.ParentDetached != null)
                    {
                        this.ParentDetached(this, new EventArgs());
                    }

                    this._parent = null;
                }
            }
        }

        #endregion

        #region Public Methods and Operators

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

        #endregion

        #region Methods

        private IKeyboardContainerViewModelBase GetKeyboard()
        {
            return GetKeyboard(this);
        }

        #endregion
    }
}