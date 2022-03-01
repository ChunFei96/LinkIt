BEGIN TRANSACTION;
DROP TABLE IF EXISTS "Patient";
CREATE TABLE IF NOT EXISTS "Patient" (
	"PatientId"	INTEGER NOT NULL UNIQUE,
	"Name"	TEXT NOT NULL,
	PRIMARY KEY("PatientId" AUTOINCREMENT)
);
INSERT INTO "Patient" ("PatientId","Name") VALUES (1,'WenJun'),
 (2,'John'),
 (3,'Bella'),
 (4,'Bolo');
COMMIT;
