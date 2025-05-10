namespace Models.Requests;
public class PlanSessionRequest
{
    public DateTime Date { get; set; }
    public string Notes { get; set; }
    public bool Completed { get; set; }
}
