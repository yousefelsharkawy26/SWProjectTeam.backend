namespace Models.Responses;

public class ClinicMemberResponse
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Role { get; set; }
    public DateOnly CreateAt { get; set; }
}
