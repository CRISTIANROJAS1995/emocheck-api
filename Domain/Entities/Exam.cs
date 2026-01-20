namespace Domain.Entities
{
    public class Exam
    {
        public int ExamID { get; set; }
        public int ExamTypeID { get; set; }
        public ExamType ExamType { get; set; }
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

        private Exam() { }

        public Exam(int examTypeID, string externalTemplateId, string externalExamName, string externalLocale, string externalCustomerId)
        {
            if (string.IsNullOrEmpty(externalTemplateId))
            {
                throw new ArgumentException("El TemplateId no puede ser nulo o vacío.");
            }

            ExamTypeID = examTypeID;
            ExternalTemplateId = externalTemplateId;
            ExternalExamName = externalExamName;
            ExternalLocale = externalLocale;
            ExternalCustomerId = externalCustomerId;
            CreatedAt = DateTime.Now;
            ModifiedAt = DateTime.Now;
        }
    }
}
