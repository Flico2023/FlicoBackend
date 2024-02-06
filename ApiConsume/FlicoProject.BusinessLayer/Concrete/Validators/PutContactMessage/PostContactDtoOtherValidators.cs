using FlicoProject.DtoLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ganss.Xss;
using FlicoProject.BusinessLayer.Concrete.Validators.CoreValidators;

namespace FlicoProject.BusinessLayer.Concrete.Validators.PostContactMessage
{
    public class PutContactDtoOtherValidators : IPutContactDtoOtherValidators
    {
        public ResultDTO<PutContactMessageDto> ValidateXSSPutContactMessageDto(PutContactMessageDto putContactMessageDto)
        {
            var htmlDecoder = new HtmlDecoder();
            var sanitizer = new HtmlSanitizer();

            var answer = htmlDecoder.DecodeHtml(putContactMessageDto.Answer);
            var sanitizedAnswer = sanitizer.Sanitize(answer);

            if(answer != sanitizedAnswer)
            {
                return new ResultDTO<PutContactMessageDto>("XSS attack detected");
            }
            
            return new ResultDTO<PutContactMessageDto>(putContactMessageDto);  
        }


    }
}
