import { useState } from "react";

function App() {
    const [title, setTitle] = useState("");
    const [price, setPrice] = useState("");
    const [brand, setBrand] = useState("");
    const [model, setModel] = useState("");
    const [image, setImage] = useState("");

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
            console.log(data);
            alert("Producto publicado!");
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
        </div>
    );
}

export default App;

