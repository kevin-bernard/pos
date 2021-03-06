/****** Script for SelectTopNRows command from SSMS  ******/

SELECT SUM(qtyorder) ytdsaleqty, SUM(qtyorder) ptdsaleqty from [ESQ+].[dbo].[artran]
where itemcode = '019-C0517';

SELECT SUM(artran.cost * artran.qtyorder)  ptdsalevalue, SUM(artran.cost * artran.qtyorder)  ytdsalevalue
FROM [ESQ+].[dbo].[artran]
where artran.itemcode = '019-C0517'

SELECT MAX(invdate) lastsale, MAX(invdate) edtdatetime from [ESQ+].[dbo].[artran]
where itemcode = '019-C0517';

SELECT  ytdsaleqty,
		ptdsaleqty,
		ptdsalevalue,
		ytdsalevalue,
		lastsale,
		edtdatetime
  FROM [ESQ+].[dbo].[icitemlocation]
  
  where itemcode = '019-C0517';