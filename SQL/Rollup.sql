-- rollup : 
SELECT Itemtype , count(ItemType) as count FROM [Zayed110820240406].[dbo].[IC_Item]
group by Itemtype with rollup
-- OR group by rollup( Itemtype )

Itemtype	count
	0		     1764
	1		     561
	4		     26
	6		     139
	7		     116
	8		     47
	9		     909
--NULL	   3562  --this Line is rollup as the big sum 
---------------------------------------------------------------------------------------- 

SELECT Category_Id, Itemtype , Count(*) FROM [Zayed110820240406].[dbo].[IC_Item]
group by Category_Id , ItemType with Rollup
-- OR group by rollup( Category_Id , ItemType)

Category_Id	Itemtype	Count
	10	0	2
	10	1	1
--10	NULL	3      #First rollup The  Category_Id = 10
	1002	0	808
	1002	1	15
	1002	4	4
	1002	6	5
	1002	7	1
	1002	9	3
--1002	NULL	836  #Secound rollup The  Category_Id	= 1002
	2002	0	1
	2002	1	62
	2002	9	1
--2002	NULL	64   #Third rollup The  Category_Id = 2002	
--NULL	NULL	3562 #Grand Total
