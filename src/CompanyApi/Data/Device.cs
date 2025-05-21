using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CompanyApi.Data;

[Table("Device")]
public partial class Device
{
    [Key]
    public int Id { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    public bool IsEnabled { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string AdditionalProperties { get; set; } = null!;

    public int? DeviceTypeId { get; set; }

    [InverseProperty("Device")]
    public virtual ICollection<DeviceEmployee> DeviceEmployees { get; set; } = new List<DeviceEmployee>();

    [ForeignKey("DeviceTypeId")]
    [InverseProperty("Devices")]
    public virtual DeviceType? DeviceType { get; set; }
}
