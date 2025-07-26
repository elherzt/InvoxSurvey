using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Models;

public  class Survey
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }


    public DateTime CreatedDate { get; set; }

    [Required]
    [StringLength(255)]
    public string Place { get; set; }

    public int UserId { get; set; }

    public bool Active { get; set; }

    public int Target { get; set; }

    [Required]
    public string Instructions { get; set; }

    public int StatusId { get; set; }

    public ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public ICollection<Section> Sections { get; set; } = new List<Section>();

    [ForeignKey("UserId")]
    public User User { get; set; }

    [ForeignKey("StatusId")]
    public SurveysStatus Status { get; set; }
}
