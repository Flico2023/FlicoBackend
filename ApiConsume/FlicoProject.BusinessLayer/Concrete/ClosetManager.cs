using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DataAccessLayer.Abstract;
using FlicoProject.DataAccessLayer.EntityFramework;
using FlicoProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.String;

namespace FlicoProject.BusinessLayer.Concrete
{
    public class ClosetManager : IClosetService
    {
        private readonly IClosetDal _ClosetDal;

        public ClosetManager(IClosetDal closetDal) {
            _ClosetDal = closetDal;
        }


        public int TDelete(int id)
        {
            var closet = _ClosetDal.GetByID(id);
            if (closet == null) 
            {
                return 0;
            } 
            else
            { 
                _ClosetDal.Delete(closet);
                return 1; 
            }
        }

        public Closet TGetByID(int id)
        {
            return _ClosetDal.GetByID(id);
        }

        public List<Closet> TGetList() 
        { 
            return _ClosetDal.GetList();
        }
        public int TInsert(Closet t)
        {
            var closet = _ClosetDal.GetList().Find(x => x.ClosetNo == t.ClosetNo);
            if(IsNullOrWhiteSpace(t.Status) || t.ClosetNo < 0 || t.AirportID < 0 || t.OrderID < 0 || t.Password < 0 || closet != null )
            { 
                return 0;
            }
            else
            {
                _ClosetDal.Insert(t);
                return 1;
            }
        }
        public int TUpdate(Closet t )
        {
            var isused = _ClosetDal.GetList().FirstOrDefault(x => x.ClosetNo == t.ClosetNo);
            var isvalid = _ClosetDal.GetList().FirstOrDefault(x => x.ClosetID == t.ClosetID);

            if (IsNullOrWhiteSpace(t.Status) || t.ClosetNo < 0 || t.AirportID < 0 || t.OrderID < 0 || t.Password < 0 || isused != null || isvalid == null)
            {
                return 0;
            }
            else
            {
                _ClosetDal.Update(t);
                return 1;
            }

        }
    }
}
