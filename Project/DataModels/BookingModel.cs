public class BookingModel
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string Date { get; set; }
    public double TotalPrice { get; set; }

    private string _status;
    public string Status
    {
        get => _status;
        set => _status = NormalizeStatus(value);
    }

    public static string NormalizeStatus(string status)
    {
        if (string.IsNullOrEmpty(status))
            return status;

        return char.ToUpper(status[0]) + status[1..].ToLower();
    }

    public BookingModel() { }

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