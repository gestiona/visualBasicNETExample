Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Class RestError
    Public Property code() As Long
        Get
            Return m_code
        End Get
        Set
            m_code = Value
        End Set
    End Property
    Private m_code As Long
    Public Property name() As String
        Get
            Return m_name
        End Get
        Set
            m_name = Value
        End Set
    End Property
    Private m_name As String
    Public Property description() As String
        Get
            Return m_description
        End Get
        Set
            m_description = Value
        End Set
    End Property
    Private m_description As String
    Public Property details() As List(Of String)
        Get
            Return m_details
        End Get
        Set
            m_details = Value
        End Set
    End Property
    Private m_details As List(Of String)
    Public Property internalCodeError() As String
        Get
            Return m_internalCodeError
        End Get
        Set
            m_internalCodeError = Value
        End Set
    End Property
    Private m_internalCodeError As String
    Public Property technicalDetails() As String
        Get
            Return m_technicalDetails
        End Get
        Set
            m_technicalDetails = Value
        End Set
    End Property
    Private m_technicalDetails As String
End Class