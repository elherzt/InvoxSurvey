using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Models;

/// Represents a survey answer stored in the database.
/// Each instance corresponds to a submitted survey response by a user.

public class Answer
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int? UserId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string UserName { get; set; }

    [StringLength(255)]
    public string Place { get; set; }

    public int SurveyId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? PlaceId { get; set; }

    public int? Time { get; set; }

  
    public  ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();

    [ForeignKey("PlaceId")]
    public  Place PlaceNavigation { get; set; }

    [ForeignKey("SurveyId")]
    public  Survey Survey { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
}
