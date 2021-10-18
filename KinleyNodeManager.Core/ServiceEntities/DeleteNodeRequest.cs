using System;
using System.Collections.Generic;
using System.Text;

namespace KinlyNodeManager.Core.ServiceEntities
{
    public class DeleteNodeRequest
    {
        public int? Id { get; set; }
        public string ServiceName { get; set; }
    }
}
