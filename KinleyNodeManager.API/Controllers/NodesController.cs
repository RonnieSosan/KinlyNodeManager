using KinlyNodeManager.Core.ServiceEntities;
using KinlyNodeManagerService;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KinleyNodeManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NodesController : ControllerBase
    {
        public readonly NodeService _nodeservice = new NodeService();

        // GET: api/<NodesController>
        [HttpGet]
        public IActionResult Get()
        {
            var response = _nodeservice.GetAllNodes();

            return Ok(response);
        }

        // GET api/<NodesController>/5
        [HttpGet("{ServiceName}")]
        public IActionResult Get(string ServiceName)
        {
            var response = _nodeservice.GetAllNodes(ServiceName);

            return Ok(response);
        }

        // GET api/<NodesController>/5
        [HttpGet("GetbyLabel")]
        public IActionResult GetbyLabel([FromQuery] string Label)
        {
            var values = Label.Split(":", StringSplitOptions.None);
            var response = _nodeservice.GetAllNodes(null, values[0], values[1]);

            return Ok(response);
        }

        // POST api/<NodesController>
        [HttpPost]
        public IActionResult Post([FromBody] NodeRequestPayload value)
        {
            var response = _nodeservice.AddNode(value);

            return Ok(response);
        }

        // PUT api/<NodesController>/5
        [HttpPatch("{id}")]
        public IActionResult Put(int id, [FromBody] JObject value)
        {
            JsonPatchDocument doc = new JsonPatchDocument();
            foreach (var item in value)
            {
                doc.Add(item.Key, item.Value);
            }
            var response = _nodeservice.UpdateNode(id, doc);

            return Ok(response);
        }

        // DELETE api/<NodesController>/5
        [HttpDelete]
        public IActionResult Delete([FromQuery] DeleteNodeRequest payload)
        {
            var response = _nodeservice.DeleteNode(payload);
            return Ok(response);
        }
    }
}
