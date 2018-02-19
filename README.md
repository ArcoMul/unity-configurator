# Unity Configurator

Read from extendable config files.

## Usage

Get values from JSON file by specifying the key. Seperate levels with a dot. See the Example section below on how to extend one config with another.

```C#
// Define path to directory with all config files and the config you'd like to read
#if UNITY_STANDALONE
Configurator.Initialize(Directory.GetParent(Application.dataPath).FullName + @"\Configs", "production");
#else
Configurator.Initialize(Directory.GetParent(Application.dataPath).FullName + @"\Configs", "editor");
#endif

// Read values
string apiKey = Configurator.Get("api.key"));
float defaultVolume = Configurator.GetFloat("audio.defaults.volume"));
string[] tags = Configurator.GetStringArray("tags"));
bool sendEmail = Configurator.GetBool("email.enabled"));
```

## Example

**Common.json**

```
{
    "title": "Generic application",
    "version": "0.1",
    "api": {
        "host": "http://localhost:3000",
        "path": "/api/v1",
        "key": "123",
        "secret: "456"
    }
}
```

**Production.json**

```
{
    "_extends": "Common",
    "api": {
        "host": "https://production.com",
        "key": "abc",
        "secret: "def"
    }
}
```

**Resulting config**

```
{
    "title": "Generic application",
    "version": "0.1",
    "api": {
        "host": "https://production.com",
        "path": "/api/v1",
        "key": "abc",
        "secret: "def"
    }
}
```
