# TestNote

CREATE DATABASE NoteDB;
USE NoteDB;

CREATE TABLE Users (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    IP VARCHAR(128),
    UserName VARCHAR(64),
    BlockDate DATETIME
);

CREATE TABLE Notes (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Content VARCHAR(512),
    CreateDate DATETIME,
    UserId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES USERS(Id)
);

SQL BLOCK
------------------ 1
SELECT dbo.Dep.name	
FROM dbo.Dep 
LEFT JOIN dbo.Work 
	ON dbo.Dep.id = dbo.Work.dep_id 
WHERE dbo.Work.id IS NULL


SELECT dbo.Dep.name
FROM dbo.Dep 
WHERE dbo.Dep.id 
	NOT IN 
	(
		SELECT DISTINCT dbo.Work.dep_id 
		FROM dbo.Work
		WHERE dep_id IS NOT NULL
	)
-- second query excute faster


------------------ 2

Cluster index for f0 and index for f1

------------------ 3
SELECT dbo.detections.virus_name, 
		dbo.detections.detection_date as min_date, 
		dbo.detections.detections_cnt as max_detections_count 
FROM dbo.detections 
WHERE dbo.detections.detections_cnt 
	IN (SELECT MAX(dbo.detections.detections_cnt)
		FROM dbo.detections)
