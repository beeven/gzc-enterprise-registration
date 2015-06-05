using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using EnterpriseRegistration.Interfaces;
using EnterpriseRegistration.Interfaces.Entities;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EnterpriseRegistration.Web.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        readonly IDataService dataService;

        public MessagesController(IDataService dataService)
        {
            this.dataService = dataService;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Message> Get(int pageSize, int offset)
        {
            return dataService.GetMessages(pageSize, offset);
        }
        

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<Message> Get(Guid id)
        {
            return await dataService.GetMessageByIdAsync(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
