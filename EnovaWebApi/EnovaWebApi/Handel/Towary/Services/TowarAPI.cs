using EnovaWebApi.Handel.Models;
using EnovaWebApi.Handel.Towary.Services;
using EnovaWebApi.Interfaces;
using Soneta.Business;
using Soneta.Handel;
using Soneta.Magazyny;
using Soneta.Towary;
using Soneta.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[assembly: Service(typeof(ITowarAPI), typeof(TowarAPI), ServiceScope.Session, Published = true)]

namespace EnovaWebApi.Handel.Towary.Services
{
    public class TowarAPI : ITowarAPI
    {
        GetTowarInfo getTowar;
        TowaryModule tw;
        HandelModule hm;

        public TowarAPI(Session session)
        {
            getTowar = new GetTowarInfo(session);
            tw = TowaryModule.GetInstance(session);
            hm = HandelModule.GetInstance(session);
        }

        public string NazwaTowaru(string code) => getTowar.GetTowar(code).Nazwa;

        public TowarModel GetTowar(string code, string storeName = "Firma")
        {
            var towar = getTowar.GetTowar(code);
            var defCeny = tw.DefinicjeCen.WgNazwy["Podstawowa"];
            var magazyn = hm.Magazyny.Magazyny.WgNazwa[storeName];
            var stanMagazynuWorker = new StanMagazynuWorker() { Magazyn = magazyn, Towar = towar };
            var stanNaMagazynie = stanMagazynuWorker.Stan;

            return new TowarModel()
            {
                Nazwa = towar.Nazwa,
                Stan = stanNaMagazynie.Value,
                Jednostka = towar.Jednostka.ToString(),
                Cena = towar.PobierzCenę(defCeny, null, null).Netto.Value,
                Kod = towar.Kod,
                EAN = towar.EAN
            };
        }
        public List<TowarModel> GetAllTowary(string nazwaMagazynu)
        {
            List<TowarModel> towarModels = new List<TowarModel>();
            List<Towar> productsList = getTowar.GetAllTowary();

            foreach(var product in productsList)
            {
                try
                {
                    var towar = getTowar.GetTowar(product.Kod);
                    var defCeny = tw.DefinicjeCen.WgNazwy["Podstawowa"];
                    var magazyn = hm.Magazyny.Magazyny.WgNazwa[nazwaMagazynu];
                    var stanMagazynuWorker = new StanMagazynuWorker() { Magazyn = magazyn, Towar = towar };
                    var stanNaMagazynie = stanMagazynuWorker.Stan;

                    TowarModel tm = new TowarModel
                    {
                        Nazwa = towar.Nazwa,
                        Stan = stanNaMagazynie.Value,
                        Jednostka = towar.Jednostka.ToString(),
                        Cena = towar.PobierzCenę(defCeny, null, null).Netto.Value,
                        Kod = towar.Kod,
                        EAN = towar.EAN
                    };

                    towarModels.Add(tm);
                }
                catch(Exception ex) { continue; }
                
            }

            return towarModels;
        }
        public bool UtworzTowar(TowarModel towarModel)
        {
            using (Session s = hm.Session.Login.CreateSession(false, true))
            {
                var tw = TowaryModule.GetInstance(s);
                using (ITransaction t = s.Logout(true))
                {
                    var nowyTowar = new Towar()
                    {
                        Nazwa = towarModel.Nazwa,
                        Kod = towarModel.Kod,
                        EAN = towarModel.EAN
                    };
                    tw.Towary.AddRow(nowyTowar);

                    nowyTowar.Jednostka = tw.Jednostki.WgKodu[towarModel.Jednostka];

                    var defCeny = tw.DefinicjeCen.WgNazwy["Podstawowa"];
                    nowyTowar.Jednostka = tw.Jednostki.WgKodu[towarModel.Jednostka];
                    nowyTowar.Ceny[tw.DefinicjeCen.WgNazwy["Podstawowa"]].Netto = new DoubleCy(towarModel.Cena, "PLN");

                    t.Commit();
                }
                s.Save();
            }

            return true;
        }
    }
}

