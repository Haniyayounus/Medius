using Medius.Data.Entities;
using System;

namespace Medius.MobileApplication.Model.IRepository
{
    public interface IUsers : IDisposable
    {
        void SignUp(User user);
        User Login(User user);
    }
}
