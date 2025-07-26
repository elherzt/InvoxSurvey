using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Models;

public class QuestionsType
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string Description { get; set; }

    public ICollection<Question> Questions { get; set; } = new List<Question>();
}
