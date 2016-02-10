Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Class Link
    Public Property rel() As String
        Get
            Return m_rel
        End Get
        Set
            m_rel = Value
        End Set
    End Property

    Private m_rel As String

    Public Property href() As String
        Get
            Return m_href
        End Get
        Set
            m_href = Value
        End Set
    End Property

    Private m_href As String
End Class