using System.ComponentModel.DataAnnotations;

namespace CompanyApi.DTO;
public class UpdateDeviceRequestDto
{
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string DeviceTypeName { get; set; } = string.Empty;

    public bool IsDeviceEnabled { get; set; }

    public string? AdditionalPropertiesJson { get; set; }
}