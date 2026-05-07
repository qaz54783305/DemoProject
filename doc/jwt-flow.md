# JWT 驗證流程

## 登入取得 Token

```mermaid
sequenceDiagram
    participant Client as 前端
    participant Auth as AuthController
    participant DB as SQL Server

    Client->>Auth: POST /api/auth/login
    Auth->>Auth: SHA256 雜湊密碼
    Auth->>DB: 查詢 Users 資料表
    DB-->>Auth: 回傳使用者資料

    alt 帳號密碼正確
        Auth->>Auth: 產生 JWT Token
        Auth-->>Client: 200 OK - token / username / role / expiration
    else 帳號密碼錯誤
        Auth-->>Client: 401 Unauthorized
    end
```

---

## 攜帶 Token 呼叫 API

```mermaid
sequenceDiagram
    participant Client as 前端
    participant MW as JWT 驗證中介層
    participant Controller as Controller
    participant DB as SQL Server

    Client->>MW: GET /api/product (Authorization: Bearer token)

    alt Token 有效
        MW->>Controller: 通過驗證，傳入使用者資訊
        Controller->>DB: 查詢資料
        DB-->>Controller: 回傳資料
        Controller-->>Client: 200 OK
    else Token 無效或過期
        MW-->>Client: 401 Unauthorized
    else 權限不足
        MW-->>Client: 403 Forbidden
    end
```

---

## JWT Token 結構

```
eyJhbGciOiJIUzI1NiJ9  .  eyJuYW1lIjoiYWRtaW4ifQ  .  簽章
──────────────────────    ───────────────────────    ────
       Header                     Payload            Signature
   （加密演算法）              （使用者資訊）           （防偽）
```

### Payload 內容

| 欄位 | 說明 |
|------|------|
| `ClaimTypes.Name` | 使用者名稱 |
| `ClaimTypes.Role` | 角色（Admin / User） |
| `exp` | 過期時間（60 分鐘） |
| `iss` | 發行者（DemoProject） |
| `aud` | 接收者（DemoProject） |

---

## 角色權限對照

| API | User | Admin |
|-----|------|-------|
| 登入 | ✅ | ✅ |
| 查詢商品 | ✅ | ✅ |
| 新增／更新商品 | ✅ | ✅ |
| **刪除商品** | ❌ | ✅ |
| 查詢預算 | ✅ | ✅ |
| 新增／更新預算 | ✅ | ✅ |
| 匯出 Excel | ✅ | ✅ |
