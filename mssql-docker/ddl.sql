IF NOT EXISTS 
(
   SELECT name FROM master.dbo.sysdatabases 
   WHERE name = N'dotnetsample'
)
CREATE DATABASE dotnetsample;
GO
