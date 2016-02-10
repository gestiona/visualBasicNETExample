Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Class OficinaRegistroContent
    Public Property content() As OficinaRegistro()
        Get
            Return m_content
        End Get
        Set
            m_content = Value
        End Set
    End Property
    Private m_content As OficinaRegistro()
End Class