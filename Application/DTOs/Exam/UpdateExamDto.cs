namespace Application.DTOs.Exam
{
    public class UpdateExamDto
    {
        public int ExamTypeID { get; set; }
        public string? ExternalTemplateId { get; set; }
        public string? ExternalExamName { get; set; }
        public string? ExternalLocale { get; set; }
        public string? ExternalCustomerId { get; set; }
        public string? Description { get; set; }
        public string? Company { get; set; }
    }
}
