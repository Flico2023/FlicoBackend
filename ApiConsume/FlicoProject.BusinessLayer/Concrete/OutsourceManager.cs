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
    public class OutsourceManager : IOutsourceService
    {
        private readonly IOutsourceDal _OutserviceDal;

        public OutsourceManager(IOutsourceDal outserviceDal)
        {
            _OutserviceDal = outserviceDal;
        }

        public int TDelete(int id)
        {
            var outsource = _OutserviceDal.GetByID(id);
            if (outsource == null)
            {
                return 0;
            }
            else
            {
                _OutserviceDal.Delete(outsource);
                return 1;
            }
        }

        public Outsource TGetByID(int id) 
        {
            return _OutserviceDal.GetByID(id);
        }

        public List<Outsource> TGetList()
        {
            return _OutserviceDal.GetList();
        }


        public int TInsert(Outsource t)
        {
            var outsource = _OutserviceDal.GetList().Find(x => x.CompanyName == t.CompanyName);
            if (IsNullOrWhiteSpace(t.CompanyName) || IsNullOrWhiteSpace(t.City) || IsNullOrWhiteSpace(t.Address) || IsNullOrWhiteSpace(t.Email) || IsNullOrWhiteSpace(t.Phone) || IsNullOrWhiteSpace(t.ContactPerson) || outsource != null  )
            {
                return 0;
            }
            else
            {
                _OutserviceDal.Insert(t);
                return 1;
            }
        }

        public int TUpdate(Outsource t)
        {
            var isused = _OutserviceDal.GetList().FirstOrDefault(x => x.CompanyName == t.CompanyName);
            var isvalid = _OutserviceDal.GetList().FirstOrDefault(x => x.OutsourceId == t.OutsourceId);

            if (IsNullOrWhiteSpace(t.CompanyName) || IsNullOrWhiteSpace(t.City) || IsNullOrWhiteSpace(t.Address) || IsNullOrWhiteSpace(t.Email) || IsNullOrWhiteSpace(t.Phone) || IsNullOrWhiteSpace(t.ContactPerson) || isused != null || isvalid == null )
            {
                return 0;
            }
            else
            {
                 _OutserviceDal.Update(t);
                return 1;
            }
        }
    }
}
