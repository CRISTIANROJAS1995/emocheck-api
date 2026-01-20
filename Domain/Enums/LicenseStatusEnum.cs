namespace Domain.Enums
{
    public enum LicenseStatusEnum
    {
        Assigned = 1,    // Licencia asignada pero no usada
        Used = 2,        // Licencia consumida (cuando se crea ExamResult)
        Expired = 3      // Licencia expirada (futuro)
    }
}
