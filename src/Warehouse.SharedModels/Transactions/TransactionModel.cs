namespace Warehouse.SharedModels.Transactions
{
    public class TransactionModel
    {
        public string Id { get; set; }
        public string Customer { get; set; }
        public MemoModel[] Memos { get; set; }
    }
}
