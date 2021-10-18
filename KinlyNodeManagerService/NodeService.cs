using KinlyNodeManager.Core.Entities;
using KinlyNodeManager.Core.ServiceEntities;
using KinlyNodeManager.Persistence.Repositories;
using KinlyNodeManagerService.Utility;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace KinlyNodeManagerService
{
    public class NodeService
    {
        private readonly NodeRepository _nodeRepository = new NodeRepository();
        private readonly NodeLabelRepository _nodeLabelRepository = new NodeLabelRepository();

        /// <summary>
        /// This service function add a service node to the Kinly node repository.
        /// </summary>
        /// <param name="ServiceNode">This is the node to be added.</param>
        /// <returns>This service returns a successful message for the add action and the details of the node that has been added.</returns>
        public CreateNodeResponse AddNode(NodeRequestPayload ServiceNode)
        {
            Logger.LogInfo("NodeService.AddNode", ServiceNode);

            CreateNodeResponse response;

            try
            {
                Node existingNode = _nodeRepository.GetAll().Where(x => x.Port == ServiceNode.Port).FirstOrDefault();

                if (existingNode != null)
                {
                    //return that an existing node with this port number exists
                    response = new CreateNodeResponse()
                    {
                        ResponseCode = "06",
                        ResponseDescription = "A servie with this port number already"
                    };
                }
                else
                {
                    existingNode = _nodeRepository.GetAll().Where(x => x.Name == ServiceNode.Name).FirstOrDefault();

                    if (existingNode != null)
                    {
                        //return that a node with this name already exists
                        response = new CreateNodeResponse()
                        {
                            ResponseCode = "06",
                            ResponseDescription = "A servie with this name already"
                        };
                    }
                    else
                    {
                        //create the node
                        Node NodeToSave = new Node
                        {
                            Name = ServiceNode.Name,
                            Port = ServiceNode.Port,
                            Maintainer = ServiceNode.Maintainer
                        };

                        _nodeRepository.Save(NodeToSave);

                        existingNode = _nodeRepository.GetAll().Where(x => x.Name == ServiceNode.Name).FirstOrDefault();

                        if (ServiceNode.Labels != null)
                        {
                            ICollection<NodeLabel> labelCollection = new List<NodeLabel>();

                            foreach (var item in ServiceNode.Labels)
                            {
                                NodeLabel nodeLabel = new NodeLabel { Key = item.Key, Value = item.Value, NodeId = existingNode.Id };

                                _nodeLabelRepository.Save(nodeLabel);
                            }

                        }

                        existingNode = _nodeRepository.GetAllNodes().Where(x => x.Name == ServiceNode.Name).FirstOrDefault();

                        response = new CreateNodeResponse()
                        {
                            ResponseCode = "00",
                            ResponseDescription = "Successful",
                            Node = existingNode
                        };
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);

                //return the error
                response = new CreateNodeResponse()
                {
                    ResponseCode = "06",
                    ResponseDescription = "An error occured qwhile creating the node"
                };
            }

            return response;
        }

        /// <summary>
        /// This service function deletes
        /// </summary>
        /// <param name="node"></param>
        /// <returns>This service return a successful message for the delete action</returns>
        public DeleteNodeResponse DeleteNode(DeleteNodeRequest node)
        {
            Logger.LogInfo("NodeService.DeleteNode", node);

            DeleteNodeResponse response;

            Node existingNode;

            try
            {
                //try to search with the ID specified 
                if (node.Id != null)
                    existingNode = _nodeRepository.GetAllNodes().Where(x => x.Id == node.Id).FirstOrDefault();
                else
                {
                    //search by the node name
                    existingNode = _nodeRepository.GetAllNodes().Where(x => x.Name == node.ServiceName).FirstOrDefault();
                }

                if (existingNode != null)
                {
                    List<NodeLabel> labels = existingNode.Labels.ToList();

                    //remove all related labels
                    foreach (var item in labels)
                    {
                        _nodeLabelRepository.Remove(item);
                    }

                    //delete node
                    _nodeRepository.Remove(existingNode);

                    response = new DeleteNodeResponse()
                    {
                        ResponseCode = "00",
                        ResponseDescription = "Successful"
                    };
                }
                else
                {
                    response = new DeleteNodeResponse()
                    {
                        ResponseCode = "06",
                        ResponseDescription = "Node not found"
                    };

                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);

                //return the error
                response = new DeleteNodeResponse()
                {
                    ResponseCode = "06",
                    ResponseDescription = "An error occured while deleting the node"
                };
            }

            return response;
        }

        /// <summary>
        /// This service function gets all Nodes bt either their name or their attached labels
        /// </summary>
        /// <param name="Name">The name of the node</param>
        /// <param name="key">The node label key</param>
        /// <param name="value">The node lable value</param>
        /// <returns> This service return the list of nodes that fits the filter</returns>
        public GetAllNodesResponse GetAllNodes(string Name = null, string key = null, string value = null)
        {
            Logger.LogInfo("NodeService.GetAllNodes", "");

            GetAllNodesResponse response;

            IQueryable<Node> nodes;

            try
            {
                nodes = _nodeRepository.GetAllNodes();

                //try to search with the ID specified 
                if (Name != null)
                    nodes = nodes.Where(x => x.Name == Name);

                //try to search using the key,pair value
                if (key != null && value != null)
                    nodes = nodes.Where(x => x.Labels.Any(l => l.Key == key && l.Value == value));


                //check if the queriable collection has any item
                if (nodes.Count() > 0)
                {
                    response = new GetAllNodesResponse()
                    {
                        Nodes = nodes.ToList(),
                        ResponseCode = "00",
                        ResponseDescription = "Successful"
                    };
                }
                else
                {
                    response = new GetAllNodesResponse()
                    {
                        ResponseCode = "06",
                        ResponseDescription = "Node not found"
                    };

                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);

                //return the error
                response = new GetAllNodesResponse()
                {
                    ResponseCode = "06",
                    ResponseDescription = "An error occured while deleting the node"
                };
            }

            return response;
        }

        /// <summary>
        /// This service function updates the current for the Id specified, with the properties and atttributes specified in the JsonPatchDocument
        /// </summary>
        /// <param name="Id">The Id for the node to be updated</param>
        /// <param name="NodeModel">This is a JsonPatchDocument for the update to be made</param>
        /// <returns>This service return the updated state od the model and the previous state of the model</returns>
        public UpdateNodeResponse UpdateNode(int Id, JsonPatchDocument NodeModel)
        {
            Logger.LogInfo("NodeService.GetAllNodes", "");

            UpdateNodeResponse response;

            Node existingNode;

            try
            {
                existingNode = _nodeRepository.Get(Id);


                //check if the queriable collection has any item
                if (existingNode != null)
                {
                    Node OldNode = _nodeRepository.Get(Id);

                    NodeModel.ApplyTo(existingNode);

                    _nodeRepository.Update(existingNode);

                    response = new UpdateNodeResponse()
                    {
                        OldNode = OldNode,
                        UpdatedNode = existingNode,
                        ResponseCode = "00",
                        ResponseDescription = "Successful"
                    };
                }
                else
                {
                    response = new UpdateNodeResponse()
                    {
                        ResponseCode = "06",
                        ResponseDescription = "Node not found"
                    };

                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);

                //return the error
                response = new UpdateNodeResponse()
                {
                    ResponseCode = "06",
                    ResponseDescription = "An error occured while updating the node"
                };
            }

            return response;
        }
    }
}
