using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.BusinessLayer.Abstract
{
    public interface IGenericService<T> where T : class
    {
        int TInsert(T t);
        int TDelete(int t);
        int TUpdate(T t);
        List<T> TGetList();
        T TGetByID(int id);
    }
}
