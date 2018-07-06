# Space Engineers Mod Utils

Some libraries used by my other mods for Space Engineers.

* [Localization](##Localization)

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
