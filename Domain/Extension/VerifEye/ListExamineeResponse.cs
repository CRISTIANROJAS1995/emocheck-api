namespace Domain.Extension.VerifEye
{
    public class ListExamineeResponse
    {
        public string CustomerId { get; set; } = default!;
        public string SubjectId { get; set; } = default!;
        public string SubjectName { get; set; } = default!;
        public string SubjectEmail { get; set; } = default!;
        public string SubjectMobile { get; set; } = default!;
        public string SubjectPhoto { get; set; } = default!;
        public string? SubjectToken { get; set; }
    }
}
