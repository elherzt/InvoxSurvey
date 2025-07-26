using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Models;


public class Option
{
    [Key]
    public int Id { get; set; }

    public int QuestionId { get; set; }

    [Required]
    [StringLength(255)]
    public string Description { get; set; }

    public bool IsOpen { get; set; }

    public bool SkipSection { get; set; }

    public int? NextQuestionId { get; set; }

    public ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();

    [ForeignKey("QuestionId")]
    public Question Question { get; set; }
}
