using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntrestRatesProvider.Models
{
    public class ViewModel
    {
        public IEnumerable<Clients> Clients { get; set; }
        public IEnumerable<Agreements> Agreemenat { get; set; }
    }
}
