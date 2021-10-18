using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KinlyNodeManager.Core.ServiceEntities
{
    public class NodeRequestPayload
    {
        /// <summary>
        /// This signifies the name used to Identify the node. 
        /// The service name, must be unique(should be a name from 4 to 30 characters).
        /// </summary>
        [Required]
        [StringLength(30, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]

        public string Name { get; set; }
        /// <summary>
        /// This is the port number assigned to the node/service on the cluster (should be a valid port).
        /// </summary>
        [Required]
        [RegularExpression("^([0-9]{1,4}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])$", ErrorMessage = "Port Number is not valid")]
        public string Port { get; set; }
        /// <summary>
        /// The person that is responsible for the service(should be a valid email).
        /// </summary>
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Maintainer { get; set; }
        /// <summary>
        /// Can be multiple labels, following a 'key:value' convention.
        /// </summary>
        public Dictionary<string,string> Labels { get; set; }
    }
}
