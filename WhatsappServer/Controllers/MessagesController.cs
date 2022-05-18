using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WhatsappServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/contacts/{id}/messages")]
    public class MessagesController : ControllerBase
    {
        public string user = "chen"; // owner of the list of contacts ("belongTo") NEEDS TO BE CHANGED
        MessagesService messagesService = new MessagesService();
        ContactsService contactsService = new ContactsService();

        [HttpGet]
        public IActionResult GetAllMessages(string? id)
        {
            try
            {
                return Ok(messagesService.GetAll(user, id));
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IActionResult addMessage(string id, [FromBody] Message message)
        {
            try
            {
                message.sent = true;
                message.contactUsername = id;
                message.belongs = user;
                message.created = DateTime.Now;
                messagesService.Add(message);

                Contact contact = new Contact { belongTo = user, id = id, lastdate = DateTime.Now, last = message.content };
                contactsService.Edit(contact);
                return Created("", message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id2}")]
        public IActionResult GetMessage(string id, int id2)
        {
            try
            {
                // user - the owner of the contacts list
                // id - username in the list
                // id2 - unique id of a message in the chat between "user" and "id"
                return Ok(messagesService.GetMessage(user, id, id2));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id2}")]
        public IActionResult EditMessage(string id, int id2, [FromBody] Message message)
        {
            try
            {
                message.belongs = user;
                message.contactUsername = id;
                message.id = id2;
                messagesService.Edit(message);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id2}")]
        public IActionResult DeleteMessage(string id, int id2)
        {
            try
            {
                messagesService.Delete(user, id, id2);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}