1. Creación del proyecto en mercadolibre
Se creó una aplicación en el panel de desarrolladores de Mercado Libre, obteniendo:
client_id
client_secret
Además, se configuró una Redirect URI, que será utilizada por Mercado Libre para redirigir al usuario luego del login exitoso.
2. Problema de entorno local
Mercado Libre no permite callbacks hacia localhost, lo cual impide realizar el flujo OAuth directamente desde una aplicación local.
3. Uso de ngrok
Para resolver el problema del entorno local, se utilizó ngrok, una herramienta que permite exponer un servidor local a internet mediante una URL pública temporal.
Ejemplo:
https://xxxx.ngrok-free.dev → https://localhost:7006

Para poder activarla, una vez abris la aplicacion te va a aparecer una consola y lo activas mediante “ngrok http https://localhostPUERTO”
4. Estructura general del proyecto
Para cumplir con el flujo de autenticación requerido por Mercado Libre, se decidió separar la solución en dos proyectos independientes, cada uno con una responsabilidad clara:
4.1 Proyecto de Consola – StartechML
Aplicación C# de tipo Console App
Responsable de consumir la API de Mercado Libre
Realiza llamadas HTTP autenticadas utilizando un access_token
No maneja login ni autenticación interactiva


4.2 Proyecto Web – StartechML.web
Aplicación ASP.NET Core
Responsable exclusivamente del proceso de autenticación OAuth
5. Implementación del flujo OAuth 2.0
5.1 Endpoint de inicio de login
Se implementó un controlador (AuthController) con un endpoint que:
Construye la URL de autorización de Mercado Libre
Incluye client_id y redirect_uri
5.2 Callback de autorización
Se implementó un controlador (OAuthController) con un endpoint /callback que:
Recibe el parámetro code enviado por Mercado Libre
Valida su existencia
Realiza una llamada HTTP al endpoint /oauth/token de Mercado Libre
Intercambia el code por un access_token
6. Obtención del Access Token
Como resultado del proceso de autenticación:
Se obtuvo un access_token válido
Se confirmó su funcionamiento mediante respuestas exitosas de la API
El token incluye:
Tiempo de expiración
Alcances
Identificador del usuario autenticado
7. Pruebas realizadas
Con el token obtenido:
Se realizaron llamadas a endpoints protegidos de Mercado Libre
Se obtuvo información real del usuario autenticado
Se validó que el token permite acceder correctamente a la API
8. Estructura del archivo
Para mayor orden se creó esta estructura de carpeta y archivos en la solución de consola, cada una cumpliendo con un rol exclusivo, haciendo más fácil el entendimiento del código y su funcionamiento.



9. Publicación individual (fase 1)
En la Fase 1 se implementó exitosamente la creación de una publicación individual en Mercado Libre.
El proceso incluye:
Construcción del objeto PublicationRequest.
Validaciones básicas (título, categoría, precio, cantidad).
Envío de la publicación a la API.
Recepción del ID de publicación creada

10. Estructura del proyecto (actualizada)
Para que al momento de cambiar las publicaciones masivas no haya error, se cambió la estructura del proyecto:
StartechML: Tiene solamente una carpeta la cual tiene un archivo .json que se puede ir actualizando y cambiando para cambiar las publicaciones que se quieren hacer. No tiene otro propósito además de ese.
StartechML.core: Dentro de esta solución está toda la lógica del proyecto la API, los requerimientos y respuestas de las publicaciones y el logger. 
StartechML.web:  Por último, acá se obtiene el token, se hace la publicación y es donde se cumple todos los requerimientos de mercado libre.
11. Cómo iniciarlo 
Iniciar ngrok: Para poder iniciarlo escribis en la consola que se te abre cuando inicias la aplicación “ngrok http https://localhost:PUERTO”
Iniciar Visual Studio (back-end): Seteas para que se inicien dos soluciones a la misma vez, la de consola y la web.
Iniciar Visual Studio Code (front-end): Abrir el link que te aparece una vez que iniciaste el código.
