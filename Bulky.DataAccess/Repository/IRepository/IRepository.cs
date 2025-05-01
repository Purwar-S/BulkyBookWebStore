using System.Linq.Expressions;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T: class
    {
        IEnumerable<T>  GetAll();
        T GetValue(Expression<Func<T,bool>> filter);
        void Add(T entity);

        //To implement according to the internal model logic
        //void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);

    }
}
