# USER SERVICE

Language : C#

Framework : .NET 6

Architecture : Onion Architecture

ORM: MSSQL

IDE: Visual Studio 2022

## Biến môi trường
- API_KEY : **3EB76D87D97C427943957C555AB0B60847582D38CB1688ED86C59251206305E3**
- DefaultConnection: **Server=ip;Database=BookingApp;User Id=username;Password=password**
- RedisConnection: **ip:port**
- MongoConnection: **mongodb://username:password@ip:port**

## Clean Architecture
![This is a alt text.](http://thecodewrapper.com/wp-content/uploads/2021/09/clean-architecture-1024x1017.png "This is a sample image.")

## Domain layer

> Chưa các entities , enum , DTO 

## Application layer

> chứa các interface và business logic 

## Infastructure layer
> implement interface từ application layer và connect với db



## Presentation
> GUI hoặc API

## Đặt tên table

Tên bảng : Viết Hoa, Tiền tố_*
vd STORE , SUPPLIER , SUPPLIER_DETAIL

Tên cột : viết thường , 2 từ trở lên thì có đâu _
vd : full_name , address , city , created_at , created_by ,last_online_at

## Extension
> Microsoft.AspNetCore.Authentication.JwtBearer 6.0.7
 
> Microsoft.EntityFrameworkCore 6.0.7

> Microsoft.EntityFrameworkCore.Design 6.0.7

> Microsoft.EntityFrameworkCore.SqlServer 6.0.7

> Microsoft.EntityFrameworkCore.Tools 6.0.7

> Microsoft.Extensions.Caching.StackExchangeRedis 6.0.7

## Tài liệu tham khảo
[https://www.buymeacoffee.com/thecodewrapper/implementing-clean-architecture-asp-net-core-6?PageSpeed=noscript](https://www.buymeacoffee.com/thecodewrapper/implementing-clean-architecture-asp-net-core-6?PageSpeed=noscript)

[https://www.ssw.com.au/rules/rules-to-better-clean-architecture](https://www.ssw.com.au/rules/rules-to-better-clean-architecture)

[https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)

[https://medium.com/dotnet-hub/clean-architecture-with-dotnet-and-dotnet-core-aspnetcore-overview-introduction-getting-started-ec922e53bb97]https://medium.com/dotnet-hub/clean-architecture-with-dotnet-and-dotnet-core-aspnetcore-overview-introduction-getting-started-ec922e53bb97