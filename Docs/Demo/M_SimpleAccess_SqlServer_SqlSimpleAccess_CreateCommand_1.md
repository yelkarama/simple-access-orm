# SqlSimpleAccess.CreateCommand Method (String, CommandType, SqlParameter[])
 

Creates a command.

**Namespace:**&nbsp;<a href="N_SimpleAccess_SqlServer">SimpleAccess.SqlServer</a><br />**Assembly:**&nbsp;SimpleAccess.SqlServer (in SimpleAccess.SqlServer.dll) Version: 0.2.3.0 (0.2.8.0)

## Syntax

**C#**<br />
``` C#
public SqlCommand CreateCommand(
	string commandText,
	CommandType commandType,
	params SqlParameter[] sqlParameters
)
```

**VB**<br />
``` VB
Public Function CreateCommand ( 
	commandText As String,
	commandType As CommandType,
	ParamArray sqlParameters As SqlParameter()
) As SqlCommand
```

**C++**<br />
``` C++
public:
virtual SqlCommand^ CreateCommand(
	String^ commandText, 
	CommandType commandType, 
	... array<SqlParameter^>^ sqlParameters
) sealed
```

**F#**<br />
``` F#
abstract CreateCommand : 
        commandText : string * 
        commandType : CommandType * 
        sqlParameters : SqlParameter[] -> SqlCommand 
override CreateCommand : 
        commandText : string * 
        commandType : CommandType * 
        sqlParameters : SqlParameter[] -> SqlCommand 
```


#### Parameters
&nbsp;<dl><dt>commandText</dt><dd>Type: System.String<br />The query string.</dd><dt>commandType</dt><dd>Type: System.Data.CommandType<br />Type of the command.</dd><dt>sqlParameters</dt><dd>Type: System.Data.SqlClient.SqlParameter[]<br />Options for controlling the SQL.</dd></dl>

#### Return Value
Type: SqlCommand<br />The new command.

#### Implements
<a href="M_SimpleAccess_SqlServer_ISqlSimpleAccess_CreateCommand_1">ISqlSimpleAccess.CreateCommand(String, CommandType, SqlParameter[])</a><br />

## See Also


#### Reference
<a href="T_SimpleAccess_SqlServer_SqlSimpleAccess">SqlSimpleAccess Class</a><br /><a href="Overload_SimpleAccess_SqlServer_SqlSimpleAccess_CreateCommand">CreateCommand Overload</a><br /><a href="N_SimpleAccess_SqlServer">SimpleAccess.SqlServer Namespace</a><br />