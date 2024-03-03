using AngleSharp;
using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.BusinessLayer.Concrete.Validators.PostContactMessage;
using FlicoProject.DataAccessLayer.Abstract;
using FlicoProject.DataAccessLayer.EntityFramework;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.String;
using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using FlicoProject.BusinessLayer.Concrete.Mail;
using FlicoProject.BusinessLayer.Concrete.Mail.MailFormatters;

namespace FlicoProject.BusinessLayer.Concrete
{
    public class ContactMessageManager : IContactMessageService
    {
        private readonly IContactMessageDal _ContactMessageDal;
        private readonly IValidator<PostContactMessageDto> _validator;
        private readonly IValidator<PutContactMessageDto> _putValidator;
        private readonly IPostContactDtoOtherValidators _postContactDtoOtherValidators;
        private readonly IPutContactDtoOtherValidators _putContactDtoOtherValidators;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;

        public ContactMessageManager(IMailService MailService ,IConfiguration Configuration,
                                IPutContactDtoOtherValidators PutContactDtoOtherValidators,
                                IPostContactDtoOtherValidators PostContactDtoOtherValidators,
                                IContactMessageDal ContactMessageDal,
                                IValidator<PostContactMessageDto> validator,
                                IValidator<PutContactMessageDto> putValidator)
        {
            _configuration = Configuration;
            _ContactMessageDal = ContactMessageDal;
            _validator = validator;
            _postContactDtoOtherValidators = PostContactDtoOtherValidators;
            _putContactDtoOtherValidators = PutContactDtoOtherValidators;
            _putValidator = putValidator;
            _mailService = MailService;
        }


        public void SendAnswerMail(ContactMessage message)
        {
            var mailFormatter = new ContactMessageAnswerMailFormatter();
            var html = mailFormatter.FormatContactMessageEmail(message);
            var subject = $"Ticket {message.Subject} has answered";

            _mailService.SendMail(message.Email, subject, html);
        }

        public List<ContactMessage> FilterContactMessageList(List<ContactMessage> messages,string email, 
            string subject, string date, string status, int? id,string name )
        {

            if (id != null)
            {
                messages = messages.Where(x => x.Id == id).ToList();
            }
            if(!IsNullOrWhiteSpace(email))
            {
                messages = messages.Where(x => x.Email == email).ToList();
            }
            if (!IsNullOrWhiteSpace(subject))
            {
                messages = messages.Where(x => x.Subject.ToLower() == subject.ToLower()).ToList();
            }
            if (!IsNullOrWhiteSpace(status) && status != "All")
            {
                messages = messages.Where(x => x.Status == status).ToList();
            }
            if (!IsNullOrWhiteSpace(name))
            {
                messages = messages.Where(x => x.Name.ToLower() == name.ToLower()).ToList();
            }
            if (!IsNullOrWhiteSpace(date) && date != "All")
            {
                if(date == "today")
                {
                    messages = messages.Where(x => x.MessageDate >= DateTime.Now.Date).ToList();
                }
                if (date == "weekly")
                {
                    messages = messages.Where(x => x.MessageDate >= DateTime.Now.Date.AddDays(-7)).ToList();
                }
                if(date == "monthly")
                {
                    messages = messages.Where(x => x.MessageDate >= DateTime.Now.Date.AddMonths(-1)).ToList();
                }
            }

            return messages;    
        }

        public ResultDTO<PostContactMessageDto> ValidatePostContactMessageDto(PostContactMessageDto dto)
        {
            var result = _validator.Validate(dto);
            if (result.IsValid)
            {
                return new ResultDTO<PostContactMessageDto>(dto);
            }
            else
            {
                var errors = result.Errors.Select(x => x.ErrorMessage).ToList();
                var error = errors[0] ?? "Something went wrong";
                return new ResultDTO<PostContactMessageDto>(errors[0]);
            }
        }

        public ResultDTO<PostContactMessageDto> ValidateXSSPostContactMessageDto(PostContactMessageDto postContactMessageDto)
        {
           var result = _postContactDtoOtherValidators.ValidateXSSPostContactMessageDto(postContactMessageDto);
            
            if (result.Success != true)
            {
                return new ResultDTO<PostContactMessageDto>(result.Message);
            }

            return new ResultDTO<PostContactMessageDto>(postContactMessageDto);
        }

        public ResultDTO<PutContactMessageDto> ValidatePutContactMessageDto(PutContactMessageDto dto)
        {
            var result = _putValidator.Validate(dto);
            if (result.IsValid)
            {
                return new ResultDTO<PutContactMessageDto>(dto);
            }
            else
            {
                var errors = result.Errors.Select(x => x.ErrorMessage).ToList();
                var error = errors[0] ?? "Something went wrong";
                return new ResultDTO<PutContactMessageDto>(errors[0]);
            }
            
        }
        public ResultDTO<PutContactMessageDto> ValidateXSSPutContactMessageDto(PutContactMessageDto putContactMessageDto)
        {

            var result = _putContactDtoOtherValidators.ValidateXSSPutContactMessageDto(putContactMessageDto);

            if (result.Success != true)
            {
                return new ResultDTO<PutContactMessageDto>(result.Message);
            }

            return new ResultDTO<PutContactMessageDto>(putContactMessageDto);
        }




        public int TDelete(int id)
        {
            var ContactMessage = _ContactMessageDal.GetByID(id);
            if (ContactMessage == null)
            {
                return 0;
            }
            else
            {
                _ContactMessageDal.Delete(ContactMessage);
                return 1;
            }
        }

        public ContactMessage TGetByID(int id)
        {
            return _ContactMessageDal.GetList().Find(x => x.Id == id);
        }

        public List<ContactMessage> TGetList()
        {
            return _ContactMessageDal.GetList();
        }

        public int TInsert(ContactMessage t)
        {
            var a = _ContactMessageDal.GetList().Find(x=>x.Id == t.Id);
            if (a != null)
            {
                return 0;
            }
            else
            {
                _ContactMessageDal.Insert(t);
                return 1;
            }
        }

        public int TUpdate(ContactMessage t)
        {
            var isvalid = _ContactMessageDal.GetList().FirstOrDefault(x => x.Id == t.Id);

            if (isvalid == null)
            {
                return 0;
            }
            else
            {
                _ContactMessageDal.Update(t);
                return 1;

            }
        }
    }
}
