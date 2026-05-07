-- =============================================================
-- DemoProject 建表 SQL (SQL Server)
-- 說明：若要改用真實 SQL Server，請將 Program.cs 的 UseInMemoryDatabase
--       替換成 UseSqlServer(connectionString) 後執行此腳本
-- =============================================================

-- 建立 Users 資料表
CREATE TABLE [dbo].[Users] (
    [Id]           INT           IDENTITY(1,1) PRIMARY KEY,
    [Username]     NVARCHAR(50)  NOT NULL UNIQUE,
    [PasswordHash] NVARCHAR(255) NOT NULL,         -- SHA256 + Base64
    [Role]         NVARCHAR(20)  NOT NULL DEFAULT 'User',
    [CreatedAt]    DATETIME2     NOT NULL DEFAULT GETUTCDATE()
);

-- 建立 Products 資料表
CREATE TABLE [dbo].[Products] (
    [Id]        INT            IDENTITY(1,1) PRIMARY KEY,
    [Name]      NVARCHAR(100)  NOT NULL,
    [Brand]     NVARCHAR(50)   NOT NULL,
    [Category]  NVARCHAR(50)   NOT NULL,
    [UnitPrice] DECIMAL(10, 2) NOT NULL DEFAULT 0,
    [Stock]     INT            NOT NULL DEFAULT 0,
    [CreatedAt] DATETIME2      NOT NULL DEFAULT GETUTCDATE()
);

-- 建立 Budgets 資料表
CREATE TABLE [dbo].[Budgets] (
    [Id]           INT            IDENTITY(1,1) PRIMARY KEY,
    [Brand]        NVARCHAR(50)   NOT NULL,
    [Channel]      NVARCHAR(50)   NOT NULL,
    [Year]         INT            NOT NULL,
    [Month]        INT            NOT NULL,
    [BudgetAmount] DECIMAL(15, 2) NOT NULL DEFAULT 0,
    [ActualAmount] DECIMAL(15, 2) NOT NULL DEFAULT 0,
    [Status]       NVARCHAR(20)   NOT NULL DEFAULT 'Draft',  -- Draft / Reviewing / Approved
    [UpdatedAt]    DATETIME2      NOT NULL DEFAULT GETUTCDATE()
);

GO

-- =============================================================
-- 假資料
-- =============================================================

-- 密碼說明：PasswordHash = SHA256(明碼) 轉 Base64
--   admin / Admin@123 => 6G94qKPK8LYNjnTllCqm2G3BUM08AzOK7yW30tfjrMc=
--   user1 / User@123  => PnwZV2SIhigW8TtRLKzz5LqX3ZckPqC9airRZC2GunI=
INSERT INTO [dbo].[Users] ([Username], [PasswordHash], [Role], [CreatedAt]) VALUES
('admin', '6G94qKPK8LYNjnTllCqm2G3BUM08AzOK7yW30tfjrMc=', 'Admin', '2026-01-01'),
('user1', 'PnwZV2SIhigW8TtRLKzz5LqX3ZckPqC9airRZC2GunI=', 'User',  '2026-01-01');

INSERT INTO [dbo].[Products] ([Name], [Brand], [Category], [UnitPrice], [Stock], [CreatedAt]) VALUES
(N'經典皮革手提包', N'LuxBag',      N'包包', 8500, 50,  '2026-01-01'),
(N'休閒運動鞋',     N'SportX',     N'鞋類', 2800, 120, '2026-01-01'),
(N'限量聯名帽T',    N'StreetCo',   N'上衣', 1980, 80,  '2026-01-01'),
(N'修身牛仔褲',     N'DenimLab',   N'下著', 2200, 60,  '2026-01-01'),
(N'防水登山背包',   N'OutdoorPro', N'包包', 3500, 35,  '2026-01-01');

INSERT INTO [dbo].[Budgets] ([Brand], [Channel], [Year], [Month], [BudgetAmount], [ActualAmount], [Status], [UpdatedAt]) VALUES
(N'LuxBag',  N'百貨', 2026, 1, 500000, 480000, 'Approved',  '2026-02-01'),
(N'LuxBag',  N'電商', 2026, 1, 300000, 310000, 'Approved',  '2026-02-01'),
(N'SportX',  N'百貨', 2026, 1, 400000, 390000, 'Approved',  '2026-02-01'),
(N'LuxBag',  N'百貨', 2026, 2, 550000, 520000, 'Reviewing', '2026-03-01'),
(N'SportX',  N'電商', 2026, 2, 350000, 0,      'Draft',     '2026-03-01');

GO
