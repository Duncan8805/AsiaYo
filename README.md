SOLID 原則應用
本專案嚴格遵循 SOLID 原則，以下是具體應用：
1. 單一職責原則 (SRP)

OrderService 類別只負責訂單處理邏輯。
CurrencyConverter 類別專注於貨幣轉換。
AddressFormatter 類別專門處理地址格式化。
OrderValidator 類別專門負責訂單驗證。

2. 開放封閉原則 (OCP)

使用介面（如 IOrderService, ICurrencyConverter）允許擴展新功能而無需修改現有程式碼。
驗證規則可以通過擴展 AbstractValidator<T> 來添加，而不影響現有驗證邏輯。

3. 里氏替換原則 (LSP)

所有服務實現（如 OrderService, CurrencyConverter）都可以被其介面替換，不會影響程式的正確性。
例如，IOrderService 的任何實現都可以在 OrdersController 中使用，而不會破壞應用邏輯。

4. 介面隔離原則 (ISP)

使用多個特定的介面（如 IOrderService, ICurrencyConverter, IAddressFormatter）而不是一個大而全的介面。
每個介面都精確定義了客戶端所需的方法，避免了冗餘依賴。

5. 依賴反轉原則 (DIP)

高層模組（如 Controllers）依賴於抽象（介面），而不是具體實現。
使用依賴注入來提供具體實現，允許在運行時或測試時輕鬆切換實現。

設計模式應用
本專案運用了多種設計模式，以下是主要使用的設計模式：
1. 依賴注入模式

使用 ASP.NET Core 的內建 DI 容器來管理依賴。
在 Program.cs 中註冊服務，如：
csharpCopyservices.AddScoped<IOrderService, OrderService>();

允許鬆耦合和更容易的單元測試。

2. 策略模式

不同的驗證規則（如 OrderValidator, AddressValidator）可以被視為不同的驗證策略。
允許在運行時選擇不同的驗證策略。

3. 工廠模式

雖然沒有顯式使用，但 ASP.NET Core 的 DI 容器本質上是一個工廠，負責創建和管理物件實例。

4. 裝飾器模式

FluentValidation 的驗證規則鏈可以被視為裝飾器模式的一種應用。
每個驗證規則都是對基本驗證的一種「裝飾」。

5. 觀察者模式

ASP.NET Core 的中介軟體管道可以被視為觀察者模式的一種應用。
每個中介軟體組件「觀察」和處理請求，可以選擇將請求傳遞給下一個組件。