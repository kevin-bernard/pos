﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace accouting_system_manager.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("accouting_system_manager.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap activation_hover {
            get {
                object obj = ResourceManager.GetObject("activation_hover", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap back {
            get {
                object obj = ResourceManager.GetObject("back", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SET ANSI_PADDING ON; 
        ///GO
        ///CREATE TABLE itemtrantmp(itemcode [nvarchar](25), qtyorder [decimal](18, 6), qtystock [decimal](18, 6), totalqtyorder [decimal](18, 6) NULL, [lastsale] [datetime] NULL, [cost] [decimal](18, 6) NULL);.
        /// </summary>
        internal static string create_table_itemtrantmp {
            get {
                return ResourceManager.GetString("create_table_itemtrantmp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap db_backup {
            get {
                object obj = ResourceManager.GetObject("db_backup", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap db_config {
            get {
                object obj = ResourceManager.GetObject("db_config", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap db_config_hover {
            get {
                object obj = ResourceManager.GetObject("db_config_hover", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap db_export {
            get {
                object obj = ResourceManager.GetObject("db_export", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap icons8_Reset_96 {
            get {
                object obj = ResourceManager.GetObject("icons8_Reset_96", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] lic {
            get {
                object obj = ResourceManager.GetObject("lic", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap next {
            get {
                object obj = ResourceManager.GetObject("next", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap Remove_icon {
            get {
                object obj = ResourceManager.GetObject("Remove_icon", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap remove_invoice {
            get {
                object obj = ResourceManager.GetObject("remove_invoice", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap remove_invoice_hover {
            get {
                object obj = ResourceManager.GetObject("remove_invoice_hover", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE PROCEDURE [dbo].[reloadstock]
        ///	(@itemcode nvarchar(25), @qtyorder int, @is_rollback int)
        ///AS
        ///BEGIN
        ///	SET NOCOUNT ON;
        ///    
        ///    DECLARE @id INT
        ///    DECLARE @tranqty INT
        ///    DECLARE @totaltran INT
        ///    DECLARE @continue INT
        ///    DECLARE @commands CURSOR
        ///	
        ///	IF @is_rollback = 1
        ///	BEGIN
        ///		SET @continue = 0;
        ///		UPDATE ic SET ic.tranqty=(ic.tranqty + @qtyorder), ic.stockqty=(ic.tranqty + @qtyorder) FROM ictran ic WHERE trantype=&apos;R&apos; AND tranno=(SELECT MAX(tranno) FROM ictran ic2 where ic2.itemcode=@i [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string stored_proc_reloadstock {
            get {
                return ResourceManager.GetString("stored_proc_reloadstock", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TRIGGER [dbo].[T_Insert_Delete_Arcashd]
        ///       ON  [dbo].[arcashd]
        ///       AFTER INSERT, DELETE
        ///    AS 
        ///    BEGIN
        ///        
        ///        -- SET NOCOUNT ON added to prevent extra result sets from
        ///        -- interfering with SELECT statements.
        ///        SET NOCOUNT ON;
        ///        
        ///        -- insert
        ///        IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS(SELECT * FROM deleted)
        ///        BEGIN
        ///            INSERT INTO artrandate(invno) SELECT invno FROM inserted;
        ///        END
        ///
        ///        -- delete
        ///       [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string trigger_arcashd {
            get {
                return ResourceManager.GetString("trigger_arcashd", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TRIGGER [dbo].[T_InsertItemTranTmp]
        ///       ON  [dbo].[itemtrantmp]
        ///       AFTER INSERT
        ///    AS 
        ///    BEGIN
        ///        
        ///        -- SET NOCOUNT ON added to prevent extra result sets from
        ///        -- interfering with SELECT statements.
        ///        SET NOCOUNT ON;
        ///
        ///        -- Insert statements for trigger here
        ///        DECLARE @qtyorder INT
        ///        DECLARE @is_rollback INT
        ///        DECLARE @itemcode nvarchar(25)
        ///		DECLARE @inserted CURSOR
        ///		
        ///        SET @inserted = CURSOR FOR
        ///			SELECT qtyorder, ite [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string trigger_itemtrantmp {
            get {
                return ResourceManager.GetString("trigger_itemtrantmp", resourceCulture);
            }
        }
    }
}
