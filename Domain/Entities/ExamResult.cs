namespace Domain.Entities
{
    public class ExamResult
    {
        public int ExamResultID { get; set; }
        public int ExamSubjectID { get; set; }
        public ExamSubject ExamSubject { get; set; }
        public string ExternalExamErrors { get; set; }
        public string ExternalExamModel { get; set; }
        public string ExternalExamQuestions { get; set; }
        public DateTime? ExternalExamQueued { get; set; }
        public string ExternalExamResult1 { get; set; }
        public string ExternalExamResult2 { get; set; }
        public string ExternalExamResult3 { get; set; }
        public string ExternalExamScore1 { get; set; }
        public string ExternalExamScore2 { get; set; }
        public string ExternalExamScore3 { get; set; }
        public string ExternalExamScore4 { get; set; }
        public DateTime? ExternalExamScored { get; set; }
        public string ExternalExamTimeouts { get; set; }
        public string ExternalExamTopic { get; set; }
        public string ExternalTemplateType { get; set; }
        public string ResultExamId { get; set; }

        private ExamResult() { }

        public ExamResult(
            int examSubjectID,
            string externalExamErrors,
            string externalExamModel,
            string externalExamQuestions,
            DateTime? externalExamQueued,
            string externalExamResult1,
            string externalExamResult2,
            string externalExamResult3,
            string externalExamScore1,
            string externalExamScore2,
            string externalExamScore3,
            string externalExamScore4,
            DateTime? externalExamScored,
            string externalExamTimeouts,
            string externalExamTopic,
            string externalTemplateType,
            string resultExamId)
        {
            if (examSubjectID <= 0)
            {
                throw new ArgumentException("El ExamSubjectID debe ser mayor a cero.");
            }

            ExamSubjectID = examSubjectID;
            ExternalExamErrors = externalExamErrors ?? string.Empty;
            ExternalExamModel = externalExamModel ?? string.Empty;
            ExternalExamQuestions = externalExamQuestions ?? string.Empty;
            ExternalExamQueued = externalExamQueued;
            ExternalExamResult1 = externalExamResult1 ?? string.Empty;
            ExternalExamResult2 = externalExamResult2 ?? string.Empty;
            ExternalExamResult3 = externalExamResult3 ?? string.Empty;
            ExternalExamScore1 = externalExamScore1 ?? string.Empty;
            ExternalExamScore2 = externalExamScore2 ?? string.Empty;
            ExternalExamScore3 = externalExamScore3 ?? string.Empty;
            ExternalExamScore4 = externalExamScore4 ?? string.Empty;
            ExternalExamScored = externalExamScored;
            ExternalExamTimeouts = externalExamTimeouts ?? string.Empty;
            ExternalExamTopic = externalExamTopic ?? string.Empty;
            ExternalTemplateType = externalTemplateType ?? string.Empty;
            ResultExamId = resultExamId ?? string.Empty;
        }
    }
}
