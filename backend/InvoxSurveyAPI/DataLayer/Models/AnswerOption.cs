using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Models;




public class AnswerOption
{
    [Key]
    public int Id { get; set; }

    public int AnswerId { get; set; }

    public int OptionId { get; set; }

    [StringLength(255)]
    public string? Text { get; set; }

    public string? Audio { get; set; }

    [ForeignKey("AnswerId")]
    public Answer Answer { get; set; }

    [ForeignKey("OptionId")]
    public Option Option { get; set; }
}
