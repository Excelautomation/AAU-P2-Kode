using System;
using ARK.ViewModel.Interfaces;

namespace ARK.ViewModel
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

                if (ParentAttached != null) ParentAttached(this, new EventArgs());
            }
        }

        public event EventHandler ParentAttached;
    }
}