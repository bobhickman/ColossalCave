using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ColossalCave.Engine.Interfaces;

namespace ColossalCave.Webhook.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private ILocationProvider _locationProvider;

        public ValuesController(ILocationProvider locationProvider)
        {
            _locationProvider = locationProvider;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _locationProvider.Map
                .Take(10)
                .Select(d => d.Value.ShortDescription)
                .ToArray();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return _locationProvider.GetLocation(id).Description;
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
