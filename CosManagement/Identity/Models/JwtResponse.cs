﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosManagement.Identity.Models;

public class JwtResponse
{
	public string? Token { get; set; }
	public DateTime Expiration { get; set; }
}