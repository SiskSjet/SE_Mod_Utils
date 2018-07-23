# Space Engineers Mod Utils

Some libraries used by my other mods for Space Engineers.

* [Logging](#logging)
* [Localization](#localization)
* [Profiler](#profiler)

## Logging

### ILogEventHandler

This library comes with three default `ILogEventHandler`.

* `GlobalStorageHandler` - log to a specified file in `%appdata%\SpaceEngineers\Storage`.
* `LocalStorageHandler` - log to a specified file in `%appdata%\SpaceEngineers\Storage\{CallingType}`.
* `WorldStorageHandler` - log to a specified file in `%appdata%\SpaceEngineers\Saves\{SteamUserId}\{World}\Storage\{CallingType}`.

### Usage

To create an `ILogger` you should call `Logger.ForScope<TScope>()` from your `MySessionComponentBase`.
You can then register a `ILogEventHandler` with `Log.Register(ILogEventHandler eventHandler)`

###### Example

```csharp
[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
public class Mod : MySessionComponentBase {
    private const string LOG_FILE_TEMPLATE = "{0}.log";
    public const string NAME = "Example Mod";
    private static readonly string LogFile = string.Format(LOG_FILE_TEMPLATE, NAME);

    public Mod() {
        Static = this;
        InitializeLogging();
    }

    /// <summary>
    ///     Logger used for logging.
    /// </summary>
    public ILogger Log { get; private set; }

    public static Mod Static { get; private set; }

    /// <summary>
    ///     Unloads all data.
    /// </summary>
    protected override void UnloadData() {
        Log?.EnterMethod(nameof(UnloadData));

        if (Log != null) {
            Log.Debug("Logging stopped");
            Log.Close();
            Log = null;
        }
    }

    /// <summary>
    ///     Initalize the logging system.
    /// </summary>
    private void InitializeLogging() {
        Log = Logger.ForScope<Mod>();
        Log.Register(new WorldStorageHandler(LogFile, LogFormatter, LogEventLevel.All));
        using (Log.BeginMethod(nameof(InitializeLogging))) {
            Log.Debug("Logging initialized");
        }
    }

    private static string LogFormatter(LogEventLevel level, string message, DateTime timestamp, Type scope, string method) {
        return $"[{timestamp:HH:mm:ss:fff}] [{new string(level.ToString().Take(1).ToArray())}] [{scope}->{method}()]: {message}";
    }
}
```

## Localization

Localization helps to create and use localized strings.

### Usage

To use the localization simply call the methods in the static class `Localize`.

##### Create

To create a new localized string you can call `Localize.Create(string id, string English, string Czech = null, ...)`
It has a parameter for every available language in Space Engineers, but only the `id` and `English` parameter are required.

###### Example

```csharp
Localize.Create("DisplayName_Item_Tier_2_Upgrade", English: "Tier 2 Upgrade");
```

##### Get

To get a localized `MyStringId` you can call `Localize.Get(string stringId)`.
the result is automatic localized to the current language set in Space Engineers.

###### Example

```csharp
Localize.Get("DisplayName_Item_Tier_2_Upgrade");
```

##### GetString

If you need a formated and localized string you can use `GetString(string stringId, params object[] args))`.

###### Example

```csharp
Localize.Create("Example_Formated_String", English: "Hello {0}");
Localize.GetString("Example_Formated_String", "world!");
```

## Profiler

The profiler can be used to measure the execution time of your code.

### Usage

To use the profiler simply call the static class `Profiler`.

##### Measure

To start measure a code block you can youse `Profiler.Measure(string scope, string method)`.

###### Example

```csharp
public class DemoClass {
    public void SomeRandomMethod() {
        using (Profiler.Measure(nameof(DemoClass), nameof(SomeRandomMethod))) {
            // your code thats should be profiled.
        }
    }
}
```

##### Results

The property `Profiler.Result` is an `IEnumerable` with all profiler results.

###### Example

```csharp
private static void WriteProfileResults() {
    if (Profiler.Results.Any()) {
        using (var writer = MyAPIGateway.Utilities.WriteFileInLocalStorage("profiler.txt", typeof(Mod))) {
            foreach (var result in Profiler.Results) {
                writer.WriteLine(result);
                // Example line:
                // DemoClass.SomeRandomMethod():     600 executions, avg 0.099444ms, min 0.001500ms, max 29.107300ms, total 59.67ms
            }
        }
    }
}
```


##### SetLogger

If you want to log directly you can set an logger action with `Profiler.SetLogger(Action<string> logger)`.

###### Example

```csharp
public class DemoClass {
    public DemoClass() {
        Log = Logger.ForScope<DemoClass>();
        Profiler.SetLogger(Log.Debug);
    }
    public ILogger Log { get; }
    public void SomeRandomMethod() {
        using (Profiler.Measure(nameof(DemoClass), nameof(SomeRandomMethod))) {
            // your code thats should be profiled.
        }
    }
}
```
