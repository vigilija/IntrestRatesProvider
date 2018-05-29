using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntrestRatesProvider.Models
{
    public class Agreements
    {
        public Int32 Amount { get; set; }
        public string BaseRateCode { get; set; }
        public double Margin { get; set; }
        public Int32 Duration { get; set; }
        public double CurrentInterestRate { get; set; }
        public double NewInterestRate { get; set; }
        public double Difference { get; set; }
    }
    public enum BaseRateCode
    {
        VILIBOR1m, VILIBOR3m, VILIBOR6m, VILIBOR1y
    }
}
