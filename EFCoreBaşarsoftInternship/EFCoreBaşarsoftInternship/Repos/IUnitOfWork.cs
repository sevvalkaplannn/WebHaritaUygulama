namespace EFCoreBaşarsoftInternship.Repos
{
    public interface IUnitOfWork :IDisposable
    {

        IRepo doors { get; }
        Task<int> Save();
    }
}
