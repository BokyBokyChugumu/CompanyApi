namespace CompanyApi.DTO;
public class DeviceDetailsDto
{
    public string Name { get; set; } = string.Empty;
    public string? DeviceTypeName { get; set; }
    public bool IsDeviceEnabled { get; set; }
    public object? AdditionalProperties { get; set; } 
    public CurrentEmployeeDto? CurrentEmployee { get; set; } 
}