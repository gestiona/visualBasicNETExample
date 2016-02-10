Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Class AnnotationDocument
    Public Property name() As String
        Get
            Return m_name
        End Get
        Set
            m_name = Value
        End Set
    End Property
    Private m_name As String
    Public Property type() As String
        Get
            Return m_type
        End Get
        Set
            m_type = Value
        End Set
    End Property
    Private m_type As String
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