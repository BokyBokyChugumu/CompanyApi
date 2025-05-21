using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CompanyApi.Data;

[Table("DeviceType")]
[Index("Name", Name = "UQ__DeviceTy__737584F661B69C2C", IsUnique = true)]
public partial class DeviceType
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("DeviceType")]
    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();
}
