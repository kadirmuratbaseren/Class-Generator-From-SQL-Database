#Region "Imports"
Imports System
Imports System.Data
Imports System.Data.SqlClient
#End Region

Public Class DbOperations

    'Insert sorgusu olusturularak �al��t�r�l�r.
    Public Function Insert(ByVal ConnectionString As String, ByVal TableName As String, ByVal ColumnsValues As ArrayList) As Integer
        Dim Columns As String = ""
        Dim Values As String = ""
        Dim Query As String = ""
        Dim Result As Integer = 0

        Query = "INSERT INTO " & "TableName" & "("
        Values = " VALUES ("

        'Kolon ve Insert Degerleri okunur..
        For Each Param As QueryValues In ColumnsValues
            Columns &= Param.Columns & ","
            Values &= Param.Values & ","
        Next

        'Sorgu tamamen olusturulur..
        Query &= Columns.Remove(Columns.Length - 1, 1) & ")" & Values.Remove(Values.Length - 1, 1) & ")"

        Dim Conn As Connection = New Connection(ConnectionString, Query)

        Result = Conn.ConnectionOpen()
        Conn.ConnectionClose()
        Return Result
    End Function

    'Update sorgusu olusturularak �al��t�r�l�r.
    Public Function Update(ByVal ConnectionString As String, ByVal TableName As String, ByVal ColumnsValues As ArrayList, ByVal OrginalValues As ArrayList) As Integer
        Dim Script As String = "UPDATE " & TableName & " SET "
        Dim Result As Integer = 0

        'Update sorgusunda <where> den onceki k�s�m� olusturur..
        For Each param As QueryValues In ColumnsValues
            If LCase(param.Values) = "null" Then
                Script &= param.Columns & "is null,"
            Else
                Script &= param.Columns & "='" & param.Values & "',"
            End If
        Next

        Script = Script.Remove(Len(Script) - 1, 1) & " WHERE "

        'Tum kolonlar�n OrginalValue lar� ile sorgudan <where> den sonraki k�sm� olusturur..
        'DataGrid'de se�ilen sat�r de�erleri Orginalvalues dizisinden okunur ve sorgu lsuturulur..
        For Each Param2 As QueryValues In OrginalValues
            If Param2.Values = Nothing Then
                Script &= Param2.Columns & " is null AND "
            Else
                Script &= Param2.Columns & "='" & Param2.Values & "' AND "
            End If
        Next

        Script = Script.Remove(Len(Script) - 4, 4)

        Dim Conn As Connection = New Connection(ConnectionString, Script)

        Result = Conn.ConnectionOpen()
        Conn.ConnectionClose()
        Return Result
    End Function

    'Delete sorgusu olusturularak �al��t�r�l�r.
    Public Function Delete(ByVal ConnectionString As String, ByVal TableName As String, ByVal OrginalValues As ArrayList) As Integer
        Dim Script As String = "DELETE FROM NoName.dbo." & TableName & " WHERE "
        Dim Result As Integer = 0

        'DataGrid'de se�ilen sat�r de�erleri Orginalvalues dizisinden okunur ve sorgu lsuturulur..
        For Each Param As QueryValues In OrginalValues
            If Param.Values = Nothing Then
                Script &= Param.Columns & " is null AND "
            Else
                Script &= Param.Columns & "='" & Param.Values & "' AND "
            End If
        Next

        'Sorgunun sonuna fazladan [ AND ] eklendi�i i�in bu silinir..
        Script = Script.Remove(Script.Length - 4, 4)

        Script = Script.Remove(Len(Script) - 4, 4)

        Dim Conn As Connection = New Connection(ConnectionString, Script)

        Result = Conn.ConnectionOpen()
        Conn.ConnectionClose()
        Return Result
    End Function

    Public Sub New()

    End Sub
End Class
