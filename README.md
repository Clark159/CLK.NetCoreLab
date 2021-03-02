# CLK.NetCoreLab


##[.NET Core] OpenTelemetry程式範例

在分散式應用環境中，可以使用OpenTelemetry來進行分散式的鏈路追蹤。先釋出範例程式，後續再補齊教學文章，有興趣的讀者可以先依序學習。

原碼：https://github.com/Clark159/CLK.NetCoreLab

1. ObserverLab: .NET Core中提供IObservable<T>做為觀察者模式樣板。.NET Core內建的DiagnosticSource以IObservable<T>為核心發佈埋點訊息。

2. DiagnosticSourceLab: .NET Core使用內建的DiagnosticSource提供逐條的埋點訊息。

3. ActivitySourceLab: .NET Core使用內建的ActivitySource提供使用ParentId串聯起來的樹狀埋點訊息。

4. OpenTelemetryLab: 開源的OpenTelemetry套件，提供將埋點訊息收集、彙整、導出至APM的功能。(以Uber Jaeger為範例)

5. TraceContextLab: OpenTelemetry套件搭配W3C Trace Context進行分散式導出至APM的功能。(以Uber Jaeger為範例)

後記：這些範例盡量以純粹的.NET Core進行說明，後續將會整合OpenTelemetry + ASP.NET Core的整合範例。