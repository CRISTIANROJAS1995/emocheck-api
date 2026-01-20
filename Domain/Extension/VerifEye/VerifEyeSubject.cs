namespace Domain.Extension.VerifEye
{
    public class VerifEyeSubject
    {
        public string CustomerId { get; set; } = default!;
        public string ExamLocale { get; set; } = default!;
        public DateTime ExamQueued { get; set; }
        public string ExamStatus { get; set; } = default!;
        public string ExamStep { get; set; } = default!;
        public string ExamUrl { get; set; } = default!;
        public string SubjectId { get; set; } = default!;
        public string TemplateId { get; set; } = default!;
    }
}
