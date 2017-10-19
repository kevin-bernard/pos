CREATE TRIGGER [dbo].[T_Insert_Delete_Arcashd]
       ON  [dbo].[arcashd]
       AFTER INSERT, DELETE
    AS 
    BEGIN
        
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;
        
        -- insert
        IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS(SELECT * FROM deleted)
        BEGIN
            INSERT INTO artrandate(invno) SELECT invno FROM inserted;
        END

        -- delete
        IF EXISTS (SELECT * FROM deleted) AND NOT EXISTS(SELECT * FROM inserted)
        BEGIN
            DELETE FROM artrandate WHERE invno NOT IN (SELECT invno FROM arcashd);
        END

    END;