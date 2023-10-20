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
    public class AirportManager : IAirportService
    {
        private readonly IAirportDal _AirportDal;

        public AirportManager(IAirportDal airportDal)
        {
            _AirportDal = airportDal;
        }

        public int TDelete(int id)
        {
            var airport = _AirportDal.GetByID(id);
            if (airport == null)
            {
                return 0;
            }
            else
            {
                _AirportDal.Delete(airport);
                return 1;
            }
        }

        public Airport TGetByID(int id)
        {
            return _AirportDal.GetByID(id);
        }

        public List<Airport> TGetList()
        {
            return _AirportDal.GetList();
        }

        public int TInsert(Airport t)
        {
            var airport = _AirportDal.GetList().Find(x => x.AirportName == t.AirportName);
            if (IsNullOrWhiteSpace(t.AirportName) || IsNullOrWhiteSpace(t.City) || airport != null)
            {
                return 0;
            }
            /*else if (_AirportDal.GetList().Count == 0) {
                _AirportDal.Insert(t);
                return 1;
            }*/
            else
            {
                _AirportDal.Insert(t);
                return 1;
            }
        }
      
        public int TUpdate(Airport t)
        {
            var isused = _AirportDal.GetList().FirstOrDefault(x => x.AirportName == t.AirportName);
            var isvalid = _AirportDal.GetList().FirstOrDefault(x => x.AirportID == t.AirportID);

            if (IsNullOrWhiteSpace(t.AirportName) || IsNullOrWhiteSpace(t.City) || isused != null || isvalid == null)
            {
                return 0;
            }
            else
            {
                _AirportDal.Update(t);
                return 1;

            }
        }
    }
}
