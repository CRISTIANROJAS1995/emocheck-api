using Application.DTOs.Company;
using Application.DTOs.State;

namespace Application.DTOs.Area
{
    public class AreaDto
    {
        public int AreaID { get; set; }
        public CompanyDto Company { get; set; }
        public StateDto State { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsVisible { get; set; }
    }
}
