using FlicoProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DtoLayer
{
    public class ResultDTO<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ResultDTO()
        {
            Success = true;
            Message = null;
            Data = default(T);
        }

        public ResultDTO(string message)
        {
            Success = false;
            Message = message;
            Data = default(T);
        }

        public ResultDTO(T data)
        {
            Success = true;
            Message = "the request was completed successfully";
            Data = data;
        }

    }
}
