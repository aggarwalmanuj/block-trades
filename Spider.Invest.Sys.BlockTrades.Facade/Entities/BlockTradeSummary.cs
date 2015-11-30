using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Invest.Sys.BlockTrades.Facade.Entities
{
    public class BlockTradeSummary
    {
        public decimal Price { get; set; }

        public long TotalShares { get; set; }

        public decimal TotalAmount { get; set; }

        public int TotalTrades { get; set; }

        public DateTime TradeDate { get; set; }
    }
}
