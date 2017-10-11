SET ANSI_PADDING ON; 
GO
CREATE TABLE itemtrantmp(itemcode [nvarchar](25), qtyorder [decimal](18, 6), qtystock [decimal](18, 6), totalqtyorder [decimal](18, 6) NULL, [lastsale] [datetime] NULL, [cost] [decimal](18, 6) NULL);