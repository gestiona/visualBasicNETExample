Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Net.Http.Formatting

Class AddonAuthorizationFormatter
    Inherits JsonMediaTypeFormatter
    Public Sub New()
        SupportedMediaTypes.Add(New MediaTypeHeaderValue("application/vnd.gestiona.addon-authorization+json"))
    End Sub

    Public Overrides Function CanReadType(type As Type) As Boolean
        Return MyBase.CanReadType(type) OrElse type = GetType(AddonAuthorization)
    End Function
End Class