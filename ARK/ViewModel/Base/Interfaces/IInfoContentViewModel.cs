namespace ARK.ViewModel.Base.Interfaces
{
    public interface IInfoContentViewModel<T> : IContentViewModelBase
    {
        T Info { get; set; }
    }
}