/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- For whatever reason SQL CMD mode doesn't like it when you make statements like this -> :r .\$(scriptName)
-- Therefore the dot backslash part had to be put in a variable too
:setvar d ".\"

:setvar scriptName "Seed.dbo.Concert.sql"
PRINT '$(scriptName)'
:r $(d)$(scriptName)

GO

:setvar scriptName "Seed.dbo.Purchaser.sql"
PRINT '$(scriptName)'
:r $(d)$(scriptName)

GO
