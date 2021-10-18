using KinlyNodeManager.Core.ServiceEntities;
using KinlyNodeManagerService;
using NUnit.Framework;

namespace KinlyNodeManager.Test
{
    public class Tests
    {
        public readonly NodeService _nodeservice = new NodeService();

        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// An assertion to check that that service name already exists, this is a negative test to confirm that the name exists
        /// </summary>
        /// <param name="serviceName">The name of the service you want to test</param>
        [TestCase("SecondName", "06")]
        [TestCase("", "00")]
        [TestCase(null, "06")]
        public void UniqueServiceName(string serviceName, string responseCode)
        {
            NodeRequestPayload payload = new NodeRequestPayload()
            {
                Name = serviceName,
                Port = "7007",
                Maintainer = "j@mail.ca"
            };

            var response = _nodeservice.AddNode(payload);

            Assert.AreEqual(responseCode, response.ResponseCode);
        }

        [TestCase("00")]
        public void NodeAddedSuccessfully(string responseCode)
        {
            NodeRequestPayload payload = new NodeRequestPayload()
            {
                Name = "ThirdNode",
                Port = "8080",
                Maintainer = "j@mail.ca"
            };

            var response = _nodeservice.AddNode(payload);

            Assert.AreEqual(responseCode, response.ResponseCode);
        }

        /// <summary>
        /// Test to confirm that the node does not exist
        /// </summary>
        [TestCase(1, "00")]
        [TestCase(10, "06")]
        [TestCase(null, "06")]
        public void NodeDoesNotExist(int id, string responsecode)
        {
            DeleteNodeRequest payload = new DeleteNodeRequest { Id = id };
            var response = _nodeservice.DeleteNode(payload);

            Assert.AreEqual(responsecode, response.ResponseCode);
        }

        /// <summary>
        /// Test to confirm that the node does not exist
        /// </summary>
        [TestCase("SecondServiceName", "00")]
        [TestCase("SecondServiceName2", "06")]
        public void Get(string serviceName, string responseCode)
        {

            var response = _nodeservice.GetAllNodes(serviceName);

            Assert.AreEqual(responseCode, response.ResponseCode);
        }
    }
}