using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeSave.DB
{
    public partial class Agent
    {
        public int SalesCount
        {
            get => ProductSales.Count(x => x.SaleDate >= DateTime.Now.AddYears(-1));

            set { }
        }

        public int Discount
        {
            get
            {
                var sum = ProductSales.Sum(x => x.ProductCount * x.Product.MinCostForAgent);
                var discount = sum < 10000 ? 0 : (sum < 50000 ? 5 : (sum < 150000 ? 10 : (sum < 500000 ? 20 : 25)));
                
                return discount;
            }
                
            set { }
        }

        public string Color => Discount >= 10 ? "#FF27E05D" : "#FFE9F9";
    }
}
