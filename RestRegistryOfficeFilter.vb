Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Class RestRegistryOfficeFilter
    Public Property code() As String
        Get
            Return m_code
        End Get
        Set
            m_code = Value
        End Set
    End Property
    Private m_code As String
End Class