namespace ARK.ViewModel.Interfaces
{
    public interface IInfoContentViewModel<T> : IContentViewModelBase
    {
        T Info { get; set; }
    }
}