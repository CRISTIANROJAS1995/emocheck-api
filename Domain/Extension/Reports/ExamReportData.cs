namespace Domain.Extension.Reports
{
    public class ExamReportData
    {
        public string Company { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string SubjectLastName { get; set; } = string.Empty;
        public string SubjectId { get; set; } = string.Empty;
        public string SubjectEmail { get; set; } = string.Empty;
        public string SubjectPhone { get; set; } = string.Empty;
        public string Conclusion { get; set; } = string.Empty;
        public List<ExamResultTopic> Results { get; set; } = new();
        public int DataQuality { get; set; }

        // Tabla de análisis detallado
        public List<ExamAnalysisRow> Analysis { get; set; } = new();

        // Fotos del examinado (byte[])
        public List<byte[]> Photos { get; set; } = new();

        // Procedimiento y recomendaciones
        public string Procedure { get; set; } = string.Empty;
        public List<string> Recommendations { get; set; } = new();

        // Logo (byte[])
        public byte[]? Logo { get; set; }
    }

    public class ExamAnalysisRow
    {
        public string Topic { get; set; } = string.Empty;
        public string Unexpected { get; set; } = string.Empty;
        public string NoAnswer { get; set; } = string.Empty;
        public string NotRecognized { get; set; } = string.Empty;
    }
}
