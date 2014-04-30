using System.Windows;

namespace ARK.ViewModel.Base.Interfaces.Info
{
    public interface IInfoContainerViewModel : IViewModelBase
    {
        FrameworkElement CurrentInfo { get; }
        void ChangeInfo<T>(FrameworkElement infopage, T info);
    }
}