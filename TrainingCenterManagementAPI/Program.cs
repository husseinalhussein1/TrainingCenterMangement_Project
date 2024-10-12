
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using TrainingCenterManagement.Infrastructure;
using TrainingCenterManagementAPI.Interfaces;
using TrainingCenterManagementAPI.Services.Repositories;
using Microsoft.Extensions.FileProviders;//for static file
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);


// log files
Log.Logger = new LoggerConfiguration()
                  .MinimumLevel.Debug()
                  .WriteTo.Console()
                  .WriteTo.File("aaplog.txt", rollingInterval: RollingInterval.Day)
                  .CreateLogger();



// Add services to the container.

builder.Services.AddControllers(option =>
{
    option.ReturnHttpNotAcceptable = true;
})
    .AddXmlDataContractSerializerFormatters()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;//مشان ما يصير سلاسل غير منتهية
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();




builder.Services.AddSwaggerGen(options =>
{
/*    // جلب مسار الملف الافضل هكذا لتجنب الخطأ بجلبه بتغيير الاماكن
    var pathFileXml = Path.Combine(AppContext.BaseDirectory, "TrainingCenterManagementAPI.xml");
    // تضمين هذا الملف لل swagger
    options.IncludeXmlComments(pathFileXml);*/
    //name selection
    options.AddSecurityDefinition("TrainingCenterManagementAPIAuthentcate", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
    {
        // محدد الوصول في نوع الحماية الذي تستخدمه
        Scheme = "Bearer",
        // نوع الاتصال الذي نعمل عليه والشامل لل https هو http
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        // وصف سيظهر فوق الحقل
        Description = "Please enter valid token"
    });

});


builder.Services.AddDbContext<TrainingCenterManagementDbContext>(
    options => options.UseSqlServer(builder.Configuration["ConnectionStrings:TrainingCenterManagementDBConnectionString"]),
                                    ServiceLifetime.Scoped);
//builder.Services.AddDbContext<TrainingCenterManagementDbContext>();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



// record Repository Services
builder.Services.AddScoped<IAdministratorRepository, AdministratorRepository>(); // light weight
builder.Services.AddScoped<ICourseRepository, CourseRepository>(); // light weight
builder.Services.AddScoped<IExamRepository, ExamRepository>(); // light weight
builder.Services.AddScoped<ILectureRepository, LectureRepository>(); // light weight
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>(); // light weight
builder.Services.AddScoped<IPresenceRepository, PresenceRepository>(); // light weight
builder.Services.AddScoped<IReceptionistRepository, ReceptionistRepository>(); // light weight
builder.Services.AddScoped<ITraineeRepository, TraineeRepository>(); // light weight
builder.Services.AddScoped<ITrainerRepository, TrainerRepository>(); // light weight
builder.Services.AddScoped<ITrainingOfficerRepository, TrainingOfficerRepository>(); // light weight
builder.Services.AddScoped<ICertificateRepository, CertificateRepository>();




builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Host.UseSerilog();   // Register serilog 



//Police later
//builder.Services.AddAuthorization(
//    options =>
//    {
//        options.AddPolicy("userShouldBeAdminAndCourse11", policy =>
//        {
//            policy.RequireRole("user");
//            policy.RequireClaim("course", "midad_11");
//            policy.RequireAuthenticatedUser();
//        });
//    });


// Ahmad

//token
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                                                .GetBytes(builder.Configuration["Authentication:SecretKey"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true
    };
});





// cookie Don`t setting now be can set later

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
//{
//    options.Cookie.Name = "TrainingCenterManagementCookies";
//    options.Cookie.MaxAge = TimeSpan.FromDays(2);
//});






var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//for static file
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "StaticFiles")),
    RequestPath = "/StaticFiles"
});


// token
app.UseAuthentication();


app.UseAuthorization();


app.MapControllers();

// for Testing Database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TrainingCenterManagementDbContext>();
    TrainingCenterManagementDbContext.CreatInitalTestingDatabase(context);
}

app.Run();
