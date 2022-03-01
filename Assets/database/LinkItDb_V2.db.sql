BEGIN TRANSACTION;
DROP TABLE IF EXISTS "Patient";
CREATE TABLE IF NOT EXISTS "Patient" (
	"PatientId"	INTEGER NOT NULL UNIQUE,
	"Name"	TEXT NOT NULL,
	PRIMARY KEY("PatientId" AUTOINCREMENT)
);
DROP TABLE IF EXISTS "Score";
CREATE TABLE IF NOT EXISTS "Score" (
	"Id"	INTEGER NOT NULL,
	"PatientId"	INTEGER NOT NULL UNIQUE,
	"GameMode"	TEXT NOT NULL,
	"TimeTaken"	TEXT NOT NULL,
	PRIMARY KEY("Id" AUTOINCREMENT)
);
INSERT INTO "Patient" ("PatientId","Name") VALUES (1,'WenJun'),
 (2,'John'),
 (3,'Bella'),
 (4,'Bollo'),
 (6,'Jack');
COMMIT;
