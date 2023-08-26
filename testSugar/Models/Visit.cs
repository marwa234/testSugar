using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace testSuger.Models;

public partial class Visit
{
    public decimal PatientId { get; set; }

    public int VisitNo { get; set; }

    public double Wt { get; set; }

    public string History { get; set; } = null!;

    public string Examination { get; set; } = null!;

    public string Notes { get; set; } = null!;

    public double LongVal { get; set; }

    public double CorrectVal { get; set; }

    public string LongMed { get; set; } = null!;

    public string ShortMed { get; set; } = null!;

    public string CorrectMed { get; set; } = null!;
    [JsonIgnore]
    public virtual Patient Patient { get; set; } = null!;
}
