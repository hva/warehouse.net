using System;

namespace Warehouse.Wpf.Models
{
    public class TransactionModel
    {
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Employee { get; set; }
        public string Customer { get; set; }
        public string Status { get; set; }
        public MemoModel[] Memos { get; set; }
    }
}
