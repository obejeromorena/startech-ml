    const API_URL = "https://localhost:7006/api/publications";

    export const getPublications = async () => {
    const res = await fetch(`${API_URL}/ml`);
    return await res.json();
};

    export const createPublication = async (data) => {
    const res = await fetch(API_URL, {
        method: "POST",
        headers: {
        "Content-Type": "application/json"
        },
        body: JSON.stringify(data)
    });

    return await res.json();
    };

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

    export const deletePublication = async (id) => {
    const res = await fetch(`${API_URL}/${id}`, {
        method: "DELETE"
    });

    return await res.json();
    };