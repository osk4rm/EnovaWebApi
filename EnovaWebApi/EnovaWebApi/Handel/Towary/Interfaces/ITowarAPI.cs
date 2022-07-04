using EnovaWebApi.Handel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnovaWebApi.Interfaces
{
    public interface ITowarAPI
    {
        string NazwaTowaru(string code);
        TowarModel GetTowar(string code, string store);
        List<TowarModel> GetAllTowary(string store);
        bool UtworzTowar(TowarModel towarModel);
    }
}
