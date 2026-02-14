using System.Collections.Generic;

namespace Startech_ML.Web.Storage
{
    public static class PublicationStorage
    {
        // Ahora guardamos objetos reales, no strings
        public static List<object> Publications { get; set; } = new List<object>();
    }
}
