using KinlyNodeManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinlyNodeManager.Core.ServiceEntities
{
    public class CreateNodeResponse : BaseResponse
    {
        public Node Node { get; set; }
    }
}
