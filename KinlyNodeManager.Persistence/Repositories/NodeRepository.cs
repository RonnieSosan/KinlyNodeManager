using KinlyNodeManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace KinlyNodeManager.Persistence.Repositories
{
    public class NodeRepository:GenericRepository<Node,ApplicationDbContext>
    {
        public IQueryable<Node> GetAllNodes()
        {
            var Context = DataContext;
            return Context.Nodes.Include(x => x.Labels);
        }

    }
}
