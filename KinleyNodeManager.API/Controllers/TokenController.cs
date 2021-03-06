using KinlyNodeManagerService.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinleyNodeManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(JwtManager.GenerateToken());
        }
    }
}
