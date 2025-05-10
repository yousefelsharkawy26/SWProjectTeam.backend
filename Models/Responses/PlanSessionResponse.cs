namespace Models.Responses;
public class PlanSessionResponse
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string Notes { get; set; }
    public bool Completed { get; set; }
    public string Status { get; set; }
}