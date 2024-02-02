using AutoMapper;
using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DataAccessLayer.Migrations;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.String;

namespace FlicoProject.WebApi.Controllers
{
    [Route("api/Faqs")]
    [ApiController]
    public class FaqController : ControllerBase
    {
        
        private readonly IFaqService _faqService;
        private readonly IMapper _mapper;

        public FaqController(IFaqService faqService, IMapper mapper)
        {
            _faqService = faqService;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult FaqList()
        {
            var Faqs = _faqService.TGetList();

            // Dinamik kategorileri çıkarın
            var categories = Faqs.Select(f => f.Category).Distinct().ToList();

            // Kategorilere göre ayrıştırma işlemi
            var faqsByCategory = new Dictionary<string, List<Faq>>();
            foreach (var category in categories)
            {
                faqsByCategory[category] = Faqs.Where(f => f.Category == category).ToList();
            }

            return Ok(new ResultDTO<Dictionary<string, List<Faq>>>(faqsByCategory));
        }
        [HttpPost]
        public IActionResult AddFaq(FaqDto Faqdto)
        {

            var Faq = new Faq();
            Faq = _mapper.Map<Faq>(Faqdto);

            var result = _faqService.Validate(Faq);
            if (result.Success != true)
            {
                return BadRequest(new ResultDTO<Faq>(result.Message));
            }

            if (_faqService.TInsert(Faq) == 1)
            {
                return Created("", new ResultDTO<Faq>(Faq));
            }
            else
            {
                return BadRequest(new ResultDTO<Faq>("Form values are not valid."));
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteFaq(int id)
        {
            var Faqid = _faqService.TDelete(id);
            if (Faqid == 0)
            {
                return BadRequest(new ResultDTO<Faq>("The id to be deleted was not found."));
            }
            else
            {
                var Faq = _faqService.TGetByID(id);
                _faqService.TDelete(id);

                return Ok(new ResultDTO<Faq>(Faq));
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateFaq(int id,FaqDto FaqDto)
        {
            var Faq = _mapper.Map<Faq>(FaqDto);
            Faq.FaqID = id;

            var validation = _faqService.Validate(Faq);
            if (validation.Success != true)
            {
                return BadRequest(new ResultDTO<Faq>(validation.Message));
            }

            int result = _faqService.TUpdate(Faq);
            if (result == 0)
            {
                return BadRequest(new ResultDTO<Faq>("The Faq wanted to update could not be updated."));
            }
            else
            {
                return Ok(new ResultDTO<Faq>(Faq));
            }

        }
        [HttpGet("{id}")]
        public IActionResult GetFaq(int id)
        {
            var Faq = _faqService.TGetByID(id);
            if (Faq == null)
            {
                return BadRequest(new ResultDTO<Faq>("The id to be looking for was not found."));
            }
            return Ok(new ResultDTO<Faq>(Faq));
        }
    }
}
