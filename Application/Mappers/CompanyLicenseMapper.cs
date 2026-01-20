using Application.DTOs.CompanyLicense;
using Domain.Entities;

namespace Application.Mappers
{
    public static class CompanyLicenseMapper
    {
        public static CompanyLicenseDto ToDto(CompanyLicense entity)
        {
            if (entity == null) return null;

            return new CompanyLicenseDto
            {
                CompanyLicenseID = entity.CompanyLicenseID,
                CompanyID = entity.CompanyID,
                Company = entity.Company != null ? CompanyMapper.ToDto(entity.Company) : null,
                AllocatedLicenses = entity.AllocatedLicenses,
                UsedLicenses = entity.UsedLicenses,
                CreatedBy = entity.CreatedBy,
                CreatedAt = entity.CreatedAt,
                ModifiedBy = entity.ModifiedBy,
                ModifiedAt = entity.ModifiedAt
            };
        }

        public static LicenseStatusDto ToLicenseStatusDto(CompanyLicense entity)
        {
            if (entity == null) return null;

            return new LicenseStatusDto
            {
                CompanyId = entity.CompanyID,
                CompanyName = entity.Company?.Name ?? string.Empty,
                AllocatedLicenses = entity.AllocatedLicenses,
                UsedLicenses = entity.UsedLicenses,
                AvailableLicenses = entity.AvailableLicenses,
                LastModified = entity.ModifiedAt,
                LastModifiedBy = entity.ModifiedBy
            };
        }

        public static List<CompanyLicenseDto> ToDto(IEnumerable<CompanyLicense> entities)
        {
            return entities?.Select(ToDto).Where(dto => dto != null).ToList() ?? new List<CompanyLicenseDto>();
        }

        public static List<LicenseStatusDto> ToLicenseStatusDto(IEnumerable<CompanyLicense> entities)
        {
            return entities?.Select(ToLicenseStatusDto).Where(dto => dto != null).ToList() ?? new List<LicenseStatusDto>();
        }
    }
}