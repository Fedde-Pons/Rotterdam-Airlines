public class BookingsModel
{
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
    }
}


