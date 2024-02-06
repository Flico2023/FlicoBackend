using FlicoProject.DtoLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.BusinessLayer.Concrete.Validators.PostContactMessage
{
    public interface IPutContactDtoOtherValidators
    {
        ResultDTO<PutContactMessageDto> ValidateXSSPutContactMessageDto(PutContactMessageDto postContactMessageDto);
    }
}
