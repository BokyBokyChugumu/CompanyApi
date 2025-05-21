using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CompanyApi.Data;

[Table("Person")]
[Index("PassportNumber", Name = "UQ__Person__45809E71070E6865", IsUnique = true)]
[Index("PhoneNumber", Name = "UQ__Person__85FB4E38A536C6B1", IsUnique = true)]
[Index("Email", Name = "UQ__Person__A9D105349DC043C4", IsUnique = true)]
public partial class Person
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string PassportNumber { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? MiddleName { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string PhoneNumber { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [InverseProperty("Person")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
