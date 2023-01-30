using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeSave.DB
{
    public class DataAccess
    {
        private static DbSet<Agent> _agents => EyeSaveEntities.GetContext().Agents;
        private static DbSet<AgentType> _agentTypes => EyeSaveEntities.GetContext().AgentTypes;
        private static DbSet<Product> _products => EyeSaveEntities.GetContext().Products;

        public static List<Agent> GetAgents() => _agents.ToList();
        public static List<AgentType> GetAgentTypes() => _agentTypes.ToList();
        public static List<Product> GetProducts() => _products.ToList();
        

        public static void SaveAgent(Agent agent)
        {
            if (agent.ID == 0)
                _agents.Add(agent);

            EyeSaveEntities.GetContext().SaveChanges();
        }

        public static void DeleteAgent(Agent agent)
        {
            agent.IsDeleted = true;
            SaveAgent(agent);
        }
        /* Agents
         * AgentTypes
         * Products
         */
    }
}
