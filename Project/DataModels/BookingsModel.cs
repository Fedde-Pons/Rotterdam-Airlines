namespace Project.DataModels
{

    public class BookingsModel
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public string Date { get; set; }
        public double TotalPrice { get; set; }
        public string Status { get; set; }

        public BookingsModel()
        {
        }

        public BookingsModel(long accountId, string date, double totalPrice, string status)
        {
            AccountId = accountId;
            Date = date;
            TotalPrice = totalPrice;
            Status = status;
        }

    public long Id { get; set; }
    public long AccountId { get; set; }
    public string Date { get; set; }
    public double TotalPrice { get; set; }
    public enum Status { Done = 0, onGoing = 1, Canceled = 2}
    public Status BookingStatus { get; set; }

    public BookingsModel(long accountId, string date, double totalPrice, int status)
    {
        AccountId = accountId;
        Date = date;
        TotalPrice = totalPrice;
        BookingStatus = (Status)status;
 fab11e6c03fc3a7043fa742b4054f75b81c17d2b
    }
}