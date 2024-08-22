# Hướng dẫn cài đặt

## Bước 1

- Tải dự án về và mở dự án

## Bước 2

- Xoá thư mục migration trong dự án

## Bước 3

- Sửa file "appseting.json" phần "ConnectionString" để kết nối đến Database

```bash
Server=serverName;Database=mobieappapi;User Id=SA;Password=Password123; TrustServerCertificate=true; Encrypt=true
```

## Bước 4

Tạo `Migration`

- Trong Visual studio mở `Nuget package manager console` và chạy câu lệnh:

```bash
    Add Migrations InitializedDatabase
```

Tiếp theo thực hiện câu lệnh:

```bash
    Update Database
```

Để update database

- Trong Visual Studio Code (Dotnet CLI) mở `Terminal` và chạy câu lệnh:

```bash
    dotnet tool install --global dotnet-ef
```

Để tiến hành cài tool `dotnet ef`. Sau đó thực hiện câu lệnh:

```bash
    dotnet ef migrations add initdb
```

Để tiến hành tạo Migration, sau đó thực hiện câu lệnh:

```bash
    dotnet ef database update
```

Để tiến hành cập nhật database

## Bước 5

- Tiến hành chạy dự án

## Truy cập API theo đường dẫn

```bash
    /api/cocktails
```

Có thể sửa đường dẫn của api trong controller CockTails
