using Microsoft.Extensions.Options;
using OptionsPattern.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.Configure<PersonOptions>(builder.Configuration.GetSection(PersonOptions.Person));
// builder.Services.AddOptions<PersonOptions>()
//     .Bind(builder.Configuration.GetSection(PersonOptions.Person))
//     .ValidateDataAnnotations()
//     .Validate((personOptions) =>
//     {
//         // do something
//         string name = personOptions.Name.ToLower();
//         if (name.Equals("furkan"))
//             return personOptions.SurName.ToLower() == "aydın" ? true : false;
//         return true;
//     }, "Should be equal surname property to 'aydın' when name propery was 'furkan'");

builder.Services.Configure<PersonOptions>(builder.Configuration.GetSection(PersonOptions.Person));
builder.Services.AddSingleton<IValidateOptions<PersonOptions>, PersonOptionsValidation>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
