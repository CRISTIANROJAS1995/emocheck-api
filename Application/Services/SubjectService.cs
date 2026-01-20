using Domain.Extension;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Security.Cryptography;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Enums;

namespace Application.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IVerifEyeService _verifEyeService;
        private readonly ICompanyRepository _companyRepository;
        private readonly ILogger<SubjectService> _logger;

        public SubjectService(ISubjectRepository subjectRepository, IVerifEyeService verifEyeService, ICompanyRepository companyRepository, ILogger<SubjectService> logger)
        {
            _subjectRepository = subjectRepository;
            _verifEyeService = verifEyeService;
            _companyRepository = companyRepository;
            _logger = logger;
        }

        public async Task<Subject?> GetByIdentifierAsync(string identifier)
        {
            var subject = await _subjectRepository.GetSubjectByIdentifierAsync(identifier);
            //if (subject == null)
            //{
            //    throw new EntityNotFoundException("El Candidado");
            //}
            return subject;
        }

        public async Task<List<Subject>> GetAllByCompanyAsync(int companyID)
        {
            return await _subjectRepository.GetAllSubjectByCompanyAsync(companyID);
        }

        public async Task<Subject?> GetByNameAsync(string name)
        {
            var subject = await _subjectRepository.GetSubjectByNameAsync(name);
            //if (subject == null)
            //{
            //    throw new EntityNotFoundException("El Candidado");
            //}
            return subject;
        }

        public async Task<Subject?> GetByIdExternalAsync(string idExternal)
        {
            var subject = await _subjectRepository.GetSubjectByIdExternalAsync(idExternal);
            //if (subject == null)
            //{
            //    throw new EntityNotFoundException("El Candidado");
            //}
            return subject;
        }

        public async Task<List<Subject>> GetAllAsync(int companyId)
        {
            var existingCompany = await _companyRepository.GetByIdAsync(companyId);
            if (existingCompany == null)
            {
                throw new ArgumentException("El cliente enviado no existe");
            }

            return await _subjectRepository.GetAllSubjectByCompanyAsync(companyId);
        }

        public async Task<List<Subject>> GetAllSubjectAsync()
        {
            return await _subjectRepository.GetAllSubjectAsync();
        }

        public async Task<Subject?> CreateAsync(Subject subject, int userCreatedBy)
        {
            var existingCompany = await _companyRepository.GetByIdAsync(subject.CompanyID);
            if (existingCompany == null)
            {
                throw new ArgumentException("El cliente enviado no existe");
            }

            var existingSubject = await _subjectRepository.GetValidExistAsync(subject.Identifier, subject.CompanyID);
            if (existingSubject != null)
            {
                throw new EntityAlreadyExistsException("El candidato");
            }

            // Primero guardar en converus y luego en base de datos.
            string subjectId = await _verifEyeService.CreateExamineeAsync(subject.Name, subject.Email, subject.Phone, subject.Identifier);
            if (subjectId != string.Empty)
            {
                subject.IdentifierExternal = subjectId;
                await _subjectRepository.AddAsync(subject);

                return await GetByIdentifierAsync(subject.Identifier);
            }
            else
            {
                throw new ArgumentException("No se logro crear el candidato");
            }
        }

        public async Task<List<Subject>> MigrationOfConverusAsync()
        {
            var resultConverus = await _verifEyeService.ListExamineesAsync();

            foreach (var item in resultConverus)
            {
                var consultSubject = await _verifEyeService.GetExamineeByIdAsync(item.SubjectId);
                if (consultSubject != null)
                {
                    if (consultSubject.Token != string.Empty)
                    {
                        var consult = await _subjectRepository.GetSubjectByIdExternalAsync(consultSubject.SubjectId);
                        if (consult == null)
                        {
                            var (firstName, lastName) = SeparateFullName(consultSubject!.Name);

                            Guid identifier = Guid.NewGuid();
                            var subject = new Subject(
                                identifier: consultSubject.Token!,
                                companyID: 1, // Empresa inicial
                                name: firstName,
                                lastName: lastName,
                                email: consultSubject.Email,
                                phone: consultSubject.Mobile,
                                createdBy: "system",
                                modifiedBy: "system"
                            )
                            {
                                IdentifierExternal = consultSubject.SubjectId
                            };

                            await _subjectRepository.AddAsync(subject);
                        }
                    }
                }
            }

            return await GetAllSubjectAsync();
        }

        private (string firstName, string lastName) SeparateFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return (string.Empty, string.Empty);

            var words = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return words.Length switch
            {
                0 => (string.Empty, string.Empty),
                1 => (words[0], words[0]), // Ejemplo: "Cristian" -> name="Cristian", lastName="Cristian"
                2 => (words[0], words[1]), // Ejemplo: "Cristian Rojas" -> name="Cristian", lastName="Rojas"
                _ => SeparateMultipleWords(words) // 3 o más palabras
            };
        }

        private (string firstName, string lastName) SeparateMultipleWords(string[] words)
        {
            int totalWords = words.Length;
            int firstNameWordCount;

            if (totalWords % 2 == 1) // Número impar de palabras
            {
                firstNameWordCount = (totalWords + 1) / 2;
            }
            else // Número par de palabras
            {
                firstNameWordCount = totalWords / 2;
            }

            var firstName = string.Join(" ", words.Take(firstNameWordCount));
            var lastName = string.Join(" ", words.Skip(firstNameWordCount));

            return (firstName, lastName);
        }
    }
}
