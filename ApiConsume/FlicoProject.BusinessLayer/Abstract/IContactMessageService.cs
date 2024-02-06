using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FlicoProject.BusinessLayer.Abstract
{
    public interface IContactMessageService 
    {
        int TInsert(ContactMessage t);
        int TDelete(int t);
        int TUpdate(ContactMessage t);
        List<ContactMessage> TGetList();
        ContactMessage TGetByID(int id);

        void SendAnswerMail(ContactMessage message);
        ResultDTO<PostContactMessageDto> ValidatePostContactMessageDto(PostContactMessageDto dto);
        ResultDTO<PostContactMessageDto> ValidateXSSPostContactMessageDto(PostContactMessageDto postContactMessageDto);


        ResultDTO<PutContactMessageDto> ValidatePutContactMessageDto(PutContactMessageDto dto);
        ResultDTO<PutContactMessageDto> ValidateXSSPutContactMessageDto(PutContactMessageDto postContactMessageDto);

        List<ContactMessage> FilterContactMessageList(List<ContactMessage> messages, string email, string subject, string date, string status, int? id, string name);
}
}
