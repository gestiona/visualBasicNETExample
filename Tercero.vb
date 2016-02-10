Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Class Tercero
    Public Property links() As Link()
        Get
            Return m_links
        End Get
        Set
            m_links = Value
        End Set
    End Property
    Private m_links As Link()
    Public Property name() As String
        Get
            Return m_name
        End Get
        Set
            m_name = Value
        End Set
    End Property
    Private m_name As String
    Public Property nif() As String
        Get
            Return m_nif
        End Get
        Set
            m_nif = Value
        End Set
    End Property
    Private m_nif As String
    Public Property personType() As String
        Get
            Return m_personType
        End Get
        Set
            m_personType = Value
        End Set
    End Property
    Private m_personType As String
    Public Property relation() As String
        Get
            Return m_relation
        End Get
        Set
            m_relation = Value
        End Set
    End Property
    Private m_relation As String
    Public Property notificationChannel() As String
        Get
            Return m_notificationChannel
        End Get
        Set
            m_notificationChannel = Value
        End Set
    End Property
    Private m_notificationChannel As String
    Public Property address() As String
        Get
            Return m_address
        End Get
        Set
            m_address = Value
        End Set
    End Property
    Private m_address As String
    Public Property country() As String
        Get
            Return m_country
        End Get
        Set
            m_country = Value
        End Set
    End Property
    Private m_country As String
    Public Property province() As String
        Get
            Return m_province
        End Get
        Set
            m_province = Value
        End Set
    End Property
    Private m_province As String
    Public Property zipCode() As String
        Get
            Return m_zipCode
        End Get
        Set
            m_zipCode = Value
        End Set
    End Property
    Private m_zipCode As String
    Public Property zone() As String
        Get
            Return m_zone
        End Get
        Set
            m_zone = Value
        End Set
    End Property
    Private m_zone As String
    Public Property departament() As String
        Get
            Return m_departament
        End Get
        Set
            m_departament = Value
        End Set
    End Property
    Private m_departament As String
    Public Property email() As String
        Get
            Return m_email
        End Get
        Set
            m_email = Value
        End Set
    End Property
    Private m_email As String
    Public Property phone() As String
        Get
            Return m_phone
        End Get
        Set
            m_phone = Value
        End Set
    End Property
    Private m_phone As String
    Public Property fax() As String
        Get
            Return m_fax
        End Get
        Set
            m_fax = Value
        End Set
    End Property
    Private m_fax As String
    Public Property mobile() As String
        Get
            Return m_mobile
        End Get
        Set
            m_mobile = Value
        End Set
    End Property
    Private m_mobile As String
    Public Property notes() As String
        Get
            Return m_notes
        End Get
        Set
            m_notes = Value
        End Set
    End Property
    Private m_notes As String
End Class