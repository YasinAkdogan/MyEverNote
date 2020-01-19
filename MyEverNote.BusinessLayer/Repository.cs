using MyEverNote.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEverNote.BusinessLayer
{
    public class Repository<T>:RepositoryBase where T:class//generic olması için T koyduk.bizim classlarımızda işlem yapsın diye t:class yaptık.
    {
        //private DatabaseContext db = new DatabaseContext();//repository base i miras alınca buna gerek kalmadı.
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
        public List<T> List(Expression<Func<T,bool>> where)//tabloya gidip ıd si 1 olan varsa true verecek sonucu eğer yoksa false verecek.
        {
            return _objectSet .Where(where).ToList();
           // db.Categories.Where(x => x.Id == 1).ToList(); yukarıdakinin açıklaması.
        }
        public T Find(Expression<Func<T,bool>> where)//tabloya gidip ıd si 1 olan varsa true verecek sonucu eğer yoksa false verecek.
        {
            return _objectSet.FirstOrDefault(where);          
        }
        public int Insert(T obj)
        {
            _objectSet.Add(obj);
            return save();
        }
        public int Update(T obj)
        {
            return save();//kaydet işlemi felan olmadığı için sadece save desek yeterli olur.
        }

        public int Delete(T obj)
        {
            _objectSet.Remove(obj);
            return save();
        }

        public int save()
        {
          return db.SaveChanges();
        }
    }
}
