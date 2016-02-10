Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Class AddonAuthorization
    Public Property access_token() As String
        Get
            Return m_access_token
        End Get
        Set
            m_access_token = Value
        End Set
    End Property
    Private m_access_token As String
    Public Property authorization_date() As String
        Get
            Return m_authorization_date
        End Get
        Set
            m_authorization_date = Value
        End Set
    End Property
    Private m_authorization_date As String
    Public Property links() As Link()
        Get
            Return m_links
        End Get
        Set
            m_links = Value
        End Set
    End Property
    Private m_links As Link()
End Class