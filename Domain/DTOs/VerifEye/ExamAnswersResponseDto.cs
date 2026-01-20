namespace Domain.DTOs.VerifEye
{
    public class ExamAnswersResponseDto
    {
        public string customerId { get; set; }
        public List<AnswerDto> examAnswers { get; set; }
        public string examId { get; set; }
    }
}
