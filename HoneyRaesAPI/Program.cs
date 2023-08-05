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

app.MapPost("/servicetickets", (ServiceTicket serviceTicket) =>
{
    // creates a new id (When we get to it later, our SQL database will do this for us like JSON Server did!)
    serviceTicket.Id = ServiceTicket.Max(st => st.Id) + 1;
    ServiceTicket.Add(serviceTicket);
    return serviceTicket;
});

app.MapDelete("/servicetickets/{id}", (int id) =>
{
    ServiceTicket.Remove(ServiceTicket.FirstOrDefault(ticket => ticket.Id == id));
});

app.MapPut("/servicetickets/{id}", (int id, ServiceTicket serviceTicket) =>
{
    ServiceTicket ticketToUpdate = ServiceTicket.FirstOrDefault(st => st.Id == id);
    int ticketIndex = ServiceTicket.IndexOf(ticketToUpdate);
    if (ticketToUpdate == null)
    {
        return Results.NotFound();
    }
    //the id in the request route doesn't match the id from the ticket in the request body. That's a bad request!
    if (id != serviceTicket.Id)
    {
        return Results.BadRequest();
    }
    ServiceTicket[ticketIndex] = serviceTicket;
    return Results.Ok();
});

app.MapPost("/servicetickets/{id}/complete", (int id) =>
{
    ServiceTicket ticketToComplete = ServiceTicket.FirstOrDefault(st => st.Id == id);
    ticketToComplete.DateCompleted = DateTime.Today;
});

app.MapPost("/servicetickets/{id}/complete", (int id) =>
{
    ServiceTicket ticketToComplete = ServiceTicket.FirstOrDefault(st => st.Id == id);
    ticketToComplete.DateCompleted = DateTime.Today;
});

/*app.MapGet("/servicetickets/emergency", () =>
{
    List<ServiceTicket> emergencyTicket = ServiceTicket.Where(st => st.Emergency == true && st.DateCompleted == null).ToList();
    return Results.Ok(emergencyTicket);
});

app.MapGet("/servicetickets/unassigned", () =>
{
    List<ServiceTicket> unassignedTickets = ServiceTicket.Where(st => st.EmployeeId == null).ToList();
    return Results.Ok(unassignedTickets);
});

app.MapGet("servicetickets/inactive", () =>
{
    DateTime oneYearAgo = DateTime.Today.AddYears(-1);
    List<int> activeCustomerIds = ServiceTicket.Where(ticket => ticket.DateCompleted >= oneYearAgo).Select(ticket => ticket.CustomerId).ToList();
    List<Customer> inactiveCustomers = Customers.Where(customer => !activeCustomerIds.Contains(customer.Id)).ToList();
    return Results.Ok(inactiveCustomers);
});

app.MapGet("/servicetickets/employee/unassigned", () =>
{
    List<Employee> unassignedEmployees = employees.Where(emp => ServiceTicket.All(st => st.EmployeeId != emp.Id)).ToList();
    return Results.Ok(unassignedEmployees);
});

app.MapGet("/servicetickets/employee/{id}", (int id) => {
    var employee = employees.FirstOrDefault(e => e.Id == id);
    if (employee == null)
    {
        return Results.NotFound();
    }

    var employeeCustomers = Customers.Where(c => ServiceTicket.Any(st => st.CustomerId == c.Id && st.EmployeeId == id));
    return Results.Ok(employeeCustomers);
});

app.MapGet("/employeeofthemonth", () =>
{
    var lastMonth = DateTime.Now.AddMonths(-1);
    var employeeOfTheMonth = employees.OrderByDescending(e => ServiceTicket.Count(st => st.EmployeeId == e.Id && st.DateCompleted >= lastMonth)).FirstOrDefault();
    return Results.Ok(employeeOfTheMonth);
});

app.MapGet("/completedtickets", () =>
{
    var completedTickets = ServiceTicket.Where(st => st.DateCompleted != null).OrderBy(st => st.DateCompleted);
    return Results.Ok(completedTickets);
});

app.MapGet("/prioritizedtickets", () =>
{
    var prioritizedTickets = ServiceTicket
        .Where(st => st.DateCompleted == null)
        .OrderByDescending(st => st.Emergency)
        .ThenBy(st => st.EmployeeId == 0);
    return Results.Ok(prioritizedTickets);
});*/

app.Run();

/*record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}*/

