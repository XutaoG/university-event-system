using System.Text;
using CollegeEvent.API.CustomAuthorizations;
using CollegeEvent.API.Mappers;
using CollegeEvent.API.Repositories;
using CollegeEvent.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(typeof(PasswordHashService));

// Add repositories to the container
builder.Services.AddScoped<IUserRepository, SQLUserRepository>();
builder.Services.AddScoped<IUniversityRepository, SQLUniversityRepository>();
builder.Services.AddScoped<IRSORepository, SQLRSORepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddSingleton<JwtTokenService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.SaveToken = true;
		options.RequireHttpsMetadata = false;

		// Set JWT validation
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("Jwt:secret_key")!)),
			ClockSkew = TimeSpan.Zero,
			ValidateAudience = false,
			ValidateIssuer = false,
		};

		options.Events = new JwtBearerEvents
		{
			// Extract token from cookie header
			OnMessageReceived = context =>
			{
				if (context.Request.Cookies.ContainsKey("X-Access-Token"))
				{
					context.Token = context.Request.Cookies["X-Access-Token"];
				}

				return Task.CompletedTask;
			},
		};
	});

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("SuperAdminPolicy", policy => policy.Requirements.Add(new UserRoleAuthorizationRequirement("SuperAdmin")));
	options.AddPolicy("AdminPolicy", policy => policy.Requirements.Add(new UserRoleAuthorizationRequirement("Admin")));
	options.AddPolicy("StudentPolicy", policy => policy.Requirements.Add(new UserRoleAuthorizationRequirement("Student")));
	options.AddPolicy("UniversityFound", policy => policy.Requirements.Add(new UniversityFoundAuthorizationRequirement()));
});

builder.Services.AddSingleton<IAuthorizationHandler, UserRoleAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, UniversityFoundAuthorizationHandler>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


