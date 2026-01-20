using DinkToPdf;
using DinkToPdf.Contracts;
using Domain.Extension.Reports;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class ExamReportService : IExamReportService
    {
        private readonly ILogger<ExamReportService> _logger;
        private readonly IConverter _converter;
        private readonly IVerifEyeService _verifEyeService;

        public ExamReportService(ILogger<ExamReportService> logger, IConverter converter, IVerifEyeService verifEyeService)
        {
            _logger = logger;
            _converter = converter;
            _verifEyeService = verifEyeService;
        }

        public async Task<byte[]> GenerateExamReportPdfAsync(string externalExamId)
        {
            try
            {
                //Consultar el resultado del examen.
                var repoData = await _verifEyeService.GetExamRepositoryByIdAsync(externalExamId);
                if (repoData == null)
                {
                    throw new Exception($"No se encontró información de examen en Converus para el id {externalExamId}");
                }

                //Consultar el detalle del candidato
                var consultSubject = await _verifEyeService.GetExamineeByIdAsync(repoData.subjectId);
                if (consultSubject == null)
                {
                    throw new Exception($"No se encontró información del candidato");
                }

                // Convertir los examScore de string a double
                double score1Raw = 0, score2Raw = 0, score3Raw = 0, score4Raw = 0;
                double.TryParse(repoData.examScore1?.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out score1Raw);
                double.TryParse(repoData.examScore2?.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out score2Raw);
                double.TryParse(repoData.examScore3?.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out score3Raw);
                double.TryParse(repoData.examScore4?.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out score4Raw);

                int examScore1 = score1Raw > 0 ? (int)Math.Round(score1Raw * 100) : (int)score1Raw;
                int examScore2 = score2Raw > 0 ? (int)Math.Round(score2Raw * 100) : (int)score2Raw;
                int examScore3 = score3Raw > 0 ? (int)Math.Round(score3Raw * 100) : (int)score3Raw;
                int examScore4 = score4Raw > 0 ? (int)Math.Round(score4Raw * 100) : (int)score4Raw;

                examScore1 = examScore1 > 99 ? 99 : examScore1 < 1 && examScore1 > 0 ? 1 : examScore1;
                examScore2 = examScore2 > 99 ? 99 : examScore2 < 1 && examScore2 > 0 ? 1 : examScore2;
                examScore3 = examScore3 > 99 ? 99 : examScore3 < 1 && examScore3 > 0 ? 1 : examScore3;
                examScore4 = examScore4 > 99 ? 99 : examScore4 < 1 && examScore4 > 0 ? 1 : examScore4;

                // Calcular outcomeStatus, outcomeDetail, outcomeImage, outcomeColor
                string outcomeStatus = "";
                string outcomeDetail = "";
                string outcomeImage = "";
                string outcomeColor = "";
                string outcomeMessage = "";

                if (examScore1 == -3)
                {
                    outcomeStatus = "Indeterminado";
                    outcomeDetail = "Sin licencia";
                    outcomeImage = @"<svg width='36' height='36' viewBox='0 0 36 36' xmlns='http://www.w3.org/2000/svg'><circle cx='18' cy='18' r='18' fill='#FFD700'/><text x='18' y='24' text-anchor='middle' font-size='24' fill='#fff'>?</text></svg>";
                    outcomeColor = "#FFD700"; // Amarillo
                    outcomeMessage = "El resultado es indeterminado por falta de licencia.";
                }
                else if ((examScore1 > 0 && examScore1 < 50) || (examScore2 > 0 && examScore2 < 50) ||
                         (examScore3 > 0 && examScore3 < 50) || (examScore4 > 0 && examScore4 < 50))
                {
                    outcomeStatus = "No confiable";
                    outcomeDetail = "Engañoso";
                    outcomeImage = @"<svg width='36' height='36' viewBox='0 0 36 36' xmlns='http://www.w3.org/2000/svg'><circle cx='18' cy='18' r='18' fill='#DD2233'/><line x1='12' y1='12' x2='24' y2='24' stroke='#fff' stroke-width='3.5'/><line x1='24' y1='12' x2='12' y2='24' stroke='#fff' stroke-width='3.5'/></svg>";
                    outcomeColor = "#DD2233"; // Rojo
                    outcomeMessage = "El candidato presentó resultados engañosos en al menos un tema.";
                }
                else if ((examScore1 == -7) || (examScore2 == -7) || (examScore3 == -7) || (examScore4 == -7))
                {
                    outcomeStatus = "No confiable";
                    outcomeDetail = "Probable confesión";
                    outcomeImage = @"<svg width='36' height='36' viewBox='0 0 36 36' xmlns='http://www.w3.org/2000/svg'><circle cx='18' cy='18' r='18' fill='#DD2233'/><line x1='12' y1='12' x2='24' y2='24' stroke='#fff' stroke-width='3.5'/><line x1='24' y1='12' x2='12' y2='24' stroke='#fff' stroke-width='3.5'/></svg>";
                    outcomeColor = "#DD2233";
                    outcomeMessage = "El candidato realizó una probable confesión durante la prueba.";
                }
                else if ((examScore1 == -10) || (examScore2 == -10) || (examScore3 == -10) || (examScore4 == -10))
                {
                    outcomeStatus = "No confiable";
                    outcomeDetail = "Respuestas aleatorias";
                    outcomeImage = @"<svg width='36' height='36' viewBox='0 0 36 36' xmlns='http://www.w3.org/2000/svg'><circle cx='18' cy='18' r='18' fill='#DD2233'/><line x1='12' y1='12' x2='24' y2='24' stroke='#fff' stroke-width='3.5'/><line x1='24' y1='12' x2='12' y2='24' stroke='#fff' stroke-width='3.5'/></svg>";
                    outcomeColor = "#DD2233";
                    outcomeMessage = "El candidato dio respuestas aleatorias, no se puede confiar en el resultado.";
                }
                else if ((examScore1 == -6) || (examScore2 == -6) || (examScore3 == -6) || (examScore4 == -6))
                {
                    outcomeStatus = "No confiable";
                    outcomeDetail = "Demasiados timeouts";
                    outcomeImage = @"<svg width='36' height='36' viewBox='0 0 36 36' xmlns='http://www.w3.org/2000/svg'><circle cx='18' cy='18' r='18' fill='#DD2233'/><line x1='12' y1='12' x2='24' y2='24' stroke='#fff' stroke-width='3.5'/><line x1='24' y1='12' x2='12' y2='24' stroke='#fff' stroke-width='3.5'/></svg>";
                    outcomeColor = "#DD2233";
                    outcomeMessage = "Demasiados timeouts, resultado no confiable.";
                }
                else if ((examScore1 == -4) || (examScore2 == -4) || (examScore3 == -4) || (examScore4 == -4))
                {
                    outcomeStatus = "No confiable";
                    outcomeDetail = "Datos insuficientes";
                    outcomeImage = @"<svg width='36' height='36' viewBox='0 0 36 36' xmlns='http://www.w3.org/2000/svg'><circle cx='18' cy='18' r='18' fill='#DD2233'/><line x1='12' y1='12' x2='24' y2='24' stroke='#fff' stroke-width='3.5'/><line x1='24' y1='12' x2='12' y2='24' stroke='#fff' stroke-width='3.5'/></svg>";
                    outcomeColor = "#DD2233";
                    outcomeMessage = "No se obtuvo suficiente información para determinar el resultado.";
                }
                else if ((examScore1 == -11) || (examScore2 == -11) || (examScore3 == -11) || (examScore4 == -11))
                {
                    outcomeStatus = "No confiable";
                    outcomeDetail = "Audio defectuoso";
                    outcomeImage = @"<svg width='36' height='36' viewBox='0 0 36 36' xmlns='http://www.w3.org/2000/svg'><circle cx='18' cy='18' r='18' fill='#DD2233'/><line x1='12' y1='12' x2='24' y2='24' stroke='#fff' stroke-width='3.5'/><line x1='24' y1='12' x2='12' y2='24' stroke='#fff' stroke-width='3.5'/></svg>";
                    outcomeColor = "#DD2233";
                    outcomeMessage = "El audio no fue suficiente para determinar el resultado.";
                }
                else if ((examScore1 == -9) || (examScore2 == -9) || (examScore3 == -9) || (examScore4 == -9))
                {
                    outcomeStatus = "Indeterminado";
                    outcomeDetail = "Prueba demo";
                    outcomeImage = @"<svg width='36' height='36' viewBox='0 0 36 36' xmlns='http://www.w3.org/2000/svg'><circle cx='18' cy='18' r='18' fill='#FFD700'/><text x='18' y='24' text-anchor='middle' font-size='24' fill='#fff'>?</text></svg>";
                    outcomeColor = "#FFD700";
                    outcomeMessage = "Prueba de demostración, resultado no válido para evaluación.";
                }
                else if ((examScore1 >= 50) && (examScore2 >= 50) && (examScore3 >= 50) && (examScore4 >= 50))
                {
                    outcomeStatus = "Confiable";
                    outcomeDetail = "Veraz";
                    outcomeImage = @"<svg width='36' height='36' viewBox='0 0 36 36' xmlns='http://www.w3.org/2000/svg'><circle cx='18' cy='18' r='18' fill='#00b050'/><polyline points='10,19 16,25 26,13' fill='none' stroke='#fff' stroke-width='3.5' stroke-linecap='round' stroke-linejoin='round'/></svg>";
                    outcomeColor = "#00b050"; // Verde
                    outcomeMessage = "Todos los resultados por tema del candidato son confiables.";
                }

                // Obtener preguntas y respuestas del examen
                var questions = await _verifEyeService.GetExamQuestionsAsync(repoData.templateId, externalExamId);
                var answers = await _verifEyeService.GetExamAnswersAsync(externalExamId);

                int totalQuestions = questions?.Count ?? 0;
                int unexpectedAnswers = 0;
                int unansweredQuestions = 0;
                // Variables para estadísticas por grupo de preguntas
                int R1Wrong = 0, R1Timeout = 0, R1Bad = 0, R1Tot = 0;
                int R2Wrong = 0, R2Timeout = 0, R2Bad = 0, R2Tot = 0;
                int R3Wrong = 0, R3Timeout = 0, R3Bad = 0, R3Tot = 0;
                int R4Wrong = 0, R4Timeout = 0, R4Bad = 0, R4Tot = 0;

                double R1WrongPerc = 0, R1TOPerc = 0, R1BadPerc = 0;
                double R2WrongPerc = 0, R2TOPerc = 0, R2BadPerc = 0;
                double R3WrongPerc = 0, R3TOPerc = 0, R3BadPerc = 0;
                double R4WrongPerc = 0, R4TOPerc = 0, R4BadPerc = 0;

                string PreTestInstructions = "";
                string R1ExampleQuestion = "", R1ExampleAnswer = "", R1ExampleAnswerText = "";
                string R2ExampleQuestion = "", R2ExampleAnswer = "", R2ExampleAnswerText = "";
                string R3ExampleQuestion = "", R3ExampleAnswer = "", R3ExampleAnswerText = "";
                string R4ExampleQuestion = "", R4ExampleAnswer = "", R4ExampleAnswerText = "";

                var questionAnswerList = new List<(string Question, string Answer)>();

                if (answers != null && totalQuestions > 0)
                {
                    foreach (var answer in answers)
                    {
                        var RQ = answer.itemId.Substring(0, 2);
                        if (answer.itemActual == "1" || answer.itemActual == "0")
                        {
                            if (answer.itemActual != answer.itemExpected)
                            {
                                if (RQ == "R1") R1Wrong++;
                                if (RQ == "R2") R2Wrong++;
                                if (RQ == "R3") R3Wrong++;
                                if (RQ == "R4") R4Wrong++;
                            }
                        }
                        else if (answer.itemActual == "-1")
                        {
                            if (RQ == "R1") R1Timeout++;
                            if (RQ == "R2") R2Timeout++;
                            if (RQ == "R3") R3Timeout++;
                            if (RQ == "R4") R4Timeout++;
                        }
                        else
                        {
                            if (RQ == "R1") R1Bad++;
                            if (RQ == "R2") R2Bad++;
                            if (RQ == "R3") R3Bad++;
                            if (RQ == "R4") R4Bad++;
                        }
                        if (RQ == "R1") R1Tot++;
                        if (RQ == "R2") R2Tot++;
                        if (RQ == "R3") R3Tot++;
                        if (RQ == "R4") R4Tot++;

                        // Contar para totales generales
                        if (string.IsNullOrEmpty(answer.itemActual))
                        {
                            unansweredQuestions++;
                        }
                        else if (answer.itemActual != answer.itemExpected)
                        {
                            unexpectedAnswers++;
                        }
                    }
                }

                R1WrongPerc = R1Tot == 0 ? 0 : Math.Round((double)R1Wrong / R1Tot * 100, 2);
                R1TOPerc = R1Tot == 0 ? 0 : Math.Round((double)R1Timeout / R1Tot * 100, 2);
                R1BadPerc = R1Tot == 0 ? 0 : Math.Round((double)R1Bad / R1Tot * 100, 2);
                R2WrongPerc = R2Tot == 0 ? 0 : Math.Round((double)R2Wrong / R2Tot * 100, 2);
                R2TOPerc = R2Tot == 0 ? 0 : Math.Round((double)R2Timeout / R2Tot * 100, 2);
                R2BadPerc = R2Tot == 0 ? 0 : Math.Round((double)R2Bad / R2Tot * 100, 2);
                R3WrongPerc = R3Tot == 0 ? 0 : Math.Round((double)R3Wrong / R3Tot * 100, 2);
                R3TOPerc = R3Tot == 0 ? 0 : Math.Round((double)R3Timeout / R3Tot * 100, 2);
                R3BadPerc = R3Tot == 0 ? 0 : Math.Round((double)R3Bad / R3Tot * 100, 2);
                R4WrongPerc = R4Tot == 0 ? 0 : Math.Round((double)R4Wrong / R4Tot * 100, 2);
                R4TOPerc = R4Tot == 0 ? 0 : Math.Round((double)R4Timeout / R4Tot * 100, 2);
                R4BadPerc = R4Tot == 0 ? 0 : Math.Round((double)R4Bad / R4Tot * 100, 2);

                if (questions != null)
                {
                    foreach (var question in questions)
                    {
                        var RQ = question.itemId.Substring(0, 2);
                        if (RQ == "PI")
                        {
                            PreTestInstructions += question.itemQuestion + "<br/><br/>";
                        }
                        if (RQ == "R1" && string.IsNullOrEmpty(R1ExampleQuestion))
                        {
                            R1ExampleQuestion = question.itemQuestion;
                            R1ExampleAnswer = question.itemAnswer;
                            R1ExampleAnswerText = question.itemAnswer == "1" ? "Verdadero" : (question.itemAnswer == "0" ? "Falso" : question.itemAnswer);
                        }
                        if (RQ == "R2" && string.IsNullOrEmpty(R2ExampleQuestion))
                        {
                            R2ExampleQuestion = question.itemQuestion;
                            R2ExampleAnswer = question.itemAnswer;
                            R2ExampleAnswerText = question.itemAnswer == "1" ? "Verdadero" : (question.itemAnswer == "0" ? "Falso" : question.itemAnswer);
                        }
                        if (RQ == "R3" && string.IsNullOrEmpty(R3ExampleQuestion))
                        {
                            R3ExampleQuestion = question.itemQuestion;
                            R3ExampleAnswer = question.itemAnswer;
                            R3ExampleAnswerText = question.itemAnswer == "1" ? "Verdadero" : (question.itemAnswer == "0" ? "Falso" : question.itemAnswer);
                        }
                        if (RQ == "R4" && string.IsNullOrEmpty(R4ExampleQuestion))
                        {
                            R4ExampleQuestion = question.itemQuestion;
                            R4ExampleAnswer = question.itemAnswer;
                            R4ExampleAnswerText = question.itemAnswer == "1" ? "Verdadero" : (question.itemAnswer == "0" ? "Falso" : question.itemAnswer);
                        }
                        // Listado de preguntas con respuestas
                        questionAnswerList.Add((question.itemQuestion, question.itemAnswer));
                    }
                }

                if (answers != null && totalQuestions > 0)
                {
                    foreach (var answer in answers)
                    {
                        // Buscar la pregunta correspondiente por itemId si necesitas validar tipo
                        // var question = questions.FirstOrDefault(q => q.itemId == answer.itemId);
                        if (string.IsNullOrEmpty(answer.itemActual))
                        {
                            unansweredQuestions++;
                        }
                        else if (answer.itemActual != answer.itemExpected)
                        {
                            unexpectedAnswers++;
                        }
                    }
                }

                double unexpectedAnswersPct = totalQuestions > 0 ? Math.Round((double)unexpectedAnswers * 100 / totalQuestions, 2) : 0;
                double unansweredQuestionsPct = totalQuestions > 0 ? Math.Round((double)unansweredQuestions * 100 / totalQuestions, 2) : 0;

                // Variables para Appendix A y B (en español)
                string appendixA = "";
                string appendixB = "";

                // Appendix A
                if (outcomeStatus == "Confiable")
                {
                    appendixA = "El examinado fue categorizado como 'Confiable – Veraz' porque ninguno de los temas de la prueba produjo una reacción similar a la observada en sujetos culpables en estudios de laboratorio validados científicamente donde se conoce la verdad absoluta (conocimiento absoluto de inocencia y culpa).";
                }
                else if (outcomeStatus == "No confiable" && outcomeDetail == "Engañoso")
                {
                    appendixA = "El examinado fue categorizado como 'No Confiable – Engañoso' porque al menos un tema de la prueba produjo una reacción similar a la observada en sujetos engañosos en estudios de laboratorio validados científicamente donde se conoce la verdad absoluta.";
                }
                else if (outcomeStatus == "No confiable" && outcomeDetail == "Respuestas aleatorias")
                {
                    appendixA = $"El examinado fue categorizado como 'No Confiable – Respuestas Aleatorias' porque un alto porcentaje de preguntas fueron respondidas de manera contraria a lo esperado. En este caso, el examinado dio respuestas inesperadas en {unexpectedAnswers} preguntas ({unexpectedAnswersPct}% del total).";
                }
                else if (outcomeStatus == "No confiable" && outcomeDetail == "Probable confesión")
                {
                    appendixA = $"El examinado fue categorizado como 'No Confiable – Probable Confesión' porque un alto porcentaje de preguntas fueron respondidas de manera contraria a lo esperado, como si admitiera participación en uno o más de los temas evaluados. En este caso, el examinado dio respuestas inesperadas en {unexpectedAnswers} preguntas ({unexpectedAnswersPct}% del total).";
                }

                // Appendix B
                if (unansweredQuestionsPct > 0 && unansweredQuestionsPct < 33)
                {
                    appendixB = $"El examinado también dejó {unansweredQuestions} preguntas sin responder ({unansweredQuestionsPct}% del total). Esto no es inusual considerando la cantidad de preguntas y la velocidad de presentación. Esta tasa no tuvo un impacto significativo en el resultado global.";
                }
                else if (unansweredQuestionsPct >= 33)
                {
                    appendixB = $"El examinado también dejó {unansweredQuestions} preguntas sin responder ({unansweredQuestionsPct}% del total). Un porcentaje alto de preguntas sin responder puede indicar que el examinado dejó preguntas sin responder intencionalmente o no comprende suficientemente las preguntas. Si fue intencional, esto puede ser un problema de no cumplimiento.";
                }
                else if (unexpectedAnswersPct > 0 && outcomeDetail != "Respuestas aleatorias" && outcomeDetail != "Probable confesión")
                {
                    appendixB = $"El examinado dio respuestas inesperadas en {unexpectedAnswers} preguntas ({unexpectedAnswersPct}% del total). En una prueba de detección de mentiras, la mayoría niega participación en el tema evaluado. Si responde contrario a lo esperado, puede ser accidental, una admisión o una respuesta intencionalmente aleatoria. Esta tasa no tuvo un impacto significativo en el resultado global.";
                }
                else if (unexpectedAnswersPct == 0 && unansweredQuestionsPct == 0)
                {
                    appendixB = "Durante esta prueba, el examinado cumplió con las instrucciones.";
                }

                //fotos del candidato
                string photo1Src = string.IsNullOrEmpty(repoData.examFace1)
                     ? "foto1.jpg"
                     : ToDataUri(repoData.examFace1);

                string photo2Src = string.IsNullOrEmpty(repoData.examFace2)
                     ? "foto2.jpg"
                     : ToDataUri(repoData.examFace2);

                string photo3Src = string.IsNullOrEmpty(repoData.examFace3)
                     ? "foto3.jpg"
                     : ToDataUri(repoData.examFace3);

                string photo4Src = string.IsNullOrEmpty(repoData.examFace4)
                     ? "foto4.jpg"
                     : ToDataUri(repoData.examFace4);

                DateTime examStart = DateTime.Parse(repoData.examStart);
                DateTime? examFace1TS = !string.IsNullOrEmpty(repoData.examFace1TS) ? DateTime.Parse(repoData.examFace1TS) : (DateTime?)null;
                DateTime? examFace2TS = !string.IsNullOrEmpty(repoData.examFace2TS) ? DateTime.Parse(repoData.examFace2TS) : (DateTime?)null;
                DateTime? examFace3TS = !string.IsNullOrEmpty(repoData.examFace3TS) ? DateTime.Parse(repoData.examFace3TS) : (DateTime?)null;
                DateTime? examFace4TS = !string.IsNullOrEmpty(repoData.examFace4TS) ? DateTime.Parse(repoData.examFace4TS) : (DateTime?)null;

                string GetElapsedTime(DateTime? ts)
                {
                    if (ts == null) return "";
                    var elapsed = ts.Value - examStart;
                    return $"{elapsed.Minutes}:{elapsed.Seconds:D2}.{elapsed.Milliseconds:D3}";
                }

                string photo1Time = GetElapsedTime(examFace1TS);
                string photo2Time = GetElapsedTime(examFace2TS);
                string photo3Time = GetElapsedTime(examFace3TS);
                string photo4Time = GetElapsedTime(examFace4TS);

                // Determinar el color para DATA_QUALITY basado en el valor
                int dataQualityValue = 0;
                int.TryParse(repoData.dataQuality?.ToString(), out dataQualityValue);
                string dataQualityColor = dataQualityValue < 50 ? "#DD2233" : "#00b050"; // Rojo si < 50, Verde si >= 50

                string templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "report-template.html");
                var html = File.ReadAllText(templatePath);

                html = html.Replace("{{COMPANY}}", repoData.customerName ?? "")
                           .Replace("{{SUBJECT_NAME}}", consultSubject.Name ?? "")
                           .Replace("{{SUBJECT_ID}}", consultSubject.SubjectId ?? "")
                           .Replace("{{SUBJECT_EMAIL}}", consultSubject.Email ?? "")
                           .Replace("{{SUBJECT_PHONE}}", consultSubject.Mobile ?? "")
                           .Replace("{{EXAM_SCORE_1}}", examScore1.ToString())
                           .Replace("{{EXAM_SCORE_2}}", examScore2.ToString())
                           .Replace("{{EXAM_SCORE_3}}", examScore3.ToString())
                           .Replace("{{EXAM_SCORE_4}}", examScore4.ToString())
                           .Replace("{{OUTCOME_STATUS}}", outcomeStatus)
                           .Replace("{{OUTCOME_DETAIL}}", outcomeDetail)
                           .Replace("{{OUTCOME_IMAGE}}", outcomeImage)
                           .Replace("{{OUTCOME_COLOR}}", outcomeColor)
                           .Replace("{{OUTCOME_MESSAGE}}", outcomeMessage)
                           .Replace("{{APPENDIX_A}}", appendixA)
                           .Replace("{{APPENDIX_B}}", appendixB)
                           .Replace("{{PRE_TEST_INSTRUCTIONS}}", PreTestInstructions)
                           .Replace("{{R1_EXAMPLE_QUESTION}}", R1ExampleQuestion)
                           .Replace("{{R1_EXAMPLE_ANSWER_TEXT}}", R1ExampleAnswerText)
                           .Replace("{{R2_EXAMPLE_QUESTION}}", R2ExampleQuestion)
                           .Replace("{{R2_EXAMPLE_ANSWER_TEXT}}", R2ExampleAnswerText)
                           .Replace("{{R3_EXAMPLE_QUESTION}}", R3ExampleQuestion)
                           .Replace("{{R3_EXAMPLE_ANSWER_TEXT}}", R3ExampleAnswerText)
                           .Replace("{{R4_EXAMPLE_QUESTION}}", R4ExampleQuestion)
                           .Replace("{{R4_EXAMPLE_ANSWER_TEXT}}", R4ExampleAnswerText)
                           .Replace("{{R1_WRONG_PERC}}", R1WrongPerc.ToString())
                           .Replace("{{R1_TO_PERC}}", R1TOPerc.ToString())
                           .Replace("{{R1_BAD_PERC}}", R1BadPerc.ToString())
                           .Replace("{{R2_WRONG_PERC}}", R2WrongPerc.ToString())
                           .Replace("{{R2_TO_PERC}}", R2TOPerc.ToString())
                           .Replace("{{R2_BAD_PERC}}", R2BadPerc.ToString())
                           .Replace("{{R3_WRONG_PERC}}", R3WrongPerc.ToString())
                           .Replace("{{R3_TO_PERC}}", R3TOPerc.ToString())
                           .Replace("{{R3_BAD_PERC}}", R3BadPerc.ToString())
                           .Replace("{{R4_WRONG_PERC}}", R4WrongPerc.ToString())
                           .Replace("{{R4_TO_PERC}}", R4TOPerc.ToString())
                           .Replace("{{R4_BAD_PERC}}", R4BadPerc.ToString())
                           .Replace("{{PHOTO_1}}", photo1Src)
                           .Replace("{{PHOTO_2}}", photo2Src)
                           .Replace("{{PHOTO_3}}", photo3Src)
                           .Replace("{{PHOTO_4}}", photo4Src)
                           .Replace("{{PHOTO_1_TIME}}", photo1Time)
                           .Replace("{{PHOTO_2_TIME}}", photo2Time)
                           .Replace("{{PHOTO_3_TIME}}", photo3Time)
                           .Replace("{{PHOTO_4_TIME}}", photo4Time)
                           .Replace("{{R1_TITLE}}", "R1. " + (repoData.examTopic ?? ""))
                           .Replace("{{R1_SCORE}}", examScore1.ToString())
                           .Replace("{{R1_STATUS}}", GetOutcomeDetail(examScore1))
                           .Replace("{{R1_COLOR}}", GetOutcomeColor(examScore1))
                           .Replace("{{R2_TITLE}}", "R2. " + (repoData.examResult1 ?? ""))
                           .Replace("{{R2_SCORE}}", examScore2.ToString())
                           .Replace("{{R2_STATUS}}", GetOutcomeDetail(examScore2))
                           .Replace("{{R2_COLOR}}", GetOutcomeColor(examScore2))
                           .Replace("{{R3_TITLE}}", "R3. " + (repoData.examResult2 ?? ""))
                           .Replace("{{R3_SCORE}}", examScore3.ToString())
                           .Replace("{{R3_STATUS}}", GetOutcomeDetail(examScore3))
                           .Replace("{{R3_COLOR}}", GetOutcomeColor(examScore3))
                           .Replace("{{R4_TITLE}}", "R4. " + (repoData.examResult3 ?? ""))
                           .Replace("{{R4_SCORE}}", examScore4.ToString())
                           .Replace("{{R4_STATUS}}", GetOutcomeDetail(examScore4))
                           .Replace("{{R4_COLOR}}", GetOutcomeColor(examScore4))
                           .Replace("{{DATA_QUALITY}}", repoData.dataQuality ?? "")
                           .Replace("{{DATA_QUALITY_COLOR}}", dataQualityColor);

                string questionAnswerHtml = string.Join("<br/>", questionAnswerList.Select(q => $"<b>{q.Question}</b>: {q.Answer}"));
                html = html.Replace("{{QUESTION_ANSWER_LIST}}", questionAnswerHtml);

                var doc = new HtmlToPdfDocument()
                {
                    GlobalSettings = {
                        ColorMode = ColorMode.Color,
                        Orientation = Orientation.Portrait,
                        PaperSize = PaperKind.A4,
                        Margins = new MarginSettings { Top = 2, Bottom = 4, Left = 4, Right = 4 }
                    },
                    Objects = {
                        new ObjectSettings() {
                            HtmlContent = html,
                            WebSettings = { DefaultEncoding = "utf-8", LoadImages = true },
                            FooterSettings = { Right = "Página [page] de [toPage]", FontSize = 9 }
                        }
                    }
                };
                var pdfBytes = _converter.Convert(doc);
                _logger.LogInformation("PDF generado correctamente, tamaño: {Size} bytes", pdfBytes.Length);
                return pdfBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar el PDF");
                throw;
            }
        }

        private string GetOutcomeDetail(int score)
        {
            if (score >= 50) return "Confiable";
            if (score > 0 && score < 50) return "Engañoso";
            if (score == -3) return "Sin licencia";
            if (score == -7) return "Probable confesión";
            if (score == -10) return "Respuestas aleatorias";
            if (score == -6) return "Demasiados timeouts";
            if (score == -4) return "Datos insuficientes";
            if (score == -11) return "Audio defectuoso";
            if (score == -9) return "Prueba demo";
            return "Indeterminado";
        }

        private string GetOutcomeColor(int score)
        {
            if (score >= 50) return "#00b050"; // Verde
            if (score > 0 && score < 50) return "#DD2233"; // Rojo
            if (score == -3 || score == -9) return "#FFD700"; // Amarillo
            return "#DD2233"; // Rojo por defecto
        }

        private string GetOutcomeImage(int score)
        {
            if (score >= 50)
                return @"<svg width='36' height='36' viewBox='0 0 36 36' xmlns='http://www.w3.org/2000/svg'><circle cx='18' cy='18' r='18' fill='#00b050'/><polyline points='10,19 16,25 26,13' fill='none' stroke='#fff' stroke-width='3.5' stroke-linecap='round' stroke-linejoin='round'/></svg>";
            if (score > 0 && score < 50)
                return @"<svg width='36' height='36' viewBox='0 0 36 36' xmlns='http://www.w3.org/2000/svg'><circle cx='18' cy='18' r='18' fill='#DD2233'/><line x1='12' y1='12' x2='24' y2='24' stroke='#fff' stroke-width='3.5'/><line x1='24' y1='12' x2='12' y2='24' stroke='#fff' stroke-width='3.5'/></svg>";
            return @"<svg width='36' height='36' viewBox='0 0 36 36' xmlns='http://www.w3.org/2000/svg'><circle cx='18' cy='18' r='18' fill='#FFD700'/><text x='18' y='24' text-anchor='middle' font-size='24' fill='#fff'>?</text></svg>";
        }

        private static string ToDataUri(string input)
        {
            byte[] bytes;

            // Caso base64 (ej: "iVBOR..." para PNG o "/9j..." para JPG)
            if (input.StartsWith("iVBOR", StringComparison.OrdinalIgnoreCase) ||
                input.StartsWith("/9j", StringComparison.OrdinalIgnoreCase))
            {
                bytes = Convert.FromBase64String(input);
            }
            // Caso HEX (ej: "89504E..." para PNG)
            else if (System.Text.RegularExpressions.Regex.IsMatch(input, @"\A\b[0-9a-fA-F]+\b\Z"))
            {
                bytes = Enumerable.Range(0, input.Length / 2)
                                  .Select(i => Convert.ToByte(input.Substring(i * 2, 2), 16))
                                  .ToArray();
                input = Convert.ToBase64String(bytes); // ahora sí lo transformamos a base64 real
            }
            else
            {
                throw new Exception("Formato desconocido: no es base64 ni hex.");
            }

            // Detectar MIME por cabecera
            string mime = "application/octet-stream";
            if (bytes[0] == 0x89 && bytes[1] == 0x50) mime = "image/png";
            else if (bytes[0] == 0xFF && bytes[1] == 0xD8) mime = "image/jpeg";
            else if (bytes[0] == 0x47 && bytes[1] == 0x49) mime = "image/gif";

            return $"data:{mime};base64,{input}";
        }


    }

}
