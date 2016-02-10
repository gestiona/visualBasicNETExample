Imports System.IO
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Net.Http.Formatting
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Module Module1

    Private httpClient As HttpClient = Nothing
    Private serverURL As String = "http://192.168.5.105:8080/rest/"
    Private addon As String = "_gecor-registry8"
    Private recursosDictionary As New Dictionary(Of String, String)()
    Private token As String = "_OGkQNittKMTuJKjbMKg4xQ__1"
    Private accessToken As String = "_ok6xLblsp5jBmtvZmhasug__c"
    Private tokenAutorizado As Boolean = True

    Sub Main()

        Try
            httpClient = New HttpClient()
            httpClient.BaseAddress = New Uri(serverURL)

            ' Obtenemos los Bookmarks de los recursos de la API, para a partir
            ' de ellos empezar a 'navegar' haciendo las peticiones.
            getRecursos()

            ' Creamos un token de acceso, al crearlo estará en estado
            ' 'pendiente de autorizar' a la espera de que nos logueemos con un
            ' usuario y lo autoricemos
            If token Is Nothing Then
                token = createToken()
            End If

            ' Comprobamos el estado del token
            While tokenAutorizado = False
                tokenAutorizado = comprobarToken(token)
            End While

            Console.WriteLine("============================== LOGIN CORRECTO ==============================")

            ' Obtenemos la oficina de registro en la que queremos crear la
            ' anotación
            Dim [or] As OficinaRegistro = getOficinaRegistro("RC")

            ' Creamos el tercero y el solicitante a añadir en las anotaciones
            Dim tercero As New Tercero()
            tercero.nif = "33333333T"
            tercero.name = "Tercero-CSharp-03"
            tercero.relation = "INVOLVED"
            tercero.address = "address"
            tercero.zone = "zone"
            tercero.country = "España"
            tercero.province = "Zaragoza"
            tercero.zipCode = "50009"
            tercero.notificationChannel = "PAPER"
            tercero.personType = "JURIDICAL"

            Dim provider As New Tercero()
            provider.nif = "44444444P"
            provider.name = "Tercero-CSharp-04"
            provider.address = "address"
            provider.zone = "zone"
            provider.country = "España"
            provider.province = "Zaragoza"
            provider.zipCode = "50012"
            provider.notificationChannel = "PAPER"
            provider.personType = "JURIDICAL"

            For i As Integer = 0 To 2
                ' Creamos la anotación
                Dim anotacion As New Anotacion()
                anotacion.incomeType = "PRESENTIAL"
                anotacion.shortDescription = "API prueba rendimiento"
                anotacion.classification = "REQUERIMENT"
                anotacion.longDescription = "Aqui van las observaciones de la anotacíon"
                anotacion.originCode = "C0D-O4161N"
                anotacion = crearAnotacion([or].links(1).href, anotacion)

                ' Añadirmos el solicitante y el tercero
                addTercero(anotacion, provider, True)
                addTercero(anotacion, tercero, False)

                ' Subimos un documento a la anotación
                Dim upload As [String] = crearUpload()
                Dim subido As Boolean = subirFichero(upload, "C:/Users/eedevadmin/Downloads/a.pdf")
                addFileToAnotacion(anotacion, upload, "documentoPrueba")

                ' Finalizamos la anotación
                finalizarAnotacion(anotacion)
            Next

            Console.WriteLine("Pulse una tecla para continuar...")
            Console.ReadLine()
        Finally
            If httpClient IsNot Nothing Then
                httpClient = Nothing
            End If
        End Try
    End Sub


    '
    ' Rellena el Dictionary 'recursosDictionary' con todos los bookmarks de la API
    '         
    Private Sub getRecursos()

        Dim request = New HttpRequestMessage()

        request.RequestUri = New Uri(serverURL)
        request.Method = HttpMethod.[Get]

        ' Cabecera con el addon-token
        request.Headers.Add("X-Gestiona-Addon-Token", addon)
        Dim response As HttpResponseMessage = httpClient.SendAsync(request).Result

        If response.StatusCode <> HttpStatusCode.OK Then
            Throw New System.InvalidOperationException("Failed: HTTP error code: " + response.StatusCode)
        Else
            Dim recurso As Recursos = response.Content.ReadAsAsync(Of Recursos)({New LinksFormatter()}).Result

            For Each link As Link In recurso.links
                Console.WriteLine("{0}" & vbLf & vbCr & "{1}" & vbLf & vbCr, link.rel, link.href)
                recursosDictionary.Add(link.rel, link.href)
            Next
        End If
    End Sub


    '
    ' Crea el token con el que tendremos que loguearnos para obtener la autorización
    '         
    Private Function createToken() As String
        Dim request = New HttpRequestMessage()

        request.RequestUri = New Uri(recursosDictionary("vnd.gestiona.addon.authorizations"))
        request.Method = HttpMethod.Post

        ' Cabecera con el addon-token
        request.Headers.Add("X-Gestiona-Addon-Token", addon)
        Dim response As HttpResponseMessage = httpClient.SendAsync(request).Result

        If response.StatusCode = HttpStatusCode.Created Then
            Dim location As String = response.Headers.Location.ToString()

            token = location.Substring(location.LastIndexOf("/"c) + 1)

            Console.WriteLine("::TOKEN ==> " + token)

            Return token
        ElseIf response.StatusCode = HttpStatusCode.Forbidden Then
            Throw New System.InvalidOperationException("Error al crear el accessToken, no se encuentra el addon " + addon)
        Else
            Throw New System.InvalidOperationException("Error al crear el accessToken: " + response.StatusCode)
        End If
    End Function


    '
    ' Compueba que el token que se le pasa como parámetro esté en estado autorizado. En
    ' caso de estar pendiente de autorización nos devuelve la URL en la que nos debemos
    ' loguear con un usuario y contraseña para autorizar ese token
    '         
    Private Function comprobarToken(token As String) As Boolean
        Dim request = New HttpRequestMessage()

        request.RequestUri = New Uri(Convert.ToString(recursosDictionary("vnd.gestiona.addon.authorizations") + "/") & token)
        request.Method = HttpMethod.[Get]

        ' Cabecera con el addon-token
        request.Headers.Add("X-Gestiona-Addon-Token", addon)
        Dim response As HttpResponseMessage = httpClient.SendAsync(request).Result

        ' Si devuelve estado 200 es que ya está autorizado y devuelve los datos del
        ' token y el access-token
        If response.StatusCode = HttpStatusCode.OK Then
            Dim addonAuthorization As AddonAuthorization = response.Content.ReadAsAsync(Of AddonAuthorization)({New AddonAuthorizationFormatter()}).Result

            accessToken = addonAuthorization.access_token

            Console.WriteLine((Convert.ToString("token: ") & token) + " accessToken: " + accessToken)

            Return True
            ' Si devuelve estado 401, nos muestra la URL en la que el usuario se debe
            ' loguear
        ElseIf response.StatusCode = HttpStatusCode.Unauthorized Then
            Dim urlLogin As String = response.Headers.Location.ToString()

            Console.WriteLine("Entre en esta URL y logueese con su usuario y contraseña para validar el token: " & vbLf & "{0}" & vbLf & " [Pulse intro cuando ya lo haya actualizado]", urlLogin)

            Console.ReadLine()

            Return False
        Else
            Console.WriteLine(response.StatusCode)
            Dim restError As RestError = response.Content.ReadAsAsync(Of RestError)({New RestErrorFormatter()}).Result

            Throw New System.InvalidOperationException("Error al comprobar autorización: " + restError.description)
        End If
    End Function

    '
    ' Buscar oficina de registro según el código de la oficina que se le pasa como
    ' parámetro
    '         

    Private Function getOficinaRegistro(code As String) As OficinaRegistro
        Dim restRegistryOfficeFilter As New RestRegistryOfficeFilter()
        restRegistryOfficeFilter.code = code

        Dim b64 As String = base64Encode(Newtonsoft.Json.JsonConvert.SerializeObject(restRegistryOfficeFilter))

        Dim request = New HttpRequestMessage()
        request.RequestUri = New Uri(Convert.ToString(recursosDictionary("vnd.gestiona.registry.offices") + "?filter-view=") & b64)
        request.Method = HttpMethod.[Get]

        request.Headers.Add("X-Gestiona-Access-Token", accessToken)
        Dim response As HttpResponseMessage = httpClient.SendAsync(request).Result

        If response.StatusCode = HttpStatusCode.OK Then
            Dim oficinaRegistro As OficinaRegistroContent = response.Content.ReadAsAsync(Of OficinaRegistroContent)({New OficinaRegistroFormatter()}).Result

            Return oficinaRegistro.content(0)
        Else
            Throw New System.InvalidOperationException("Error al obtener la oficina de registro: " + response.ReasonPhrase)
        End If
    End Function

    '
    ' Hace el POST sobre la uri que le pasamos para crear la anotación con los datos
    ' que le pasamos en el objeto Anotacion
    '         

    Private Function crearAnotacion(uri As String, anotacion As Anotacion) As Anotacion
        Dim request = New HttpRequestMessage()
        request.RequestUri = New Uri(uri)
        request.Method = HttpMethod.Post

        request.Headers.Add("X-Gestiona-Access-Token", accessToken)
        request.Content = New StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(anotacion))
        request.Content.Headers.ContentType = New MediaTypeHeaderValue("application/vnd.gestiona.registry-annotation")
        Dim response As HttpResponseMessage = httpClient.SendAsync(request).Result

        If response.StatusCode = HttpStatusCode.Created Then
            Dim location As String = response.Headers.Location.ToString()

            Return getAnotacion(location)
        Else
            Throw New System.InvalidOperationException("Error al crear anotación: " + response.ReasonPhrase)
        End If
    End Function

    '
    ' Dado el link de una anotación existente, hará la petición GET y nos devolverá los
    ' datos de dicha anotación mapeados en el objeto Anotacion que nos hemos creado
    '         

    Private Function getAnotacion(uri As String) As Anotacion
        Dim request = New HttpRequestMessage()
        request.RequestUri = New Uri(uri)
        request.Method = HttpMethod.[Get]

        request.Headers.Add("X-Gestiona-Access-Token", accessToken)
        Dim response As HttpResponseMessage = httpClient.SendAsync(request).Result

        If response.StatusCode = HttpStatusCode.OK Then
            Dim anotacion As Anotacion = response.Content.ReadAsAsync(Of Anotacion)({New AnotacionFormatter()}).Result

            Return anotacion
        Else
            Throw New System.InvalidOperationException("Error al obtener anotación: " + response.StatusCode)
        End If
    End Function

    '
    ' Crea un nuevo recurso upload sobre el cual tendremos que hacer la subida del
    ' fichero posteriormente
    '         

    Private Function crearUpload() As String
        Dim request = New HttpRequestMessage()
        request.RequestUri = New Uri(recursosDictionary("vnd.gestiona.uploads"))
        request.Method = HttpMethod.Post

        request.Headers.Add("X-Gestiona-Access-Token", accessToken)
        Dim response As HttpResponseMessage = httpClient.SendAsync(request).Result

        If response.StatusCode = HttpStatusCode.Created Then
            Dim location As String = response.Headers.Location.ToString()

            Return location
        ElseIf response.StatusCode = HttpStatusCode.Unauthorized Then
            Throw New System.InvalidOperationException("Error al crear el upload, no tiene autorización")
        Else
            Throw New System.InvalidOperationException("Error al crear el upload: " + response.StatusCode)
        End If
    End Function

    '
    ' Hace el PUT para subir el fichero
    '         

    Private Function subirFichero(uri As String, pathfile As String) As Boolean
        Dim file__1 As FileStream = File.Open(pathfile, FileMode.Open)
        Dim request = New HttpRequestMessage()
        request.RequestUri = New Uri(uri)
        request.Method = HttpMethod.Put

        request.Headers.Add("X-Gestiona-Access-Token", accessToken)
        request.Headers.Add("Accept", "aaplication/octet-stream")
        request.Headers.Add("Slug", "prueba.pdf")
        request.Content = New StreamContent(file__1)
        Dim response As HttpResponseMessage = httpClient.SendAsync(request).Result

        If response.StatusCode = HttpStatusCode.OK Then
            Return True
        ElseIf response.StatusCode = HttpStatusCode.Unauthorized Then
            Throw New System.InvalidOperationException("Error al crear upload: no tiene autorización")
        Else
            Throw New System.InvalidOperationException("Error al crear upload: " + response.ReasonPhrase)
        End If
    End Function

    '
    ' Añadir un documento a la anotación
    '         

    Private Function addFileToAnotacion(anotacion As Anotacion, uri As String, nombreDoc As String) As Boolean
        If anotacion Is Nothing OrElse uri Is Nothing OrElse nombreDoc Is Nothing Then
            Return False
        End If

        Dim annotationDocument As New AnnotationDocument()
        annotationDocument.name = nombreDoc
        annotationDocument.type = "DIGITAL"

        Dim link As New Link()
        link.rel = "content"
        link.href = uri
        Dim arrayLinks As Link() = New Link(0) {link}
        annotationDocument.links = arrayLinks

        Dim request = New HttpRequestMessage()
        request.RequestUri = New Uri(anotacion.links(5).href)
        request.Method = HttpMethod.Post

        request.Headers.Add("X-Gestiona-Access-Token", accessToken)
        request.Content = New StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(annotationDocument))
        request.Content.Headers.ContentType = New MediaTypeHeaderValue("application/vnd.gestiona.annotation-document")
        Dim response As HttpResponseMessage = httpClient.SendAsync(request).Result

        If response.StatusCode = HttpStatusCode.Created Then
            Return True
        Else
            Return False
        End If
    End Function

    '
    ' Finaliza la anotación
    '         

    Private Function finalizarAnotacion(anotacion As Anotacion) As Boolean
        Dim request = New HttpRequestMessage()
        request.RequestUri = New Uri(anotacion.links(6).href)
        request.Method = HttpMethod.Post

        request.Headers.Add("X-Gestiona-Access-Token", accessToken)
        Dim response As HttpResponseMessage = httpClient.SendAsync(request).Result

        If response.StatusCode = HttpStatusCode.OK Then
            Return True
        ElseIf response.StatusCode = HttpStatusCode.Unauthorized Then
            Throw New System.InvalidOperationException("Error al finalizar anotación, no tiene autorización")
        Else
            Throw New System.InvalidOperationException("Error al finalizar anotación: " + response.ReasonPhrase)
        End If
    End Function

    '
    ' Añade el tercero que se le pasa como parámetro a la anotación que también se le
    ' pasa como parámetro
    '         

    Private Function addTercero(anotacion As Anotacion, tercero As Tercero, isProvider As Boolean) As Boolean
        Dim uri As Uri = Nothing
        If isProvider Then
            uri = New Uri(anotacion.links(2).href)
        Else
            uri = New Uri(anotacion.links(3).href)
        End If

        Dim request = New HttpRequestMessage()
        request.RequestUri = uri
        request.Method = HttpMethod.Post

        request.Headers.Add("X-Gestiona-Access-Token", accessToken)
        request.Content = New StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(tercero))
        request.Content.Headers.ContentType = New MediaTypeHeaderValue("application/vnd.gestiona.thirdparty")
        Dim response As HttpResponseMessage = httpClient.SendAsync(request).Result

        If response.StatusCode = HttpStatusCode.Created Then
            Return True
        ElseIf response.StatusCode = HttpStatusCode.Unauthorized Then
            Throw New System.InvalidOperationException("Error al añadir tercero: no tiene autorización " + response.ReasonPhrase)
        Else
            Throw New System.InvalidOperationException("Error al añadir tercero: " + response.StatusCode)
        End If
    End Function

    '
    ' Realiza la codificación en base64, utilizado para los filtros de búsqueda
    '         

    Private Function base64Encode(text As String) As String
        Dim textBytes = System.Text.Encoding.UTF8.GetBytes(text)
        Return System.Convert.ToBase64String(textBytes)
    End Function

End Module
