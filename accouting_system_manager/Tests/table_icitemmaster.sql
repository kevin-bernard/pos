/****** Script for SelectTopNRows command from SSMS  ******/
SELECT SUM(qtyorder) ytdsaleqty, SUM(qtyorder) ptdsaleqty from [ESQ+].[dbo].[artran]
where itemcode = '019-C0517';

SELECT MAX(invdate) lastsale from [ESQ+].[dbo].[artran]
where itemcode = '019-C0517';

SELECT SUM(artran.cost * artran.qtyorder)  ytdsalevalue
FROM [ESQ+].[dbo].[artran]
where artran.itemcode = '019-C0517'

SELECT TOP 1000 ytdsaleqty, ptdsaleqty, ytdsalevalue, lastsale
FROM [ESQ+].[dbo].[icitemmaster]
where itemcode = '019-C0517'