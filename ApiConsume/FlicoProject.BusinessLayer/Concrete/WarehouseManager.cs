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
using Microsoft.EntityFrameworkCore;

namespace FlicoProject.BusinessLayer.Concrete
{
    public class WarehouseManager : IWarehouseService
    {
        private readonly IWarehouseDal _WarehouseDal;

        public WarehouseManager(IWarehouseDal warehouseDal)
        {
            _WarehouseDal = warehouseDal;
        }

        public int TDelete(int id)
        {
            var warehouse = _WarehouseDal.GetByID(id);
            if ( warehouse == null)
            {
                return 0;
            }
            else
            {
                _WarehouseDal.Delete(warehouse);
                return 1;
            }
        }

        public Warehouse TGetByID(int id)
        {
            return _WarehouseDal.GetByID(id);
        }

        public List<Warehouse> TGetList()
        {
            return _WarehouseDal.GetList();
        }

        public int TInsert(Warehouse t)
        {
            var warehouse = _WarehouseDal.GetList().Find(x => x.WarehouseName == t.WarehouseName);
            if (IsNullOrWhiteSpace(t.WarehouseName) || IsNullOrWhiteSpace(t.City)|| warehouse != null)
            {
                return 0;
            }
            else {
                _WarehouseDal.Insert(t);
                return 1;
            }
        }

        public int TUpdate(Warehouse t)
        {

            
            var isused = _WarehouseDal.GetList().FirstOrDefault(x => x.WarehouseName == t.WarehouseName);
            var isvalid = _WarehouseDal.GetList().FirstOrDefault(x => x.WarehouseID == t.WarehouseID);

            if (IsNullOrWhiteSpace(t.WarehouseName) || IsNullOrWhiteSpace(t.City) || isused != null || isvalid == null)
            {
                return 0;
            }
            else
            {
                _WarehouseDal.Update(t);
                return 1;

            }
            
            
        }
    }
}
