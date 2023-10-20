using FlicoProject.DataAccessLayer.Abstract;
using FlicoProject.DataAccessLayer.Concrete;
using FlicoProject.DataAccessLayer.Repositories;
using FlicoProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DataAccessLayer.EntityFramework
{
    public class EFWarehouseDal : GenericRepository<Warehouse>, IWarehouseDal
    {
        public EFWarehouseDal(Context context) : base(context)
        {

        }
    }
}
