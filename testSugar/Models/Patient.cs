using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace testSuger.Models;

public partial class Patient
{
    public decimal Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime Dob { get; set; }

    public decimal? Age { get; set; }

    public bool? Gender { get; set; }

    public double? Wt { get; set; }

    public string? Phone { get; set; }
    [JsonIgnore]
    public virtual ICollection<Dose> Doses { get; set; } = new List<Dose>();
    [JsonIgnore]
    public virtual User IdNavigation { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<PatientDoctor> PatientDoctors { get; set; } = new List<PatientDoctor>();
    [JsonIgnore]
    public virtual ICollection<Read> Reads { get; set; } = new List<Read>();
    [JsonIgnore]
    public virtual Time? Time { get; set; }
    [JsonIgnore]
    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
