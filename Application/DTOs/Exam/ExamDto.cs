namespace Application.DTOs.Exam
{
    public class ExamDto
    {
        public int ExamID { get; set; }
        public ExamTypeDto ExamType { get; set; }
        public string ExternalTemplateId { get; set; }
        public string ExternalExamName { get; set; }
        public string ExternalLocale { get; set; }
        public string ExternalCustomerId { get; set; }
        public string Description { get; set; }
        public string Company { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
