USE [master];
GO

IF EXISTS (SELECT 1 FROM   [master].[sys].[databases] WHERE  [name] = 'HOCKEY_API')
  BEGIN
    PRINT 'DROPPING HOCKEY_API';
    ALTER DATABASE [HOCKEY_API] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [HOCKEY_API];
  END;
GO

PRINT 'CREATING HOCKEY_API';
CREATE DATABASE [HOCKEY_API];
ALTER AUTHORIZATION ON DATABASE::[HOCKEY_API] TO [sa];
ALTER AUTHORIZATION ON DATABASE::[HOCKEY_API] TO [sa];
GO

USE [HOCKEY_API];
GO

PRINT 'SCHEMA HOCKEY_API';
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
GO

SET XACT_ABORT ON;
SET NOCOUNT ON;

PRINT 'HYDRATING HOCKEY_API';
INSERT [team] ([team_code], [team_name])
VALUES 
				('MAJ', 'Majestic')
			, ('DIN', 'Dinos')
			, ('GAT', 'Gators')
			, ('HWK', 'Hawks')
			, ('STR', 'Storm')
			, ('KNI', 'Knights')
			, ('GRZ', 'Grizzlies')
			, ('SND', 'Sounders');

SET IDENTITY_INSERT [roster_transaction_type] ON;
INSERT [roster_transaction_type] ([roster_transaction_type_id], [label])
VALUES 
				(1, 'Signed')
			,	(2, 'Injured')
			, (3, 'Healthy')
			, (4, 'Traded');
SET IDENTITY_INSERT [roster_transaction_type] OFF;

SET IDENTITY_INSERT [player] ON;

INSERT [player] ([player_id], [last_name], [first_name])
VALUES 
				(1, 'Luna', 'Neil')
			, (2, 'Sawyer', 'Kadin')
			, (3, 'Michael', 'Kamari')
			, (4, 'Foster', 'Steve')
			, (5, 'Kidd', 'Luciano')
			, (6, 'Price', 'Maximilian')
			, (7, 'Guerra', 'Hamza')
			, (8, 'Holmes', 'Jorge')
			, (9, 'Daniel', 'Rogelio')
			, (10, 'Dougherty', 'Samson')
			, (11, 'Brandt', 'Jamar')
			, (12, 'Newton', 'Wayne')
			, (13, 'Barton', 'Jordan')
			, (14, 'Barnett', 'Desmond')
			, (15, 'Hebert', 'Emiliano')
			, (16, 'Huffman', 'Samir')
			, (17, 'Adkins', 'Nathan')
			, (18, 'Duran', 'Frank')
			, (19, 'Booth', 'Yosef')
			, (20, 'Wang', 'Hunter')
			, (21, 'Rios', 'Dangelo')
			, (22, 'Gill', 'Dillon')
			, (23, 'Flores', 'Cale')
			, (24, 'Rivers', 'Ryland')
			, (25, 'Greer', 'Arnav')
			, (26, 'Ellis', 'London')
			, (27, 'Burton', 'Darryl')
			, (28, 'Leach', 'Haiden')
			, (29, 'Lang', 'River')
			, (30, 'Lamb', 'Xander')
			, (31, 'Ferguson', 'Lyric')
			, (32, 'Salas', 'Winston')
			, (33, 'Kline', 'Gerald')
			, (34, 'Rangel', 'Brett')
			, (35, 'Kelly', 'Kendrick')
			, (36, 'Duncan', 'Salvador')
			, (37, 'Pearson', 'Quinton')
			, (38, 'Cooke', 'Carmelo')
			, (39, 'Roberts', 'Lucas')
			, (40, 'Eaton', 'Isai')
			, (41, 'Snyder', 'Corey')
			, (42, 'Bowen', 'Ricardo')
			, (43, 'Vasquez', 'Felix')
			, (44, 'Wall', 'Carlo')
			, (45, 'Preston', 'Michael')
			, (46, 'Koch', 'Trenton')
			, (47, 'Guerrero', 'Jimmy')
			, (48, 'Mckay', 'Franklin')
			, (49, 'Harper', 'Cameron')
			, (50, 'Long', 'David')
			, (51, 'Madden', 'Harrison')
			, (52, 'Glover', 'Landyn')
			, (53, 'Dean', 'Koen')
			, (54, 'Huff', 'Aydan')
			, (55, 'Jefferson', 'Eliezer')
			, (56, 'Wells', 'Moises')
			, (57, 'Larsen', 'Odin')
			, (58, 'Hancock', 'Rylee')
			, (59, 'Watkins', 'Santiago')
			, (60, 'Marks', 'Jamie')
			, (61, 'Gordon', 'Chad')
			, (62, 'Cook', 'Derrick')
			, (63, 'Winters', 'Braylon')
			, (64, 'Gould', 'Andrew')
			, (65, 'Knight', 'Tristan')
			, (66, 'Acevedo', 'Zane')
			, (67, 'Carrillo', 'Talon')
			, (68, 'Ashley', 'Greyson')
			, (69, 'Malone', 'Raphael')
			, (70, 'Guzman', 'Keon')
			, (71, 'Walker', 'Henry')
			, (72, 'Osborn', 'Cael')
			, (73, 'Mata', 'Kaeden')
			, (74, 'Vincent', 'Ben')
			, (75, 'Cowan', 'Joel')
			, (76, 'Ayala', 'Jace')
			, (77, 'Griffith', 'Leon')
			, (78, 'Alexander', 'Jorden')
			, (79, 'Bates', 'Mohammed')
			, (80, 'David', 'Cash');
SET IDENTITY_INSERT [player] OFF;

SET IDENTITY_INSERT [roster_transaction] ON;
INSERT [roster_transaction] ([roster_transaction_id], [roster_transaction_type_id], [player_id], [team_code], [effective_date])
VALUES 
				(1,1,1,'MAJ','2020-01-15')
			, (2,1,2,'MAJ','2020-01-15')
			, (3,1,3,'MAJ','2020-01-15')
			, (4,1,4,'MAJ','2020-01-15')
			, (5,1,5,'MAJ','2020-01-15')
			, (6,1,6,'MAJ','2020-01-15')
			, (7,1,7,'MAJ','2020-01-15')
			, (8,1,8,'MAJ','2020-01-15')
			, (9,1,9,'MAJ','2020-01-15')
			, (10,1,10,'MAJ','2020-01-15')
			, (11,1,11,'DIN','2020-01-15')
			, (12,1,12,'DIN','2020-01-15')
			, (13,1,13,'DIN','2020-01-15')
			, (14,1,14,'DIN','2020-01-15')
			, (15,1,15,'DIN','2020-01-15')
			, (16,1,16,'DIN','2020-01-15')
			, (17,1,17,'DIN','2020-01-15')
			, (18,1,18,'DIN','2020-01-15')
			, (19,1,19,'DIN','2020-01-15')
			, (20,1,20,'DIN','2020-01-15')
			, (21,1,21,'GAT','2020-01-15')
			, (22,1,22,'GAT','2020-01-15')
			, (23,1,23,'GAT','2020-01-15')
			, (24,1,24,'GAT','2020-01-15')
			, (25,1,25,'GAT','2020-01-15')
			, (26,1,26,'GAT','2020-01-15')
			, (27,1,27,'GAT','2020-01-15')
			, (28,1,28,'GAT','2020-01-15')
			, (29,1,29,'GAT','2020-01-15')
			, (30,1,30,'GAT','2020-01-15')
			, (31,1,31,'HWK','2020-01-15')
			, (32,1,32,'HWK','2020-01-15')
			, (33,1,33,'HWK','2020-01-15')
			, (34,1,34,'HWK','2020-01-15')
			, (35,1,35,'HWK','2020-01-15')
			, (36,1,36,'HWK','2020-01-15')
			, (37,1,37,'HWK','2020-01-15')
			, (38,1,38,'HWK','2020-01-15')
			, (39,1,39,'HWK','2020-01-15')
			, (40,1,40,'HWK','2020-01-15')
			, (41,1,41,'STR','2020-01-15')
			, (42,1,42,'STR','2020-01-15')
			, (43,1,43,'STR','2020-01-15')
			, (44,1,44,'STR','2020-01-15')
			, (45,1,45,'STR','2020-01-15')
			, (46,1,46,'STR','2020-01-15')
			, (47,1,47,'STR','2020-01-15')
			, (48,1,48,'STR','2020-01-15')
			, (49,1,49,'STR','2020-01-15')
			, (50,1,50,'STR','2020-01-15')
			, (51,1,51,'KNI','2020-01-15')
			, (52,1,52,'KNI','2020-01-15')
			, (53,1,53,'KNI','2020-01-15')
			, (54,1,54,'KNI','2020-01-15')
			, (55,1,55,'KNI','2020-01-15')
			, (56,1,56,'KNI','2020-01-15')
			, (57,1,57,'KNI','2020-01-15')
			, (58,1,58,'KNI','2020-01-15')
			, (59,1,59,'KNI','2020-01-15')
			, (60,1,60,'KNI','2020-01-15')
			, (61,1,61,'GRZ','2020-01-15')
			, (62,1,62,'GRZ','2020-01-15')
			, (63,1,63,'GRZ','2020-01-15')
			, (64,1,64,'GRZ','2020-01-15')
			, (65,1,65,'GRZ','2020-01-15')
			, (66,1,66,'GRZ','2020-01-15')
			, (67,1,67,'GRZ','2020-01-15')
			, (68,1,68,'GRZ','2020-01-15')
			, (69,1,69,'GRZ','2020-01-15')
			, (70,1,70,'GRZ','2020-01-15')
			, (71,1,71,'SND','2020-01-15')
			, (72,1,72,'SND','2020-01-15')
			, (73,1,73,'SND','2020-01-15')
			, (74,1,74,'SND','2020-01-15')
			, (75,1,75,'SND','2020-01-15')
			, (76,1,76,'SND','2020-01-15')
			, (77,1,77,'SND','2020-01-15')
			, (78,1,78,'SND','2020-01-15')
			, (79,1,79,'SND','2020-01-15')
			, (80,1,80,'SND','2020-01-15');
SET IDENTITY_INSERT [roster_transaction] OFF;
