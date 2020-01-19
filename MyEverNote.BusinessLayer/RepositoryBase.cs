using MyEverNote.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEverNote.BusinessLayer
{
    public class RepositoryBase
    {
        //Singlton Patern
        protected static DatabaseContext db;

        //lock için
        private static object _lockSync = new object();

        protected RepositoryBase()//protexted:dışarıdan ulaşılamıyor ama miras verilebilir.
        {
            CreateContext();
        }

        private static void CreateContext()
        {
            if (db==null)
            {
                lock (_lockSync)
                {
                    if (db == null)
                    {
                        db = new DatabaseContext();
                    }
                }
            }
        }
    }
}
