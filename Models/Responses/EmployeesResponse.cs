namespace Models.Responses;

public class EmployeesResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string Image { get; set; }
    public string Specialization { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string status { get; set; }
    public DateOnly CreateAt { get; set; }
}