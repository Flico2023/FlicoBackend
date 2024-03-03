using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.BusinessLayer.Abstract
{
    public interface IFaqService : IGenericService<Faq>
    {
        ResultDTO<Faq> Validate(Faq Faq);
    }
}
