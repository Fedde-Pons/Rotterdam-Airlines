namespace Project.DataModels
{
    public class BookingsModel
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public string Date { get; set; }
        public double TotalPrice { get; set; }
        public string Status { get; set; }

        public BookingsModel(long accountId, string date, double totalPrice, string status)
        {
            AccountId = accountId;
            Date = date;
            TotalPrice = totalPrice;
            Status = status;
        }
    }
}