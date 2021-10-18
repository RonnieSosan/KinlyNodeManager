using System;
using System.Collections.Generic;
using System.Text;

namespace KinlyNodeManager.Core.ServiceEntities
{
    public abstract class BaseResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
    }
}
