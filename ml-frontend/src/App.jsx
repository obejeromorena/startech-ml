import { useState } from "react";

function App() {
    const [title, setTitle] = useState("");
    const [price, setPrice] = useState("");
    const [brand, setBrand] = useState("");
    const [model, setModel] = useState("");
    const [image, setImage] = useState("");

    const [result, setResult] = useState(null);

    const handleSubmit = async (e) => {
        e.preventDefault();

        const product = {
            title: title,
            categoryId: "MLA3530",
            price: parseFloat(price),
            availableQuantity: 1,
            attributes: [
                { id: "BRAND", value_name: brand },
                { id: "MODEL", value_name: model }
            ],
            pictures: [
            { source: image }
            ]
        };

        try {
            const response = await fetch("https://localhost:7006/api/publications", {
            method: "POST",
            headers: {
            "Content-Type": "application/json"
            },
            body: JSON.stringify(product)
            });

            const data = await response.json();

            setResult(data); // 👈 guardamos la respuesta
        } catch (error) {
            console.error(error);
            alert("Error al publicar");
        }
    };

    return (
        <div style={{ padding: "30px" }}>
        <h1>Publicar en Mercado Libre</h1>

        <form onSubmit={handleSubmit}>
            <input
                placeholder="Título"
                value={title}
                onChange={(e) => setTitle(e.target.value)}
            /><br /><br />

            <input
                placeholder="Precio"
                value={price}
                onChange={(e) => setPrice(e.target.value)}
            /><br /><br />

            <input
                placeholder="Marca"
                value={brand}
                onChange={(e) => setBrand(e.target.value)}
            /><br /><br />

            <input
                placeholder="Modelo"
                value={model}
                onChange={(e) => setModel(e.target.value)}
            /><br /><br />

            <input
                placeholder="URL Imagen"
                value={image}
                onChange={(e) => setImage(e.target.value)}
            /><br /><br />

            <button type="submit">Publicar</button>
        </form>

        {/* 👇 MOSTRAR RESULTADO */}
        {result && (
            <div style={{ marginTop: "40px", border: "1px solid gray", padding: "20px" }}>
            <h2>Publicación creada</h2>
            <p><strong>ID:</strong> {result.id}</p>
            <p><strong>Título:</strong> {result.title}</p>
            <p><strong>Precio:</strong> ${result.price}</p>
            <p><strong>Estado:</strong> {result.status}</p>
            <p>
                <strong>Link:</strong>{" "}
                <a href={result.permalink} target="_blank" rel="noreferrer">
                Ver en Mercado Libre
                </a>
            </p>
        </div>
        )}
        </div>
    );
}

export default App;
