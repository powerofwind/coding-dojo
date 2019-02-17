# Coding Dojo, สนามฝึกวรยุทธ 

## การทำงานกับ repo นี้
1. ในครั้งแรกให้ทำการ fork ตัว repo นี้ไปเป็นของตัวเอง
1. เมื่อมี feature เข้ามาใหม่ ให้ทำการ pull request กลับไปที่ repo ที่ทำการ fork ออกมาแล้วกด merge ซะ
1. เมื่อจะเริ่มทำ feature ไหนก็ตาม ให้แตก branch ใหม่เป็นชื่อ feature นั้นๆก่อนเสมอ แล้วทำงานใน branch นั้น
1. เมื่อต้องการส่งงานให้ทำ pull request จาก branch ที่ทำงานเข้ามา

## Features
1. Login
1. Register

## Create WebApi + XUnit projects
**Command line**
```
dotnet new webapi -n WebApi
dotnet new xunit -n WebApi.Tests
dotnet add WebApi.Tests/WebApi.Tests.csproj reference WebApi.Tests/WebApi.Tests.csproj
```

## Swagger
**Command line**
```
dotnet add WebApi package Swashbuckle.AspNetCore
```
**Code:** `Startup.cs`
```
using Swashbuckle.AspNetCore.Swagger;

public void ConfigureServices(IServiceCollection services)
{
    services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" }));
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
}
```

## CORS - Cross-Origin Resource Sharing
**Code:** `Startup.cs`
```
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
}
```