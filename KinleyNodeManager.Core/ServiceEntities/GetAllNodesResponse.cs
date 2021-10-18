using KinlyNodeManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinlyNodeManager.Core.ServiceEntities
{
    public class GetAllNodesResponse:BaseResponse
    {
        public List<Node> Nodes { get; set; }
    }
}
