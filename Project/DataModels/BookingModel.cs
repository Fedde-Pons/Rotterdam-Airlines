public class BookingModel
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string Date { get; set; }
    public double TotalPrice { get; set; }
    public string Status { get; set; }

    public BookingModel(int accountId, string date, double totalPrice, string status)
    {
        AccountId = accountId;
        Date = date;
        TotalPrice = totalPrice;
        Status = status;
    }
    public BookingModel(int accountId, string date, string status)
    {
        AccountId = accountId;
        Date = date;
        Status = status;
    }
}