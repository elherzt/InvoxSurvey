using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Models;

public class Question
{
    [Key]
    public int Id { get; set; }


    public int? SectionId { get; set; }

   
    [StringLength(1500)]
    public string Description { get; set; }

   
    public int? TypeId { get; set; }

   
    public bool? HasOther { get; set; }

    
    public ICollection<Option> Options { get; set; } = new List<Option>();

    [ForeignKey("SectionId")]
    
    public Section Section { get; set; }

    [ForeignKey("TypeId")]
   
    public QuestionsType Type { get; set; }
}
