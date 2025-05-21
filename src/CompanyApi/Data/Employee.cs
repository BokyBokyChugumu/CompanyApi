using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CompanyApi.Data;

[Table("Employee")]
public partial class Employee
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Salary { get; set; }

    public int PositionId { get; set; }

    public int PersonId { get; set; }

    public DateTime HireDate { get; set; }

    [InverseProperty("Employee")]
    public virtual ICollection<DeviceEmployee> DeviceEmployees { get; set; } = new List<DeviceEmployee>();

    [ForeignKey("PersonId")]
    [InverseProperty("Employees")]
    public virtual Person Person { get; set; } = null!;

    [ForeignKey("PositionId")]
    [InverseProperty("Employees")]
    public virtual Position Position { get; set; } = null!;
}
