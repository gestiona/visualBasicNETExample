Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Class Anotacion
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
    Public Property [date]() As String
        Get
            Return m_date
        End Get
        Set
            m_date = Value
        End Set
    End Property
    Private m_date As String
    Public Property state() As String
        Get
            Return m_state
        End Get
        Set
            m_state = Value
        End Set
    End Property
    Private m_state As String
    Public Property originDate() As String
        Get
            Return m_originDate
        End Get
        Set
            m_originDate = Value
        End Set
    End Property
    Private m_originDate As String
    Public Property originCode() As String
        Get
            Return m_originCode
        End Get
        Set
            m_originCode = Value
        End Set
    End Property
    Private m_originCode As String
    Public Property originOrganization() As String
        Get
            Return m_originOrganization
        End Get
        Set
            m_originOrganization = Value
        End Set
    End Property
    Private m_originOrganization As String
    Public Property originRegistryOffice() As String
        Get
            Return m_originRegistryOffice
        End Get
        Set
            m_originRegistryOffice = Value
        End Set
    End Property
    Private m_originRegistryOffice As String
    Public Property shortDescription() As String
        Get
            Return m_shortDescription
        End Get
        Set
            m_shortDescription = Value
        End Set
    End Property
    Private m_shortDescription As String
    Public Property longDescription() As String
        Get
            Return m_longDescription
        End Get
        Set
            m_longDescription = Value
        End Set
    End Property
    Private m_longDescription As String
    Public Property classification() As String
        Get
            Return m_classification
        End Get
        Set
            m_classification = Value
        End Set
    End Property
    Private m_classification As String
    Public Property incomeType() As String
        Get
            Return m_incomeType
        End Get
        Set
            m_incomeType = Value
        End Set
    End Property
    Private m_incomeType As String
    Public Property deliveryType() As String
        Get
            Return m_deliveryType
        End Get
        Set
            m_deliveryType = Value
        End Set
    End Property
    Private m_deliveryType As String
    Public Property type() As String
        Get
            Return m_type
        End Get
        Set
            m_type = Value
        End Set
    End Property
    Private m_type As String
    Public Property annulledDate() As String
        Get
            Return m_annulledDate
        End Get
        Set
            m_annulledDate = Value
        End Set
    End Property
    Private m_annulledDate As String
    Public Property annulledReason() As String
        Get
            Return m_annulledReason
        End Get
        Set
            m_annulledReason = Value
        End Set
    End Property
    Private m_annulledReason As String
    Public Property category() As String
        Get
            Return m_category
        End Get
        Set
            m_category = Value
        End Set
    End Property
    Private m_category As String
End Class