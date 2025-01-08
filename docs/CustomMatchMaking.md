## [NEX-Protocols](https://github.com/kinnay/NintendoClients/wiki/NEX-Protocols) > CustomMatchMaking (110)

| Method ID | Method Name |
| --- | --- |
| 1 | [CustomFind](#1-customfind) |

# (1) CustomFind

## Request
| Type | Name | Description |
| --- | --- | --- |
| Sint32 | queryID |  |
| Sint32 | gameMode |  |
| Bool | ranked |  |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;Sint32&#x3E; | props |  |
| [ResultRange](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#resultrange) | resultRange |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;any&#x3E; | lstGatherings |  |

# Types

## HarmonixGathering (Gathering)
| Name | Type |
| --- | --- |
| mGameMode | Sint32 |
| mRanked | Bool |
| mPublic | Bool |
| mProp0 | Sint32 |
| mProp1 | Sint32 |
| mProp2 | Sint32 |
| mProp3 | Sint32 |
| mProp4 | Sint32 |
| mProp5 | Sint32 |
| mProp6 | Sint32 |
| mProp7 | Sint32 |
| mProp8 | Sint32 |
| mBuffer | [Buffer](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#buffer) |
