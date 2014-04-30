namespace ARK.ViewModel.Base.Interfaces.Info
{
    public interface IInfoContentViewModel<T> : IContentViewModelBase
    {
        T Info { get; set; }
    }
}