    import { useState } from "react";
    import { createPublication } from "../services/Api";

    export default function CreatePublication() {

    const [title, setTitle] = useState("");
    const [price, setPrice] = useState("");

    const handleSubmit = async () => {

        const data = {
        title: title,
        price: Number(price),
        currency_id: "ARS",
        category_id: "MLA3530",
        available_quantity: 10,
        buying_mode: "buy_it_now",
        condition: "new",
        listing_type_id: "gold_special",
        pictures: [
            {
            source: "https://http2.mlstatic.com/D_NQ_NP_2X_821944-MLA54941784923_042023-F.webp"
            }
        ]
        };

        await createPublication(data);

        alert("Publicación creada");
    };

    return (
        <div>

        <h2>Crear publicación</h2>

        <input
            placeholder="Título"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
        />

        <input
            placeholder="Precio"
            value={price}
            onChange={(e) => setPrice(e.target.value)}
        />

        <button onClick={handleSubmit}>
            Publicar
        </button>

        </div>
    );
    }