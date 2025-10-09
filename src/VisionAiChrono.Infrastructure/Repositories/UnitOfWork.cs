using Microsoft.EntityFrameworkCore.Storage;
using VisionAiChrono.Domain.RepositoryContract;
using VisionAiChrono.Infrastructure.Data;

namespace VisionAiChrono.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork , IDisposable
    {
        private readonly ApplicationDbContext _db;

        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }
        public IGenericRepository<T> Repository<T>() where T : class
        {
            if(_repositories.ContainsKey(typeof(T)))
            {
                return (IGenericRepository<T>)_repositories[typeof(T)];
            }
            var repository = new GenericRepository<T>(_db);
            _repositories.Add(typeof(T), repository);

            return repository;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _db.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _db.Database.CommitTransactionAsync();
        }

        public async Task<int> CompleteAsync()
        {
            return await _db.SaveChangesAsync();
        }
        public async Task RollbackTransactionAsync()
        {
            await _db.Database.RollbackTransactionAsync();
        }
        public void Dispose()
        {
            _db.Dispose();
        }

    }
}
