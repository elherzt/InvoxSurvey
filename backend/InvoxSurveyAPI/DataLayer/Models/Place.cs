using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Models;

public class Place
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Description { get; set; }

    
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
}
