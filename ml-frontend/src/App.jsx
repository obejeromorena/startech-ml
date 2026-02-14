import { useEffect, useState } from "react";

function App() {

    const [publications, setPublications] = useState([]);

    // Cuando el componente se carga, llama al backend
    useEffect(() => {
        fetchPublications();
    }, []);

    const fetchPublications = async () => {
    try {
        const response = await fetch("https://localhost:7006/api/publications");
        const data = await response.json();

        // Ahora ya vienen como objetos
        setPublications(data);
    } catch (error) {
        console.error(error);
        alert("Error al obtener publicaciones");
    }
};


    return (
        <div style={{ padding: "30px" }}>
        <h1>Publicaciones realizadas</h1>

        {publications.length === 0 && (
            <p>No hay publicaciones todavía.</p>
        )}

        {publications.map((pub, index) => (
            <div
            key={index}
            style={{
                marginTop: "20px",
                border: "1px solid gray",
                padding: "20px"
            }}
            >
            <h2>{pub.title}</h2>
            <p><strong>ID:</strong> {pub.id}</p>
            <p><strong>Precio:</strong> ${pub.price}</p>
            <p><strong>Estado:</strong> {pub.status}</p>

            <a href={pub.permalink} target="_blank" rel="noreferrer">
                Ver en Mercado Libre
            </a>
            </div>
        ))}
        </div>
    );
}

export default App;
