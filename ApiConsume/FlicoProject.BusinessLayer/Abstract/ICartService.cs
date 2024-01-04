using FlicoProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.BusinessLayer.Abstract
{
    public interface ICartService
    {
        int TInsert(Cart t);
        int TDelete(int t);
        int TUpdate(Cart t);
        List<Cart> TGetList();
        List<Cart> TGetByID(int id);
}
}
