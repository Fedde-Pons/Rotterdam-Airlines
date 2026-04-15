public class SeatModel
{
    public int Id { get; set; }
    public int AircraftId { get; set; }
    public string SeatNumber { get; set; }
    public int RowNumber { get; set; }
    public string Seatclass { get; set; }
    public bool IsWindows {get; set;}
    public bool IsExitRow {get; set;}
    public bool IsFirstRow {get; set;}
    public bool IsLastRow {get; set;}

    public SeatModel(int aircraftId, string seatNumber, int rowNumber, string seatclass, bool isWindows, bool isExitRow, bool isFirstRow, bool isLastRow)
    {
        AircraftId = aircraftId ; 
        SeatNumber = seatNumber;
        RowNumber = rowNumber;
        Seatclass = seatclass;
        IsWindows = isWindows;
        IsExitRow =  isExitRow;
        IsFirstRow = isFirstRow;
        IsLastRow = isLastRow;
    }
}