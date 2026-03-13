const API_URL = "https://localhost:7006/api/publications"

// Obtener publicaciones
export async function getPublications() {
    const res = await fetch(`${API_URL}/ml`)

    if (!res.ok) {
        console.error("Error:", res.status)
        return []
    }

    return await res.json()
}


// Editar publicación
export const updatePublication = async (id, data) => {
    const res = await fetch(`${API_URL}/${id}`, {
        method: "PUT",
        headers: {
        "Content-Type": "application/json"
        },
        body: JSON.stringify(data)
    });

    return await res.json();
};

//eliminar publicacion
export const deletePublication = async (id) => {
    const res = await fetch(`${API_URL}/${id}`, {
        method: "DELETE"
    });

    return await res.json();
};

//crear oublicacion
export async function createPublication(data) {

    const res = await fetch(`${API_URL}`, {
    method: "POST",
    headers: {
        "Content-Type": "application/json"
    },
    body: JSON.stringify(data)
    })

    return await res.json()
}