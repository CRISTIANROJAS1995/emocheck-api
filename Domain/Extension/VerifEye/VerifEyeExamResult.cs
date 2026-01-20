namespace Domain.Extension.VerifEye
{
    public class VerifEyeExamResult
    {
        public string CustomerId { get; set; } = default!;
        public string ExamErrors { get; set; } = default!;
        public string ExamId { get; set; } = default!;
        public string ExamLocale { get; set; } = default!;
        public string ExamModel { get; set; } = default!;
        public string ExamName { get; set; } = default!;
        public string ExamQuestions { get; set; } = default!;
        public DateTime ExamQueued { get; set; }
        public string ExamResult1 { get; set; } = default!;
        public string ExamResult2 { get; set; } = default!;
        public string ExamResult3 { get; set; } = default!;
        public string ExamScore1 { get; set; } = default!;
        public string ExamScore2 { get; set; } = default!;
        public string ExamScore3 { get; set; } = default!;
        public string ExamScore4 { get; set; } = default!;
        public DateTime ExamScored { get; set; }
        public string ExamTimeouts { get; set; } = default!;
        public string ExamTopic { get; set; } = default!;
        public string SubjectId { get; set; } = default!;
        public string TemplateId { get; set; } = default!;
        public string TemplateType { get; set; } = default!;
    }
}
