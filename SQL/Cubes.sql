-- cube : get All Possiable rollups 
SELECT Category_Id, Itemtype , Count(*) as Count FROM [Zayed110820240406].[dbo].[IC_Item]
group by Category_Id , ItemType with cube 
-- OR group by Cube (Category_Id , ItemType)

Category_Id	Itemtype	Count
	10	0	2
	1002	0	808
	2002	0	1
--NULL	0	811       #Rollups ItemType = 0 
	10	1	1
	1002	1	15
	2002	1	62
	2003	1	14
--NULL	1	92        #Rollups ItemType = 1 
--10	NULL	3       #Rollups Category_Id	= 10 
--1002	NULL	836   #Rollups Category_Id	= 1002 
--2002	NULL	64    #Rollups Category_Id	= 2002 
--NULL	NULL	3562  #Grand Total