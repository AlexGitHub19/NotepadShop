using NotepadShop.DAL.EF;
using NotepadShop.DAL.Interfaces;
using System.Data.Entity;
using System.Linq;

namespace NotepadShop.DAL.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        ApplicationContext context;
        DbSet<TEntity> dbSet;
        private bool isDisposed = false;

        public Repository(ApplicationContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public TEntity Get(int id)
        {
            return dbSet.Find(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return dbSet.AsQueryable();
        }

        public void Create(TEntity item)
        {
            dbSet.Add(item);
        }

        public void Update(TEntity item)
        {
            context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            TEntity item = dbSet.Find(id);
            dbSet.Remove(item);
        }
    }
}
