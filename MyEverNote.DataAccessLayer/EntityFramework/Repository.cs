using MyEverNote.Common;
using MyEverNote.DataAccessLayer;
using MyEverNote.DataAccessLayer.Abstract;
using MyEverNote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEverNote.DataAccessLayer.EntityFramework
{
    public class Repository<T>:RepositoryBase,IRepository<T> where T:class
    {
        private DbSet<T> _objectSet;
        public Repository()
        {
            _objectSet = db.Set<T>();
        }

        public List<T> List()
        {
           // return db.Set<T>().ToList();
            return _objectSet.ToList();
        }
        public List<T> List(Expression<Func<T,bool>> where)
        {
            return _objectSet .Where(where).ToList();
          
        }
        public T Find(Expression<Func<T,bool>> where)
        {
            return _objectSet.FirstOrDefault(where);          
        }
        public int Insert(T obj)
        {
            _objectSet.Add(obj);
            if (obj is BaseEntity)
            {
               
                BaseEntity o = obj as BaseEntity;
                DateTime Now = DateTime.Now;
                o.CreatedOn = DateTime.Now;
                o.ModifiedOn = DateTime.Now;
                o.ModifiedUserName = App.Common.GetCurrentUserName();
            }
            return Save();
        }
        public int Update(T obj)
        {
            if (obj is BaseEntity)
            {
                BaseEntity o = obj as BaseEntity;
                DateTime Now = DateTime.Now;
                o.ModifiedOn = DateTime.Now;
                o.ModifiedUserName = App.Common.GetCurrentUserName();
            }
            return Save();
        }

        public int Delete(T obj)
        {
            _objectSet.Remove(obj);
            return Save();
        }  
    
        public int Save()
        {
          return db.SaveChanges();
        }

        public IQueryable<T> ListQueryable()
        {
            return _objectSet.AsQueryable<T>();
        }
    }
}
