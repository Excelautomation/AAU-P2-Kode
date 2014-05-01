using System;
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
    }
}