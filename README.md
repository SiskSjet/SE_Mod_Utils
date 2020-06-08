# Space Engineers Mod Utils

Some libraries used by my other mods for Space Engineers.

* [Logging](#logging)
* [Localization](#localization)
* [Profiler](#profiler)
* [Net](#net)

## Logging

### Options

* bool LogOnEnterAndLeaveMethods
  * set if ILogger should create a log entry on `BeginMethod`, `EnterMethod` and `LeaveMethod` methods.

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

With 1.6.8 a `SessionComponent` for localization is included. It no longer required to include it for yourself.
Put your `MyTexts{xx}.resx` files in `Data\Localization` and it will load the resx files.

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

## Net

The Net library helps to send and receive Messages in multiplayer.

### Usage

First you have to create a new instance of `Network`.
It requires a unique network id. No other mod should use this id.

###### Example
```csharp
using ProtoBuf;
using Sandbox.ModAPI;
using Sisk.Utils.Net;
using Sisk.Utils.Net.Messages;
using VRage.Game;
using VRage.Game.Components;

namespace Demo {
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class DemoSessionComponent : MySessionComponentBase {
        private const ushort NETWORK_ID = 45531;

        public Network Network { get; set; }

        public override void Init(MyObjectBuilder_SessionComponent sessionComponent) {
            base.Init(sessionComponent);

            Network = new Network(NETWORK_ID);
            if (Network.IsClient) {
                MyAPIGateway.Utilities.MessageEntered += OnMessageEntered;
                Network.Register<PongMessage>(OnPongMessageReceived);
            }

            if (Network.IsServer) {
                Network.Register<PingMessage>(OnPingMessageReceived);
            }
        }

        protected override void UnloadData() {
            if (Network != null) {
                if (Network.IsClient) {
                    MyAPIGateway.Utilities.MessageEntered -= OnMessageEntered;
                    Network.Unregister<PongMessage>(OnPongMessageReceived);
                }

                if (Network.IsServer) {
                    Network.Unregister<PingMessage>(OnPingMessageReceived);
                }
            }
        }

        private void OnMessageEntered(string messagetext, ref bool sendtoothers) {
            if (messagetext.ToLower().StartsWith("ping")) {
                sendtoothers = false;
                Network.SendToServer(new PingMessage());
            }
        }

        private void OnPingMessageReceived(ulong sender, PingMessage message) {
            Network.Send(new PongMessage(), sender);
        }

        private void OnPongMessageReceived(ulong sender, PongMessage message) {
            MyAPIGateway.Utilities.ShowMessage("server", "received pong response from server.");
        }
    }

    [ProtoContract]
    public class PingMessage : IMessage {
        public byte[] Serialze() {
            return MyAPIGateway.Utilities.SerializeToBinary(this);
        }
    }

    [ProtoContract]
    public class PongMessage : IMessage {
        public byte[] Serialze() {
            return MyAPIGateway.Utilities.SerializeToBinary(this);
        }
    }
}
```