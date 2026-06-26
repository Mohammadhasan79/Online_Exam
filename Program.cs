using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineExam.DbContext;
using OnlineExam.Entity;
using OnlineExam.Repository;
using OnlineExam.RepositoryInterfaces;
using OnlineExam.Seader;
using OnlineExam.Service;
using OnlineExam.ServiceInterfaces;
using OnlineExam.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5198",
                "https://localhost:9050"
              )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//fluentvalidator
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});
//DbContext
builder.Services.AddDbContext<OnlineExamDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//IdentityContext
builder.Services.AddDataProtection();
builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<OnlineExamDbContext>().
    AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = false;
});
//AddInterface
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<IExamRepository, ExamRepository>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//*//
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
    };
});

//addswagger
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: Bearer {your token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var roleManager =  service.GetRequiredService<RoleManager<IdentityRole>>();
    await Seed.RoleSeeder(roleManager);
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("frontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
