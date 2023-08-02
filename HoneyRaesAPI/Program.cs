using HoneyRaesAPI.Models;
using System.Net;
using System.Xml.Linq;


var builder = WebApplication.CreateBuilder(args);

List<HoneyRaesAPI.Models.Customer> Customers = new List<HoneyRaesAPI.Models.Customer>
{
    new Customer()
    {
        Id = 1,
        Name = "Steven",
        Address = "185 Cherry Hill"

    },
    new Customer() 
    {
        Id = 2,
        Name = "James",
        Address = "121 Hill Rd"

    },
    new Customer()
    {
        Id = 3,
        Name = "Todd",
        Address = "985 Slong Dr."

    }
};

List<HoneyRaesAPI.Models.Employee> employees = new List<HoneyRaesAPI.Models.Employee>
{
    new Employee()
    {
        Id = 1,
        Name = " June Bug ",
        Specialty = "Networking ",

    },
    new Employee() 
    {
         Id = 2,
        Name = " Damage Control ",
        Specialty = "Risk Assesment ",
    },
    new Employee() 
    {
         Id = 3,
        Name = " Code Ing ",
        Specialty = "Backend ",
    },
    new Employee()
    {
         Id = 4,
        Name = " Justin Case ",
        Specialty = "Going Witrh The Flow",
    }
};

List<HoneyRaesAPI.Models.ServiceTicket> ServiceTicket = new List<HoneyRaesAPI.Models.ServiceTicket>
{
    new ServiceTicket
    {
        Id = 1,
        CustomerId = 1,
        EmployeeId = 1,
        Description = " Helpp!!",
        Emergency = true,
        DateCompleted = new DateTime(2023,8,1),
    },
    new ServiceTicket()
    {
         Id = 1,
        CustomerId = 1,
        EmployeeId = 4,
        Description = " I need to speak with some one",
        Emergency = false,
        DateCompleted = new DateTime(2023,9,11),
    },
    new ServiceTicket()
    {
        Id = 2,
        CustomerId = 2,
        EmployeeId = 4,
        Description = " I want to pay my bill",
        Emergency = true,
        DateCompleted = new DateTime(2022, 5, 12),
    },
    new ServiceTicket()
    {
        Id = 3,
        CustomerId = 4,
        EmployeeId = 2,
        Description = "My computer went down I need it for work",
        Emergency = true,
        DateCompleted = new DateTime(2023, 5, 3),
     },
    new ServiceTicket()
    {
        Id = 5,
        CustomerId = 5,
        EmployeeId = 1,
        Description = "Where is the best Tech store in the area",
        Emergency = true,
        DateCompleted = new DateTime(2023,5,2),
    }
};



// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

/*app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();*/

app.MapGet("/servicetickets", () =>
{
    return ServiceTicket;
});

app.MapGet("/servicetickets/{id}", (int id) =>
{
    ServiceTicket serviceTicket = ServiceTicket.FirstOrDefault(st => st.Id == id);
    if (serviceTicket == null)
    {
        return Results.NotFound();
    }
    serviceTicket.Employee = employees.FirstOrDefault(e => e.Id == serviceTicket.EmployeeId);
    return Results.Ok(serviceTicket);
});



app.MapGet("/employee/{id}", (int id) =>
{
    Employee employee = employees.FirstOrDefault(e => e.Id == id);
    if (employee == null)
    {
        return Results.NotFound();
    }
    employee.ServiceTickets = ServiceTicket.Where(st => st.EmployeeId == id).ToList();
    return Results.Ok(employee);
});


app.Run();

/*record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}*/

