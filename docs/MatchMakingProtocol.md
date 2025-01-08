## [NEX-Protocols](https://github.com/kinnay/NintendoClients/wiki/NEX-Protocols) > MatchMakingProtocol (21)

| Method ID | Method Name |
| --- | --- |
| 1 | [RegisterGathering](#1-registergathering) |
| 2 | [UnregisterGathering](#2-unregistergathering) |
| 3 | [UnregisterGatherings](#3-unregistergatherings) |
| 4 | [UpdateGathering](#4-updategathering) |
| 5 | [Invite](#5-invite) |
| 6 | [AcceptInvitation](#6-acceptinvitation) |
| 7 | [DeclineInvitation](#7-declineinvitation) |
| 8 | [CancelInvitation](#8-cancelinvitation) |
| 9 | [GetInvitationsSent](#9-getinvitationssent) |
| 10 | [GetInvitationsReceived](#10-getinvitationsreceived) |
| 11 | [Participate](#11-participate) |
| 12 | [CancelParticipation](#12-cancelparticipation) |
| 13 | [GetParticipants](#13-getparticipants) |
| 14 | [AddParticipants](#14-addparticipants) |
| 15 | [GetDetailedParticipants](#15-getdetailedparticipants) |
| 16 | [GetParticipantsURLs](#16-getparticipantsurls) |
| 17 | [FindByType](#17-findbytype) |
| 18 | [FindByDescription](#18-findbydescription) |
| 19 | [FindByDescriptionRegex](#19-findbydescriptionregex) |
| 20 | [FindByID](#20-findbyid) |
| 21 | [FindBySingleID](#21-findbysingleid) |
| 22 | [FindByOwner](#22-findbyowner) |
| 23 | [FindByParticipants](#23-findbyparticipants) |
| 24 | [FindInvitations](#24-findinvitations) |
| 25 | [FindBySQLQuery](#25-findbysqlquery) |
| 26 | [LaunchSession](#26-launchsession) |
| 27 | [UpdateSessionURL](#27-updatesessionurl) |
| 28 | [GetSessionURL](#28-getsessionurl) |
| 29 | [GetState](#29-getstate) |
| 30 | [SetState](#30-setstate) |
| 31 | [ReportStats](#31-reportstats) |
| 32 | [GetStats](#32-getstats) |
| 33 | [DeleteGathering](#33-deletegathering) |
| 34 | [GetPendingDeletions](#34-getpendingdeletions) |
| 35 | [DeleteFromDeletions](#35-deletefromdeletions) |
| 36 | [MigrateGatheringOwnership](#36-migrategatheringownership) |
| 37 | [FindByDescriptionLike](#37-findbydescriptionlike) |
| 38 | [RegisterLocalURL](#38-registerlocalurl) |
| 39 | [RegisterLocalURLs](#39-registerlocalurls) |
| 40 | [UpdateSessionHost](#40-updatesessionhost) |
| 41 | [GetSessionURLs](#41-getsessionurls) |

# (1) RegisterGathering

## Request
| Type | Name | Description |
| --- | --- | --- |
| any&#x3C;Gathering,string&#x3E; | anyGathering |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | %retval% |  |

# (2) UnregisterGathering

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | idGathering |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Bool | %retval% |  |

# (3) UnregisterGatherings

## Request
| Type | Name | Description |
| --- | --- | --- |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;Uint32&#x3E; | lstGatherings |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Bool | %retval% |  |

# (4) UpdateGathering

## Request
| Type | Name | Description |
| --- | --- | --- |
| any&#x3C;Gathering,string&#x3E; | anyGathering |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Bool | %retval% |  |

# (5) Invite

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | idGathering |  |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;Uint32&#x3E; | lstPrincipals |  |
| [String](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#string) | strMessage |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Bool | %retval% |  |

# (6) AcceptInvitation

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | idGathering |  |
| [String](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#string) | strMessage |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Bool | %retval% |  |

# (7) DeclineInvitation

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | idGathering |  |
| [String](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#string) | strMessage |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Bool | %retval% |  |

# (8) CancelInvitation

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | idGathering |  |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;Uint32&#x3E; | lstPrincipals |  |
| [String](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#string) | strMessage |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Bool | %retval% |  |

# (9) GetInvitationsSent

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | idGathering |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;[Invitation](#invitation)&#x3E; | lstInvitations |  |

# (10) GetInvitationsReceived

## Request
This method does not take any parameters

## Response
| Type | Name | Description |
| --- | --- | --- |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;[Invitation](#invitation)&#x3E; | lstInvitations |  |

# (11) Participate

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | idGathering |  |
| [String](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#string) | strMessage |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Bool | %retval% |  |

# (12) CancelParticipation

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | idGathering |  |
| [String](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#string) | strMessage |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Bool | %retval% |  |

# (13) GetParticipants

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | idGathering |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;Uint32&#x3E; | lstParticipants |  |

# (14) AddParticipants

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | idGathering |  |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;Uint32&#x3E; | lstParticipants |  |
| [String](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#string) | strMessage |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Bool | %retval% |  |

# (15) GetDetailedParticipants

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | idGathering |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;[ParticipantDetails](#participantdetails)&#x3E; | lstParticipants |  |

# (16) GetParticipantsURLs

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | idGathering |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;[StationURL](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#stationurl)&#x3E; | lstStationURL |  |

# (17) FindByType

## Request
| Type | Name | Description |
| --- | --- | --- |
| [String](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#string) | strType |  |
| [ResultRange](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#resultrange) | resultRange |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;any&#x3E; | lstGathering |  |

# (18) FindByDescription

## Request
| Type | Name | Description |
| --- | --- | --- |
| [String](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#string) | strDescription |  |
| [ResultRange](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#resultrange) | resultRange |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;any&#x3E; | lstGathering |  |

# (19) FindByDescriptionRegex

## Request
| Type | Name | Description |
| --- | --- | --- |
| [String](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#string) | strDescriptionRegex |  |
| [ResultRange](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#resultrange) | resultRange |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;any&#x3E; | lstGathering |  |

# (20) FindByID

## Request
| Type | Name | Description |
| --- | --- | --- |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;Uint32&#x3E; | lstID |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;any&#x3E; | lstGathering |  |

# (21) FindBySingleID

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | id |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Bool | bResult |  |
| any&#x3C;Gathering,string&#x3E; | pGathering |  |

# (22) FindByOwner

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | id |  |
| [ResultRange](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#resultrange) | resultRange |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;any&#x3E; | lstGathering |  |

# (23) FindByParticipants

## Request
| Type | Name | Description |
| --- | --- | --- |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;Uint32&#x3E; | pid |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;any&#x3E; | lstGathering |  |

# (24) FindInvitations

## Request
| Type | Name | Description |
| --- | --- | --- |
| [ResultRange](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#resultrange) | resultRange |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;any&#x3E; | lstGathering |  |

# (25) FindBySQLQuery

## Request
| Type | Name | Description |
| --- | --- | --- |
| [String](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#string) | strQuery |  |
| [ResultRange](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#resultrange) | resultRange |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;any&#x3E; | lstGathering |  |

# (26) LaunchSession

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | idGathering |  |
| [String](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#string) | strURL |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Bool | %retval% |  |

# (27) UpdateSessionURL

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | idGathering |  |
| [String](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#string) | strURL |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Bool | %retval% |  |

# (28) GetSessionURL

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | idGathering |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Bool | %retval% |  |
| [String](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#string) | strURL |  |

# (29) GetState

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | idGathering |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Bool | %retval% |  |
| Uint32 | uiState |  |

# (30) SetState

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | idGathering |  |
| Uint32 | uiNewState |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Bool | %retval% |  |

# (31) ReportStats

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | idGathering |  |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;[GatheringStats](#gatheringstats)&#x3E; | lstStats |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Bool | %retval% |  |

# (32) GetStats

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | idGathering |  |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;Uint32&#x3E; | lstParticipants |  |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;Uint8&#x3E; | lstColumns |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Bool | %retval% |  |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;[GatheringStats](#gatheringstats)&#x3E; | plstStats |  |

# (33) DeleteGathering

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | gid |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Bool | %retval% |  |

# (34) GetPendingDeletions

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | uiReason |  |
| [ResultRange](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#resultrange) | resultRange |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Bool | %retval% |  |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;[DeletionEntry](#deletionentry)&#x3E; | lstDeletions |  |

# (35) DeleteFromDeletions

## Request
| Type | Name | Description |
| --- | --- | --- |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;Uint32&#x3E; | lstDeletions |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Bool | %retval% |  |

# (36) MigrateGatheringOwnership

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | gid |  |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;Uint32&#x3E; | lstPotentialNewOwnersID |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| Bool | %retval% |  |

# (37) FindByDescriptionLike

## Request
| Type | Name | Description |
| --- | --- | --- |
| [String](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#string) | strDescriptionLike |  |
| [ResultRange](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#resultrange) | resultRange |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;any&#x3E; | lstGathering |  |

# (38) RegisterLocalURL

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | gid |  |
| [String](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#string) | url |  |

## Response
This method does not return anything

# (39) RegisterLocalURLs

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | gid |  |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;[StationURL](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#stationurl)&#x3E; | lstUrls |  |

## Response
This method does not return anything

# (40) UpdateSessionHost

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | gid |  |

## Response
This method does not return anything

# (41) GetSessionURLs

## Request
| Type | Name | Description |
| --- | --- | --- |
| Uint32 | gid |  |

## Response
| Type | Name | Description |
| --- | --- | --- |
| [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;[StationURL](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#stationurl)&#x3E; | lstURLs |  |

# Types

## ParticipantDetails ([Structure](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#structure))
| Name | Type |
| --- | --- |
| m_idParticipant | Uint32 |
| m_strName | [String](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#string) |
| m_strMessage | [String](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#string) |

## Gathering ([Structure](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#structure))
| Name | Type |
| --- | --- |
| m_idMyself | Uint32 |
| m_pidOwner | Uint32 |
| m_pidHost | Uint32 |
| m_uiMinParticipants | Uint16 |
| m_uiMaxParticipants | Uint16 |
| m_uiParticipationPolicy | Uint32 |
| m_uiPolicyArgument | Uint32 |
| m_uiFlags | Uint32 |
| m_uiState | Uint32 |
| m_strDescription | [String](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#string) |

## DynamicGathering ([Gathering](#gathering))
| Name | Type |
| --- | --- |
| m_bufTail | buffertail |

## GameSession ([Gathering](#gathering))
| Name | Type |
| --- | --- |

## Invitation ([Structure](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#structure))
| Name | Type |
| --- | --- |
| m_idGathering | Uint32 |
| m_idGuest | Uint32 |
| m_strMessage | [String](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#string) |

## GatheringStats ([Structure](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#structure))
| Name | Type |
| --- | --- |
| m_pidParticipant | Uint32 |
| m_uiFlags | Uint32 |
| m_lstValues | [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;float&#x3E; |

## DeletionEntry ([Structure](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#structure))
| Name | Type |
| --- | --- |
| m_idGathering | Uint32 |
| m_pid | Uint32 |
| m_uiReason | Uint32 |

## GatheringURLs ([Structure](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#structure))
| Name | Type |
| --- | --- |
| m_gid | Uint32 |
| m_lstStationURLs | [List](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#list)&#x3C;[StationURL](https://github.com/kinnay/NintendoClients/wiki/NEX-Common-Types#stationurl)&#x3E; |
