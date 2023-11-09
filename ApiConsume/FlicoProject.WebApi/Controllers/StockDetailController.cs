using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using static System.String;

namespace FlicoProject.WebApi.Controllers
{
    [Route("api/stockdetail")]
    [ApiController]
    public class StockDetailController : ControllerBase
    {

        private readonly IStockDetailService _stockDetailservice;
        private readonly IWebHostEnvironment _environment;

        public StockDetailController(IStockDetailService stockDetailservice, IWebHostEnvironment environment)
        {
            _stockDetailservice = stockDetailservice;
            _environment = environment;
        }
        [HttpGet]
        public IActionResult StockDetailList()
        {
            var stockDetails = _stockDetailservice.TGetList();
            return Ok(new ResultDTO<List<StockDetail>>(stockDetails));
        }
        [HttpPost]
        public async Task<IActionResult> AddStockDetail([FromForm] StockDetail stockDetail)
        {
            if (stockDetail.Image != null && stockDetail.Image.Length > 0)
            {
                // Güvenli bir dosya adı oluşturun ve dosya yolu oluşturun
                var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(stockDetail.Image.FileName);
                var filePath = Path.Combine(_environment.WebRootPath, "product_images", fileName);

                // Klasör yoksa oluştur
                var directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Dosyayı kaydet
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await stockDetail.Image.CopyToAsync(stream);
                }

                // TODO: Resmin yoluyla ilgili diğer işlemleri burada yapın (örneğin, veritabanında yolu saklayın)
            }

            // Veritabanı işlemi
            if (_stockDetailservice.TInsert(stockDetail) == 1)
            {
                return Created("", new ResultDTO<StockDetail>(stockDetail));
            }
            else
            {
                return BadRequest(new ResultDTO<StockDetail>("Form values are not valid."));
            }
        }
        /*[HttpPost]
        public IActionResult AddStockDetail(StockDetail stockDetail)
        {

            if (_stockDetailservice.TInsert(stockDetail) == 1)
            {
                return Created("", new ResultDTO<StockDetail>(stockDetail));
            }
            else
            {
                return BadRequest(new ResultDTO<StockDetail>("Form values are not valid."));
            }
        }*/
        [HttpDelete("{id}")]
        public IActionResult DeleteStockDetail(int id)
        {
            var stockDetailid = _stockDetailservice.TDelete(id);
            if (stockDetailid == 0)
            {
                return BadRequest(new ResultDTO<StockDetail>("The id to be deleted was not found."));
            }
            else
            {
                var stockDetail = _stockDetailservice.TGetByID(id);
                _stockDetailservice.TDelete(id);

                return Ok(new ResultDTO<StockDetail>(stockDetail));
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateStockDetail(int id, StockDetail stockDetail)
        {
            //stockDetail.StockDetailID = id;
            int result = _stockDetailservice.TUpdate(stockDetail);
            if (result == 0)
            {
                return BadRequest(new ResultDTO<StockDetail>("The stockDetail wanted to update could not be updated."));
            }
            else
            {
                return Ok(new ResultDTO<StockDetail>(stockDetail));
            }

        }
        [HttpGet("{id}")]
        public IActionResult GetStockDetail(int id)
        {
            var stockDetail = _stockDetailservice.TGetByID(id);
            if (stockDetail == null)
            {
                return BadRequest(new ResultDTO<StockDetail>("The id to be looking for was not found."));
            }
            return Ok(new ResultDTO<StockDetail>(stockDetail));
        }
    }
}
