namespace ARK.ViewModel.Base
{
    public interface IInfoContentViewModel<T> : IContentViewModelBase
    {
        T Info { get; set; }
    }
}