using Application.DTOs.ExamSubject;

namespace Application.DTOs.ExamResult
{
    public class ExamResultDto
    {
        public int ExamResultID { get; set; }
        public ExamSubjectDto ExamSubject { get; set; }
        public string ExternalExamErrors { get; set; }
        public string ExternalExamModel { get; set; }
        public string ExternalExamQuestions { get; set; }
        public string ExternalExamQueued { get; set; }
        public string ExternalExamResult1 { get; set; }
        public string ExternalExamResult2 { get; set; }
        public string ExternalExamResult3 { get; set; }
        public string ExternalExamScore1 { get; set; }
        public string ExternalExamScore2 { get; set; }
        public string ExternalExamScore3 { get; set; }
        public string ExternalExamScore4 { get; set; }
        public string ExternalExamScored { get; set; }
        public string ExternalExamTimeouts { get; set; }
        public string ExternalExamTopic { get; set; }
        public string ExternalTemplateType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ResultExamId { get; set; }
    }
}
