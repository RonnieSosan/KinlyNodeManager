using KinlyNodeManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinlyNodeManager.Core.ServiceEntities
{
    public class UpdateNodeResponse:BaseResponse
    {
        /// <summary>
        /// This is the original version of the node
        /// </summary>
        public Node OldNode { get; set; }
        /// <summary>
        /// This is the updated version of the node
        /// </summary>
        public Node UpdatedNode { get; set; }
    }
}
