## Permissions

* `simplewhitelist.use` -- Allows player to use the whitelist command.

## Commands

To use this plugin simple run the `/whitelist` command

Usage: `/whitelist <add|remove> <steam64>`

**Please Note**: If you miss enter their steam64 it will not work. It does not do any checks for if it is a real steam64 id.

## Configuration

The configuration doesn't need to be touched at all, instead feel free to use the available commands, however it does allow you to visualise your whitelisted players.

`Whitelisted` -- This is an array of whitelisted players, it displays their name and their steam64 id.

```json
{
  "Whitelisted": [
    {
      "name": "Wolfleader",
      "steamId": "76561198144671984"
    }
  ]
}
```

Feel free to DM me on discord `WOLFLEADER#0999` if you have any issues
