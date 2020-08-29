# HockeyAPI
> This is the NHLPA software developer test. Any information contained within is completely fictitious.

**Software Requirements:**
- .NET Core 3.1 (or greater)
- SQL Server 2012 (or greater)

## Setup
1. Execute `HOCKEY_API.sql` (found in the source root) to create and hydrate the database.
2. Update `appsettings.json` with your connection string.

## The Task
A 3-on-3 hockey league is seeking a REST API to support their mobile app, for managing team rosters. 
- Teams must have a minimum of __4 and a maximum of 10__  _active players_ at all times. Injured players are exempt from the maximum.  
- A player cannot be declared healthy unless they were previously injured.
- A player cannot be declare injured unless they were previously not injured.
- A player cannot be in the league without being *signed*.
- A player cannot be traded when *injured*.
### Required Endpoints
> Note: The `/team` endpoint has already been completed to demonstrate desired form, though it does not need to be considered final form. Feel free to optimize where desired.

#### `[GET] /team`
List all teams.

#### `[GET] /team/{team_code}`
Team details, including active and inactive players.

#### `[GET] /player?q={search}`
Search players by last & first name. Limited to top 10 results.

#### `[GET] /player/{player_id}`
Player details, including (up to) 10 most recent transactions.

#### `[POST] /player`
Create new player and assign to team. Request body example:
```json
{
    "lastName": "doe",
    "firstName": "john",
    "teamCode": "MAJ",
    "effectiveDate": "2020-01-01"
}
```

#### `[POST] /player/{player_id}/injured`
Assign a player to the injured reserve. Request body example:
```json
{
    "playerId": 1,
    "effectiveDate": "2020-01-01"
}
```

#### `[POST] /player/{player_id}/healthy`
Remove a player from the injured reserve. Request body example:
```json
{
    "playerId": 1,
    "effectiveDate": "2020-01-01"
}
```

#### `[POST] /player/{player_id}/trade`
Trade player to a new team. Request body example:
```json
{
    "playerId": 1,
    "teamCode": "MAJ",
    "effectiveDate": "2020-01-01"
}
```

### Database Schema
```sql
CREATE TABLE [team] (
      PRIMARY KEY ([team_code])
    , [team_code] CHAR(3)      NOT NULL
    , [team_name] NVARCHAR(16) NOT NULL UNIQUE);

CREATE TABLE [player] (
      PRIMARY KEY ([player_id])
    , [player_id]  INT          NOT NULL IDENTITY
    , [last_name]  NVARCHAR(32) NOT NULL
    , [first_name] NVARCHAR(32) NOT NULL);

CREATE TABLE [roster_transaction_type] (
      PRIMARY KEY ([roster_transaction_type_id])
    , [roster_transaction_type_id] INT          NOT NULL IDENTITY
    , [label]                      NVARCHAR(16) NOT NULL);

CREATE TABLE [roster_transaction] (
      PRIMARY KEY ([roster_transaction_id])
    , [roster_transaction_id]      INT      NOT NULL IDENTITY
    , [roster_transaction_type_id] INT      NOT NULL FOREIGN KEY ([roster_transaction_type_id]) REFERENCES [roster_transaction_type] ([roster_transaction_type_id])
    , [player_id]                  INT      NOT NULL FOREIGN KEY ([player_id]) REFERENCES [player] ([player_id])
    , [team_code]                  CHAR(3)  NOT NULL FOREIGN KEY ([team_code]) REFERENCES [team] ([team_code])
    , [effective_date]             DATETIME NOT NULL);
```
