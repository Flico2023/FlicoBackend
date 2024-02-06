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
    public class PostContactDtoOtherValidators : IPostContactDtoOtherValidators
    {
        public ResultDTO<PostContactMessageDto> ValidateXSSPostContactMessageDto(PostContactMessageDto postContactMessageDto)
        {
            var sanitizer = new HtmlSanitizer();
            var htmlDecoder = new HtmlDecoder();

            var name = htmlDecoder.DecodeHtml(postContactMessageDto.Name);
            var email = htmlDecoder.DecodeHtml(postContactMessageDto.Email);
            var subject = htmlDecoder.DecodeHtml(postContactMessageDto.Subject);
            var message = htmlDecoder.DecodeHtml(postContactMessageDto.Message);

            var sanitizedName = sanitizer.Sanitize(name);
            var sanitizedEmail = sanitizer.Sanitize(email);
            var sanitizedSubject = sanitizer.Sanitize(subject);
            var sanitizedMessage = sanitizer.Sanitize(message);

            if(name != sanitizedName || email != sanitizedEmail || subject != sanitizedSubject || message != sanitizedMessage)
            {
                return new ResultDTO<PostContactMessageDto>("XSS attack detected");
            }
            
            return new ResultDTO<PostContactMessageDto>(postContactMessageDto) ;  
        }
    }
}
