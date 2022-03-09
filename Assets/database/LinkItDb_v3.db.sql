BEGIN TRANSACTION;
DROP TABLE IF EXISTS "Patient";
CREATE TABLE IF NOT EXISTS "Patient" (
	"PatientId"	INTEGER NOT NULL UNIQUE,
	"Name"	TEXT NOT NULL,
	PRIMARY KEY("PatientId" AUTOINCREMENT)
);
DROP TABLE IF EXISTS "Score";
CREATE TABLE IF NOT EXISTS "Score" (
	"Id"	INTEGER NOT NULL UNIQUE,
	"PatientId"	INTEGER,
	"GameMode"	TEXT,
	"TimeTaken"	TEXT,
	PRIMARY KEY("Id" AUTOINCREMENT)
);
INSERT INTO "Patient" ("PatientId","Name") VALUES (1,'WenJun'),
 (2,'John'),
 (3,'Bella'),
 (4,'Bollo'),
 (6,'Jack');
INSERT INTO "Score" ("Id","PatientId","GameMode","TimeTaken") VALUES (1,1,'A','00:05:30'),
 (2,2,'A','00:02:30'),
 (3,3,'B','00:03:33');
COMMIT;
