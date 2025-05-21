using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CompanyApi.Data;

[Table("Position")]
[Index("Name", Name = "UQ__Position__737584F680BDB491", IsUnique = true)]
public partial class Position
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    public int MinExpYears { get; set; }

    [InverseProperty("Position")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
