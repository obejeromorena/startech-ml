// Importamos los hooks useEffect y useState desde React
// useState: permite manejar estado dentro del componente
// useEffect: permite ejecutar código cuando el componente se monta o actualiza
import { useEffect, useState } from "react";

function App() {

    // Creamos un estado llamado "publications"
    // publications: guarda la lista de publicaciones
    // setPublications: función para actualizar ese estado
    // El valor inicial es un array vacío []
    const [publications, setPublications] = useState([]);

    // useEffect se ejecuta cuando el componente se monta (se carga por primera vez)
    // El array vacío [] indica que solo se ejecuta una vez
    useEffect(() => {
        fetchPublications(); // Llamamos a la función que trae las publicaciones del backend
    }, []);

    // Función asincrónica para obtener las publicaciones desde el backend
    const fetchPublications = async () => {
        try {
            // Hacemos una petición GET al backend
            const response = await fetch("https://localhost:7006/api/publications");
            
            // Convertimos la respuesta a formato JSON
            const data = await response.json();

            // Guardamos los datos recibidos en el estado publications
            // Esto provoca que el componente se vuelva a renderizar
            setPublications(data);

        } catch (error) {
            // Si ocurre un error (por ejemplo, el backend está apagado)
            // lo mostramos en consola y también con un alert
            console.error(error);
            alert("Error al obtener publicaciones");
        }
    };
    console.log(publications);

    // Render del componente
    return (
        // Contenedor principal con padding
        <div style={{ padding: "30px" }}>
            
            {/* Título principal */}
            <h1>Publicaciones realizadas</h1>

            {/* 
                Si no hay publicaciones (array vacío),
                mostramos este mensaje
            */}
            {publications.length === 0 && (
                <p>No hay publicaciones todavía.</p>
            )}

            {/*
                Recorremos el array publications con map()
                y generamos un bloque por cada publicación
            */}
            {publications.map((pub, index) => (
                <div
                    key={index} // Clave única para que React identifique cada elemento
                    style={{
                        marginTop: "20px",
                        border: "1px solid gray",
                        padding: "20px"
                    }}
                >
                    {/* Mostramos los datos de cada publicación */}
                    <h2>{pub.title}</h2>
                    <p><strong>ID:</strong> {pub.id}</p>
                    <p><strong>Precio:</strong> ${pub.price}</p>
                    <p><strong>Estado:</strong> {pub.status}</p>

                    {/* 
                        Enlace a la publicación en Mercado Libre
                        target="_blank" abre en nueva pestaña
                        rel="noreferrer" es por seguridad
                    */}
                    <a href={pub.permalink} target="_blank" rel="noreferrer">
                        Ver en Mercado Libre
                    </a>
                </div>
            ))}
        </div>
    );
}

// Exportamos el componente para poder usarlo en otros archivos
export default App;