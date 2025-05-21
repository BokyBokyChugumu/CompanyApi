namespace CompanyApi.DTO;
public class EmployeeDetailsDto
{
    public string PassportNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    
    public decimal Salary { get; set; }
    public PositionDto? Position { get; set; }
    public DateTime HireDate { get; set; }
}