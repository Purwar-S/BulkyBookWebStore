using System.Linq.Expressions;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public DbSet<T> dbset { get; set; }
        private readonly ApplicationDbContext _db;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbset = db.Set<T>();
            //_db.Categories = dbset;
        }
        public IEnumerable<T> GetAll()
        {
            return dbset.ToList();
        }

        public T GetValue(Expression<Func<T, bool>> filter)
        {
            var entity = dbset.Where(filter).FirstOrDefault();
            return entity;
        }

        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbset.RemoveRange(entity);
        }

    }
}
