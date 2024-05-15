using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using SuperCat.MyObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SuperCat.Paterns
{
    internal class MyUnitOfWork : IMyUnitOfWork
    {
        private readonly DbContext _context;
        public MyRepository<UserInfo> userRepository;
        private bool disposed = false;

        public MyUnitOfWork(DbContext context)
        {
            _context = context;
            userRepository = new MyRepository<UserInfo>(_context);
        }

        public IMyRepository<UserInfo> UserRepository
        {
            get
            {
                if (userRepository == null)
                    userRepository = new MyRepository<UserInfo>(_context);
                return userRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
