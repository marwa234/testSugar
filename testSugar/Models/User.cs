using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace testSuger.Models;

public partial class User
{
    public decimal Id { get; set; }

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;
    [JsonIgnore]
    public virtual Doctor? Doctor { get; set; }
    [JsonIgnore]
    public virtual Patient? Patient { get; set; }
}
