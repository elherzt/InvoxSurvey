using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Models;

public  class SurveysStatus
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string Name { get; set; }

    public ICollection<Survey> Surveys { get; set; } = new List<Survey>();
}
