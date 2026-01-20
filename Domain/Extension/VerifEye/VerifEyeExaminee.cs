namespace Domain.Extension.VerifEye
{
    public class VerifEyeExaminee
    {
        public string CustomerId { get; set; } = default!;
        public string SubjectId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Mobile { get; set; } = default!;
        public string? Token { get; set; }
        public string Photo { get; set; } = default!;
    }
}
