using Soneta.Business;
using Soneta.Towary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnovaWebApi.Handel.Towary
{
    public class GetTowarInfo
    {
        Session session { get; set; }

        public GetTowarInfo(Session session)
        {
            this.session = session;
        }

        public Towar GetTowar(string code)
        {
            var towaryModule = TowaryModule.GetInstance(session);

            return towaryModule.Towary.WgKodu[code];
        }
        public List<Towar> GetAllTowary()
        {
            List<Towar> listOfProducts = new List<Towar>();
            
            var towaryModule = TowaryModule.GetInstance(session);

            foreach(var row in towaryModule.Towary)
            {
                listOfProducts.Add(row as Towar);
            }

            return listOfProducts;
        }
    }
}
