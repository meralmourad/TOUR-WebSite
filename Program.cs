builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            RoleClaimType = ClaimTypes.Role // This line is important for role-based auth
        };

        // Custom token validation using JwtTokenService
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var jwtTokenService = context.HttpContext.RequestServices.GetRequiredService<JwtTokenService>();
                var token = context.SecurityToken as JwtSecurityToken;

                if (token == null || jwtTokenService.ValidateToken(token.RawData) == null)
                {
                    context.Fail("Token validation failed.");
                }

                await Task.CompletedTask;
            },
            OnMessageReceived = context =>
            {
                // Optionally handle token extraction from custom locations (e.g., query string)
                if (string.IsNullOrEmpty(context.Token) && context.HttpContext.Request.Query.ContainsKey("token"))
                {
                    context.Token = context.HttpContext.Request.Query["token"];
                }
                return Task.CompletedTask;
            }
        };
    });
