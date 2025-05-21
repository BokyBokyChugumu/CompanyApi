using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CompanyApi.Data;

[Table("DeviceEmployee")]
public partial class DeviceEmployee
{
    [Key]
    public int Id { get; set; }

    public int DeviceId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    [ForeignKey("DeviceId")]
    [InverseProperty("DeviceEmployees")]
    public virtual Device Device { get; set; } = null!;

    [ForeignKey("EmployeeId")]
    [InverseProperty("DeviceEmployees")]
    public virtual Employee Employee { get; set; } = null!;
}
