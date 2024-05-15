using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using SuperCat.MyObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCat
{
    public class MyRepository<T> : IMyRepository<T> where T : class
    {
        private readonly DbContext context;
        private readonly DbSet<T> dbSet;

        public MyRepository(DbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.AsNoTracking().ToList();
        }
        public T Get(int id)
        {
            return dbSet.Find(id) ?? dbSet.First();
        }
        public void Add(T item)
        {
            dbSet.Add(item);
        }

        public void Create(T item)
        {
            dbSet.Add(item);
            context.SaveChanges();
        }

        public void Update(T item)
        {
            context.Entry(item).State = EntityState.Modified;
        }
        public void Updated(T item)
        {
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            T item = dbSet.Find(id)!;
            if (item != null)
                dbSet.Remove(item);
            context.SaveChanges();
        }

        public void Dek(int id)
        {
            T item = dbSet.Find(id)!;
            if (item != null)
                dbSet.Remove(item);
        }
    }
}
