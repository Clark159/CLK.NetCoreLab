# CLK.NetCoreLab


## [.NET Core] Quartz.Net 程式範例

.NET Core裡可以使用Quartz.Net進行工作排程。本篇範例程式展示如何使用內建DI及外掛的Autofac進行排程作業掛載，也展示如何使用Cron expression設定每五秒一次的執行時間。除範例程式外，技術細節也推薦下列兩篇參考資料輔助學習。

範例原碼：https://github.com/Clark159/CLK.NetCoreLab

範例專案：QuartzLab

參考資料：
  
  1. 建立Quartz.Net基礎知識及如何在.NET Core專案內掛載排程。 [在 ASP.NET Core和Worker Service中使用Quartz.Net](https://codingnote.cc/zh-tw/p/292319/)  
  
  2. 了解Cron expression規則，以滿足各種排程執行時間需求，例如每月15號執行、每個星期三早上5點執行。 [Cron expression - 蚂蚁集团金融科技](https://tech.antfin.com/docs/10/64769)


## [.NET Core] IOptions<T\> 程式範例

在.NET Core中可以套用內建的Options Pattern，來進行強型別參數的設定+注入。除範例程式外，也推薦兩篇參考資料引導學習。

範例原碼：https://github.com/Clark159/CLK.NetCoreLab

範例專案：OptionsLab

參考資料：

  1. 先從使用場景切入，了解Options Pattern的用途。 [【5min+】更好的选项实践。.Net Core中的IOptions](https://www.cnblogs.com/uoyo/p/12583149.html)。   
    
  2. 再從原始碼切入，剖析Options Pattern的原理。 [ASP.NET Core 2.1 源码学习之 Options:Configure]( https://www.cnblogs.com/RainingNight/p/strongly-typed-options-configure-in-asp-net-core.html)。
  

## [.NET Core] AutoActivate 程式範例

.NET Core裡的Dependency Injection怎麼啟動沒有被注入的Class?我自己的做法，是選用Autofac的AutoActivate功能來完成這個需求。在Autofac裡註冊Class的時候，可以透過宣告AutoActivate，讓Autofac容器啟動的時候生成Class，並且納入容器管理。

範例原碼：https://github.com/Clark159/CLK.NetCoreLab

範例專案：AutoActivateLab

參考資料：[Autofac官網 - 自动激活组件](https://autofaccn.readthedocs.io/zh/latest/lifetime/startup.html?highlight=AutoActivate#id3)


## [.NET Core] OpenTelemetry 程式範例

在分散式微服務環境中，可以使用OpenTelemetry來進行分散式的鏈路追蹤。提供範例程式，給有興趣的朋友依序學習。

範例原碼：https://github.com/Clark159/CLK.NetCoreLab

範例專案：

  1. ObserverLab: .NET Core中提供IObservable<T>做為觀察者模式樣板。.NET Core內建的DiagnosticSource以IObservable<T>為核心發佈埋點訊息。

  2. DiagnosticSourceLab: .NET Core使用內建的DiagnosticSource提供逐條的埋點訊息。

  3. ActivitySourceLab: .NET Core使用內建的ActivitySource提供使用ParentId串聯起來的樹狀埋點訊息。

  4. OpenTelemetryLab: 開源的OpenTelemetry套件，提供將埋點訊息收集、彙整、導出至APM的功能。(以Uber Jaeger為範例)

  5. TraceContextLab: OpenTelemetry套件搭配W3C Trace Context進行分散式導出至APM的功能。(以Uber Jaeger為範例)

後記：這些範例盡量以純粹的.NET Core進行說明。