using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopeServer.al.aldi.topeServer.model;

namespace TopeServer.al.aldi.topeServer.control.db.tables
{
    class TopeRequestDAO
    {
        public const String TABLE_NAME      = "TOPEREQUESTS";
        public const String ID              = "topeRequestId";
        public const String TOPE_ACTION     = "actionId";
        public const String USER            = "user";
        public const String DOMAIN          = "domain";
        public const String DATE            = "date";
        public const String REQUEST_HASH    = "requestHash";
        public const String RESPONSE_HASH   = "responseHash";
        public const String MESSAGE         = "message";
        public const String SUCCESS         = "success";
        public const String TIME_TO_EXECUTE = "timeToExecute";
        public const String TIME_TO_WAIT    = "timeToWait";
        public const String ARG_0           = "arg0";
        public const String ARG_1           = "arg1";
        public const String ARG_2           = "arg2";
        public const String ARG_3           = "arg3";
        public const String ARG_4           = "arg4";
        

        public const String DB_CREATE_TABLE = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ("
           + ID             + " integer not null primary key autoincrement, "
           + TOPE_ACTION    + " integer, "
           + USER           + " varchar(100), "
           + DOMAIN         + " varchar(100), "
           + DATE           + " varchar(100), "
           + REQUEST_HASH   + " varchar(100), "
           + RESPONSE_HASH  + " varchar(100), "
           + MESSAGE        + " varchar(100), "
           + SUCCESS        + " varchar(100), "
           + ARG_0          + " varchar(100), "
           + ARG_1          + " varchar(100), "
           + ARG_2          + " varchar(100), "
           + ARG_3          + " varchar(100), "
           + ARG_4          + " varchar(100), "
           + TIME_TO_EXECUTE + " varchar(100), "
           + TIME_TO_WAIT   + " varchar(100) )" // last one, remove comma
           ;

        public const String DB_DROP_TABLE = "DROP TABLE IF EXISTS " + TABLE_NAME;

        SQLiteDbConnector topeDatabase;

        public TopeRequestDAO()
        {
            // taking the default database name
            topeDatabase = new SQLiteDbConnector();
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
