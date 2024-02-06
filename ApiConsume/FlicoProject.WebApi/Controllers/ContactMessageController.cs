using AutoMapper;
using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.BusinessLayer.Concrete.Mail;
using FlicoProject.DataAccessLayer.Migrations;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using static System.String;

namespace FlicoProject.WebApi.Controllers
{
    [Route("api/contact_messages")]
    [ApiController]
    public class ContactMessageController : ControllerBase
    {

        private readonly IContactMessageService _contactMessageService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public ContactMessageController(IConfiguration config, IContactMessageService faqService, IMapper mapper)
        {
            _contactMessageService = faqService;
            _mapper = mapper;
            _config = config;
        }

        [HttpGet]
        public IActionResult ContactMessageList([FromQuery] int pageSize, int pageIndex, string? email, string? subject, string? date, string? status, int? id, string? name)
        {
            var ContactMessages = _contactMessageService.TGetList().ToList();

            List<ContactMessage> FilteredContactMessages = _contactMessageService.FilterContactMessageList(ContactMessages, email, subject, date, status, id, name);


            var totalCount = FilteredContactMessages.Count;
            FilteredContactMessages = FilteredContactMessages.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

            var ContactMessageListDTO = new ContactMessageListResultDto();
            ContactMessageListDTO.ContactMessages = FilteredContactMessages;
            ContactMessageListDTO.TotalCount = totalCount;
            ContactMessageListDTO.PageIndex = pageIndex;
            ContactMessageListDTO.PageSize = pageSize;


            return Ok(new ResultDTO<ContactMessageListResultDto>(ContactMessageListDTO));
        }
        [HttpPost]
        public IActionResult AddContactMessage(PostContactMessageDto ContactMessageDto)
        {
            ContactMessageDto.Email = ContactMessageDto.Email.Trim();
            ContactMessageDto.Name = ContactMessageDto.Name.Trim();
            ContactMessageDto.Subject = ContactMessageDto.Subject.Trim();
            ContactMessageDto.Message = ContactMessageDto.Message.Trim();



            var result = _contactMessageService.ValidatePostContactMessageDto(ContactMessageDto);
            if (result.Success != true)
            {
                return BadRequest(new ResultDTO<PostContactMessageDto>(result.Message));
            }

            var resultXSS = _contactMessageService.ValidateXSSPostContactMessageDto(ContactMessageDto);
            if (resultXSS.Success != true)
            {
                return BadRequest(new ResultDTO<PostContactMessageDto>("Something went wrong"));
            }

            var ContactMessage = _mapper.Map<ContactMessage>(ContactMessageDto);


            if (_contactMessageService.TInsert(ContactMessage) == 0)
            {
                return BadRequest(new ResultDTO<ContactMessage>("Form values are not valid."));
            }

            var ContactMessageId = _contactMessageService.TGetByID(ContactMessage.Id);

            return Ok(new ResultDTO<ContactMessage>(ContactMessage));


        }

        [HttpDelete("{id}")]
        public IActionResult DeleteContactMessage(int id)
        {
            var ContactMessageId = _contactMessageService.TDelete(id);
            if (ContactMessageId == 0)
            {
                return BadRequest(new ResultDTO<ContactMessage>("The message to be deleted was not found."));
            }
            else
            {
                var ContactMessage = _contactMessageService.TGetByID(id);
                _contactMessageService.TDelete(id);

                return Ok(new ResultDTO<ContactMessage>(ContactMessage));
            }
        }


        [HttpPut("{id}")]
        public IActionResult UpdateContactMessage(int id, PutContactMessageDto contactMessageDto)
        {
            contactMessageDto.Answer = contactMessageDto.Answer.Trim();

            var result = _contactMessageService.ValidatePutContactMessageDto(contactMessageDto);
            if (result.Success != true)
            {
                return BadRequest(new ResultDTO<PostContactMessageDto>(result.Message));
            }

            var resultXSS = _contactMessageService.ValidateXSSPutContactMessageDto(contactMessageDto);
            if (resultXSS.Success != true)
            {
                return BadRequest(new ResultDTO<PostContactMessageDto>("Something went wrong"));
            }


            var message = _contactMessageService.TGetByID(id);
            if (message == null)
            {
                return BadRequest(new ResultDTO<ContactMessage>("The id to be looking for was not found."));
            }

            message.Answer = contactMessageDto.Answer;
            message.AnswerDate = DateTime.Now;
            message.Status = "Closed";

            if (_contactMessageService.TUpdate(message) == 0)
            {
                return BadRequest(new ResultDTO<ContactMessage>("The message wanted to update could not be updated."));
            }

            _contactMessageService.SendAnswerMail(message);

            return Ok(new ResultDTO<ContactMessage>(message));

        }

        [HttpGet("{id}")]
        public IActionResult GetContactMessage(int id)
        {
            var message = _contactMessageService.TGetByID(id);
            if (message == null)
            {
                return BadRequest(new ResultDTO<ContactMessage>("The id to be looking for was not found."));
            }
            return Ok(new ResultDTO<ContactMessage>(message));
        }
    }

 
    
}
