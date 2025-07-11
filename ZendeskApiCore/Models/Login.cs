// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZendeskApiCore.Models;

[Keyless]
public partial class Login
{
    [Column("id")]
    public Guid? Id { get; set; }

    [Column("password")]
    [StringLength(60)]
    public string Password { get; set; }

    [Column("user")]
    [StringLength(120)]
    public string User { get; set; }

    [Column("name")]
    [StringLength(200)]
    public string Name { get; set; }

    [Column("lastname")]
    [StringLength(200)]
    public string LastName { get; set; }

    [Column("mail")]
    [StringLength(250)]
    public string Mail { get; set; }

    [Column("rol_id")]
    public Guid? RolId { get; set; }

    [Column("rol")]
    [StringLength(150)]
    public string Rol { get; set; }
}