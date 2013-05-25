using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopeServer.al.aldi.topeServer.control.db.contexts;
using TopeServer.al.aldi.topeServer.model;

namespace TopeServer.al.aldi.topeServer.control.db.tables
{
    class TopeActionDAO
    {
        public const String TABLE_NAME      = "TOPEACTIONS";
        public const String ID              = "actionId";
        public const String ITEM_ID         = "itemId";
        public const String MODULE          = "module";
        public const String METHOD          = "method";
        public const String COMMAND_FULL    = "commandFullPath";
        public const String TITLE           = "title";
        public const String ACTIVE          = "active";
        public const String REVISION_ID     = "revisionId";
        public const String OPPOSITE_ACTION = "oppositeActionId";

        public const String DB_CREATE_TABLE = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " (" 
            + ID            + " integer not null primary key autoincrement, "
            + ITEM_ID       + " integer, "
            + METHOD        + " varchar(100), "
            + COMMAND_FULL  + " varchar(100), "
            + MODULE        + " varchar(255), "
            + TITLE         + " varchar(100), "
            + ACTIVE        + " varchar(100), "
            + REVISION_ID   + " integer, "
            + OPPOSITE_ACTION  + " integer )" // last one, remove comma
            ;

        public const String DB_DROP_TABLE = "DROP TABLE IF EXISTS " + TABLE_NAME;

        SQLiteDbConnector topeDatabase;

        public TopeActionDAO()
        {
            // taking the default database name
            topeDatabase = new SQLiteDbConnector();
        }

        public static List<TopeAction> getAllActions()
        {
            List<TopeAction> actions = new List<TopeAction>();
            TopeActionContext tac = new TopeActionContext();

            IQueryable<TopeAction> actionsQuery = from TOPEACTIONS in tac.actions select TOPEACTIONS;
            foreach (var action in actionsQuery)
            {
                actions.Add(action);
            }

            return actions;

        }

        public static TopeAction getAction(Int64 actionId)
        {
            TopeActionContext tac = new TopeActionContext();
            IQueryable<TopeAction> actionsQuery = from TOPEACTIONS in tac.actions where TOPEACTIONS.actionId == actionId select TOPEACTIONS;
            return actionsQuery.First<TopeAction>();
        }

        public static TopeAction getAction(TopeRequest request)
        {
            return getAction(request.actionId);
        }

        public static TopeAction getAction(String method)
        {
            TopeActionContext tac = new TopeActionContext();
            IQueryable<TopeAction> actionsQuery = from TOPEACTIONS in tac.actions where TOPEACTIONS.method == method select TOPEACTIONS;
            return actionsQuery.First<TopeAction>();
        }

        public void insertIntoDb()
        {

        }

        public void deleteFromDb()
        {

        }

        public void updateInDb()
        {

        }

        public void createTable() 
        {
            topeDatabase.executeNonQuery(DB_CREATE_TABLE);
        }

        public void dropTable()
        {
            topeDatabase.executeNonQuery(DB_DROP_TABLE);
        }
    }
}
