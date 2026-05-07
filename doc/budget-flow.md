# 預算審核流程

## 狀態流程圖

```mermaid
stateDiagram-v2
    [*] --> Draft : 新增預算
    Draft --> Reviewing : 送審\nPATCH /api/budget/{id}/status
    Reviewing --> Approved : 核准\nPATCH /api/budget/{id}/status
    Reviewing --> Draft : 退回\nPATCH /api/budget/{id}/status
    Approved --> [*]

    note right of Draft : 可修改金額
    note right of Reviewing : 可修改金額
    note right of Approved : 金額鎖定，不可修改
```

---

## 各狀態說明

| 狀態 | 說明 | 可修改金額 |
|------|------|-----------|
| `Draft` | 草稿，剛建立或被退回 | ✅ |
| `Reviewing` | 審核中，已送出等待核准 | ✅ |
| `Approved` | 已核准，金額鎖定 | ❌ |

---

## API 操作流程

```mermaid
sequenceDiagram
    participant User as 業務人員
    participant Manager as 主管
    participant API as BudgetController
    participant DB as SQL Server

    User->>API: POST /api/budget\n新增預算（自動為 Draft）
    API->>DB: INSERT Budget (Status = Draft)
    DB-->>API: 成功
    API-->>User: 200 OK

    User->>API: PATCH /api/budget/{id}/status\n{ status: "Reviewing" }
    API->>DB: UPDATE Status = Reviewing
    DB-->>API: 成功
    API-->>User: 200 OK

    Manager->>API: PATCH /api/budget/{id}/status\n{ status: "Approved" }
    API->>DB: UPDATE Status = Approved
    DB-->>API: 成功
    API-->>Manager: 200 OK

    User->>API: PUT /api/budget/{id}\n嘗試修改已核准的預算
    API-->>User: 400 BadRequest\n已核准的預算不可修改
```

---

## DTO 設計說明

| DTO | 用途 | 包含欄位 |
|-----|------|---------|
| `BudgetDto` | 新增／更新預算時前端傳入 | Brand、Channel、Year、Month、BudgetAmount、ActualAmount |
| `UpdateBudgetStatusDto` | 審核狀態變更時前端傳入 | Status |

> `Status` 不包含在 `BudgetDto` 內，確保狀態只能透過審核 API 依流程變更，避免前端任意設定狀態。
