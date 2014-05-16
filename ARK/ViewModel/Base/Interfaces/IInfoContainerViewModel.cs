using System.Windows;

namespace ARK.ViewModel.Base.Interfaces
{
    public interface IInfoContainerViewModel : IViewModelBase
    {
        #region Public Properties

        FrameworkElement CurrentInfo { get; }

        #endregion

        #region Public Methods and Operators

        void ChangeInfo<T>(FrameworkElement infopage, T info);

        #endregion
    }
}