Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Class Recursos
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