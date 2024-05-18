using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DataAccessLayer.Abstract;
using FlicoProject.DataAccessLayer.EntityFramework;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.String;

namespace FlicoProject.BusinessLayer.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;
        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public int TDelete(int id)
        {
            var user = _userDal.GetByID(id);
            if (user == null)
            {
                return 0;
            }
            else
            {
                _userDal.Delete(user);
                return 1;
            }
        }

        public AppUser TGetByID(int id)
        {
            return _userDal.GetByID(id);
        }

        public List<AppUser> TGetList()
        {
            return _userDal.GetList();
        }

        public int TInsert(AppUser t)
        {
            var airport = _userDal.GetList().Find(x => x.Email == t.Email);
            if (IsNullOrWhiteSpace(t.Name) || IsNullOrWhiteSpace(t.Surname) || IsNullOrWhiteSpace(t.Email) || IsNullOrWhiteSpace(t.PasswordHash) || IsNullOrWhiteSpace(t.PhoneNumber) || airport != null)
            {
                return 0;
            }
            else
            {
                _userDal.Insert(t);
                return 1;
            }
        }

        public int TUpdate(AppUser t)
        {
            var isvalid = _userDal.GetList().FirstOrDefault(x => x.Id == t.Id);
            if (IsNullOrWhiteSpace(t.Name) || IsNullOrWhiteSpace(t.Surname) || IsNullOrWhiteSpace(t.Email) || IsNullOrWhiteSpace(t.PasswordHash) || IsNullOrWhiteSpace(t.PhoneNumber) || isvalid == null)
            {
                return 0;
            }
            else
            {
                _userDal.Update(t);
                return 1;
            }
        }
    }
}
