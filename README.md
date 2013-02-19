# JibbR
JibbR is a bot for [JabbR](https://github.com/davidfowl/JabbR) similar to [Hubot](https://github.com/github/hubot) but written in C# and using things like JabbR.Client, NancyFX, and Reactive Extensions.

## Notes

* This project is in a constant state to flux and it should be expected that things will break from time to time and that the api may change without notice.
* An updated fork of [JabbR.Client](https://github.com/xt0rted/JabbR.Client) is being used.
* SignalR is coming from http://www.myget.org/F/aspnetwebstacknightly/

## Setup
JibbR currently runs in either the console or as a website.
It is recomended the you use the website though since the console will be removed at some point.

1. You will need to create a username/password account in your copy of JabbR that JibbR will use.

2. Your JabbR settings go in the `web.config` file.

3. All other settings go in a file called `jibbr.json` that is placed along side your config file. This contains things such as the rooms to join and what adapters should be loaded.

4. Once the settings are configured you should then be able to run the project and see JibbR join your room.

**NOTE:** The Bing image search adapter requires an api key. You can get a free 5,000 transactions per month key [here](http://www.bing.com/developers/).

### Example jibbr.json

```json
{
  "Rooms": [
  "development"
  ],
  "Adapters": [
    {
      "Name": "default-adapter",
      "Enabled": true
    },
    {
      "Name": "bing-image-adapter",
      "Enabled": true,
      "Settings": {
        "ApiKey": "..."
      }
    },
    {
      "Name": "bing-web-adapter",
      "Enabled": true
    },
    {
      "Name": "github-adapter",
      "Enabled": true
    },
    {
      "Name": "math-adapter",
      "Enabled": true
    }
  ]
}
```