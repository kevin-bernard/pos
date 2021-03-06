/****** Script for SelectTopNRows command from SSMS  ******/
SELECT MAX(invno) lastInvno
  FROM [ESQ+].[dbo].[artran];

DECLARE @total table(value INT);
  
insert INTO @total SELECT COUNT(*) - (SELECT COUNT(*) - 1 FROM [ESQ+].[dbo].[artran] artran2 where artran2.invno=artran.invno) FROM [ESQ+].[dbo].[artran] GROUP BY invno;

SELECT SUM(value) countInvno FROM @total;

SELECT int1, int2, int3 FROM [ESQ+].[dbo].sysdata WHERE sysid='AR';

SELECT int1 FROM [ESQ+].[dbo].sysdata WHERE sysid='GL';