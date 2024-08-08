﻿using System;
using System.Collections.Generic;

namespace PROJ_API_N_NDORO.Models;

public partial class Client
{
    public Guid ClientId { get; set; }

    public string? ClientName { get; set; }

    public string? PrimaryContactEmail { get; set; }

    public DateTime? DateOnboarded { get; set; }
}
