// Describes the seat-grid layout of an aircraft: how many rows, which rows are
// business class, the seat letters per row and where the exit rows sit.
// Used by the seat map to render the cabin and by SeatingLogic to reason about seats.
public class AircraftLayoutModel
{
    public string Title { get; set; }
    public string[] Letters { get; set; }
    public int TotalRows { get; set; }
    public int BusinessRows { get; set; }
    public int FirstExitRow { get; set; }
    public int SecondExitRow { get; set; }

    public AircraftLayoutModel(string title, string[] letters, int totalRows, int businessRows, int firstExitRow, int secondExitRow)
    {
        Title = title;
        Letters = letters;
        TotalRows = totalRows;
        BusinessRows = businessRows;
        FirstExitRow = firstExitRow;
        SecondExitRow = secondExitRow;
    }
}
