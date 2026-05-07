# DemoProject API

ASP.NET Core 8 Web API 展示專案，包含 JWT 驗證、商品管理、預算審核流程與 Excel 匯出功能。

---

## 技術架構

| 項目 | 使用技術 |
|------|----------|
| 框架 | ASP.NET Core 8 Web API |
| 資料庫 | Microsoft SQL Server + Entity Framework Core 8 |
| 驗證 | JWT Bearer Token |
| Excel 匯出 | EPPlus 7 |
| API 文件 | Swagger / OpenAPI |

---

## 專案結構

```
DemoProject/
├── Controllers/
│   ├── AuthController.cs        # JWT 登入
│   ├── ProductController.cs     # 商品 CRUD
│   ├── BudgetController.cs      # 預算管理 + 審核狀態機
│   └── ExcelController.cs       # Excel 匯出
├── Data/
│   └── AppDbContext.cs          # EF Core DbContext
├── Database/
│   └── init.sql                 # 建表 SQL + 假資料
├── doc/
│   ├── architecture.md          # 系統架構圖
│   ├── jwt-flow.md              # JWT 驗證流程圖
│   └── budget-flow.md           # 預算審核流程圖
├── Models/
│   ├── ApiResponse.cs           # 統一回應格式
│   ├── Entities/                # 資料庫實體
│   └── Dto/                     # 資料傳輸物件
└── Services/
    └── ExcelExportService.cs    # EPPlus Excel 產生邏輯
```

---

## API 一覽

### Auth
| Method | 路徑 | 說明 | 需登入 |
|--------|------|------|--------|
| POST | `/api/auth/login` | 登入取得 JWT Token | ❌ |

### 商品管理
| Method | 路徑 | 說明 | 需登入 |
|--------|------|------|--------|
| GET | `/api/product` | 取得所有商品 | ✅ |
| GET | `/api/product/{id}` | 依 ID 取得商品 | ✅ |
| POST | `/api/product` | 新增商品 | ✅ |
| PUT | `/api/product/{id}` | 更新商品 | ✅ |
| DELETE | `/api/product/{id}` | 刪除商品（僅 Admin）| ✅ |

### 預算管理
| Method | 路徑 | 說明 | 需登入 |
|--------|------|------|--------|
| GET | `/api/budget` | 取得所有預算 | ✅ |
| GET | `/api/budget/brand/{brand}` | 依品牌查詢 | ✅ |
| POST | `/api/budget` | 新增預算 | ✅ |
| PUT | `/api/budget/{id}` | 更新預算金額 | ✅ |
| PATCH | `/api/budget/{id}/status` | 更新審核狀態 | ✅ |

### Excel 匯出
| Method | 路徑 | 說明 | 需登入 |
|--------|------|------|--------|
| GET | `/api/excel/products` | 下載商品清單 .xlsx | ✅ |
| GET | `/api/excel/budgets` | 下載預算報表 .xlsx（含狀態顏色）| ✅ |

---

## 預算審核流程

```
Draft（草稿）→ Reviewing（審核中）→ Approved（已核准）
```
- 狀態為 `Approved` 時，金額不可再修改
- 詳細流程圖請參考 [doc/budget-flow.md](doc/budget-flow.md)

---

## 快速啟動

### 環境需求
- .NET 8 SDK
- SQL Server（本機或遠端）

### 步驟

**1. 建立資料庫**
```sql
CREATE DATABASE DemoProject;
```

**2. 執行建表腳本**
```
DemoProject/Database/init.sql
```

**3. 設定連線字串**

修改 `appsettings.json`：
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=DemoProject;Integrated Security=True;TrustServerCertificate=True"
}
```

**4. 啟動專案**
```bash
dotnet run
```

**5. 開啟 Swagger**
```
https://localhost:{port}/swagger
```

---

## 測試帳號

專案內建兩組測試帳號，角色分別為 `Admin` 與 `User`。

> 帳號資訊請洽作者，或於本機執行後參考 `Database/init.sql` 內的說明。

### 登入流程
1. 呼叫 `POST /api/auth/login` 取得 Token
2. 點 Swagger 右上角 **Authorize**
3. 輸入 `Bearer {token}`
4. 即可呼叫所有需要登入的 API

詳細 JWT 驗證流程請參考 [doc/jwt-flow.md](doc/jwt-flow.md)
