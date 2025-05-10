using Models;

namespace DataAccess.Repository.IRepository;
public interface IMedicalExaminationRepository : IRepository<MedicalExamination>
{
    void Update(MedicalExamination obj);
}
