using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EyeSave.DB
{
    public class DataAccess
    {
        public delegate void NewItemAddedDeledate();
        public static event NewItemAddedDeledate NewItemAddedEvent;

        private static DbSet<Agent> _agents => EyeSaveEntities.GetContext().Agents;
        private static DbSet<AgentType> _agentTypes => EyeSaveEntities.GetContext().AgentTypes;
        private static DbSet<Product> _products => EyeSaveEntities.GetContext().Products;
        private static DbSet<AgentPriorityHistory> _agentPriorityHistories=> EyeSaveEntities.GetContext().AgentPriorityHistories;
        private static DbSet<ProductSale> _productSales=> EyeSaveEntities.GetContext().ProductSales;

        public static List<Agent> GetAgents() => _agents.ToList().FindAll(x => !x.IsDeleted);
        public static List<AgentType> GetAgentTypes() => _agentTypes.ToList();
        public static List<Product> GetProducts() => _products.ToList();
        

        public static void SaveAgent(Agent agent)
        {
            if (agent.ID == 0)
                _agents.Add(agent);

            EyeSaveEntities.GetContext().SaveChanges();
            NewItemAddedEvent?.Invoke();
        }

        public static void DeleteAgent(Agent agent)
        {
            agent.IsDeleted = true;
            SaveAgent(agent);
        }

        public static bool IsINNnKPPUnique(Agent agent)
        {
            return _agents.Any(x => x.ID != agent.ID && (x.INN == agent.INN || x.KPP == agent.KPP));
        }

        public static void AddPriorityHistory(AgentPriorityHistory history)
        {
            _agentPriorityHistories.Add(history);
            EyeSaveEntities.GetContext().SaveChanges();
        }

        internal static void DeleteProductSale(ProductSale productSale)
        {
            _productSales.Remove(productSale);
        }
        /* Agents
* AgentTypes
* Products
*/
    }
}
