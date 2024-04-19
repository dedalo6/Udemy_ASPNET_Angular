namespace API.Errores
{
    public class ApiException : ApiErrorResponse
    {
        
        public string Detalle { get; set; }

        public ApiException(int statusCode, string mensaje = null, string detalle = null) : base(statusCode,mensaje)
        {
            Detalle = detalle;
        }
    }
}
