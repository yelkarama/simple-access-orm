# SqlRepository.Insert(*TEntity*) Method (SqlTransaction, StoredProcedureParameters)
 

Inserts the given SQL parameters.

**Namespace:**&nbsp;<a href="N_SimpleAccess_SqlServer_Repository">SimpleAccess.SqlServer.Repository</a><br />**Assembly:**&nbsp;SimpleAccess.SqlServer (in SimpleAccess.SqlServer.dll) Version: 0.2.3.0 (0.2.8.0)

## Syntax

**C#**<br />
``` C#
public int Insert<TEntity>(
	SqlTransaction sqlTransaction,
	StoredProcedureParameters storedProcedureParameters
)
where TEntity : class

```

**VB**<br />
``` VB
Public Function Insert(Of TEntity As Class) ( 
	sqlTransaction As SqlTransaction,
	storedProcedureParameters As StoredProcedureParameters
) As Integer
```

**C++**<br />
``` C++
public:
generic<typename TEntity>
where TEntity : ref class
virtual int Insert(
	SqlTransaction^ sqlTransaction, 
	StoredProcedureParameters^ storedProcedureParameters
) sealed
```

**F#**<br />
``` F#
abstract Insert : 
        sqlTransaction : SqlTransaction * 
        storedProcedureParameters : StoredProcedureParameters -> int  when 'TEntity : not struct
override Insert : 
        sqlTransaction : SqlTransaction * 
        storedProcedureParameters : StoredProcedureParameters -> int  when 'TEntity : not struct
```


#### Parameters
&nbsp;<dl><dt>sqlTransaction</dt><dd>Type: System.Data.SqlClient.SqlTransaction<br />The SQL transaction.</dd><dt>storedProcedureParameters</dt><dd>Type: <a href="T_SimpleAccess_StoredProcedureParameters">SimpleAccess.StoredProcedureParameters</a><br />Options for controlling the stored procedure.</dd></dl>

#### Type Parameters
&nbsp;<dl><dt>TEntity</dt><dd>Type of the entity.</dd></dl>

#### Return Value
Type: Int32<br />.

#### Implements
<a href="M_SimpleAccess_Repository_ISqlRepository_Insert__1_2">ISqlRepository.Insert(TEntity)(SqlTransaction, StoredProcedureParameters)</a><br />

## See Also


#### Reference
<a href="T_SimpleAccess_SqlServer_Repository_SqlRepository">SqlRepository Class</a><br /><a href="Overload_SimpleAccess_SqlServer_Repository_SqlRepository_Insert">Insert Overload</a><br /><a href="N_SimpleAccess_SqlServer_Repository">SimpleAccess.SqlServer.Repository Namespace</a><br />