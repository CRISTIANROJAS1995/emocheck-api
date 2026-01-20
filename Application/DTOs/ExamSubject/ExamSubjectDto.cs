using Application.DTOs.Exam;
using Application.DTOs.Subject;

namespace Application.DTOs.ExamSubject
{
    public class ExamSubjectDto
    {
        public int ExamSubjectID { get; set; }
        public ExamDto Exam { get; set; }
        public SubjectDto Subject { get; set; }
        public string ExternalExamId { get; set; }
        public string ExternalExamUrl { get; set; }
        public string ExternalExamQueued { get; set; }
        public string ExternalExamStatus { get; set; }
        public string ExternalExamStep { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
