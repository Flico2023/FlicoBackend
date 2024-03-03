using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DataAccessLayer.Abstract;
using FlicoProject.DataAccessLayer.EntityFramework;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.String;

namespace FlicoProject.BusinessLayer.Concrete
{
    public class FaqManager : IFaqService
    {
        private readonly IFaqDal _FaqDal;

        public FaqManager(IFaqDal FaqDal)
        {
            _FaqDal = FaqDal;
        }

        public ResultDTO<Faq> Validate(Faq t)
        {
            string error = "";

            if (IsNullOrWhiteSpace(t.Question) || t.Question.Length > 100)
            {
                error = "Soru boş veya çok uzun";
            }
            if (IsNullOrWhiteSpace(t.Answer) || t.Answer.Length > 500)
            {
                error = "Cevap boş veya çok uzun";
            }
            if (IsNullOrWhiteSpace(t.Category) || t.Category.Length > 100)
            {
                error = "Kategori boş veya çok uzun";
            }
            
            if (error.Length > 0)
            {
                return new ResultDTO<Faq>(error);
               
            }

            return new ResultDTO<Faq>(t);
        }   

        public int TDelete(int id)
        {
            var Faq = _FaqDal.GetByID(id);
            if (Faq == null)
            {
                return 0;
            }
            else
            {
                _FaqDal.Delete(Faq);
                return 1;
            }
        }

        public Faq TGetByID(int id)
        {
            return _FaqDal.GetByID(id);
        }

        public List<Faq> TGetList()
        {
            return _FaqDal.GetList();
        }

        public int TInsert(Faq t)
        {
            var Faq = _FaqDal.GetByID(t.FaqID);
            if (IsNullOrWhiteSpace(t.Question) || IsNullOrWhiteSpace(t.Answer) || IsNullOrWhiteSpace(t.Category) || Faq != null)
            {
                return 0;
            }

            else
            {
                _FaqDal.Insert(t);
                return 1;
            }
        }
      
        public int TUpdate(Faq t)
        {
            //var isused = _FaqDal.GetList().FirstOrDefault(x => x.Question == t.Question);
            var isvalid = _FaqDal.GetList().FirstOrDefault(x => x.FaqID == t.FaqID);

            if (IsNullOrWhiteSpace(t.Question) || IsNullOrWhiteSpace(t.Answer) || IsNullOrWhiteSpace(t.Category) || isvalid == null)
            {
                return 0;
            }
            else
            {        
                _FaqDal.Update(t);
                return 1;
            }
        }
    }
}
