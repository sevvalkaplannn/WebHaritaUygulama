using EFCoreBaşarsoftInternship.Repos; 

namespace EFCoreBaşarsoftInternship.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            doors = new Repo(_dbContext); 
        }

        public IRepo doors { get; private set; }

        public async Task<int> Save()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
