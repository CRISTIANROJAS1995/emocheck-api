namespace Domain.Entities
{
    public class ExamSubject
    {
        public int ExamSubjectID { get; set; }
        public int ExamID { get; set; }
        public Exam Exam { get; set; }
        public int SubjectID { get; set; }
        public Subject Subject { get; set; }
        public string ExternalExamId { get; set; }
        public string ExternalExamUrl { get; set; }
        public string ExternalExamQueued { get; set; }
        public string ExternalExamStatus { get; set; }
        public string ExternalExamStep { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

        private ExamSubject() { }

        public ExamSubject(int examID, int subjectID, string externalExamId, string externalExamUrl, string createdBy, string modifiedBy)
        {
            if (string.IsNullOrEmpty(externalExamId))
            {
                throw new ArgumentException("El ExternalExamId no puede ser nulo o vacío.");
            }

            ExamID = examID;
            SubjectID = subjectID;
            ExternalExamId = externalExamId;
            ExternalExamUrl = externalExamUrl;
            CreatedBy = createdBy;
            CreatedAt = DateTime.Now;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }
    }
}
