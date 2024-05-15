using Microsoft.VisualBasic.ApplicationServices;
using SuperCat.MyObjects;

namespace SuperCat.Paterns
{
    internal interface IMyUnitOfWork:IDisposable
    {
        IMyRepository<UserInfo> UserRepository { get; }
        void Save();
    }
}
