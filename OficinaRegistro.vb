Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Class OficinaRegistro
    Public Property links() As Link()
        Get
            Return m_links
        End Get
        Set
            m_links = Value
        End Set
    End Property
    Private m_links As Link()
    Public Property code() As String
        Get
            Return m_code
        End Get
        Set
            m_code = Value
        End Set
    End Property
    Private m_code As String
    Public Property nombre() As String
        Get
            Return m_nombre
        End Get
        Set
            m_nombre = Value
        End Set
    End Property
    Private m_nombre As String
End Class