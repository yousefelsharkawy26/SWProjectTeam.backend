using Microsoft.AspNetCore.Http;

namespace Models.Requests;
public class MedicalExaminationRequest
{
    public int PatientId { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public int Doctor { get; set; }
    public IFormFile File { get; set; }
}
